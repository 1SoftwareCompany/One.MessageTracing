using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace One.MessageTracing;

public static class MessageTracingServiceCollectionExtensions
{
    public static IServiceCollection AddMessageTracing(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<MessageTracer>();

        return services;
    }
}
