using Microsoft.AspNetCore.Http;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Presentation.Extensions;

public static class ResultExtension
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        return HandleMatch(result.Errors);
    }
    
    public static IResult ToHttpResult(this Result result)
    {
        return HandleMatch(result.Errors);
    }
    
    private static IResult HandleMatch(IReadOnlyList<Error> errors)
    {
        var firstError = errors.FirstOrDefault();
        if (firstError == null) return Results.BadRequest();

        var code = firstError.Code;

        return code switch
        {
            _ when code.EndsWith("NotFound") || code.EndsWith("NotExists") => Results.NotFound(errors),
            _ when code.Contains("Unauthorized") => Results.Unauthorized(),
            _ when code.Contains("Conflict") || code.Contains("AlreadyExists") => Results.Conflict(errors),
            _ when code.StartsWith("Database") => Results.StatusCode(500),
            _ => Results.BadRequest(errors)
        };
    }
}