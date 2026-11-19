using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed class GetBookingHandler(IBookingRepository bookings) : IQueryHandler<GetBookingQuery, BookingResponse>
{
    public async Task<Result<BookingResponse>> HandleAsync(GetBookingQuery query, CancellationToken ct)
    {
        var booking = await bookings.GetByIdAsync(query.Id, ct);
        if (booking is null)
            return Booking.NotFound(query.Id);

        return BookingResponse.From(booking);
    }
}
