using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using project.Data;
using project.middleware;
using project.middlewares;
using project.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookApi", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddService();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    // .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, 
                  fileSizeLimitBytes: 100_000_000)
    .CreateLogger();

 builder.Host.UseSerilog();
var app = builder.Build();
// Log.Information("Application is starting...");
// app.Use(async (context, next) =>
// {
//     Log.Information($"Handling request: {context.Request.Method} {context.Request.Path}");
//     await next.Invoke();
//     Log.Information($"Finished handling request: {context.Request.Method} {context.Request.Path}");
// });
// Log.Information("Application is running...");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


 app.UseHttpsRedirection();
//app.UseLogMiddleware();
//app.UseErrorMiddleware();
app.UseStaticFiles(); 
app.UseRouting();
//app.UseAuthMiddleware(); // מיקום זה חשוב כדי שיתבצע על כל בקשה
app.UseAuthentication();
app.UseAuthorization();
//app.UseDefaultFiles();
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "book.html" }
});

app.UseUserMiddleware();
app.MapControllers();

  
Console.WriteLine("Application is running on:");
Console.WriteLine("HTTP: http://localhost:5172");
Console.WriteLine("HTTPS: https://localhost:7148");   
app.Run();


