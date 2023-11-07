using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class Calc : Function
    {
        [JsonProperty("formula")] public string Formula { get; set; }
        [JsonProperty("operator")] public OperatorValue Operator { get; set; }
        [JsonProperty("numericCheck")] public bool NumericCheck { get; set; }
        public enum OperatorValue
        {
            VerticalSum
        }
        public Calc()
        {
            Section = SectionValue.Tbody;
            FuncName = GetType().Name;
            NumericCheck = true;
        }
        public override object DeepCopy()
        {
            Calc c = (Calc)MemberwiseClone();
            return c;
        }
    }
}