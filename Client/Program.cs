using Microsoft.AspNetCore.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//builder.Services.AddSocialMediaSharing();
builder.AddClientServices();
builder.Logging.SetMinimumLevel(LogLevel.None);

// Authorization/authentication services registration.
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore(options =>
{
    //options.AddPolicy("ManualQuoteDap",
    //    policy => policy.RequireClaim(ClaimTypes.Role, Permissions.Quote.ManualQuoteDap));
    //options.AddPolicy("ManualQuoteExworks",
    //    policy => policy.RequireClaim(ClaimTypes.Role, Permissions.Quote.ManualQuoteExWorks));
    //options.AddPolicy("ImportPricingMmsrgLcl",
    //    policy => policy.RequireClaim(ClaimTypes.Role, Permissions.ImportPricing.MmsrgLcl));
    //options.AddPolicy("ImportPricingBuyFcl",
    //    policy => policy.RequireClaim(ClaimTypes.Role, Permissions.ImportPricing.BuyFcl));

    //options.AddPolicy("ExportPricingMmsrgLcl",
    //    policy => policy.RequireClaim(ClaimTypes.Role, Permissions.ExportPricing.MmsrgLcl));
    //options.AddPolicy("ExportPricingBuyFcl",
    //    policy => policy.RequireClaim(ClaimTypes.Role, Permissions.ExportPricing.BuyFcl));
    //options.AddPolicy("ExportPricingSellLcl",
    //    policy => policy.RequireClaim(ClaimTypes.Role, Permissions.ExportPricing.SellLcl));
    //options.AddPolicy("ExportPricingSellFcl",
    //    policy => policy.RequireClaim(ClaimTypes.Role, Permissions.ExportPricing.SellFcl));


    ////options.AddPolicy("AgentEdit", policy => policy.RequireClaim(ClaimTypes.Role, new string[] { Permissions.Agents.Edit }));
    ////options.AddPolicy("AgentDelete", policy => policy.RequireClaim(ClaimTypes.Role, new string[] { Permissions.Agents.Delete }));
    //options.AddPolicy("AgentUpload", policy => policy.RequireClaim(ClaimTypes.Role, Permissions.Agents.Upload));
});


// Global HTTP exception handler services
//builder.Services.AddHttpClient();
builder.Services.AddHttpClient("MyHttpClient", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddHttpClientInterceptor();
builder.Services.AddScoped<HttpInterceptorService>();
builder.Services.AddMudServices();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<CookieStorageAccessor>();
//builder.Services.AddScoped<IClipboardService, ClipboardService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader() //set the allowed origin  
                .SetIsOriginAllowed(origin => true);
        });
});

var currentAssembly = typeof(Program).Assembly;
builder.Services.AddFluxor(options => { options.ScanAssemblies(currentAssembly); });

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
builder.Services.AddPWAUpdater();
await builder.Build().RunAsync().ConfigureAwait(false);
