using System;
using System.Collections.Generic;
using SAP.WebControls;
using SAP.Utility;
using System.Data;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;
using SAP.OfficeTools;

public partial class ExcelTestPage_Simple : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    public DataTable MakeDataTable()
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
        return oDT;
    }


    protected void DownloadExcelFile_Click(object sender, EventArgs e)
    {
        DataTable dt = MakeDataTable();
        ToExcel oToExcel = new ToExcel
        {
            Data = dt,
            FileName = "ExcelFileName.xlsx"
        };
        oToExcel.Download();
    }
}