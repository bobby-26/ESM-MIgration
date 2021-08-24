<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSDrawingDistributedVesselList.aspx.cs"
    Inherits="DocumentManagementFMSDrawingDistributedVesselList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="titlecontent" runat="server">Distributed VesselList</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocument" runat="server" submitdisabledcontrols="true">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselListByDrawing" runat="server"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="94%"
            CellSpacing="0" CellPadding="3" GridLines="None" OnNeedDataSource="gvVesselListByDrawing_NeedDataSource"
            ShowFooter="true" ShowHeader="true" EnableViewState="true">
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="S.No">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblSNOHeader" runat="server" Text="S.No">
                            </telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSerialNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblVesselNameHeader" runat="server" Text="Vessel [ID]">
                            </telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                    Font-Bold="true">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder"
                AllowDragToGroup="false">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="395px" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
