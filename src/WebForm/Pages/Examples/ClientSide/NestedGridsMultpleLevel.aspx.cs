using Newtonsoft.Json;
using SAP.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;

public partial class NestedGridsMultpleLevel : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            var mockData = new MockData().GetMockData();
            var oSGV = CreatePersonInfoGrid(mockData);
            oSGV.GridBind("PersonInfo");
        }
    }

    public SAPGridView CreatePersonInfoGrid(MockData mockData)
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
            Data = mockData.PersonInfoList,
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
    public static Grid CreateNextGrids(string nextGrid, Dictionary<string, string> filters)
    {
        SAPGridView oSGV = new SAPGridView();
        MockData mockData = new MockData().GetMockData();
        //-----------------------AcademicRecords Grid------------------------
        oSGV.Grids["AcademicRecords"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 300,
            GridTitle = "GridTitle Test 2",
            Columns = new List<Column>() {
                new Column { Title ="id", Data = "Id" },
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
            Columns = new List<Column>() {
                new Column { Title ="id", Data = "Id" },
                new Column { Title ="Start Date", Data = "StartDate" },
                new Column { Title ="End Date", Data = "EndDate" },
                new Column { Title ="Average", Data = "Average" }
            }
        };

        var grid = oSGV.Grids[nextGrid];
        if (nextGrid == "AcademicRecords")
        {
            grid.Data = mockData.AcademicRecordsList.Where(c => c.PersonInfoId == int.Parse(filters["Id"]));
        }
        else
        {
            grid.Data = mockData.AcademicRecordDetailsList.Where(c => c.AcademicRecordsId == int.Parse(filters["Id"]));
        }
        return grid;
    }

    [WebMethod]
    public static string SapGridEvent(string CallBackData)
    {
        SapGridCallBackEvent oData = JsonConvert.DeserializeObject<SapGridCallBackEvent>(CallBackData);
        //--clicked row data-------------------------------------
        var rowData = oData.RowData;
        List<string> dataKeys = oData.FuncArray.DataKeys;
        string nextGrid = oData.FuncArray.NextGrid;
        string clicked_CellName = oData.TableDetails["CellName"];
        int level = int.Parse(oData.FuncArray.Level);

        var filters = new Dictionary<string, string>(oData.GridParameters);
        foreach (string dataKey in dataKeys)
        {
            if (rowData.Count != 0)
                filters[dataKey] = rowData[dataKey];
        }

        var oSGV = new SAPGridView();
        oSGV.Grids[nextGrid] = CreateNextGrids(nextGrid, filters);
        oSGV.Grids[nextGrid].GridParameters = filters;
        oSGV.Grids[nextGrid].GridParameters["Level"] = oData.FuncArray.Level;

        return oSGV.AjaxBind(nextGrid);
    }
}

public class MockData
{
    public List<PersonInfo> PersonInfoList { get; set; }
    public List<AcademicRecords> AcademicRecordsList { get; set; }
    public List<AcademicRecordDetails> AcademicRecordDetailsList { get; set; }
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

    public MockData GetMockData()
    {
        var mockData = new MockData();
        mockData.PersonInfoList = new List<PersonInfo>
        {
            new PersonInfo { Id = 1, FirstName = "John", LastName = "Doe", Age = "30" },
            new PersonInfo { Id = 2, FirstName = "Jane", LastName = "Smith", Age = "28" },
            new PersonInfo { Id = 3, FirstName = "Alice", LastName = "Johnson", Age = "32" }
        };

        mockData.AcademicRecordsList = new List<AcademicRecords>
        {
            new AcademicRecords { Id = 1, Degree = "B.Sc.", Major = "Computer Science", Institute = "MIT", PersonInfoId = 1 },
            new AcademicRecords { Id = 2, Degree = "M.Sc.", Major = "Computer Science", Institute = "Stanford", PersonInfoId = 1 },
            new AcademicRecords { Id = 3, Degree = "B.A.", Major = "English Literature", Institute = "Harvard", PersonInfoId = 2 },
            new AcademicRecords { Id = 4, Degree = "M.A.", Major = "English Literature", Institute = "Yale", PersonInfoId = 2 },
            new AcademicRecords { Id = 5, Degree = "B.Sc.", Major = "Physics", Institute = "Caltech", PersonInfoId = 3 },
            new AcademicRecords { Id = 6, Degree = "Ph.D.", Major = "Physics", Institute = "UC Berkeley", PersonInfoId = 3 }
        };

        mockData.AcademicRecordDetailsList = new List<AcademicRecordDetails>
        {
            new AcademicRecordDetails { Id = 1, StartDate = "2010-09-01", EndDate = "2014-06-15", Average = "20", AcademicRecordsId = 1 },
            new AcademicRecordDetails { Id = 2, StartDate = "2014-09-01", EndDate = "2016-06-15", Average = "19", AcademicRecordsId = 2 },
            new AcademicRecordDetails { Id = 3, StartDate = "2008-09-01", EndDate = "2012-06-15", Average = "18", AcademicRecordsId = 3 },
            new AcademicRecordDetails { Id = 4, StartDate = "2012-09-01", EndDate = "2014-06-15", Average = "17", AcademicRecordsId = 4 },
            new AcademicRecordDetails { Id = 5, StartDate = "2009-09-01", EndDate = "2013-06-15", Average = "16", AcademicRecordsId = 5 },
            new AcademicRecordDetails { Id = 6, StartDate = "2013-09-01", EndDate = "2018-06-15", Average = "15", AcademicRecordsId = 6 }
        };
        return mockData;
    }
}