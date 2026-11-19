namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken ct);

    void Add(Booking booking);
}
