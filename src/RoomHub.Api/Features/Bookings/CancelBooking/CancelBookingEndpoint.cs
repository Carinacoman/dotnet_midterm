using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    // POST /api/bookings/{id}/cancel -> 204, or ProblemDetails 404 / 409
    public static void MapCancelBooking(this IEndpointRouteBuilder app) =>
        app.MapPost("/api/bookings/{id:guid}/cancel", async (
            Guid id,
            ICommandHandler<CancelBookingCommand, Guid> handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new CancelBookingCommand(id), ct);

            return result.IsSuccess
                ? Results.NoContent()
                : result.Error.ToProblem();
        });
}
