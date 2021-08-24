<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentAverageRunHours.aspx.cs" Inherits="PlannedMaintenanceComponentAverageRunHours" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Run Hour</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvClassMap">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvClassMap" UpdatePanelHeight="88%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblCNumber" Text="Component Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtCNumber" Width="100%" ></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblCName" Text="Component Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtCName" Width="100%" ></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            <br clear="all" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPlannedMaintenanceJob" DecoratedControls="All" EnableRoundedCorners="true" />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvClassMap" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvClassMap_NeedDataSource" OnItemDataBound="gvClassMap_ItemDataBound" OnItemCommand="gvClassMap_ItemCommand"
                            OnUpdateCommand="gvClassMap_UpdateCommand" OnBatchEditCommand="gvClassMap_BatchEditCommand">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="Batch" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDCOMPONENTID,FLDCOMPONENTID" CommandItemDisplay="Top">
                            <HeaderStyle Width="102px" />
                            <BatchEditingSettings EditType="Cell" />
                            <CommandItemSettings ShowRefreshButton="true" RefreshText="Search" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                            <Columns>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowKeyboardNavigation="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
