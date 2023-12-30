<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="Grid_HeaderComplex_1.aspx.cs" Inherits="Grid_HeaderComplex_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                <ProgressTemplate>
                    <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="../../../Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>
            <div id="divData1" runat="server" class="FormBox">
                <div class="HeaderBox">aaaa</div>
                <div id="ContentBoxId1" class="ContentBox">
                    <div id="MyGridId"></div>
                </div>
            </div>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
