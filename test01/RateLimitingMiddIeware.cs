namespace test01
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate next;
        private static int requestCount = 0;
        private static DateTime LastRequestTime= DateTime.Now;
        public RateLimitingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if(requestCount<10)
            {
                requestCount++;
                await next(context);
            }
            else
            { 
                var currentTime = DateTime.Now;
                if((DateTime.Now - LastRequestTime).TotalSeconds < 60)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                }
                else 
                {
                    requestCount = 1; // Reset count after 10 seconds
                    LastRequestTime = currentTime;
                    await next(context);
                }
            }

        }
    }
}
