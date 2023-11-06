using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// use serilog to log all requests to the API
builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
        .Enrich.WithProperty("Application", hostContext.HostingEnvironment.ApplicationName)
        .Enrich.WithProperty("Environment", hostContext.HostingEnvironment.EnvironmentName)
        .WriteTo.Console(new RenderedCompactJsonFormatter())
        .WriteTo.GrafanaLoki(hostContext.Configuration["loki"]);

});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();