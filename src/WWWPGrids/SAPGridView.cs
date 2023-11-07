using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class SAPGridView
    {
        public Dictionary<string, string> DefaultParameters = new Dictionary<string, string>();
        public Dictionary<string, Grid> Grids = new Dictionary<string, Grid>();
        public object CustomData;
        public SAPGridView()
        {
            Grids = new Dictionary<string, Grid>();
            DefaultParameters = new Dictionary<string, string>();
        }
        public HtmlString GridBind(params string[] gridNameList)
        {
            Dictionary<string, Grid> gridsToBind = new Dictionary<string, Grid>();
            foreach (var GridName in gridNameList)
            {
                Grid g = Grids[GridName].DeepCopy();
                if (g.GridParameters.Count == 0)
                    g.GridParameters = new Dictionary<string, string>(DefaultParameters);
                gridsToBind.Add(GridName, g);
            }
            var AllData = new Dictionary<string, object>()
            {
                { "Grids", gridsToBind },
                { "CustomData", CustomData }
            };
            var JsonData = JsonConvert.SerializeObject(AllData);

            return new HtmlString("<script>SapGridViewJSBind(" + JsonData + ", 1, '1')</script>");
        }
        public string AjaxBind(params string[] gridNameList)
        {
            Dictionary<string, Grid> gridsToBind = new Dictionary<string, Grid>();
            foreach (var GridName in gridNameList)
            {
                Grid g = Grids[GridName].DeepCopy();
                if (g.GridParameters.Count == 0)
                    g.GridParameters = new Dictionary<string, string>(DefaultParameters);
                gridsToBind.Add(GridName, g);
            }
            var allData = new Dictionary<string, object>()
            {
                { "Grids", gridsToBind },
                { "CustomData", CustomData }
            };
            var allDataJson = JsonConvert.SerializeObject(allData);
            return allDataJson;
        }
    }
}