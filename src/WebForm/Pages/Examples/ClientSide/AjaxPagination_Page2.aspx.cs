using Newtonsoft.Json;
using SAP.WebControls;
using System;
using System.Collections.Generic;
using System.Web.Services;

public partial class AjaxPagination_Page2 : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
        DefineGrids();
    }

    public static void DefineGrids()
    {
        oSGV.Grids["1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Columns = new List<Column>() {
                new Column { Data="first_name", Title ="نام" },
                new Column { Data="last_name", Title ="نام خانوادگی" },
                new Column { Data="office", Title ="دفتر" },
                new Column { Data="start_date", Title ="تاریخ شروع" },
                new Column { Data="salary", Title ="قیمت" }
            }
        };
        oSGV.GridBind("1");
    }


    [WebMethod]
    public static string TestMethod(string CallBackData)
    {
        var oData = JsonConvert.DeserializeObject<AjaxPaginationProperty2>(CallBackData);

        if (oData.order.Count > 0)
        {//sort darad ya na
            var x = oData.order[0]["column"];
            var y = oData.order[0]["dir"];
        }
        ArrayTest2 oArrayTest = new ArrayTest2();
        oArrayTest.draw = oData.draw;
        oArrayTest.recordsTotal = 56;
        oArrayTest.recordsFiltered = 56;
        for (int i = oData.start; i < (oData.length + oData.start); i++)
        {
            oArrayTest.data.Add(
                new Dictionary<string, string>() {
                    { "first_name", "Ajax FirstName " + i },
                    { "last_name", "Ajax LastName " + i },
                    { "position", "Ajax Position " + i },
                    { "office", "Ajax Office " + i },
                    { "start_date", "Ajax 1st Jan 70" },
                    { "salary", "Ajax $40000" + i }
                }
                );
        }
        var JsonData = JsonConvert.SerializeObject(oArrayTest);
        return JsonData;
    }
    [WebMethod]
    public static string SapGridEvent(string CallBackData)
    {
        SapGridCallBackEvent oData = JsonConvert.DeserializeObject<SapGridCallBackEvent>(CallBackData);
        List<string> DataKeys = oData.FuncArray.DataKeys;
        string NextGrid = oData.FuncArray.NextGrid;
        oSGV.Grids[NextGrid].GridParameters = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> item in oSGV.DefaultParameters)
        {
            oSGV.Grids[NextGrid].GridParameters[item.Key] = item.Value;
        }

        Dictionary<string, string> Clicked_GridParameters = oData.GridParameters;
        oSGV.Grids[NextGrid].GridParameters["Level"] = oData.FuncArray.Level;
        var RowData = oData.RowData;
        foreach (string DataKey in DataKeys)
        {
            if (RowData.Count != 0)
                oSGV.Grids[NextGrid].GridParameters[DataKey] = RowData[DataKey];
            else
            {
                oSGV.Grids[NextGrid].GridParameters[DataKey] = Clicked_GridParameters[DataKey];
            }
        }
        oSGV.Grids[NextGrid].Data = null;
        return oSGV.AjaxBind(NextGrid);

    }
}

class ArrayTest2
{
    public int draw = 0;
    public int recordsTotal = 56;
    public int recordsFiltered = 56;
    public List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
}
class AjaxPaginationProperty2
{
    public int draw;
    public int length;
    public int start;
    public Dictionary<string, string> search;
    public List<Dictionary<string, string>> order;
    /*public string columns;*/
}