using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class CustomizeButton
    {
        [JsonProperty("buttonName")] public ButtonNames ButtonName { get; set; }
        [JsonProperty("javascriptMethodName")] public string JavascriptMethodName { get; set; }
        [JsonProperty("data")] public Dictionary<string, string> Data { get; set; }
        public CustomizeButton()
        {
            Data = new Dictionary<string, string>();
        }
    }
}