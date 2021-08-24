<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationLineItemBulkSave.aspx.cs" Inherits="PurchaseQuotationLineItemBulkSave" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucUnit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quotation Line Bulk Save </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function PaneResized(sender, args) {

                var browserHeight = $telerik.$(window).height();
                var grid = $find("rgvLine");
                grid._gridDataDiv.style.height = (browserHeight - 130) + "px";
            }
            function pageLoad() {
                PaneResized();
            }

            function saveChangesToGrid() {
                var grid = $find('<%=rgvLine.ClientID%>');
                grid.get_batchEditingManager().saveChanges(grid.get_masterTableView());
                return false;
            }
            function cancelChangesToGrid() {
                var grid = $find('<%=rgvLine.ClientID%>');
                grid.get_batchEditingManager().cancelChanges(grid.get_masterTableView());
                return false;
            }
        </script>

    </telerik:RadCodeBlock>
    <style type="text/css">
        .RadGrid .rgCommandCellLeft {
    float: right !important;
}

    </style>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmQuotationLineBulkSave" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvLine">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="menuSaveDetails">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="menuSaveDetails" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
            </AjaxSettings>
        </telerik:RadAjaxManager>
                    <eluc:TabStrip ID="menuSaveDetails" runat="server" TabStrip="false" OnTabStripCommand="menuSaveDetails_TabStripCommand">
                    </eluc:TabStrip>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
                <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvLine_NeedDataSource" OnItemDataBound="rgvLine_ItemDataBound" 
                    OnItemCommand="rgvLine_ItemCommand" OnBatchEditCommand="rgvLine_BatchEditCommand" OnPreRender="rgvLine_PreRender">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="Batch" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" CommandItemDisplay="Top" DataKeyNames="FLDQUOTATIONLINEID,FLDUNITID,FLDQUOTATIONID,FLDVESSELID,FLDPARTID,FLDORDERLINEID,FLDCOMPONENTID" TableLayout="Fixed">    
                        <BatchEditingSettings EditType="Cell" />
                        <%--<CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" SaveChangesText="Save"  />--%>
                        <CommandItemStyle HorizontalAlign="Right" />
                        <CommandItemTemplate>
                            <telerik:RadPushButton runat="server" ID="SaveChangesButton" OnClientClicked="saveChangesToGrid" 
                                AutoPostBack="false" ToolTip="Save changes">
                                <Icon Url="../css/Theme1/images/save.png"/>
                            </telerik:RadPushButton>
                            <telerik:RadPushButton runat="server" ID="rpCancel" OnClientClicked="cancelChangesToGrid" 
                                AutoPostBack="false" ToolTip="Cancel changes">
                                <Icon Url="../css/Theme1/images/te_del.png"/>
                            </telerik:RadPushButton>
                            
                        </CommandItemTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                            PageSizeLabelText="Items per page:" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        <KeyboardNavigationSettings EnableKeyboardShortcuts="true" AllowSubmitOnEnter="true"
                                 AllowActiveRowCycle="true" MoveDownKey="DownArrow" MoveUpKey="UpArrow"></KeyboardNavigationSettings>
                        <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                    </ClientSettings>
                </telerik:RadGrid>
    </form>
</body>
</html>
