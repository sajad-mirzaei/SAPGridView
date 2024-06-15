using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WWWPGrids;
using WWWPGrids.Models;

namespace AspDotNetCoreRazor.Pages.Examples.ClientAndServerSide;

[IgnoreAntiforgeryToken]
public class NestedGridsWithServerSide : PageModel
{
    private readonly ILogger<NestedGridsWithServerSide> _logger;
    public NestedGridsWithServerSide(ILogger<NestedGridsWithServerSide> logger)
    {
        _logger = logger;
    }
    public void OnGet()
    {
        var oSGV = CreatePersonInfoGrid();
        var x = oSGV.GridBind("PersonInfo");
        TempData["SAPGridView"] = x;
    }
    public SAPGridView CreatePersonInfoGrid()
    {
        SAPGridView oSGV = new SAPGridView();
        oSGV.DefaultParameters = new Dictionary<string, string>()
        {
            { "Level", "1" }
        };
        //-----------------------Grid1------------------------
        oSGV.Grids["PersonInfo"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 300,
            GridTitle = "GridTitle Test",
            Data = MockDataWithServerSide.GetPersonInfo(),
            Columns = new List<Column>() {
                new Column { Title ="id", Data = "Id" },
                new Column { Title ="First Name", Data = "FirstName" },
                new Column { Title ="Last Name", Data = "LastName" },
                new Column { Title ="Age", Data = "Age" },
                new Column
                {
                    Title ="Academic Records",
                    Data = "AcademicRecords",
                    DefaultContent = "<i class='fa fa-list-alt'></i>",
                    CssClass = "text-center",
                    Functions =
                    {
                        new OnClick()
                        {
                            Section = Function.SectionValue.Tbody,
                            NextGrid = "AcademicRecords",
                            Level = "2",
                            DataKeys = { "Id" },
                            NextTabTitle = "FirstName LastName Academic Records"
                        }
                    }
                }
            }
        };
        return oSGV;
    }
    public static Grid CreateNextGrids(string nextGrid, Dictionary<string, string> gridParameters, bool isSum)
    {
        SAPGridView oSGV = new SAPGridView();
        //-----------------------AcademicRecords Grid------------------------
        oSGV.Grids["AcademicRecords"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 300,
            GridTitle = "GridTitle Test 2",
            Columns = new List<Column>() {
                new Column {
                    Title ="id",
                    Data = "Id",
                    Functions =
                    {
                        new OnClick()
                        {
                            Section = Function.SectionValue.Tfoot,
                            NextGrid = "AcademicRecordDetails",
                            Level = "3",
                            NextTabTitle = "Degree Major Academic Records Details",
                            FooterText = "Sum"
                        }
                    }
                },
                new Column {
                    Title ="id + 1",
                    Data = "Id1",
                    DefaultContent = "",
                    Functions =
                    {
                        new Calc()
                        {
                            Section = Function.SectionValue.Tbody,
                            Formula = "(Id + 1) * 1000"
                        },
                        new Calc()
                        {
                            Section = Function.SectionValue.Tfoot,
                            Operator = Calc.OperatorValue.VerticalSum
                        },
                        new OnClick()
                        {
                            Section = Function.SectionValue.Tfoot,
                            NextGrid = "AcademicRecordDetails",
                            Level = "3",
                            NextTabTitle = "Degree Major Academic Records Details"
                        },
                        new Separator()
                        {
                            Section = Function.SectionValue.Tfoot,
                            DecimalPlaces = 1
                        },
                        new TextFeature()
                        {
                            Section = Function.SectionValue.Tfoot,
                            Condition = "1==1",
                            IsTrueText = "(Id1)"
                        }
                    }
                },
                new Column
                {
                    Title ="id + 2",
                    Data = "id2",
                    DefaultContent = "",
                    Functions =
                    {
                        new Calc()
                        {
                            Section = Function.SectionValue.Tbody,
                            Formula = "(Id + 2) * 1000"
                        },
                        new Calc()
                        {
                            Section = Function.SectionValue.Tfoot,
                            Operator = Calc.OperatorValue.VerticalSum
                        }
                    }
                },
                new Column
                {
                    Title ="id + 3",
                    Data = "id3",
                    DefaultContent = "",
                    Functions =
                    {
                        new Calc()
                        {
                            Section = Function.SectionValue.Tbody,
                            Formula = "(Id + 3) * 1000"
                        },
                        new Calc()
                        {
                            Section = Function.SectionValue.Tfoot,
                            Operator = Calc.OperatorValue.VerticalSum
                        },
                        new Separator()
                        {
                            Section = Function.SectionValue.Tfoot,
                            DecimalPlaces = 2
                        }
                    }
                },
                new Column { Title ="Degree", Data = "Degree" },
                new Column { Title ="Major", Data = "Major" },
                new Column { Title ="Institute", Data = "Institute" },
                new Column
                {
                    Title ="Academic Record Details",
                    Data = "AcademicRecordDetails",
                    DefaultContent = "<i class='fa fa-list-alt'></i>",
                    Functions =
                    {
                        new OnClick()
                        {
                            Section = Function.SectionValue.Tbody,
                            NextGrid = "AcademicRecordDetails",
                            Level = "3",
                            DataKeys = { "Id" },
                            NextTabTitle = "Degree Major Academic Records Details"
                        }
                    }
                }
            }
        };
        //-----------------------AcademicRecordDetails Grid------------------------
        oSGV.Grids["AcademicRecordDetails"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 300,
            GridTitle = "GridTitle Test 3",
            ServerSide = true,
            Processing = true,
            Columns = new List<Column>() {
                new Column { Title ="id", Data = "Id" },
                new Column { Title ="Start Date", Data = "StartDate" },
                new Column { Title ="End Date", Data = "EndDate" },
                new Column { Title ="Average", Data = "Average" },
                new Column { Title ="Academic Records Id", Data = "AcademicRecordsId" }
            }
        };

        var grid = oSGV.Grids[nextGrid];
        grid.Data = GetDataByFilter(nextGrid, gridParameters, isSum);
        return grid;
    }
    public IActionResult OnPostSapGridServerSide([FromHeader] DatatablesFiltersModel filters)
    {
        var data = MockDataWithServerSide.GetAcademicRecordDetails();
        //var data = MakeList().OrderBy(c => c.id).ToList();
        var dt = data
            .OrderBy(c => c.Id)
            .Skip(filters.Start)
            .Take(filters.Length)
            .ToList();

        var oDatatablesModel = new DatatablesModel<MockDataWithServerSide.AcademicRecordDetails>()
        {
            Draw = filters.Draw,
            RecordsFiltered = data.Count(),
            RecordsTotal = data.Count(),
            Data = dt
        };
        return new JsonResult(oDatatablesModel);
        //return new JsonResult(new EmptyResult());
    }
    public static object GetDataByFilter(string nextGrid, Dictionary<string, string> gridParameters, bool isSum)
    {
        object d;
        if (nextGrid == "AcademicRecords")
        {
            d = MockDataWithServerSide.GetAcademicRecords().Where(c => c.PersonInfoId == int.Parse(gridParameters["Id"]));
        }
        else
        {
            if (isSum)
            {
                var academicRecordsIds = MockDataWithServerSide.GetAcademicRecords()
                    .Where(c => c.PersonInfoId == int.Parse(gridParameters["Id"]))
                    .Select(c => c.Id);
                d = MockDataWithServerSide.GetAcademicRecordDetails().Where(c => academicRecordsIds.Contains(c.AcademicRecordsId));
            }
            else
                d = MockDataWithServerSide.GetAcademicRecordDetails().Where(c => c.AcademicRecordsId == int.Parse(gridParameters["Id"]));
        }
        return d;
    }
    public IActionResult OnPostSapGridEvent([FromBody] SAPGridEventInputModel inputs)
    {
        //--clicked row data-------------------------------------
        var rowData = inputs.RowData;
        List<string> dataKeys = inputs.FuncArray.DataKeys;
        string nextGrid = inputs.FuncArray.NextGrid;
        string clicked_CellName = inputs.TableDetails["CellName"];
        int level = int.Parse(inputs.FuncArray.Level);

        var filters = new Dictionary<string, string>(inputs.GridParameters);
        foreach (string dataKey in dataKeys)
        {
            if (rowData.Count != 0)
                filters[dataKey] = rowData[dataKey];
        }

        var oSGV = new SAPGridView();
        oSGV.Grids[nextGrid] = CreateNextGrids(nextGrid, filters, rowData.Count == 0);
        oSGV.Grids[nextGrid].GridParameters = filters;
        oSGV.Grids[nextGrid].GridParameters["Level"] = inputs.FuncArray.Level;
        if (nextGrid == "AcademicRecordDetails")
            oSGV.Grids[nextGrid].Data = null;

        return new JsonResult(oSGV.AjaxBind(nextGrid));
    }

}

public static class MockDataWithServerSide
{
    public class PersonInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
    }
    public class AcademicRecords
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Major { get; set; }
        public string Institute { get; set; }
        public int PersonInfoId { get; set; }
    }
    public class AcademicRecordDetails
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Average { get; set; }
        public int AcademicRecordsId { get; set; }
    }

    public static List<PersonInfo> GetPersonInfo()
    {
        return new List<PersonInfo>
        {
            new PersonInfo { Id = 1, FirstName = "John", LastName = "Doe", Age = "30" },
            new PersonInfo { Id = 2, FirstName = "Jane", LastName = "Smith", Age = "28" },
            new PersonInfo { Id = 3, FirstName = "Alice", LastName = "Johnson", Age = "32" }
        };
    }

    public static List<AcademicRecords> GetAcademicRecords()
    {
        return new List<AcademicRecords>
        {
            new AcademicRecords
                { Id = 1, Degree = "B.Sc.", Major = "Computer Science", Institute = "MIT", PersonInfoId = 1 },
            new AcademicRecords
                { Id = 2, Degree = "M.Sc.", Major = "Computer Science", Institute = "Stanford", PersonInfoId = 1 },
            new AcademicRecords
                { Id = 3, Degree = "B.A.", Major = "English Literature", Institute = "Harvard", PersonInfoId = 2 },
            new AcademicRecords
                { Id = 4, Degree = "M.A.", Major = "English Literature", Institute = "Yale", PersonInfoId = 2 },
            new AcademicRecords
                { Id = 5, Degree = "B.Sc.", Major = "Physics", Institute = "Caltech", PersonInfoId = 3 },
            new AcademicRecords
                { Id = 6, Degree = "Ph.D.", Major = "Physics", Institute = "UC Berkeley", PersonInfoId = 3 }
        };
    }
    public static List<AcademicRecordDetails> GetAcademicRecordDetails()
    {
        var d = new List<AcademicRecordDetails>();
        var id = 1;
        var sal = 14;
        for (int i = 1; i <= 6; i++)
        {
            for (int j = 1; j <= 30; j++)
            {
                var ard = new AcademicRecordDetails
                {
                    Id = id++,
                    StartDate = "2010-01-" + j,
                    EndDate = "2014-02-01",
                    Average = "20",
                    AcademicRecordsId = i
                };
                d.Add(ard);
            }
        }
        return d;
        /*return new List<AcademicRecordDetails>
        {
            new AcademicRecordDetails { Id = 1, StartDate = "2010-01-01", EndDate = "2010-02-01", Average = "20", AcademicRecordsId = 1 },
            new AcademicRecordDetails { Id = 2, StartDate = "2014-09-01", EndDate = "2016-06-15", Average = "19", AcademicRecordsId = 2 },
            new AcademicRecordDetails { Id = 3, StartDate = "2008-09-01", EndDate = "2012-06-15", Average = "18", AcademicRecordsId = 3 },
            new AcademicRecordDetails { Id = 4, StartDate = "2012-09-01", EndDate = "2014-06-15", Average = "17", AcademicRecordsId = 4 },
            new AcademicRecordDetails { Id = 5, StartDate = "2009-09-01", EndDate = "2013-06-15", Average = "16", AcademicRecordsId = 5 },
            new AcademicRecordDetails { Id = 6, StartDate = "2013-09-01", EndDate = "2018-06-15", Average = "15", AcademicRecordsId = 6 }
        };*/
    }
}
