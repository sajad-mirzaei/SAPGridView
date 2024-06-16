using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WWWPGrids;

namespace AspDotNetCoreRazor.Pages.Examples.ClientSide;

public class AjaxLoad : PageModel
{
    private readonly ILogger<AjaxLoad> _logger;
    public AjaxLoad(ILogger<AjaxLoad> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }


    //public IActionResult OnPostSapGridEvent([FromBody] SAPGridEventInputModel inputs)
    public IActionResult OnPostLoadGrid1()
    {
        SAPGridView oSGV = LoadGrid();
        var r = new Dictionary<string, object>()
        {
            { "grids", oSGV.AjaxBind("MyGrid1") },
            { "level", "1" },
            { "firstTextTitle", "firstTextTitle" }
        };
        return new JsonResult(JsonConvert.SerializeObject(r));
    }

    protected SAPGridView LoadGrid()
    {
        List<AjaxLoadModel> dt = MakeList();
        SAPGridView oSGV = new();

        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Data = dt,
            GridTitle = "گزارش تست 1",
            Options = new Option() { DropDownFilterButton = true, TitleRowInExelExport = false },
            Columns = new List<Column>() {
                new Column { Data = "v1", Title = "vv1", DefaultContent = "vv2", Visible = false },
                new Column { Data = "a", Title = "aa",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Function.SectionValue.Tfoot }
                    }
                },
                new Column { Data = "v2", Title = "vv2", DefaultContent = "vv2", Visible = false },
                new Column { Data = "CumulativeTest1", Title = "CumulativeSum(aa)", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new CumulativeSum { Section = Function.SectionValue.Tbody, SourceField = "a" }
                    }
                },
                new Column { Data = "aplusOne", Title = "aplusOne", DefaultContent = "",
                    Functions = {
                        new Calc()
                        {
                            Section = Function.SectionValue.Tbody,
                            Formula = "a + 1"
                        }
                    }
                },
                new Column { Data = "b", Title = "bb",
                    Functions =
                    {
                        new TextFeature(){ Section = Function.SectionValue.Tbody, Condition = "1==1", IsTrueCssClass = "text-warning"}
                    }
                },
                new Column { Data = "c", Title = "cc",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum }
                    }
                },
                new Column { Data = "d", Title = "dd" },
                new Column { Data = "e", Title = "ee",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum }
                    }
                },
                new Column { Data = "f", Title = "ff",
                    Functions =
                    {
                        new TextFeature(){ Section = Function.SectionValue.Tbody, Condition = "1==1", IsTrueCssClass = "text-info"}
                    }
                },
                new Column { Data = "g", Title = "gg", DefaultContent = "1",
                    Functions =
                    {
                        new Calc()
                        {
                            Section = Function.SectionValue.Tbody,
                            Formula = "a + 1"
                        }
                    }
                }
            }
        };
        return oSGV;
    }

    public List<AjaxLoadModel> MakeList()
    {
        List<AjaxLoadModel> oDT = new();

        for (int i = 0; i < 100; i++)
        {
            AjaxLoadModel Row1 = new();
            Row1.a = i + 1000;
            Row1.b = "bb " + i.ToString();
            Row1.c = i + 1;

            if (i % 2 == 0)
                Row1.d = "آریا اکبری";
            else
                Row1.d = "آريا اكبري";

            if (i < 3) Row1.e = 3;
            else if (i < 6) Row1.e = 6;
            else if (i < 9) Row1.e = 9;
            else if (i < 12) Row1.e = 12;
            else if (i < 15) Row1.e = 15;
            else if (i < 18) Row1.e = 18;
            else if (i < 21) Row1.e = 21;
            else Row1.e = 22;


            if (i < 8) Row1.f = "ff1";
            else if (i < 16) Row1.f = "ff2";
            else if (i < 24) Row1.f = "ff3";
            else if (i < 32) Row1.f = "ff4";
            else if (i < 40) Row1.f = "ff5";
            else if (i < 48) Row1.f = "ff6";
            else if (i < 56) Row1.f = "ff7";
            else Row1.f = "ff8";

            oDT.Add(Row1);
        }
        return oDT;
    }
}

public class AjaxLoadModel
{
    public int a { get; set; }
    public string b { get; set; }
    public int c { get; set; }
    public string d { get; set; }
    public int e { get; set; }
    public string f { get; set; }
}