using Domain.Interface;
using Microsoft.Extensions.Caching.Distributed;

namespace Ecommerce_App
{
    public class VisitorCountMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public VisitorCountMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task Invoke(HttpContext context)
        {
            string ipAddress = context.Connection.RemoteIpAddress.ToString();
            DateTime today = DateTime.Today;

            var cacheKey = $"{ipAddress}-{today:yyyy-MM-dd}";
            var trackingPerformed = await _cache.GetStringAsync(cacheKey);

            if (trackingPerformed == null)
            {
                using (var scope = context.RequestServices.CreateScope())
                {
                    var visitorService = scope.ServiceProvider.GetRequiredService<IVisitorService>();

                    if (!await visitorService.HasVisitedToday(ipAddress, today))
                    {
                        await visitorService.AddOrUpdateVisitor(ipAddress, today);
                    }
                }

                await _cache.SetStringAsync(cacheKey, "true", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });
            }

            await _next(context);
        }
    }
}
