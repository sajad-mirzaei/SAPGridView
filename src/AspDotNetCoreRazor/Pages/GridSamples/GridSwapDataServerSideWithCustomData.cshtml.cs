using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;
using WWWPGrids;
using WWWPGrids.Models;

namespace AspDotNetCoreRazor.Pages.GridSamples;

[IgnoreAntiforgeryToken]
public class GridSwapDataServerSideWithCustomData : PageModel
{
    private readonly ILogger<GridSwapData1> _logger;
    DateTime dtbAzTarikh = DateTime.Now.AddMonths(-20);
    DateTime dtbTaTarikh = DateTime.Now;
    public GridSwapDataServerSideWithCustomData(ILogger<GridSwapData1> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        SAPGridView oSGV = CreateFirstGrid("First Data");
        TempData["SAPGridView"] = oSGV.GridBind("Grid1");
    }

    public IActionResult OnPostSapGridServerSide([FromHeader] DatatablesFiltersModel<string> filters)
    {
        var customData = JsonConvert.DeserializeObject<GridSwapDataServerSideWithCustomDataInputModel>(filters.CustomData);
        List<GridSwapDataServerSideWithCustomDataModel> data = Get_DataTable1();
        List<GridSwapDataServerSideWithCustomDataModel> dt = data
            .OrderBy(c => c.Tarikh)
            .Skip(filters.Start)
            .Take(filters.Length).ToList();

        var oDatatablesModel = new DatatablesModel<GridSwapDataServerSideWithCustomDataModel>()
        {
            Draw = filters.Draw,
            RecordsFiltered = data.Count(),
            RecordsTotal = data.Count(),
            Data = dt
        };
        return new JsonResult(oDatatablesModel);
    }

    public SAPGridView CreateFirstGrid(string firstColName, string cssClass = "text-dark")
    {
        SAPGridView oSGV = new();
        //-----------------------Grid1------------------------
        oSGV.Grids["Grid1"] = new Grid()
        {
            ServerSide = true,
            Processing = true,

            ContainerId = "MyGridId",
            ContainerHeight = 300,
            GridTitle = "aaaa",
            Options = new Option
            {
                DropDownFilterButton = true,
                ExcelButton = true,
                CopyButton = true,
                PrintButton = true,
                RecycleButton = true,
                ColumnsSearchButton = true,
                Info = true,
                GridSearchTextBox = true
            },
            Columns = new List<Column>() {
                new Column { Title ="شماره ثبت", Data="Id" },
                new Column
                {
                    Title = "جدول",
                    Data = "customCol",
                    DefaultContent = firstColName,
                    CssClass = cssClass
                },
                new Column { Title ="تاریخ", Data="Tarikh" },
                new Column { Title ="ستون 1", Data="Col1",
                    Functions =
                    {
                        new Calc {Section = Calc.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Separator.SectionValue.Tbody, DecimalPlaces = 0 },
                        new Separator { Section = Separator.SectionValue.Tfoot, DecimalPlaces = 0 }
                    }
                },
                new Column { Title ="ستون 2", Data="Col2" },
                new Column { Title ="ستون 3", Data="Col3" }
            }
        };
        return oSGV;
    }

    public List<GridSwapDataServerSideWithCustomDataModel> Get_DataTable1()
    {
        //در صورتیکه اطلاعات را از دیتابیس فراخوانی میکنید، نیازی به این متد نیست
        List<GridSwapDataServerSideWithCustomDataModel> dt = new();

        for (int i = 1; i <= 20; i++)
        {
            GridSwapDataServerSideWithCustomDataModel row = new()
            {
                Id = i,
                Col1 = i + 10000,
                Col2 = "Col2 - " + i,
                Col3 = "Col3 - " + i,
                Tarikh = DateTime.Now.AddMonths(-1 * i)
            };
            dt.Add(row);
        }
        var result = dt.Where(myRow => myRow.Tarikh >= dtbAzTarikh && myRow.Tarikh <= dtbTaTarikh).ToList();
        return result;
    }

    public IActionResult OnPostSwapToFirst([FromBody] GridSwapDataServerSideWithCustomDataInputModel inputModel)
    {
        var oSGV = CreateFirstGrid("First Data");
        oSGV.CustomData = inputModel;
        return new JsonResult(oSGV.AjaxBind("Grid1"));
    }

    public IActionResult OnPostSwapToSecond([FromBody] GridSwapDataServerSideWithCustomDataInputModel inputModel)
    {
        var oSGV = CreateFirstGrid("Second Data", "text-primary");
        oSGV.CustomData = inputModel;
        return new JsonResult(oSGV.AjaxBind("Grid1"));
    }

}

public class GridSwapDataServerSideWithCustomDataModel
{
    public int Id { get; set; }
    public int Col1 { get; set; }
    public string Col2 { get; set; }
    public string Col3 { get; set; }
    public DateTime Tarikh { get; set; }
}

public class GridSwapDataServerSideWithCustomDataInputModel
{
    public int Param1 { get; set; }
    public double Param2 { get; set; }
    public string Param3 { get; set; }
    public Param4GridSwapDataServerSideWithCustomDataInputModel Param4 { get; set; }
}

public class Param4GridSwapDataServerSideWithCustomDataInputModel
{
    public int Param41 { get; set; }
    public int Param42 { get; set; }
    public string Param43 { get; set; }
}