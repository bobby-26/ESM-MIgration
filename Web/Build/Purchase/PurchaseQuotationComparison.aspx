<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationComparison.aspx.cs" Inherits="Purchase_PurchaseQuotationComparison"  EnableEventValidation="false" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation Compare</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </div>
         <script type="text/javascript">
             function PaneResized(sender, args) {
                 
                 var browserHeight = $telerik.$(window).height();
                 var grid = $find("gvCompare");
                 grid._gridDataDiv.style.height = (browserHeight - 300) + "px";
             }
            function pageLoad() {
                PaneResized();
             }
             function saveChangesToGrid() {
                 var grid = $find('<%=gvCompare.ClientID%>');
                 grid.get_batchEditingManager().saveChanges(grid.get_masterTableView());
                 return false;
             }
             function cancelChangesToGrid() {
                 var grid = $find('<%=gvCompare.ClientID%>');
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
    <form id="frmPurchaseQuotationCompare" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radScriptManager" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager ID="RadWindowManager" runat="server"></telerik:RadWindowManager>
        <%--<telerik:RadAjaxManager ID="RadAjaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuQuotationCompare">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuQuotationCompare" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID ="gvCompare">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvCompare" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="MenuQuotationCompare" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuQuotationCompare" runat="server" OnTabStripCommand="MenuQuotationCompare_TabStripCommand"></eluc:TabStrip>
        <br clear="all" />
        <table>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblVendor" runat="Server" Text="Vendor"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlVendor" runat="server" RenderMode="Lightweight" EnableLoadOnDemand="True" Width="180px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select"
                        DataTextField="FLDNAME" DataValueField="FLDQUOTATIONID"></telerik:RadComboBox>
                     <asp:LinkButton ID="cmdSelect" ToolTip="Select Vendor" runat="server" ImageAlign="AbsMiddle" Text=".." OnClick="cmdSelect_Click">
                                    <span class="icon"><i class="fas fa-check"></i></span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvCompare" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" ShowFooter="true" EnableViewState="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvCompare_NeedDataSource" OnBatchEditCommand="gvCompare_BatchEditCommand" AllowMultiRowSelection="true"
            OnItemDataBound="gvCompare_ItemDataBound" OnItemCommand="gvCompare_ItemCommand" OnPreRender="gvCompare_PreRender">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="Batch" CommandItemDisplay="Top" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDORDERLINEID,FLDVESSELID">
                <%--<CommandItemSettings ShowAddNewRecordButton ="false" ShowRefreshButton="false" ExportToExcelText="Export" ShowExportToExcelButton="true" />--%>
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
                            <telerik:RadPushButton runat="server" ID="rpExport" AutoPostBack="true" CommandName='<%# RadGrid.ExportToExcelCommandName %>'
                                ToolTip="Export to Excel">
                                <Icon Url="../css/Theme1/images/icon_xls.png"/>
                            </telerik:RadPushButton>
                </CommandItemTemplate>
                <BatchEditingSettings EditType="Cell" />
                <Columns>
                    <%--<telerik:GridTemplateColumn UniqueName="ACTION">
                        <ItemStyle Width="40px" />
                        <HeaderStyle Width="40px" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" ID="cmdEdit"
                                ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save"
                                CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <%--<telerik:GridTemplateColumn UniqueName="SNO" HeaderText="S.No">
                        <HeaderStyle Width="40px" />
                        <ItemStyle Width="40px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSerialNo" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn UniqueName="NUMBER" HeaderText="Number">
                            <HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="NAME" HeaderText="Name">
                            <HeaderStyle Width="150px" />
                        <ItemStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UNIT" >
                            <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Req Qty" UniqueName="REQQTY">
                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuantity" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="App Qty" UniqueName="APPQTY">
                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadNumericTextBox ID="txtOrderQtyEdit" RenderMode="Lightweight" runat="server" MaxLength="22" Width="100%" Style="text-align: right;" Type="Number">
                                    <NumberFormat AllowRounding="false" DecimalSeparator="." DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="COMPONENT" HeaderText="Component">
                            <HeaderStyle Width="100px"/>
                        <ItemStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblComponentNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Requisitions matching your search criteria"
                    PageSizeLabelText="Requisitions per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ExportSettings>
                <Excel Format="Xlsx" />
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" AllowKeyboardNavigation="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                <%--<ClientEvents OnGridCreated="GridCreated" />--%>
            </ClientSettings>
        </telerik:RadGrid>

        <div>
                <b>All Figures in USD.</b>
                <br /><br />
                <table style="width: 400px">
                    <tr>
                        <td style="background-color: yellow; width: 20px">&nbsp;</td>
                        <td>Lowest Unit Price</td>
                    </tr>
                    <tr>
                        <td style="background-color: greenyellow; width: 20px">&nbsp;</td>
                        <td>Lowest Total Price for Fully Quoted Vendor</td>
                    </tr>
                </table>
            </div>
    </form>
</body>
</html>
