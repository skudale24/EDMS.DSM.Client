//using Autofac;
//using CacheLibrary;
using EDM.Common;
using EDM.ContentHandler;
using EDMS.DSM.Server.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text;
using Toolbelt.Blazor.Extensions.DependencyInjection;


//var container = AutofacConfig.Configure();
//using (var scope = container.BeginLifetimeScope())
//{
//    // Resolve dependencies and start the application
//    var cache = scope.Resolve<IDistributedCache>();
//    cache.Set("ProgramId", Encoding.UTF8.GetBytes("2"));
//    cache.Set("UserId", Encoding.UTF8.GetBytes("10572"));

//    var docTypeStorage = scope.Resolve<DocTypeStorage>();
//    docTypeStorage.Cache = cache;
//    docTypeStorage.LoadFromCache();
//    // ...

//    var myClass = scope.Resolve<FileHandlerCreator>();
//}


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001");
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials(); //set the allowed origin  
        });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")),
                ServiceLifetime.Transient);

builder.Services.AddPWAUpdater();

AppDomain.CurrentDomain.SetData("ContentRootPath", builder.Environment.ContentRootPath);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    _ = app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

await builder.Build().RunAsync().ConfigureAwait(false);
