using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms;

// { id, name, capacity }, used by CreateRoom and GetRoom.
public sealed record RoomResponse(Guid Id, string Name, int Capacity)
{
    public static RoomResponse From(Room room) => new(room.Id, room.Name, room.Capacity);
}
