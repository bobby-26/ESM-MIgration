<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersInventoryView.aspx.cs" Inherits="OwnersInventoryView" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= GVR.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="GVR">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="GVR" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="92%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuWorkOrder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuWorkOrder" />
                        <telerik:AjaxUpdatedControl ControlID="GVR" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="92%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <telerik:RadGrid ID="GVR" runat="server" Width="100%"
            AllowCustomPaging="false" AllowSorting="false" AllowFilteringByColumn="true" AllowPaging="false" CellSpacing="0" GridLines="None" OnPreRender="GVR_PreRender"
            OnNeedDataSource="GVR_NeedDataSource" OnItemDataBound="GVR_ItemDataBound"  OnItemCommand="GVR_ItemCommand" OnColumnCreated="GVR_ColumnCreated" OnItemCreated="GVR_ItemCreated" EnableLinqExpressions="false" EnableViewState="false">
             <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                AllowFilteringByColumn="True" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" FilterExpression="">
                <NoRecordsTemplate>
                    <table runat="server" width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>

                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Is Shortfalls" AllowSorting="false" UniqueName="FLDISSHORTFALL"
                        ShowSortIcon="false" FilterControlWidth="70px" AutoPostBackOnFilter="false" ShowFilterIcon="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20px"></ItemStyle>
                        <HeaderStyle Width="50px" />
                        <FilterTemplate>
                            <telerik:RadCheckBox runat="server" ID="chkIsShortfall" OnCheckedChanged="chkIsShortfall_CheckedChanged"
                                Checked='<%#GetFilter("ISSHORTFALL") == "1" ? true : false %>'>
                            </telerik:RadCheckBox>
                            <telerik:RadLabel ID="lblFIsCritial" runat="server" Text="Is Shortfalls" ToolTip="Is Shortfalls" Visible="false" AssociatedControlID="chkIsShortfall"></telerik:RadLabel>
                        </FilterTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblshortfall" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSHORTFALLYN") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component No." UniqueName="FLDCOMPONENTNUMBER"
                        AllowSorting="false" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains" HeaderStyle-Width="15%">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcompno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component Name" HeaderStyle-Width="30%"
                        UniqueName="FLDCOMPONENTNAME"
                        AllowSorting="false" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcompname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="30%"
                        UniqueName="FLDMEASURE"
                        AllowSorting="false" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Width="10%" AllowFiltering="false" HeaderStyle-HorizontalAlign="Center">
                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQTY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Min Qty" HeaderStyle-Width="10%" AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Stock Minimum Qty">
                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSMQ" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINIMUMQTY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblshortfallyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSHORTFALL") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn  HeaderText="Req Qty" HeaderStyle-Width="10%" AllowFiltering="false" HeaderStyle-HorizontalAlign="Center">
                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDQTY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings>
                <Resizing AllowColumnResize="true" />
                <Selecting AllowRowSelect="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
            </ClientSettings>
        </telerik:RadGrid>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
