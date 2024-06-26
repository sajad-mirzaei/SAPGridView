﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using WWWPGrids;

namespace AspDotNetCoreRazor.Pages.Examples.ClientSide;

public class ChangeData : PageModel
{
    private readonly ILogger<ChangeData> _logger;
    DateTime dtbAzTarikh = DateTime.Now.AddMonths(-20);
    DateTime dtbTaTarikh = DateTime.Now;
    public ChangeData(ILogger<ChangeData> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        SAPGridView oSGV = CreateFirstGrid("First Data");
        TempData["SAPGridView"] = oSGV.GridBind("Grid1");
    }

    public SAPGridView CreateFirstGrid(string firstColName, string cssClass = "text-dark")
    {
        SAPGridView oSGV = new();
        List<ChangeDataModel> dt = Get_DataTable1();
        //-----------------------Grid1------------------------
        oSGV.Grids["Grid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 300,
            GridTitle = "aaaa",
            Data = dt,
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

    public List<ChangeDataModel> Get_DataTable1()
    {
        //در صورتیکه اطلاعات را از دیتابیس فراخوانی میکنید، نیازی به این متد نیست
        List<ChangeDataModel> dt = new();

        for (int i = 1; i <= 20; i++)
        {
            ChangeDataModel row = new()
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

    public IActionResult OnPostChangeToFirst([FromBody] ChangeDataInputModel inputModel)
    {
        var oSGV = CreateFirstGrid("First Data");
        oSGV.Grids["Grid1"].Data = Get_DataTable1();
        return new JsonResult(oSGV.AjaxBind("Grid1"));
    }

    public IActionResult OnPostChangeToSecond([FromBody] ChangeDataInputModel inputModel)
    {
        var oSGV = CreateFirstGrid("Second Data", "text-primary");
        oSGV.Grids["Grid1"].Data = Get_DataTable1();
        return new JsonResult(oSGV.AjaxBind("Grid1"));
    }

}

public class ChangeDataModel
{
    public int Id { get; set; }
    public int Col1 { get; set; }
    public string Col2 { get; set; }
    public string Col3 { get; set; }
    public DateTime Tarikh { get; set; }
}

public class ChangeDataInputModel
{
    public int Param1 { get; set; }
    public double Param2 { get; set; }
    public string Param3 { get; set; }
    public ChangeDataParam4 Param4 { get; set; }
}

public class ChangeDataParam4
{
    public int Param41 { get; set; }
    public int Param42 { get; set; }
    public string Param43 { get; set; }
}