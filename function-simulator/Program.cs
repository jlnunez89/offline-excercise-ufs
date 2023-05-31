using data;
using function_simulator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.EntityFrameworkCore.Extensions;
using Newtonsoft.Json;
using services;
using System.Configuration;

namespace function_simulator;

public class Program
{
    const string CbsApiUrlPattern = @"https://api.cbssports.com/fantasy/players/list?version=3.0&response_format=json&sport={0}";

    public static async Task Main(string[] args)
    {
        // Create a cancellation token source
        var cts = new CancellationTokenSource();

        // Register the cancellation token source to handle Ctrl-C
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true; // Prevent immediate termination
            cts.Cancel(); // Cancel the task
        };

        try
        {
            var services = new ServiceCollection();
            var config = ConfigureAppConfiguration();

            ConfigureServices(services, config);

            var serviceProvider = services.BuildServiceProvider();

            await CollectPlayerInfoTask(serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<PlayerService>(), cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation gracefully if needed.
            Console.WriteLine("Collection of player data was cancelled.");
        }
        finally
        {
            cts.Dispose();
        }
    }

    private static IConfiguration ConfigureAppConfiguration()
    {
        // Create a configuration builder
        var configurationBuilder = new ConfigurationBuilder();

        // Load configuration from appsettings.json
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        // Build the configuration
        return configurationBuilder.Build();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
        });

        services.AddSingleton(loggerFactory);

        services.AddTransient<Func<DatabaseContext>>((services) => services.GetRequiredService<DatabaseContext>);

        services.AddSingleton<PlayerService>();

        var dbConnectionStr = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(dbConnectionStr))
        {
            throw new ApplicationException("A connection string for the database must be defined.");
        }

        // TODO: I would send this to it's own separate provider library, but for simplicit I'm going to
        // bridge this here.
        services.AddEntityFrameworkMySQL()
                .AddDbContext<DatabaseContext>(options => options.UseMySQL(dbConnectionStr), ServiceLifetime.Transient);
    }

    private static async Task CollectPlayerInfoTask(ILoggerFactory loggerFactory, PlayerService playerService, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var baseballPlayersTask = RetrieveAndPersistPlayerData<BaseballPlayerItem>(loggerFactory, "baseball", playerService, cancellationToken);
            var basketballPlayersTask = RetrieveAndPersistPlayerData<BasketballPlayerItem>(loggerFactory, "basketball", playerService, cancellationToken);
            var footballPlayersTask = RetrieveAndPersistPlayerData<FootballPlayerItem>(loggerFactory, "football", playerService, cancellationToken);

            await Task.WhenAll(baseballPlayersTask, basketballPlayersTask, footballPlayersTask);

            await playerService.UpdateAverageAges(cancellationToken);

            // TODO: make this configurable. 
            await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
        }
    }

    private static async Task RetrieveAndPersistPlayerData<TResponseModel>(ILoggerFactory loggerFactory, string sport, PlayerService playerService, CancellationToken cancellationToken)
        where TResponseModel : class 
    {
        // Call the external API for the sport.
        var client = new HttpClient();
        var reqUrl = string.Format(CbsApiUrlPattern, sport);

        var response = await client.GetAsync(reqUrl, cancellationToken);
        var logger = loggerFactory.CreateLogger(nameof(RetrieveAndPersistPlayerData));

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Attempted to get {Sport} players ({Url}) but got status code: {StatusCode}, and skipped.", sport, reqUrl, response.StatusCode);
            return; 
        }

        try
        {
            var responseBodyStr = await response.Content.ReadAsStringAsync(cancellationToken);
            var deserializedPlayers = JsonConvert.DeserializeObject<PlayersResponse<TResponseModel>>(responseBodyStr);

            if (deserializedPlayers != null)
            {
                logger.LogInformation("Updating players in database...");

                await playerService.AddOrUpdatePlayers(
                    deserializedPlayers.Body.Players
                        .Select(p => p.ToPlayerEntity())
                        .Where(p => p != null)
                    , cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}