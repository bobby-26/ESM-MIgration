<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderLine.aspx.cs" Inherits="PurchaseOrderLine" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Requisitions</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("rgvForm");           
            grid._gridDataDiv.style.height = (browserHeight - 165) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table width="100%" border="0" cellpadding="1">
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" AutoPostBack="True" AppendDataBoundItems="true" VesselsOnly="true" AssignedVessels="true" OnTextChangedEvent="ucVessel_TextChangedEvent" Width="180px"/>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblItemNumber" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtItemNumber" runat="server" MaxLength="20"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblName" Text="Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtName" runat="server" ></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="ltrlStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlStockType" OnTextChanged="ddlStockType_TextChanged" AutoPostBack="true" EnableLoadOnDemand="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                            <telerik:RadComboBoxItem Text="Spares" Value="SPARE" />
                            <telerik:RadComboBoxItem Text="Stores" Value="STORE" />
                            <telerik:RadComboBoxItem Text="Service" Value="SERVICE" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
              
            </tr>
            <tr>
                <td>
                   <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblmakerreference" Text="Maker Reference"></telerik:RadLabel>
                </td>
                <td>
                 <telerik:RadTextBox RenderMode="Lightweight" ID="txtmakerreference" runat="server" Width="180px"></telerik:RadTextBox>
                </td>
                 <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlFromDate" Text="From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlToDate" Text="To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" />
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuNewRequisition" runat="server" OnTabStripCommand="MenuNewRequisition_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator"  runat="server" DecorationZoneID="rgvForm" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="rgvForm" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="rgvForm_NeedDataSource" OnItemDataBound="rgvForm_ItemDataBound" OnSortCommand="rgvForm_SortCommand" OnItemCommand="rgvForm_ItemCommand" ShowFooter="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDORDERLINEID,FLDORDERID,FLDVESSELID" TableLayout="Fixed">
                <Columns>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                    PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="false" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                <%--<ClientEvents OnGridCreated="GridCreated" />--%>
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
