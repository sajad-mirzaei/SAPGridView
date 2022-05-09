using System;
using System.Collections.Generic;
using SAP.WebControls;
using SAP.Utility;
using System.Data;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;

public partial class SoalatMotadavelPage : System.Web.UI.Page
{
    private SAPGridView oSGV = new SAPGridView();
    private SoalatMotadavel oSoalatMotadavel = new SoalatMotadavel();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid_SoalatMotadavel();
        }

    }
    protected void btnSabt_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> Params = new Dictionary<string, string>() {
            { "Soal", txtSoal.Text },
            { "Pasokh", txtPasokh.Text }
        };
        var x = oSoalatMotadavel.Insert_SoalatMotadavel(Params);
        if (x > 0)
        {
            txtSoal.Text = "";
            txtPasokh.Text = "";
            MessageBox.InnerText = "ثبت سوال با موفقیت انجام شد";
            MessageBox.Attributes.Add("class", "alert alert-success text-center");
        }
        else
        {
            MessageBox.InnerText = "مشکلی در ثبت سوال وجود دارد لطفا مجددا تلاش کنید";
            MessageBox.Attributes.Add("class", "alert alert-danger text-center");
        }
        BindGrid_SoalatMotadavel();
    }
    protected void BindGrid_SoalatMotadavel()
    {
        oSGV.Grids["SoalatMotadavel"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Data = oSoalatMotadavel.Get_SoalatMotadavel(),
            Columns = new List<Column>() {
                new Column { Data = "Soal", Title = "سوال" },
                new Column { Data = "Pasokh", Title = "پاسخ" }
            }
        };
        oSGV.GridBind("SoalatMotadavel");
    }
}