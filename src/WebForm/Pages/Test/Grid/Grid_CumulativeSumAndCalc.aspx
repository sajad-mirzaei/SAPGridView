<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="Grid_CumulativeSumAndCalc.aspx.cs" Inherits="Grid_CumulativeSumAndCalc" %>

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
                    MyGrid1
                    <asp:LinkButton runat="server" ID="ChangeData" CssClass="float-left" OnClick="ChangeData_Click">تغییر اطلاعات جدول</asp:LinkButton>
                </div>
                <div class="ContentBox">
                    <div id="MyGridId1"></div>
                </div>
            </div>
            
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>