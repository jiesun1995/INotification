using INotificationShared;
using INotificationWeb;
using Serilog;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()

#else
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Information()
#endif
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("logs/log.txt",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

try
{
    Log.Information("Starting web host.");
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults<INotificationWebModule>();
    builder.Host.AddAppSettingsSecretsJson()
        .UseAutofac()
        .UseSerilog();
    await builder.AddApplicationAsync<INotificationWebModule>();
    var app = builder.Build();
    app.UseServiceDefaults();
    await app.InitializeApplicationAsync();
    await app.RunAsync();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}