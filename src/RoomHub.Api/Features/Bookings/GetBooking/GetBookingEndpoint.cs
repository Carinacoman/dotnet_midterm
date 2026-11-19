using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public static class GetBookingEndpoint
{
    // GET /api/bookings/{id} -> 200, or ProblemDetails 404
    public static void MapGetBooking(this IEndpointRouteBuilder app) =>
        app.MapGet("/api/bookings/{id:guid}", async (
            Guid id,
            IQueryHandler<GetBookingQuery, BookingResponse> handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new GetBookingQuery(id), ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.Error.ToProblem();
        });
}
