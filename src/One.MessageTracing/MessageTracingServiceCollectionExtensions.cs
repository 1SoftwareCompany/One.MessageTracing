using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace One.MessageTracing;

public static class MessageTracingServiceCollectionExtensions
{
    public static IServiceCollection AddMessageTracing(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> tracers = assembly.GetTypes().Where(t => typeof(IMessageTracer).IsAssignableFrom(t) && t.IsInterface == false && t.IsAbstract == false);
        foreach (var type in tracers)
            services.AddSingleton(typeof(IMessageTracer), type);

        IEnumerable<Type> traceWriters = assembly.GetTypes().Where(t => typeof(IMessageTraceWriter).IsAssignableFrom(t) && t.IsInterface == false && t.IsAbstract == false);
        foreach (var type in traceWriters)
            services.AddSingleton(typeof(IMessageTraceWriter), type);

        services.AddSingleton<MessageTracer>();

        return services;
    }
}
