using api.Models;
using data.Entities;

namespace api.Extensions
{
    internal static class ResponseModelExtensions
    {
        internal static PlayerResponseModel ToResponseModel(this Player entity, Dictionary<(string Sport, string Position), decimal> averageAgePerPosition)
        {
            averageAgePerPosition.TryGetValue((entity.Sport, entity.Position), out decimal avgAgeForPosition);

            return new PlayerResponseModel
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                NameBrief = entity.NameBrief,
                Age = entity.Age,
                Position = entity.Position,
                AveragePositionAgeDiff = (entity.Age == null || averageAgePerPosition == null) ? null : ((decimal)entity.Age - avgAgeForPosition).ToString(),
            };
        }
    }
}
