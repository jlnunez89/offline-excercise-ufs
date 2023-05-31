using api.Extensions;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using services;
using services.Models.Querying;
using System.Threading;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> logger;

    private readonly PlayerService playerSvc;

    public PlayersController(ILoggerFactory loggerFactory, PlayerService playerService)
    {
        this.logger = loggerFactory.CreateLogger<PlayersController>();
        this.playerSvc = playerService;
    }

    [HttpGet]
    public async Task<IResult> ListPlayers(
        [FromQuery] string? sport,
        [FromQuery] string? lastNameStartsWith,
        [FromQuery] int? age,
        [FromQuery] string? ageRange,
        [FromQuery] string? position,
        CancellationToken cancellationToken)
    {
        var queryParams = new PlayerQueryParameters()
        {
            Sport = sport,
            LastNameStartsWith = lastNameStartsWith,
            Age = age,
            AgeRange = ageRange,
            Position = position
        };

        // TODO: cache this somewhere? depends on how critical it is to get latest info.
        var avgPositionAges = await this.playerSvc.GetAverageAgesPerPosition(cancellationToken);

        var players = await this.playerSvc.ListPlayers(queryParams, cancellationToken);

        return Results.Ok(players.Select(p => p.ToResponseModel(avgPositionAges)).ToList());
    }

    [HttpGet("{playerId}")]
    public async Task<IResult> GetPlayer([FromRoute] string playerId, CancellationToken cancellationToken)
    {
        if (!int.TryParse(playerId, out var id))
        {
            return Results.BadRequest("Invalid player id.");
        }

        // TODO: cache this somewhere? depends on how critical it is to get latest info.
        var avgPositionAges = await this.playerSvc.GetAverageAgesPerPosition(cancellationToken);

        var player = await this.playerSvc.GetPlayerById(id, cancellationToken);

        if (player == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(player.ToResponseModel(avgPositionAges));
    }
}