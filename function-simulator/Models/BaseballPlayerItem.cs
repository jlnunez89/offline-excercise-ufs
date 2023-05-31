using Newtonsoft.Json;

namespace function_simulator.Models;

internal class BaseballPlayerItem
{
    [JsonProperty("throws")]
    public string Throws { get; set; }

    [JsonProperty("position")]
    public string Position { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("elias_id")]
    public string EliasId { get; set; }

    [JsonProperty("firstname")]
    public string Firstname { get; set; }

    [JsonProperty("pro_status")]
    public string ProStatus { get; set; }

    [JsonProperty("fullname")]
    public string Fullname { get; set; }

    [JsonProperty("age")]
    public int? Age { get; set; }

    [JsonProperty("pro_team")]
    public string ProTeam { get; set; }

    [JsonProperty("lastname")]
    public string Lastname { get; set; }

    [JsonProperty("eligible_for_offense_and_defense")]
    public int? EligibleForOffenseAndDefense { get; set; }

    [JsonProperty("bats")]
    public string Bats { get; set; }

    [JsonProperty("photo")]
    public string Photo { get; set; }
}
