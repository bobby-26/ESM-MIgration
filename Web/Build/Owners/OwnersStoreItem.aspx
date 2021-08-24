<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersStoreItem.aspx.cs" Inherits="OwnersStoreItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOwnersVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Store Item</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .direction {
                direction: rtl;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmStoreItemForOwner" DecoratedControls="All" />
    <form id="frmStoreItemForOwner" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AutoPostBack="true" AssignedVessels="true" CssClass="dropdown_mandatory"
                            VesselsOnly="true" OnTextChangedEvent="ucVessel_Changed" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMakerReference" runat="server" Text="Maker Reference"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMekerRef" runat="server" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblItemName" runat="server" Text="Item Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblItemNumber" runat="server" Text="Item Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNumber" runat="server" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStoreType" runat="server" Text="Store Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlStockClass" runat="server" Visible="true"
                            AutoPostBack="true" Width="240px" />
                    </td>

                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkROB" runat="server" AutoPostBack="true" Text="Show Available Store Items only" CssClass="direction" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuStockItemControl" runat="server" OnTabStripCommand="MenuStockItemControl_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="dvStoreItemControl" Height="86%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="dvStoreItemControl_ItemCommand" OnItemDataBound="dvStoreItemControl_ItemDataBound" EnableViewState="false" ShowFooter="false" ShowHeader="true" OnNeedDataSource="dvStoreItemControl_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="100%" GroupHeaderItemStyle-Wrap="false" Width="100%">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Number">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStoreItemLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStoreItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStoreItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockItemUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="In Stock">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInStockQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Actual">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActualQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>          
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
