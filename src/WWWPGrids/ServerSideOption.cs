using Newtonsoft.Json;

namespace WWWPGrids;

public class ServerSideOption
{
    [JsonProperty("onPostMethodName")] public string OnPostMethodName { get; set; } = "SapGridServerSide";
}