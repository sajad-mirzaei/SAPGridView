using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class OnClick : Function
    {
        [JsonProperty("httpRequestType")] public HttpRequestTypeValue HttpRequestType { get; set; }
        [JsonProperty("hrefLink")] public string HrefLink { get; set; }
        [JsonProperty("cssClass")] public string CssClass { get; set; }
        [JsonProperty("footerText")] public string FooterText { get; set; }
        [JsonProperty("enable")] public bool Enable { get; set; }
        [JsonProperty("webMethodName")] public string WebMethodName { get; set; }
        [JsonProperty("javaScriptMethodName")] public string JavaScriptMethodName { get; set; }
        /// <summary>yourTitle - {clickedItem} : انتخاب نام برای تب، یک کلمه یا رشته می پذیرد می توان بصورت ترکیبی کلمه - گزینه ای که کلیک شده تعریف کرد مثال </summary>
        [JsonProperty("nextTabTitle")] public string NextTabTitle { get; set; }
        public List<string> DataKeys { get; set; }
        public string NextGrid { get; set; }
        public string Level { get; set; }
        public enum HttpRequestTypeValue
        {
            /// <summary> بدون لود شدن دوباره کل صفحه </summary>
            Ajax,
            /// <summary> برای زمانیکه فقط یک لینک باید داشته باشد مثلا به صفحه ای دیگر </summary>
            PageLink,
            /// <summary> برای زمانیکه بعد از کلیک یک متد در جاوااسکریپت باید فراخوانی شود </summary>
            CallJavaScriptMethod
        }
        public OnClick()
        {
            HrefLink = "javascript:void(0)";
            FuncName = GetType().Name;
            Enable = true;
            WebMethodName = "SapGridEvent";
            HttpRequestType = HttpRequestTypeValue.Ajax;
            DataKeys = new List<string>();
            Section = SectionValue.Tbody;
            NextTabTitle = "{clickedItem}";
        }
        public override object DeepCopy()
        {
            OnClick o = (OnClick)MemberwiseClone();
            o.DataKeys = new List<string>(o.DataKeys);
            return o;
        }
    }
}