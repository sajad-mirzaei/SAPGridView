using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WWWPGrids;
using WWWPGrids.Models;

namespace AspDotNetCoreRazor.Pages.Examples.ServerSide;

public class SimpleServerSide : PageModel
{
    private readonly ILogger<SimpleServerSide> _logger;
    public SimpleServerSide(ILogger<SimpleServerSide> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        SAPGridView oSGV = CreateGrid();
        //oSGV.Grids["MyGrid1"].Data = MakeList();
        TempData["SAPGridView"] = oSGV.GridBind("MyGrid1");
    }
    public IActionResult OnPostSapGridServerSide([FromHeader] DatatablesFiltersModel filters)
    {
        var data = MakeList().OrderBy(c => c.id).ToList();
        List<SimpleServerSideModel> dt = data.OrderBy(c => c.id).Skip(filters.Start).Take(filters.Length).ToList();

        int cumulativeSumStartingNumber = data.OrderBy(c => c.id).Select(c => c.a)
            .Where(c => c < dt[0].a).Sum();
        dt[0].CumulativeTest1 += cumulativeSumStartingNumber;

        var oDatatablesModel = new DatatablesModel<SimpleServerSideModel>()
        {
            Draw = filters.Draw,
            RecordsFiltered = data.Count(),
            RecordsTotal = data.Count(),
            Data = dt
        };
        return new JsonResult(oDatatablesModel);
    }

    protected SAPGridView CreateGrid()
    {
        SAPGridView oSGV = new();

        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ServerSide = true,
            Processing = true,
            ContainerId = "MyGridId",
            ContainerHeight = 800,
            GridTitle = "گزارش تست 1",
            Options = new Option() { DropDownFilterButton = true, TitleRowInExelExport = false },
            Columns = new List<Column>() {
                //new Column { Data = "v1", Title = "vv1", DefaultContent = "vv2", Visible = false },
                new Column
                {
                    Title = "CheckBox",
                    Data = "CheckBox",
                    DefaultContent = "<input type='checkbox' class='checkbox' onclick='func1(this);' data-aaa='a'>"
                },
                new Column { Data = "a", Title = "aa",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Function.SectionValue.Tfoot },
                        new TextFeature()
                        {
                            Section = Function.SectionValue.Tbody,
                            Condition = "1==1",
                            IsTrueText = "<span class='text-danger' data-b1='b'>a</span>"
                        }
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
                new Column { Data = "v2", Title = "vv2", DefaultContent = "vv2", Visible = false },
                new Column { Data = "CumulativeTest1", Title = "CumulativeSum(aa)", CssClass = "ltr",
                    Functions = {
                        new CumulativeSum
                        {
                            Section = Function.SectionValue.Tbody,
                            SourceField = "a"
                        }
                    }
                },
                new Column { Data = "b", Title = "bb" },
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
                new Column
                {
                    Data = "f", Title = "ff",
                    Functions =
                    {
                        new TextFeature(){ Section = Function.SectionValue.Tbody, Condition = "1==1", IsTrueCssClass = "text-info"}
                    }
                },
                new Column
                {
                    Data = "g", Title = "gg",
                    Functions =
                    {
                        new TextFeature()
                        {
                            Section = Function.SectionValue.Tbody,
                            Condition = "1==1",
                            IsTrueText = "<progress value='g' max='100'></progress>"
                        }
                    }
                }
            }
        };
        return oSGV;
    }

    public List<SimpleServerSideModel> MakeList()
    {
        List<SimpleServerSideModel> oDT = new();

        for (int i = 1; i <= 100; i++)
        {
            SimpleServerSideModel Row1 = new();
            Row1.id = i;
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
            Row1.g = new Random().Next(30, 100);
            oDT.Add(Row1);
        }
        return oDT;
    }
}

public class SimpleServerSideModel
{
    public int id { get; set; }
    public int a { get; set; }
    public string b { get; set; }
    public int c { get; set; }
    public string d { get; set; }
    public int e { get; set; }
    public string f { get; set; }
    public int g { get; set; }
    public int CumulativeTest1 { get; set; } = 0;
}