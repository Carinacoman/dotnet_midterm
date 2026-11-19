using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    // R6: exactly 4 hours is allowed.
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    // For EF Core.
    private Booking()
    {
    }

    // All the booking rules (R2-R7) live here. nowUtc comes from TimeProvider (R5).
    // roomBookings are the bookings that already exist for this room.
    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc,
        IEnumerable<Booking> roomBookings)
    {
        // R2
        if (string.IsNullOrWhiteSpace(bookedBy))
            return Error.Validation("Booking.BookedByRequired", "BookedBy is required.");

        var start = ToUtc(startUtc);
        var end = ToUtc(endUtc);

        // R3
        if (end <= start)
            return Error.Validation("Booking.InvalidInterval", "EndUtc must be strictly after StartUtc.");

        // R4
        if (attendees <= 0)
            return Error.Validation("Booking.InvalidAttendees", "Attendees must be strictly positive.");

        if (attendees > room.Capacity)
            return Error.Validation("Booking.OverCapacity", "Attendees must be at most the room capacity.");

        // R5
        if (start < ToUtc(nowUtc))
            return Error.Validation("Booking.StartInPast", "A booking cannot start in the past.");

        // R6
        if (end - start > MaxDuration)
            return Error.Validation("Booking.TooLong", "A booking can last at most 4 hours.");

        // R7: only active bookings count, and touching intervals do not overlap.
        if (roomBookings.Any(b => b.OverlapsWith(start, end)))
            return Error.Conflict("Booking.Overlap", "The room is already booked in this interval.");

        return new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = bookedBy.Trim(),
            StartUtc = start,
            EndUtc = end,
            Attendees = attendees,
            Status = BookingStatus.Active
        };
    }

    // R7 + R8: a cancelled booking frees its interval; strict "<" so 10-12 and 12-14 do not overlap.
    public bool OverlapsWith(DateTime startUtc, DateTime endUtc) =>
        Status == BookingStatus.Active && StartUtc < endUtc && startUtc < EndUtc;

    // R8: Active -> Cancelled; cancelling again is a conflict.
    public Result<Booking> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return Error.Conflict("Booking.AlreadyCancelled", "The booking is already cancelled.");

        Status = BookingStatus.Cancelled;
        return this;
    }

    public static Error NotFound(Guid id) =>
        Error.NotFound("Booking.NotFound", $"Booking '{id}' was not found.");

    // Every date is kept in UTC, whatever form the client sent it in.
    private static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}
