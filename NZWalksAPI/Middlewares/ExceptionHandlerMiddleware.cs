using System.Net;

namespace NZWalksAPI.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {   
                await next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                logger.LogError(ex, $"{errorId} : {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var error = new
                {
                    ErrorId = errorId,
                    Message = "An unexpected error occurred. Please contact support with the ErrorId."
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }

    }
}
