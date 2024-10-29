using Microsoft.AspNetCore.Mvc.RazorPages;
using WWWPGrids;

namespace AspDotNetCoreRazor.Pages.Examples.ClientSide;

public class HeaderComplex : PageModel
{
    private readonly ILogger<HeaderComplex> _logger;
    public HeaderComplex(ILogger<HeaderComplex> logger)
    {
        _logger = logger;
    }
    public void OnGet()
    {
        SAPGridView oSGV = DefineGrid();
        TempData["SAPGridView"] = oSGV.GridBind("MyGrid1");
    }

    public SAPGridView DefineGrid()
    {
        List<Dictionary<string, int>> oArrayTest = new List<Dictionary<string, int>>();
        for (int i = 0; i < 200; i++)
        {
            int c = i / 6;
            oArrayTest.Add(
                new Dictionary<string, int>() {
                    { "id", i },
                    { "a", i },
                    { "b", i + 1},
                    { "c", i },
                    { "d", i },
                    { "e", i },
                    { "f", i }
                }
            );
        }
        SAPGridView oSGV = new SAPGridView();
        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerHeight = 300,
            ContainerId = "MyGridId",
            Data = oArrayTest,
            Columns = new List<Column>() {
                new Column { Data = "a", Title = "A"},
                new Column { Data = "b", Title = "B" },
                new Column { Data = "c", Title = "C" },
                new Column { Data = "d", Title = "D" },
                new Column { Data = "e", Title = "E" },
                new Column { Data = "f", Title = "F" }
            }
        };
        oSGV.Grids["MyGrid1"].HeaderComplex = new List<HeaderComplexRow>() {
            new HeaderComplexRow { Title = "AB", ColumnsToBeMerged = { "a", "b" } },
            new HeaderComplexRow { Title = "EF", ColumnsToBeMerged = { "e", "f" } }
        };
        return oSGV;
    }
}