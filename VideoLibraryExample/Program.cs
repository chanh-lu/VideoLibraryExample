using Microsoft.Extensions.DependencyInjection;
using VideoLibraryExample;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var services = ConfigureServices();
var serviceProvider = services.BuildServiceProvider();

serviceProvider.GetService<App>()?.Run();

IServiceCollection ConfigureServices()
{
    var config = LoadConfiguration();

    var appSettings = config.Get<AppSettings>();
    if (string.IsNullOrWhiteSpace(appSettings.TargetBookmarkName))
    {
        throw new Exception($"'{nameof(appSettings.TargetBookmarkName)}' setting cannot be empty");
    }
    if (string.IsNullOrWhiteSpace(appSettings.TargetBookmarkFile))
    {
        throw new Exception($"'{nameof(appSettings.TargetBookmarkFile)}' setting cannot be empty");
    }
    if (string.IsNullOrWhiteSpace(appSettings.DestinationPath))
    {
        throw new Exception($"'{nameof(appSettings.DestinationPath)}' setting cannot be empty");
    }

    var services = new ServiceCollection();

    services.AddSingleton(config);
    services.AddSingleton<IFileWrapper, FileWrapper>();
    services.AddTransient<IDataSource, BookmarksDataSource>();
    services.AddTransient<App>();

    return services;
}

IConfiguration LoadConfiguration() => new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .AddUserSecrets<AppSettings>()
        .Build();