using data;
using data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using services.Models.Querying;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace services
{
    public class PlayerService
    {
        private readonly ILogger<PlayerService> logger;
        private readonly Func<DatabaseContext> dbContextGenerationFunc;

        public PlayerService(ILoggerFactory loggerFactory, Func<DatabaseContext> dbCtxFunc)
        {
            this.logger = loggerFactory.CreateLogger<PlayerService>();
            this.dbContextGenerationFunc = dbCtxFunc;
        }

        public async Task AddOrUpdatePlayers(IEnumerable<Player> players, CancellationToken cancellationToken)
        {
            try
            {
                using var ctx = this.dbContextGenerationFunc();

                var existingPlayerIds = ctx.Players.Select(p => p.Id).ToHashSet();

                foreach (var player in players)
                {
                    if (existingPlayerIds.Contains(player.Id))
                    {
                        ctx.Update(player);
                    }
                    else
                    {
                        ctx.Players.Add(player);
                    }
                }

                await ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                this.logger.LogError(ex.Message);
            }
        }

        public async Task<Dictionary<(string Sport, string Position), decimal>> GetAverageAgesPerPosition(CancellationToken cancellationToken)
        {
            using var ctx = this.dbContextGenerationFunc();

            return await ctx.AverageAges.ToDictionaryAsync(a => (a.Sport, a.Position), a => a.Age, cancellationToken);
        }

        public async Task<Player?> GetPlayerById(int playerId, CancellationToken cancellationToken)
        {
            using var ctx = this.dbContextGenerationFunc();

            return await ctx.Players.FirstOrDefaultAsync(p => p.Id == playerId, cancellationToken);
        }

        public async Task<IList<Player>> ListPlayers(PlayerQueryParameters queryParams, CancellationToken cancellationToken)
        {
            using var ctx = this.dbContextGenerationFunc();

            var query = ctx.Players as IQueryable<Player>;

            if (!string.IsNullOrWhiteSpace(queryParams?.Sport))
            {
                query = query.Where(p => queryParams.Sport.Equals(p.Sport));
            }
            
            if (!string.IsNullOrWhiteSpace(queryParams?.LastNameStartsWith))
            {
                query = query.Where(p => p.LastName.StartsWith(queryParams.LastNameStartsWith));
            }

            if (!string.IsNullOrWhiteSpace(queryParams?.Position))
            {
                query = query.Where(p => queryParams.Position.Equals(p.Position));
            }

            if (!string.IsNullOrWhiteSpace(queryParams?.AgeRange))
            {
                Match match = Regex.Match(queryParams.AgeRange, @"^([0-9]+)\:([0-9]+)$");

                if (match.Success)
                {
                    var fromAgeInclusive = int.Parse(match.Groups[1].Value);
                    var toAgeInclusive = int.Parse(match.Groups[2].Value);

                    query = query.Where(p => p.Age >= fromAgeInclusive && p.Age <= toAgeInclusive);
                }
            }

            if (queryParams?.Age != null)
            {
                query = query.Where(p => p.Age == queryParams.Age);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task UpdateAverageAges(CancellationToken cancellationToken)
        {
            using var ctx = this.dbContextGenerationFunc();

            var existingGroups = ctx.AverageAges.Select(a => $"{a.Sport}:{a.Position}").ToHashSet();
            var groups = ctx.Players.Where(p => p.Age != null).GroupBy(p => new { p.Sport, p.Position }, p => (decimal)p.Age!);

            foreach (var group in groups)
            {
                var averageAgeRecord = new AverageAge
                {
                    Sport = group.Key.Sport,
                    Position = group.Key.Position,
                    Age = group.Average(),
                };

                if (existingGroups.Contains($"{group.Key.Sport}:{group.Key.Position}"))
                {
                    ctx.AverageAges.Update(averageAgeRecord);
                }
                else 
                {
                    ctx.Add(averageAgeRecord);
                }
            }

            await ctx.SaveChangesAsync(cancellationToken);
        }
    }
}