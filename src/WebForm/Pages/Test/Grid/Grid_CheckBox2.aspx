<%@ Page Language="C#" AutoEventWireup="True" CodeFile="Grid_CheckBox2.aspx.cs" MasterPageFile="~/MainBoard.master" Inherits="Grid_CheckBox2" Title="انتقال چک" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">

    <%--***************************** UpdatePanel *****************************************--%>
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressLogin" runat="server">
                <ProgressTemplate>
                    <asp:Label runat="server" ID="Label001" Text=""></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید" src="../../../Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>
            <%--***************************** gvMain *****************************************--%>
            <div class="FormBox">
                <div class="HeaderBox">
                    <asp:Label runat="server" ID="Labelc2" Text="ثبت اطلاعات"></asp:Label>
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
            <asp:HiddenField ID="EnteghalCheckSelectedValues" runat="server" />
            <asp:HiddenField ID="EnteghalCheckSelectedValuesSum" runat="server" />
            <asp:HiddenField ID="EnteghalCheckSelectedValuesCount" runat="server" />
            <script>
                var EnteghalCheckSelectedValues = [];
                /*function CheckBoxEnteghalCheck(oData) {
                    console.log(oData);
                    console.log(oData.CallBackData.RowData["ccDariaftPardakht"]);
                    console.log(oData.CallBackData.RowData["Mablagh"]);

                }*/

                var CountSelectedItems = 0;
                var SumSelectedItems = 0;
                function onPagesLoad() {
                    $('#selectedAllEnteghalCheck').click(function (event) {
                        setDefaults();
                        if (this.checked) {
                            $('[name="selectedEnteghalCheck"]').each(function () {
                                this.checked = true;
                                Checking($(this));
                            });
                        } else {
                            $('[name="selectedEnteghalCheck"]').each(function () {
                                this.checked = false;
                            });
                        }
                    });
                    $('[name="selectedEnteghalCheck"]').change(function () {
                        Checking($(this));
                    });
                    function Checking(ThisElement) {
                        var mablagh = strToFloat(ThisElement.attr("data-mablagh"));
                        var ccdariaftpardakht = ThisElement.attr("data-ccdariaftpardakht");
                        if (ThisElement.is(':checked')) {
                            CountSelectedItems++;
                            SumSelectedItems += mablagh;
                            EnteghalCheckSelectedValues.push(ccdariaftpardakht);
                        } else {
                            CountSelectedItems--;
                            SumSelectedItems -= mablagh;
                            const index = EnteghalCheckSelectedValues.indexOf(ccdariaftpardakht);
                            if (index > -1) {
                                EnteghalCheckSelectedValues.splice(index, 1);
                            }
                        }
                        $(".CountSelectedItems").text(CountSelectedItems);
                        $(".SumSelectedItems").text(customNumberFormat(SumSelectedItems.toString()));

                        $("#<%=EnteghalCheckSelectedValues.ClientID %>").val(JSON.stringify(EnteghalCheckSelectedValues));
                        $("#<%=EnteghalCheckSelectedValuesSum.ClientID %>").val(SumSelectedItems);
                        $("#<%=EnteghalCheckSelectedValuesCount.ClientID %>").val(CountSelectedItems);
                    }
                    function setDefaults() {
                        $(".CountSelectedItems").text(0);
                        $(".SumSelectedItems").text(0);
                        EnteghalCheckSelectedValues = [];
                        SumSelectedItems = 0;
                        CountSelectedItems = 0;
                        $("#<%=EnteghalCheckSelectedValues.ClientID %>").val("");
                        $("#<%=EnteghalCheckSelectedValuesSum.ClientID %>").val("");
                        $("#<%=EnteghalCheckSelectedValuesCount.ClientID %>").val("");
                    }
                }
            </script>
        </ContentTemplate>
    </ajax:UpdatePanel>

    <%--***************************** ObjectDataSource *****************************************--%>
    <%---------------------------------- odsTahsildar ----------------------------------------------%>
    <asp:ObjectDataSource runat="server" ID="odsTahsildar" TypeName="Tahsildar" SelectMethod="Get_EtelaatTahsildar">
        <SelectParameters>
            <asp:Parameter Name="ccMarkaz" DefaultValue="0" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%---------------------------------- odsMahaleVariz ----------------------------------------------%>
    <asp:ObjectDataSource runat="server" ID="odsMahaleVariz" TypeName="DariaftPardakhtPPC" SelectMethod="GetMahaleVariz">
        <SelectParameters>
            <asp:Parameter Name="CodeNoeVosol" DefaultValue="2" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%---------------------------------- odsGrid ----------------------------------------------%>
    <asp:ObjectDataSource runat="server" ID="odsGrid" TypeName="DariaftPardakht" SelectMethod="Get_EnteghalCheck">
        <SelectParameters>
            <asp:Parameter Name="ccMarkazAnbar" DefaultValue="0" />
            <asp:Parameter Name="ccSazmanForosh" DefaultValue="0" />
            <asp:Parameter Name="CodeVazeiat" DefaultValue="0" />
            <asp:Parameter Name="Taeed" DefaultValue="0" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
