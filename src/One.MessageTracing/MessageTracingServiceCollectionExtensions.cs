using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace One.MessageTracing;

public static class MessageTracingServiceCollectionExtensions
{
    public static IServiceCollection AddMessageTracing(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        IEnumerable<Type> tracers = assemblies.SelectMany(x => x.GetLoadableTypes())
            .Where(t => typeof(IMessageTracer).IsAssignableFrom(t) && t.IsInterface == false && t.IsAbstract == false);

        foreach (var type in tracers)
        {
            services.AddSingleton(type);
            services.AddSingleton(typeof(IMessageTracer), type);
        }

        IEnumerable<Type> traceWriters = assemblies.SelectMany(x => x.GetLoadableTypes())
            .Where(t => typeof(IMessageTraceWriter).IsAssignableFrom(t) && t.IsInterface == false && t.IsAbstract == false);

        foreach (var type in traceWriters)
        {
            services.AddSingleton(type);
            services.AddSingleton(typeof(IMessageTraceWriter), type);
        }

        services.AddSingleton<MessageTracer>();

        return services;
    }

    static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
    {
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));

        try { return assembly.GetTypes(); }
        catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
    }
}
