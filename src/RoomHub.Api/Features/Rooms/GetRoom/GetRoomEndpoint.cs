using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public static class GetRoomEndpoint
{
    // GET /api/rooms/{id} -> 200, or ProblemDetails 404
    public static void MapGetRoom(this IEndpointRouteBuilder app) =>
        app.MapGet("/api/rooms/{id:guid}", async (
            Guid id,
            IQueryHandler<GetRoomQuery, RoomResponse> handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new GetRoomQuery(id), ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.Error.ToProblem();
        });
}
