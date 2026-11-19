namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken ct);

    // R9: the active bookings of one participant, in all rooms.
    Task<List<Booking>> GetActiveForParticipantAsync(string bookedBy, CancellationToken ct);

    void Add(Booking booking);
}