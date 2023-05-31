using data;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using services;
using System.Configuration;

namespace api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var dbConnectionStr = this.Configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(dbConnectionStr))
        {
            throw new ApplicationException("A connection string for the database must be defined.");
        }

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConfiguration(this.Configuration.GetSection("Logging"));
            builder.AddConsole();
        });

        services.AddSingleton(loggerFactory);
        services.AddTransient<Func<DatabaseContext>>((services) => services.GetRequiredService<DatabaseContext>);
        services.AddSingleton<PlayerService>();

        // TODO: I would send this to it's own separate provider library, but for simplicit I'm going to
        // bridge this here.
        services.AddEntityFrameworkMySQL()
                .AddDbContext<DatabaseContext>(options => options.UseMySQL(dbConnectionStr), ServiceLifetime.Transient);

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = string.Empty;
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MY API");
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}
