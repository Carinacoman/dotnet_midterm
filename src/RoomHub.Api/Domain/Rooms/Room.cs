using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Rooms;

public sealed class Room
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }

    // For EF Core.
    private Room()
    {
    }

    // R1: non-empty name and strictly positive capacity.
    public static Result<Room> Create(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation("Room.NameRequired", "The room name is required.");

        if (capacity <= 0)
            return Error.Validation("Room.InvalidCapacity", "The capacity must be strictly positive.");

        return new Room
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Capacity = capacity
        };
    }

    public static Error NotFound(Guid id) =>
        Error.NotFound("Room.NotFound", $"Room '{id}' was not found.");
}
