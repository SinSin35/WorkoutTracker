namespace WorkoutService.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                _logger.LogError(ex, "Произошла необработанная ошибка");

                // Формируем красивый ответ клиенту
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = ex.Message,
                    details = ex.InnerException?.Message
                };

                var json = System.Text.Json.JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
