<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="CheckBox1.aspx.cs" Inherits="CheckBox1" %>

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
                    <div class="alert alert-info mt-2">
                        <span class="text-success">تعداد موارد انتخاب شده:</span>
                        <span class="text-dark countSelectedItems">0</span>
                        <br />
                        <span class="text-success">جمع موارد انتخاب شده:</span>
                        <span class="text-dark sumSelectedItems">0</span>
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                var countSelectedItems = 0;
                var sumSelectedItems = 0;

                function myJavascriptMethodName(oData) {
                    if (oData.isSelectAllCheckBoxEvent)
                        selectAllOnChanged(oData);
                    else
                        singleOnChanged(oData);
                }

                function selectAllOnChanged(oData) {
                    countSelectedItems = 0;
                    sumSelectedItems = 0;
                    var table = oData.tableObject;
                    var checked = oData.obj.checked;

                    // Loop through each row
                    table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                        var data = this.data();
                        var price = data.price;
                        // Update the checkbox state
                        $(this.node()).find('input.' + oData.rowsCssClass).prop('checked', checked);

                        // You can update your counter or sum if needed
                        if (checked) {
                            countSelectedItems++;
                            sumSelectedItems += price;
                        } else {
                            countSelectedItems = 0;
                            sumSelectedItems = 0;
                        }
                    });
                    setValues(countSelectedItems, sumSelectedItems);
                }

                function singleOnChanged(oData) {
                    var checked = $(oData.obj).is(':checked');
                    var rowData = oData.rowData;
                    var selectedAllCssClass = oData.selectedAllCssClass
                    var price = oData.rowData.price;
                    $("." + selectedAllCssClass).prop("checked", false);
                    if (checked === true) {
                        countSelectedItems++;
                        sumSelectedItems += price;
                    } else {
                        countSelectedItems--;
                        sumSelectedItems -= price;
                    }
                    setValues(countSelectedItems, sumSelectedItems);
                }

                function setValues(countSelectedItems, sumSelectedItems) {
                    $(".countSelectedItems").text(countSelectedItems);
                    $(".sumSelectedItems").text(sumSelectedItems);
                }
            </script>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
