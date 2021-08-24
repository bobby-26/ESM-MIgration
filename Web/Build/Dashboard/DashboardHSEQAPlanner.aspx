<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardHSEQAPlanner.aspx.cs" Inherits="Dashboard_DashboardHSEQAPlanner" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HSEQA Planner</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <style type="text/css">
             .RadGrid .rgRow > td ,.RadGrid .rgAltRow>td {
                 padding-left: 0px !important;
                padding-right: 0px  !important; 
             }
         </style>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
   
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
         <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="Tabstripspiaddmenu" runat="server" OnTabStripCommand="Tabstripspiaddmenu_TabStripCommand"
                TabStrip="true" />
             <eluc:Year AutoPostBack="true" runat="server" YearStartFrom="2018" ID="radcbyear" />
              <eluc:TabStrip ID="Tabstrip1" runat="server" OnTabStripCommand="Tabstrip1_TabStripCommand"
                TabStrip="true" />
              <telerik:RadGrid runat="server" ID="gvPlanner" AutoGenerateColumns="false" Height="89%"
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvPlanner_NeedDataSource" EnableViewState="false"
                OnItemDataBound="gvPlanner_ItemDataBound" OnItemCommand="gvPlanner_ItemCommand" ShowFooter="false" CssClass="plan">
                <MasterTableView EditMode="InPlace" AutoGenerateColumns="false"
                    TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns></Columns>


                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="false" AllowColumnsReorder="true" ReorderColumnsOnClient="false" AllowColumnHide="true" EnablePostBackOnRowClick="false"  >
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true"  CellSelectionMode="SingleCell"/>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
             
             </telerik:RadAjaxPanel>
    
    </form>
</body>
</html>
