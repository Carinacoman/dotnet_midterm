namespace RoomHub.Api.Features.Bookings.CreateBooking;

// { id, roomId, status }
public sealed record CreateBookingResponse(Guid Id, Guid RoomId, string Status);
