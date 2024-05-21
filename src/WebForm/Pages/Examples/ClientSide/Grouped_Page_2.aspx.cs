using System;
using System.Data;

public partial class Grid_Grouped_Page_2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DefineGrid();
    }
    public void DefineGrid()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("id", typeof(int));
        oDT.Columns.Add("first_name", typeof(string));
        oDT.Columns.Add("last_name", typeof(string));
        oDT.Columns.Add("position", typeof(string));
        oDT.Columns.Add("office", typeof(string));
        oDT.Columns.Add("start_date", typeof(string));
        oDT.Columns.Add("salary", typeof(string));

        for (int i = 0; i < 200; i++)
        {
            int c = i / 6;
            DataRow Row1 = oDT.NewRow();
            Row1["id"] = i;
            Row1["first_name"] = "FirstName " + i;
            Row1["last_name"] = "LastName " + i;
            Row1["position"] = "Position " + i;
            Row1["office"] = "Office" + c;
            Row1["start_date"] = "1st Jan 70 " + i;
            Row1["salary"] = "$40000 " + i;
            oDT.Rows.Add(Row1);
        }

        aaa.DataSource = oDT;
        aaa.DataBind();
    }
}