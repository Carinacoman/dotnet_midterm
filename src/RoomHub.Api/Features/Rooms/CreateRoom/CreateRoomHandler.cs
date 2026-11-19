using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(IRoomRepository rooms, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, RoomResponse>
{
    public async Task<Result<RoomResponse>> HandleAsync(CreateRoomCommand command, CancellationToken ct)
    {
        var created = Room.Create(command.Name, command.Capacity);
        if (created.IsFailure)
            return created.Error;

        rooms.Add(created.Value);
        await unitOfWork.CommitAsync(ct);

        return RoomResponse.From(created.Value);
    }
}
