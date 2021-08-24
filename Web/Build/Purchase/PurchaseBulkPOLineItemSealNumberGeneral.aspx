<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBulkPOLineItemSealNumberGeneral.aspx.cs" Inherits="PurchaseBulkPOLineItemSealNumberGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seal Number</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSealNumber.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealReq" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblGuidance" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
            </telerik:RadWindowManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:TabStrip ID="MenuSealNumber" runat="server" OnTabStripCommand="MenuSealNumber_TabStripCommand" Title="Seal Number Recording"></eluc:TabStrip>

            <table id="tblGuidance" runat="server">
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblNote" runat="server" Text="Note: <br>*The seal numbers once recorded can not be changed."
                            ForeColor="Blue" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPrefix" runat="server" Text="Prefix"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPrefix" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNoofdigitsinSerialNumber" runat="server" Text="No. of digits in Serial Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtNoofdigits" runat="server" CssClass="input" IsInteger="true" IsPositive="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromSerialNumber" runat="server" Text="From Serial Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtFromSerialNo" runat="server" CssClass="input_mandatory" IsInteger="true" IsPositive="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToSerialNumber" runat="server" Text="To Serial Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtToSerialNo" runat="server" CssClass="input_mandatory" IsInteger="true" IsPositive="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuSealExport" runat="server" OnTabStripCommand="MenuSealExport_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid ID="gvSealNumber" runat="server" AutoGenerateColumns="False" Font-Size="11px" Width="99.9%"
                    OnNeedDataSource="gvSealNumber_NeedDataSource" AllowSorting="true" OnItemDataBound="gvSealNumber_ItemDataBound"
                    AllowPaging="true" AllowCustomPaging="true" OnItemCommand="gvSealNumber_ItemCommand" EnableViewState="false" >
                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true">
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="S.No." ShowSortIcon="true" SortExpression="FLDROWNO">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Seal Number">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSealnumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
