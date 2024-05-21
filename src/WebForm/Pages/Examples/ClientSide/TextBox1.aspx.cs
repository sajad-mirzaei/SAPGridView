using System;
using System.Collections.Generic;
using SAP.WebControls;
using System.Data;
using System.Web.Services;
using Newtonsoft.Json;

public partial class Grid_TextBox1 : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = MakeDataTable();

        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Data = dt,
            Columns = new List<Column>() {
                new Column { Data = "y", Title = "Name 0", DefaultContent = "",
                    Functions = { 
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "1==1",
                            IsTrueText = "<input type='textbox' name='textbox1' id='textbox1-tableId' value='x1' data-value1='tableId' data-value2='x2'>",
                            IsFalseText = "",
                        }
                    }
                },
                new Column { Data = "xCustom", Title = "Submit", DefaultContent = "<span class='btn btn-primary btn-sm'>edit</span>", 
                    Functions = {
                        new OnClick { 
                            Section = Function.SectionValue.Tbody, 
                            JavaScriptMethodName = "updateRowClient", 
                            HttpRequestType = OnClick.HttpRequestTypeValue.CallJavaScriptMethod 
                        }
                    }
                },
                new Column { Data = "x1", Title = "Name 1" },
                new Column { Data = "x2", Title = "Name 2" },
                new Column { Data = "x3", Title = "Name 3" },
                new Column { Data = "x4", Title = "Name 4" }
            }
        };

        oSGV.GridBind("MyGrid1");
    }

    [WebMethod]
    public static string updateRowServer(string inputData, string rowData)
    {
        Dictionary<string, string> inputDataArray = JsonConvert.DeserializeObject<Dictionary<string, string>>(inputData);
        Dictionary<string, string> rowDataArray = JsonConvert.DeserializeObject<Dictionary<string, string>>(rowData);
        return "ok";
    }

    public DataTable MakeDataTable()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("tableId", typeof(int));
        oDT.Columns.Add("x1", typeof(int));
        oDT.Columns.Add("x2", typeof(string));
        oDT.Columns.Add("x3", typeof(string));
        oDT.Columns.Add("x4", typeof(string));

        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["tableId"] = i + 1;
            Row1["x1"] = i;
            Row1["x2"] = i + 1;
            Row1["x3"] = i + 2;
            Row1["x4"] = i + 3;
            oDT.Rows.Add(Row1);
        }
        return oDT;
    }

}