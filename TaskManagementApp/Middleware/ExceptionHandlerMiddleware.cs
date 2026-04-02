namespace TaskManagementApp.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
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
                logger.LogError($"An unhandled exception occurred: {ex}",$"{errorId}: {ex.Message}");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var errorResponse = new 
                {
                    Id =errorId, 
                    Message = "An unexpected error occurred. Please try again later." 
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
