using data.Entities;
using function_simulator.Models;

namespace function_simulator;

public static class ModelExtensions
{
    internal static Player ToPlayerEntity(this object model)
    {
        if (model is BaseballPlayerItem baseballPlayerModel)
        {
            if (string.IsNullOrWhiteSpace(baseballPlayerModel.Firstname) ||
                string.IsNullOrWhiteSpace(baseballPlayerModel.Lastname))
            {
                return null;
            }

            return new Player()
            {
                Id = int.Parse(baseballPlayerModel.Id),
                Age = baseballPlayerModel.Age,
                FirstName = baseballPlayerModel.Firstname,
                LastName = baseballPlayerModel.Lastname,
                Position = baseballPlayerModel.Position,
                NameBrief = $"{baseballPlayerModel.Firstname[0]}. {baseballPlayerModel.Lastname[0]}.",
                Sport = "baseball"
            };
        }

        if (model is BasketballPlayerItem basketballPlayerModel)
        {
            if (string.IsNullOrWhiteSpace(basketballPlayerModel.Firstname) ||
                string.IsNullOrWhiteSpace(basketballPlayerModel.Lastname))
            {
                return null;
            }

            return new Player()
            {
                Id = int.Parse(basketballPlayerModel.Id),
                Age = basketballPlayerModel.Age,
                FirstName = basketballPlayerModel.Firstname,
                LastName = basketballPlayerModel.Lastname,
                Position = basketballPlayerModel.Position,
                NameBrief = $"{basketballPlayerModel.Firstname} {basketballPlayerModel.Lastname[0]}.",
                Sport = "basketball"
            };
        }

        if (model is FootballPlayerItem footballPlayerModel)
        {
            if (string.IsNullOrWhiteSpace(footballPlayerModel.Firstname) ||
                string.IsNullOrWhiteSpace(footballPlayerModel.Lastname))
            {
                return null;
            }

            return new Player()
            {
                Id = int.Parse(footballPlayerModel.Id),
                Age = footballPlayerModel.Age,
                FirstName = footballPlayerModel.Firstname,
                LastName = footballPlayerModel.Lastname,
                Position = footballPlayerModel.Position,
                NameBrief = $"{footballPlayerModel.Firstname[0]}. {footballPlayerModel.Lastname}",
                Sport = "football"
            };
        }

        return null;
    }
}
