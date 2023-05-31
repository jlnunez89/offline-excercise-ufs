using Newtonsoft.Json;

namespace function_simulator.Models;

internal class BasketballPlayerItem
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("pro_status")]
    public string ProStatus { get; set; }

    [JsonProperty("lastname")]
    public string Lastname { get; set; }

    [JsonProperty("position")]
    public string Position { get; set; }

    [JsonProperty("elias_id")]
    public string EliasId { get; set; }

    [JsonProperty("firstname")]
    public string Firstname { get; set; }

    [JsonProperty("eligible_for_offense_and_defense")]
    public int? EligibleForOffenseAndDefense { get; set; }

    [JsonProperty("pro_team")]
    public string ProTeam { get; set; }

    [JsonProperty("age")]
    public int? Age { get; set; }

    [JsonProperty("jersey")]
    public string Jersey { get; set; }

    [JsonProperty("photo")]
    public string Photo { get; set; }

    [JsonProperty("fullname")]
    public string Fullname { get; set; }
}
