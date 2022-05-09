using SAP.WebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public partial class UpdatePanel_1 : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        BindMultipeTest1();
        BindAutoPostBackTest2();
        BindListBoxChain1();
        if (!IsPostBack) { 
            BindGrid1();
        }
    }

    public void BindMultipeTest1()
    {
        Dictionary<string, string> oArrayTest = new Dictionary<string, string>();
        for (int i = 0; i < 20; i++)
        {
            int c = i / 6;
            oArrayTest.Add("Key_" + i.ToString(), "Value_" + i.ToString());
        }
        foreach (KeyValuePair<string, string> item in oArrayTest)
        {
            ddlMultipeTest1.Items.Add(new ListItem() { Text = item.Value, Value = item.Key });
        }
    }
   
    public void BindAutoPostBackTest2()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("first_name", typeof(string));
        oDT.Columns.Add("last_name", typeof(string));
        oDT.Columns.Add("position", typeof(string));
        oDT.Columns.Add("office", typeof(string));
        oDT.Columns.Add("start_date", typeof(string));
        oDT.Columns.Add("salary", typeof(string));

        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["first_name"] = "FirstName " + i;
            Row1["last_name"] = "LastName " + i;
            Row1["position"] = "Position " + i;
            Row1["office"] = "Office " + i;
            Row1["start_date"] = "1st Jan 70 " + i;
            Row1["salary"] = "$40000 " + i;
            oDT.Rows.Add(Row1);
        }
        ddlAutoPostBackTest2.DataSource = oDT;
        ddlAutoPostBackTest2.DataValueField = "first_name";
        ddlAutoPostBackTest2.DataTextField = "last_name";
        ddlAutoPostBackTest2.DataBind();
    }

    protected void BindGrid1()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("x", typeof(int));
        oDT.Columns.Add("y", typeof(string));
        oDT.Columns.Add("z", typeof(string));
        oDT.Columns.Add("w", typeof(string));

        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["x"] = i;
            Row1["y"] = "yy " + i;
            Row1["z"] = "zz " + i;
            Row1["w"] = "ww " + i;
            oDT.Rows.Add(Row1);
        }
        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId1",
            ContainerHeight = 400,
            Data = oDT,
            Columns = new List<Column>() {
                new Column { Data = "x", Title = "کد دیتابیس" },
                new Column { Data = "y", Title = "نام", CssClass = "text-info bg-danger" },
                new Column { Data = "z", Title = "فامیلی" },
                new Column { Data = "w", Title = "معدل" }
            }
        };
        oSGV.GridBind("MyGrid1");
    }

    protected void BindGrid2_Click(object sender, EventArgs e)
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("x", typeof(int));
        oDT.Columns.Add("y", typeof(string));
        oDT.Columns.Add("z", typeof(string));
        oDT.Columns.Add("w", typeof(string));

        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["x"] = i;
            Row1["y"] = "yy " + i;
            Row1["z"] = "zz " + i;
            Row1["w"] = "ww " + i;
            oDT.Rows.Add(Row1);
        }
        oSGV.Grids["MyGrid2"] = new Grid()
        {
            ContainerId = "MyGridId2",
            ContainerHeight = 400,
            Data = oDT,
            Columns = new List<Column>() {
                new Column { Data = "x", Title = "کد دیتابیس" },
                new Column { Data = "y", Title = "نام", CssClass = "text-info bg-dark" },
                new Column { Data = "z", Title = "فامیلی" },
                new Column { Data = "w", Title = "معدل" }
            }
        };
        oSGV.GridBind("MyGrid2");
    }

    public void BindListBoxChain1()
    {
        Dictionary<string, string> oArrayTest = new Dictionary<string, string>();
        for (int i = 0; i < 20; i++)
        {
            int c = i / 6;
            oArrayTest.Add(i.ToString(), "Value_" + i.ToString());
        }
        foreach (KeyValuePair<string, string> item in oArrayTest)
        {
            ListBoxChain1.Items.Add(new ListItem() { Text = item.Value, Value = item.Key });
        }
        ListBoxChain1.DataBind();
    }

    protected void ListBoxChain1_SelectedIndexChanged(object sender, EventArgs e)
    {
        var x = ListBoxChain1.SelectedValue;

        ListBoxChain2.ClearSelection();
        ListBoxChain2.Items.Clear();
        Dictionary<string, string> oArrayTest = new Dictionary<string, string>();
        for (int i = 0; i < 20; i++)
        {
            int c = i / 6;
            oArrayTest.Add(i.ToString(), "Value_" + x + "-" + i.ToString() );
        }
        foreach (KeyValuePair<string, string> item in oArrayTest)
        {
            ListBoxChain2.Items.Add(new ListItem() { Text = item.Value, Value = item.Key });
        }
        Label3.Text = "dropdownlist Multipe ListBoxChain2 - Updated";
        ListBoxChain2.DataBind();
        UpdatePanel2.Update();
    }
}