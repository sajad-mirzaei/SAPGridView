<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="GridTestPage_1.aspx.cs" Inherits="GridTestPage_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                <ProgressTemplate>
                    <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="../../../Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>


            
            <span class="btn btn-alert-info" id="Testbtn1">Test 1</span>
            <div class="FormBox">
                <div class="HeaderBox">
                    Test1 <i class='fa fa-solid fa-filter-circle-xmark'></i>
                </div>
                <div class="ContentBox">
                    <div id="MyGridId"></div>
                </div>
            </div>


        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>