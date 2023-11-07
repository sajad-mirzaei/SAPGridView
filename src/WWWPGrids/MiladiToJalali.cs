using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class MiladiToJalali : Function
    {
        [JsonProperty("outPut")] public DateValue Output { get; set; }
        [JsonProperty("zeroPad")] public bool ZeroPad { get; set; }
        public enum DateValue
        {

            /// <summary> فقط زمان را نشان می دهد </summary>
            TimeOnly,
            /// <summary> فقط تاریخ را نشان می دهد </summary>
            DateOnly,
            /// <summary> تاریخ و زمان را نشان می دهد </summary>
            FullDate,
            /// <summary> فقط زمان را با ثانیه نشان می دهد </summary>
            TimeOnlyWithSecond,
            /// <summary> تاریخ و زمان را با ثانیه نشان می دهد  </summary>
            FullDateWithSecond
        }
        public MiladiToJalali()
        {
            Section = SectionValue.Tbody;
            FuncName = GetType().Name;
            Output = DateValue.DateOnly;
            ZeroPad = true;
        }
        public override object DeepCopy()
        {
            MiladiToJalali c = (MiladiToJalali)MemberwiseClone();
            return c;
        }
    }
}