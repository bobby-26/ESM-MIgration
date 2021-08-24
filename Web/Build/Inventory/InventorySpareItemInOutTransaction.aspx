<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareItemInOutTransaction.aspx.cs" Inherits="InventorySpareItemInOutTransaction" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskedNumber" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Item In Out Transaction</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvSpareItemInOutTransaction">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvSpareItemInOutTransaction" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="details" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:TabStrip ID="MenuSpareItemInOutTransaction" runat="server" OnTabStripCommand="SpareItemInOutTransaction_TabStripCommand"></eluc:TabStrip>
            <br clear="all" />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSpareItemNumber" runat="server" Text="Spare Item Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MaskedNumber ID="txtSpareItemNumber" runat="server" Width="200px" MaskText="###.##.##.###" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSpareItemName" runat="server" Text="Spare Item Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSpareItemName" runat="server" Width="200px" />
                    </td>
                </tr>
            </table>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" OnConfirmMesage="ucConfirm_OnClick" Visible="false" />
            <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="gvSpareItemInOutTransaction" DecoratedControls="All" EnableRoundedCorners="true" />
            <eluc:TabStrip ID="MenuGridSpareItemInOutTransaction" runat="server" OnTabStripCommand="MenuGridSpareItemInOutTransaction_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvSpareItemInOutTransaction" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Width="100%"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvSpareItemInOutTransaction_NeedDataSource" AutoGenerateColumns="false"
                OnUpdateCommand="gvSpareItemInOutTransaction_UpdateCommand" OnItemCommand="gvSpareItemInOutTransaction_ItemCommand"
                OnSortCommand="gvSpareItemInOutTransaction_SortCommand" OnItemDataBound="gvSpareItemInOutTransaction_ItemDataBound"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" DataKeyNames="FLDSPAREITEMLOCATIONID"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="10%" AllowSorting="true" SortExpression="FLDNUMBER">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="30%" AllowSorting="true" SortExpression="FLDNAME">
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSpareItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkStockItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Maker Reference"  HeaderStyle-Width="25%" AllowSorting="true" SortExpression="FLDMAKERREFERENCE">
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMakerReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'  ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location" HeaderStyle-Width="15%"  AllowSorting="true" SortExpression="LOCATIONNAME">
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.LOCATIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity" HeaderStyle-Width="7%"  ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtStockItemDispositionQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIONQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtStockItemDispositionQuantityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIONQUANTITY","{0:n0}") %>'
                                    CssClass="gridinput txtNumber" MaxLength="9" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PURCHASEDUNIT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Width="8%" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records found" AlwaysVisible="true" Wrap="false"
                        PageSizeLabelText="Items per page:" />
                </MasterTableView>
                <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br clear="all" />
            <table id="details" runat="server" width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocationStock" runat="server" Text="Location Stock"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrentStockQuantity" runat="server" CssClass="input txtNumber"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTotalStock" runat="server" Text="Total Stock"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTotalStockQuantity" runat="server" CssClass="input txtNumber"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOnOrder" runat="server" Text="On Order"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOnOrderQuantity" runat="server" CssClass="input txtNumber"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
