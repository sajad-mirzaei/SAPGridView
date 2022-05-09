<%@ Page Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="Grid_Levels1.aspx.cs" Inherits="Grid_Levels1"
    Title="گزارش اعلامیه بدهکاری - بستانکاری" %>

<asp:Content ID="Content" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always" OnLoad="Page_Load">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressLogin" runat="server" UpdateMode="Always">
                <ProgressTemplate>
                    <asp:Label runat="server" ID="Label001" Text=""></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src='<%= Page.ResolveClientUrl("~/Assets/Styles/images/wait.gif") %>' />
                </ProgressTemplate>
            </ajax:UpdateProgress>
            <div class="FormBox">
                <div class="HeaderBox">
                    <asp:Label ID="lblNameSystem" Text=" Grid_Levels1 " runat="server"></asp:Label>
                </div>
                <div class="ContentBox">
                    <asp:Label runat="server" Visible="false" ID="lblErrorBox"></asp:Label>
                    <div class="row">
                        <div class="form-group col-lg-2 col-md-2 col-sm-6 col-xs-12">
                            <span class="text-danger">*</span>
                            <asp:Label Text="از تاریخ :" label-for="dtbAzTarikh" class="label" runat="server"></asp:Label>
                            <SAP:DateBox ID="dtbAzTarikh" class="DateBox" runat="Server" />
                        </div>
                        <div class="form-group col-lg-2 col-md-2 col-sm-6 col-xs-12">
                            <span class="text-danger">*</span>
                            <asp:Label Text="تا تاریخ :" label-for="dtbTaTarikh" class="label" runat="server"></asp:Label>
                            <SAP:DateBox ID="dtbTaTarikh" class="DateBox" runat="Server" />
                        </div>
                        <%--//--================== >>> دکمه های فرم <<< ==================--//--%>
                        <div class="form-group col-lg-2 col-md-2 col-sm-6 col-12 noLabelBottom ">
                            <asp:LinkButton ID="btnView" Text="نمایش" AccessKey="ن" CssClass="btn btn-primary2 col-5" OnClick="btnView_Click"
                                OnClientClick="this.disabled=true; this.value='منتظر بمانید'" UseSubmitBehavior="false" runat="server"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Div_Grids" class="FormBox" runat="server">
                <div class="HeaderBox">
                    <asp:Label ID="lblGvHeaderBox" Text="اطلاعات" runat="server"></asp:Label>
                </div>
                <%--//--============================ >>> گرید ها <<< =============================--//--%>
                <div class="ContentBox">
                    <div id="MyGridId"></div>
                </div>
            </div>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>