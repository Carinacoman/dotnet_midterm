using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Common.Http;

public static class ErrorResults
{
    // Turns a domain Error into ProblemDetails (application/problem+json) with the right HTTP code.
    public static IResult ToProblem(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };

        return Results.Problem(detail: error.Message, statusCode: statusCode, title: error.Code);
    }
}
