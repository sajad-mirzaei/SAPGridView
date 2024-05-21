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
                    <div class="alert alert-info">
                        <span class="text-success">تعداد موارد انتخاب شده:</span>
                        <span class="text-dark CountSelectedItems">0</span>
                        <br />
                        <span class="text-success">جمع موارد انتخاب شده:</span>
                        <span class="text-dark SumSelectedItems">0</span>
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                var CountSelectedItems = 0;
                var SumSelectedItems = 0;
                /* اگر همه اطلاعات جدول را نیاز دارید این بلاک را فعال کنید
                 * var oData = null;
                function DataTableCallBackData(oTableData) {
                    oData = oTableData;
                }*/
                function onPagesLoad() {
                    $('[name="checkbox1"]').change(function () {
                        //console.log(oData);
                        var value = parseFloat($(this).attr("data-value"));
                        if ($(this).is(':checked')) {
                            CountSelectedItems++;
                            SumSelectedItems += value;
                        } else {
                            CountSelectedItems--;
                            SumSelectedItems -= value;
                        }
                        $(".CountSelectedItems").text(CountSelectedItems);
                        $(".SumSelectedItems").text(SumSelectedItems);
                    });
                }
            </script>


        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
