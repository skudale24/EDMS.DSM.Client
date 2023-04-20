namespace EDMS.DSM.Client.Extensions;

public static class HostBuilderExtensions
{
    public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
    {
        _ = builder.Services
            .AddBlazoredLocalStorage()
            .AddManagers();
        _ = builder.Services.AddScoped<CustomAuthenticationStateProvider>();
        _ = builder.Services.AddScoped<AuthenticationStateProvider>(s =>
            s.GetRequiredService<CustomAuthenticationStateProvider>());

        _ = builder.Services.AddTransient<HttpRequest>();

        return builder;
    }

    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        var managers = typeof(IManager);

        var types = managers
            .Assembly
            .GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new { Service = t.GetInterface($"I{t.Name}"), Implementation = t })
            .Where(t => t.Service != null);

        foreach (var type in types)
        {
            if (managers.IsAssignableFrom(type.Service))
            {
                _ = services.AddTransient(type.Service, type.Implementation);
            }
        }

        return services;
    }
}
