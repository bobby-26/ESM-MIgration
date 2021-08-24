<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSeaPortVisaDocuments.aspx.cs" Inherits="Registers_RegistersSeaPortVisaDocuments" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Country Visa Document</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
                fade('statusmessage');
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvCountryVisaDocument");
                grid._gridDataDiv.style.height = (browserHeight - 180) + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCountryVisaDocument" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="tblCongiSeaPort" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuRegistersCountryVisa" runat="server" OnTabStripCommand="MenuRegistersCountryVisa_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <table id="tblCongiSeaPort" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisaType" runat="server" Text="Visa Type"></telerik:RadLabel>
                <%--    </td>
                    <td>--%>
                        <eluc:Hard ID="ucHard" runat="server" CssClass="dropdown_mandatory" HardTypeCode="107" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersCountryVisaDocument" runat="server" OnTabStripCommand="RegistersCountryVisaDocument_TabStripCommand">
            </eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCountryVisaDocument" Height="81%" runat="server" 
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvCountryVisaDocument_NeedDataSource"
                CellSpacing="0" GridLines="None" EnableViewState="true" OnItemDataBound="gvCountryVisaDocument_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVISADOCUMENTID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Document Name" AllowSorting="true" SortExpression="FLDHARDNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVisaDocumentID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISADOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document Specification" AllowSorting="true" SortExpression="FLDDOCUMENTSPECIFICATION">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentSpecification" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOCUMENTSPECIFICATION").ToString().Length>50 ? DataBinder.Eval(Container, "DataItem.FLDDOCUMENTSPECIFICATION").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDDOCUMENTSPECIFICATION").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucDocumentSpecTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTSPECIFICATION") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
