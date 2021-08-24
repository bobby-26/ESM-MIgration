<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersInventorySpareTransaction.aspx.cs" Inherits="OwnersInventorySpareTransaction" %>

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
    <form id="form1" runat="server" submitdisabledcontrols="true" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>
        
        <telerik:RadGrid ID="GVR" runat="server" Width="100%" RenderMode="Lightweight" FilterMenu-EnableViewState="true"
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
                    <telerik:GridTemplateColumn HeaderText="Purchased" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEQTY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Consumed" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSMQ" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMEDQTY") %>'></telerik:RadLabel>
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
