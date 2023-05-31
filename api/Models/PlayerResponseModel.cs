using Newtonsoft.Json;

namespace api.Models
{
    /// <summary>
    /// Class that represents an entity for a player.
    /// </summary>
    public class PlayerResponseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the player.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name for this player.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name for this player.
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the position for this player.
        /// </summary>
        [JsonProperty("position")]
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name brief for this player.
        /// </summary>
        [JsonProperty("name_brief")]
        public string NameBrief { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the age for this player.
        /// </summary>
        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("average_position_age_diff")]
        public string? AveragePositionAgeDiff { get; set; }
    }
}
