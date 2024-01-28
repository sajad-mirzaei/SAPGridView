using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using WWWPGrids;
using WWWPGrids.Models;

namespace AspDotNetCoreRazor.Pages.Examples.ClientSide;

[IgnoreAntiforgeryToken]
public class NestedLevels : PageModel
{
    private readonly ILogger<NestedLevels> _logger;
    DateTime dtbAzTarikh = DateTime.Now.AddMonths(-20);
    DateTime dtbTaTarikh = DateTime.Now;
    public NestedLevels(ILogger<NestedLevels> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        SAPGridView oSGV = CreateFirstGrid();
        TempData["SAPGridView"] = oSGV.GridBind("Grid1");
    }

    public SAPGridView CreateFirstGrid()
    {
        SAPGridView oSGV = new();
        oSGV.DefaultParameters = new Dictionary<string, string>()
        {
            { "Level", "1" },
            { "AzTarikh", dtbAzTarikh.ToString() },
            { "TaTarikh", dtbTaTarikh.ToString() },
            { "Id", "" }
        };
        List<NestedLevelsL1Model> dt = Get_DataTable1();
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
                        new MiladiToJalali {Section = Function.SectionValue.Tbody, Output = MiladiToJalali.DateValue.FullDate },
                        new OnClick {
                            Section = Function.SectionValue.Tbody,
                            NextTabTitle = "Id - {clickedItem}",
                            Level = "2",
                            NextGrid = "Grid2",
                            DataKeys = { "Id" }
                        }
                    }
                },
                new Column { Title ="ستون 1", Data="Col1",
                    Functions =
                    {
                        new Calc {Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Function.SectionValue.Tbody, DecimalPlaces = 0 },
                        new Separator { Section = Function.SectionValue.Tfoot, DecimalPlaces = 0 }
                    }
                },
                new Column { Title ="ستون 2", Data="Col2" },
                new Column { Title ="ستون 3", Data="Col3" }
            }
        };
        return oSGV;
    }

    public List<NestedLevelsL1Model> Get_DataTable1()
    {
        //در صورتیکه اطلاعات را از دیتابیس فراخوانی میکنید، نیازی به این متد نیست
        List<NestedLevelsL1Model> dt = new();

        for (int i = 1; i <= 20; i++)
        {
            NestedLevelsL1Model row = new()
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

    public List<NestedLevelsL2Model> Get_DataTable2(Dictionary<string, string> param)
    {
        //در صورتیکه اطلاعات را از دیتابیس فراخوانی میکنید، نیازی به این متد نیست
        List<NestedLevelsL2Model> dt = new();

        for (int i = 1; i <= 20; i++)
        {
            NestedLevelsL2Model row = new()
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

    public IActionResult OnPostSapGridEvent([FromBody] SAPGridEventInputModel inputs)
    {
        var oSGV = CreateNextGrids();
        //--clicked row data-------------------------------------
        var rowData = inputs.RowData;
        List<string> dataKeys = inputs.FuncArray.DataKeys;
        string nextGrid = inputs.FuncArray.NextGrid;
        string clicked_CellName = inputs.TableDetails["CellName"];
        int level = int.Parse(inputs.FuncArray.Level);

        //--copy last grid parameters into new grid parameters---
        oSGV.Grids[nextGrid].GridParameters = new Dictionary<string, string>(inputs.GridParameters);

        //--change new grid parameters with new values-----------
        oSGV.Grids[nextGrid].GridParameters["Level"] = inputs.FuncArray.Level;
        foreach (string DataKey in dataKeys)
        {
            if (rowData.Count != 0)
                oSGV.Grids[nextGrid].GridParameters[DataKey] = rowData[DataKey];
        }
        oSGV.Grids[nextGrid].Data = Get_DataTable2(oSGV.Grids[nextGrid].GridParameters);
        return new JsonResult(oSGV.AjaxBind(nextGrid));
    }

    public SAPGridView CreateNextGrids()
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
                        new MiladiToJalali {Section = Function.SectionValue.Tbody, Output = MiladiToJalali.DateValue.FullDate }
                    }
                },
                new Column { Title ="مشخصه 1", Data="property1",
                    Functions =
                    {
                        new Calc {Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Function.SectionValue.Tbody, DecimalPlaces = 0 },
                        new Separator { Section = Function.SectionValue.Tfoot, DecimalPlaces = 0 }
                    }
                },
                new Column { Title ="مشخصه 2", Data="property2" },
                new Column { Title ="مشخصه 3", Data="property3" }
            }
        };
        return oSGV;
    }
}

public class NestedLevelsL1Model
{
    public int Id { get; set; }
    public int Col1 { get; set; }
    public string Col2 { get; set; }
    public string Col3 { get; set; }
    public DateTime Tarikh { get; set; }
}

public class NestedLevelsL2Model
{
    public int Id { get; set; }
    public DateTime Tarikh { get; set; }
    public int property1 { get; set; }
    public string property2 { get; set; }
    public string property3 { get; set; }
}