using Newtonsoft.Json;

namespace function_simulator.Models;

internal class PlayersResponse<TPlayerModel>
    where TPlayerModel : class 
{
    internal class Content
    {
        [JsonProperty("players")]
        public List<TPlayerModel> Players { get; set; }
    }

    [JsonProperty("statusMessage")]
    public string StatusMessage { get; set; }

    [JsonProperty("statusCode")]
    public int? StatusCode { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }

    [JsonProperty("body")]
    public Content Body { get; set; }

    [JsonProperty("uriAlias")]
    public string UriAlias { get; set; }
}
