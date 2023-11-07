using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class SAPCheckBox : Function
    {
        [JsonProperty("cssClass")] public string CssClass { get; set; }
        [JsonProperty("enable")] public bool Enable { get; set; }
        public SAPCheckBox()
        {
            Section = SectionValue.Tbody;
            FuncName = GetType().Name;
            Enable = true;
        }
        public override object DeepCopy()
        {
            SAPCheckBox c = (SAPCheckBox)MemberwiseClone();
            return c;
        }
    }
}