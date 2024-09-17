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
        /// <summary> نام متد جاوااسکریپتی خود را بدهید تا بعد از هر رخداد اطلاعات مربوط به آن به متد شما ارسال شود  </summary>
        [JsonProperty("javascriptMethodName")] public string JavascriptMethodName { get; set; }
        /// <summary> در صورتیکه نام یک ستون را به این پارامتر بدهید جمع و تعداد سطرهایی که انتخاب شده اند ارسال می شود، در صورتیکه یک فرمول از تعدادی ستون بدهید ابتدا فرمول شما حساب می شود سپس جمع و تعداد سطرهایی که انتخاب شده ارسال می شود </summary>
        [JsonProperty("sumFormula")] public string SumFormula { get; set; }
        [JsonProperty("selectAll")] public bool SelectAll { get; set; }
        public SAPCheckBox()
        {
            Section = SectionValue.Tbody;
            CssClass = "sapCheckBox";
            FuncName = this.GetType().Name;
            Enable = true;
            JavascriptMethodName = "sapJavascriptMethodName";
            SelectAll = true;
        }
        public override object DeepCopy()
        {
            SAPCheckBox c = (SAPCheckBox)MemberwiseClone();
            return c;
        }
    }
}