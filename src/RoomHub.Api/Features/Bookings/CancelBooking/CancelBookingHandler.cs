using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CancelBookingCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CancelBookingCommand command, CancellationToken ct)
    {
        var booking = await bookings.GetByIdAsync(command.Id, ct);
        if (booking is null)
            return Booking.NotFound(command.Id);

        // R8 + R11 live in Booking.Cancel(): already cancelled or less than 60 minutes before start -> 409.
        var cancelled = booking.Cancel(timeProvider.GetUtcNow().UtcDateTime);
        if (cancelled.IsFailure)
            return cancelled.Error;

        await unitOfWork.CommitAsync(ct);

        return booking.Id;
    }
}