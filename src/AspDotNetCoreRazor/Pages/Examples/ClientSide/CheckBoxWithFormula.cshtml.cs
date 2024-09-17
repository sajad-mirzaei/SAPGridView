using Microsoft.AspNetCore.Mvc.RazorPages;
using WWWPGrids;

namespace AspDotNetCoreRazor.Pages.Examples.ClientSide;

public class CheckBoxWithFormula : PageModel
{
    public void OnGet()
    {
        SAPGridView oSGV = new SAPGridView();
        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Data = FakeList(),
            CounterColumn = false,
            Columns = new List<Column>() {
                new Column {
                    Data = "checkBoxTest", //Must be uniquely defined
                    Title = "checkBoxTest", //Must be uniquely defined
                    DefaultContent = "",
                    Orderable = false,
                    Width = "20",
                    CssClass = "text-center",
                    Functions = {
                        new SAPCheckBox()
                        {
                            Section = Function.SectionValue.Tbody,
                            JavascriptMethodName = "myJavascriptMethodName",
                            SumFormula = "(price * 2) - 1"
                        }
                    }
                },
                new Column { Data = "id", Title = "Id" },
                new Column { Data = "price", Title = "Price" },
                new Column { Data = "x2", Title = "Name 2" },
                new Column { Data = "x3", Title = "Name 3" },
                new Column { Data = "x4", Title = "Name 4" }
            }
        };

        TempData["SAPGridView"] = oSGV.GridBind("MyGrid1");
    }

    public List<CheckBoxWithFormulaModel> FakeList()
    {
        List<CheckBoxWithFormulaModel> oData = new List<CheckBoxWithFormulaModel>();
        for (int i = 1; i <= 100; i++)
        {
            CheckBoxWithFormulaModel row = new CheckBoxWithFormulaModel();
            row.id = i;
            row.price = i * 10;
            row.x2 = "Name1 " + i + 1;
            row.x3 = "Name2 " + i + 2;
            row.x4 = "Name3 " + i + 3;
            oData.Add(row);
        }
        return oData;
    }

    public class CheckBoxWithFormulaModel
    {
        public int id { get; set; }
        public decimal price { get; set; }
        public string x1 { get; set; }
        public string x2 { get; set; }
        public string x3 { get; set; }
        public string x4 { get; set; }
    }
}