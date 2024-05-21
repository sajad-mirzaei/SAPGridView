<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="TextBox1.aspx.cs" Inherits="TextBox1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                <ProgressTemplate>
                    <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="../../../Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>

            <div class="FormBox">
                <div class="HeaderBox">
                    Test1
                </div>
                <div class="ContentBox">
                    <div id="MyGridId"></div>
                </div>
            </div>
            <script type="text/javascript">
                function updateRowClient(oData) {
                    //console.log(oData);
                    var tableId = oData.CallBackData.RowData["tableId"];
                    var textbox1Value = $("#textbox1-" + tableId).val();
                    var inputData = JSON.stringify({
                        textbox1Value: textbox1Value
                    });
                    var rowData = JSON.stringify(oData.CallBackData.RowData);
                    $.ajax({
                        type: "POST",
                        url: document.location.origin + document.location.pathname + "/updateRowServer",
                        data: "{ inputData: '" + inputData + "', rowData: '" + rowData + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            console.log(data);
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            </script>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
