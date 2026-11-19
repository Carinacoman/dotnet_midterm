using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<Result<CreateBookingResponse>> HandleAsync(CreateBookingCommand command, CancellationToken ct)
    {
        // 404 before any validation.
        var room = await rooms.GetByIdAsync(command.RoomId, ct);
        if (room is null)
            return Room.NotFound(command.RoomId);

        var activeBookings = await bookings.GetActiveForRoomAsync(room.Id, ct);

        // The rules are in the domain; the handler only loads, calls and saves.
        var created = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            timeProvider.GetUtcNow().UtcDateTime,
            activeBookings);

        if (created.IsFailure)
            return created.Error;

        var booking = created.Value;
        bookings.Add(booking);
        await unitOfWork.CommitAsync(ct);

        return new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString());
    }
}
