using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using WWWPGrids;

namespace AspDotNetCoreRazor.Pages.GridSamples;

[IgnoreAntiforgeryToken]
public class GridLevel1 : PageModel
{
    private readonly ILogger<GridLevel1> _logger;

    public GridLevel1(ILogger<GridLevel1> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        DateTime dtbAzTarikh = DateTime.Now.AddMonths(-10);
        DateTime dtbTaTarikh = DateTime.Now;
        SAPGridView oSGV = CreateFirstGrid(dtbAzTarikh, dtbTaTarikh);
        TempData["SAPGridView"] = oSGV.GridBind("Grid1");
    }

    public SAPGridView CreateFirstGrid(DateTime dtbAzTarikh, DateTime dtbTaTarikh)
    {
        SAPGridView oSGV = new();
        oSGV.DefaultParameters = new Dictionary<string, string>()
        {
            { "Level", "1" },
            { "AzTarikh", dtbAzTarikh.ToString() },
            { "TaTarikh", dtbTaTarikh.ToString() },
            { "Id", "" }
        };
        List<GridLevel1L1Model> dt = Get_DataTable1(oSGV.DefaultParameters);
        //-----------------------Grid1------------------------
        oSGV.Grids["Grid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 300,
            GridTitle = "aaaa",
            Data = dt,
            GridParameters = new Dictionary<string, string>(oSGV.DefaultParameters),
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
                new Column { Title ="تاریخ", Data="Tarikh",
                    Functions =
                    {
                        new MiladiToJalali {Section = Calc.SectionValue.Tbody, Output = MiladiToJalali.DateValue.FullDate },
                        new OnClick {
                            Section = WWWPGrids.Function.SectionValue.Tbody,
                            NextTabTitle = "Tarikh - {clickedItem}",
                            Level = "2",
                            NextGrid = "Grid2",
                            DataKeys = { "Id" }
                        }
                    }
                },
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

    public List<GridLevel1L1Model> Get_DataTable1(Dictionary<string, string> param)
    {
        //در صورتیکه اطلاعات را از دیتابیس فراخوانی میکنید، نیازی به این متد نیست
        List<GridLevel1L1Model> dt = new();

        for (int i = 1; i <= 20; i++)
        {
            GridLevel1L1Model row = new()
            {
                Id = i,
                Col1 = i + 10000,
                Col2 = "Col2 - " + i,
                Col3 = "Col3 - " + i,
                Tarikh = DateTime.Now.AddMonths(-1 * i)
            };
            dt.Add(row);
        }
        var result = dt.Where(myRow => myRow.Tarikh >= DateTime.Parse(param["AzTarikh"]) && myRow.Tarikh <= DateTime.Parse(param["TaTarikh"]))
                    .ToList();

        return result;
    }

    public static List<GridLevel1L2Model> Get_DataTable2(Dictionary<string, string> param)
    {
        //در صورتیکه اطلاعات را از دیتابیس فراخوانی میکنید، نیازی به این متد نیست
        List<GridLevel1L2Model> dt = new();

        for (int i = 1; i <= 20; i++)
        {
            GridLevel1L2Model row = new()
            {
                Id = i,
                property1 = i + 10000 * int.Parse(param["Id"]),
                property2 = "col" + param["Id"] + " - property2 - " + i,
                property3 = "col" + param["Id"] + " - property3 - " + i,
                Tarikh = DateTime.Now.AddMonths(-1 * i)
            };
            dt.Add(row);
        }
        var result = dt
                    .Where(myRow => myRow.Tarikh >= DateTime.Parse(param["AzTarikh"]) && myRow.Tarikh <= DateTime.Parse(param["TaTarikh"]))
                    .ToList();

        return dt;
    }

    public IActionResult OnPostSapGridEvent([FromBody] SAPGridCallBackEvent oData)
    {
        var oSGV = CreateStaticGrids();
        //--clicked row data-------------------------------------
        var RowData = oData.RowData;
        List<string> DataKeys = oData.FuncArray.DataKeys;
        string NextGrid = oData.FuncArray.NextGrid;
        string Clicked_CellName = oData.TableDetails["CellName"];
        int Level = int.Parse(oData.FuncArray.Level);

        //--copy last grid parameters into new grid parameters---
        oSGV.Grids[NextGrid].GridParameters = new Dictionary<string, string>(oData.GridParameters);

        //--change new grid parameters with new values-----------
        oSGV.Grids[NextGrid].GridParameters["Level"] = oData.FuncArray.Level;
        foreach (string DataKey in DataKeys)
        {
            if (RowData.Count != 0)
                oSGV.Grids[NextGrid].GridParameters[DataKey] = RowData[DataKey];
        }
        oSGV.Grids[NextGrid].Data = Get_DataTable2(oSGV.Grids[NextGrid].GridParameters);
        return new JsonResult(oSGV.AjaxBind(NextGrid));
    }



    public static SAPGridView CreateStaticGrids()
    {
        SAPGridView oSGV = new();
        //-----------------------Grid2------------------------
        oSGV.Grids["Grid2"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 300,
            Columns = new List<Column>() {
                new Column { Title ="شماره ثبت", Data="Id" },
                new Column { Title ="تاریخ", Data="Tarikh",
                    Functions =
                    {
                        new MiladiToJalali {Section = Calc.SectionValue.Tbody, Output = MiladiToJalali.DateValue.FullDate }
                    }
                },
                new Column { Title ="مشخصه 1", Data="property1",
                    Functions =
                    {
                        new Calc {Section = Calc.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Separator.SectionValue.Tbody, DecimalPlaces = 0 },
                        new Separator { Section = Separator.SectionValue.Tfoot, DecimalPlaces = 0 }
                    }
                },
                new Column { Title ="مشخصه 2", Data="property2" },
                new Column { Title ="مشخصه 3", Data="property3" }
            }
        };
        return oSGV;
    }
}

public class GridLevel1L1Model
{
    public int Id { get; set; }
    public int Col1 { get; set; }
    public string Col2 { get; set; }
    public string Col3 { get; set; }
    public DateTime Tarikh { get; set; }
}

public class GridLevel1L2Model
{
    public int Id { get; set; }
    public DateTime Tarikh { get; set; }
    public int property1 { get; set; }
    public string property2 { get; set; }
    public string property3 { get; set; }
}