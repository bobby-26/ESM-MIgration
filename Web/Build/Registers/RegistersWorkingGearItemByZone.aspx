<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersWorkingGearItemByZone.aspx.cs"
    Inherits="RegistersWorkingGearItemByZone" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearType" Src="~/UserControls/UserControlWorkingGearType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearItemType" Src="~/UserControls/UserControlWorkingGearItemType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Size" Src="~/UserControls/UserControlSize.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Working Gear Items</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRegistersworkinggearitem.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkingGearAdditionalItems" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Working Gear Item By Zone"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="" OnClick="cmdHiddenSubmit_Click" />
            <table id="tblConfigureWorkingGearItem" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblZone" runat="server" Text="Zone"></asp:Literal>
                    </td>

                    <td>
                        <eluc:Zone ID="ucZone" runat="server" AutoPostBack="true" />
                    </td>

                    <td>
                        <asp:Literal ID="lblItemName" runat="server" Text="Working Gear Item"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtItemSearch" runat="server" MaxLength="100" CssClass="input" Width="300px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuRegistersWorkingGearItem" runat="server" OnTabStripCommand="RegistersWorkingGearItem_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvRegistersworkinggearitem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvRegistersworkinggearitem_ItemCommand" OnNeedDataSource="gvRegistersworkinggearitem_NeedDataSource"
                OnItemDataBound="gvRegistersworkinggearitem_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" OnDeleteCommand="gvRegistersworkinggearitem_DeleteCommand">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Working Gear Item">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%--<telerik:RadLabel ID="lblworkingGearZoneid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMBYZONEID") %>'></telerik:RadLabel>--%>
                                <telerik:RadLabel ID="lblWorkingGearitemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkingGearName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblworkingGearZoneidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMBYZONEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblItemidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkWorkingGearItemNameedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Stock in Hand (Qty)">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkingGearStockinHandItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtWorkingGearStockinhandedit" IsInteger="false" runat="server" DecimalPlace="2" MaxLength="12" Width="200px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKQUANTITY") %>' />
                                <telerik:RadLabel ID="lblWorkingGearStockinHandEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKQUANTITY") %>'></telerik:RadLabel>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Unit Price">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUnitPriceItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtUnitPricedit" IsInteger="false" runat="server" DecimalPlace="2" MaxLength="12" Width="200px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE") %>'></eluc:Number>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Stock Value (INR)">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockValueItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKVALUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblStockValueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKVALUE") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Action">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>                                
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
