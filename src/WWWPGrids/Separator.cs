using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class Separator : Function
    {
        /// <summary> تعداد اعداد بعد از ممیز، پیشفرض 3 می باشد </summary>
        [JsonProperty("decimalPlaces")]
        public int DecimalPlaces
        {
            get
            {
                return MaximumFractionDigits;
            }
            set
            {
                MaximumFractionDigits = value;
            }
        }
        /// <summary> ماکزیمم تعداد اعداد بعد از ممیز، پیشفرض 3 می باشد </summary>
        [JsonProperty("minimumFractionDigits")] public int MinimumFractionDigits { get; set; }
        /// <summary> مینیمم تعداد اعداد بعد از ممیز، پیشفرض 0 می باشد </summary>
        [JsonProperty("maximumFractionDigits")] public int MaximumFractionDigits { get; set; }
        /// <summary> en-US فرمت مخصوص زبان، پیشفرض </summary>
        [JsonProperty("locales")] public string Locales { get; set; }

        public Separator()
        {
            FuncName = GetType().Name;
            Section = SectionValue.Tbody;
            DecimalPlaces = 3;
            MinimumFractionDigits = 0;
            Locales = "en-US";
        }
        public override object DeepCopy()
        {
            Separator c = (Separator)MemberwiseClone();
            return c;
        }
    }
}