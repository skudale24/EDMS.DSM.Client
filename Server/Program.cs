using EDMS.DSM.Server.Models;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Serilog;
using EDMS.DSM.Server.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001");
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.

var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var key = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)),
        LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
                                     TokenValidationParameters validationParameters) =>
        {
            return expires > DateTime.UtcNow;
        }
    };
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthenticationController>();
builder.Services.AddScoped<CustomerController>();

builder.Services.AddRazorPages();

// Use Serilog
builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
        .WriteTo.File("logs/EDMS.DSM.Api.log");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("PolicyName", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
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

app.UseCors("AllowSpecificOrigins");

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

await builder.Build().RunAsync().ConfigureAwait(false);
