using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class TextFeature : Function
    {
        [JsonProperty("isTrueText")] public string IsTrueText { get; set; }
        [JsonProperty("isFalseText")] public string IsFalseText { get; set; }
        [JsonProperty("condition")] public string Condition { get; set; }
        [JsonProperty("isTrueCssClass")] public string IsTrueCssClass { get; set; }
        [JsonProperty("isFalseCssClass")] public string IsFalseCssClass { get; set; }
        [JsonProperty("strReplace")] public Dictionary<string, string> StrReplace { get; set; }
        [JsonProperty("numericCheckInText")] public bool NumericCheckInText { get; set; }
        [JsonProperty("numericCheckInCondition")] public bool NumericCheckInCondition { get; set; }
        [JsonProperty("changeOriginalData")] public bool ChangeOriginalData { get; set; }
        public TextFeature()
        {
            StrReplace = new Dictionary<string, string>();
            FuncName = GetType().Name;
            NumericCheckInText = true;
            NumericCheckInCondition = true;
            ChangeOriginalData = false;
        }
        public override object DeepCopy()
        {
            TextFeature c = (TextFeature)MemberwiseClone();
            return c;
        }
    }
}