using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace One.MessageTracing;

public static class MessageTracingServiceCollectionExtensions
{
    public static IServiceCollection AddMessageTracing(this IServiceCollection services)
    {
        services.AddSingleton<MessageTracer>();

        return services;
    }
}
