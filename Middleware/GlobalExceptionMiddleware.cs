using Inventory.Exceptions;

public sealed class GlobalExceptionMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ProductNotFoundException ex)
        {
            _logger.LogWarning(ex, "Product not found");
            await WriteProblem(context, StatusCodes.Status404NotFound, "Product not found");
        }
        catch (DuplicateProductNameException ex)
        {
            _logger.LogWarning(ex, "Duplicate product name");
            await WriteProblem(context, StatusCodes.Status409Conflict, "A product with the same name already exists.");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation");
            await WriteProblem(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblem(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    private static async Task WriteProblem(HttpContext ctx, int status, string message)
    {
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";

        var payload = new
        {
            correlationId = ctx.TraceIdentifier,
            status,
            message
        };

        await ctx.Response.WriteAsJsonAsync(payload);
    }
}
