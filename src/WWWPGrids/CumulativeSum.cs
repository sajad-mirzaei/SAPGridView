using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class CumulativeSum : Function
    {
        [JsonProperty("sourceField")] public string SourceField { get; set; }
        public CumulativeSum()
        {
            FuncName = GetType().Name;
        }
        public override object DeepCopy()
        {
            CumulativeSum c = (CumulativeSum)MemberwiseClone();
            return c;
        }
    }
}