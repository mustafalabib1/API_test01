using System.Diagnostics;

namespace test01.Middlewares
{
    public class ProfilingMiddIeware 
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ProfilingMiddIeware> logger;

        public ProfilingMiddIeware(RequestDelegate next,ILogger<ProfilingMiddIeware>logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await next(context);
            stopwatch.Stop();
            logger.LogInformation("Request took {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
        }
    }
}
