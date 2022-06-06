using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SprintBetApi.Extensions
{
    public static class ApplicationInsights
    {
        public static void AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            var appInsightsConfiguration = new ConfigurationBuilder()
                .AddApplicationInsightsSettings(configuration["ApplicationInsights"])
                .Build();

            services.AddApplicationInsightsTelemetry(appInsightsConfiguration);
            services.AddApplicationInsightsTelemetryProcessor<FilterOutHealthCheckProcessor>();
        }
    }

    internal class FilterOutHealthCheckProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FilterOutHealthCheckProcessor(ITelemetryProcessor next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Process(ITelemetry item)
        {
            if (_httpContextAccessor?.HttpContext?.Request?.Path == "/health")
            {
                return;
            }

            _next.Process(item);
        }
    }
}

