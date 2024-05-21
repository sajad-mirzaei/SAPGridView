<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="RowComplex4.aspx.cs" Inherits="Grid_RowComplex4" %>

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
                    Test1 <i class='fa fa-solid fa-filter-circle-xmark'></i>
                </div>
                <div class="ContentBox">
                    <div class="alert alert-info text-center">Multiple product with same supplier</div>
                    <div id="MyGridId1"></div>
                    <hr />
                    <div class="alert alert-info text-center">Multiple product and same product with same supplier</div>
                    <div id="MyGridId2"></div>
                </div>
            </div>

        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
