using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class EfBookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);

    public Task<List<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken ct) =>
        db.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(ct);

    public Task<List<Booking>> GetActiveForParticipantAsync(string bookedBy, CancellationToken ct) =>
        db.Bookings
            .Where(b => b.BookedBy == bookedBy && b.Status == BookingStatus.Active)
            .ToListAsync(ct);

    public void Add(Booking booking) => db.Bookings.Add(booking);
}