namespace ePizzaHub.API.Middleware
{
    public class SecondMiddleware
    {
        private readonly RequestDelegate _next;

        public SecondMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context); //this will call the next middleware in the pipeline. If this is the last middleware, it will call the endpoint (controller action) and get the response from there.
        }
    }
}
