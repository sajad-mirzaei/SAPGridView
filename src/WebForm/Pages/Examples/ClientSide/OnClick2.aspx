<%@ Page Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="OnClick2.aspx.cs" Inherits="Grid_OnClick2"
    Title="گزارش تست" %>

<asp:Content ID="Content" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always" OnLoad="Page_Load">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressLogin" runat="server" UpdateMode="Always">
                <ProgressTemplate>
                    <asp:Label runat="server" ID="Label001" Text=""></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="/Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>

                <div id="Div_Grids" class="card" runat="server">
                    <div class="card-header">
                        <asp:Label ID="lblGvHeaderBox" Text="اطلاعات" runat="server"></asp:Label>
                    </div>
                    <%--//--============================ >>> گرید ها <<< =============================--//--%>
                    <div class="card-body">
                        <div id="MyGridId"></div>
                    </div>
                </div>

            
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
