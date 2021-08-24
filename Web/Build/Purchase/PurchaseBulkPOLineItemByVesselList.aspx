<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBulkPOLineItemByVesselList.aspx.cs" Inherits="PurchaseBulkPOLineItemByVesselList" EnableEventValidation="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabstelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DecimalNumber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerBudgetGroup" Src="~/UserControls/UserControlOwnerBudgetGroup.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Line Item By Vessel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvBulkPOLineItem");
            grid._gridDataDiv.style.height = (browserHeight / 2 - 150) + "px";

            var grid2 = $find("gvBulkPOLineItemByVessel");
            grid2._gridDataDiv.style.height = (browserHeight / 2 - 150) + "px";
        }
        function pageLoad() {
            PaneResized();
        }

    </script>
</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="frmOwnerBudgetCode" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click1" CssClass="hidden" />

        <eluc:TabStrip ID="MenuBulkPO" runat="server" OnTabStripCommand="MenuBulkPO_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvBulkPOLineItem" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvBulkPOLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvBulkPOLineItem_NeedDataSource" OnItemCommand="gvBulkPOLineItem_ItemCommand" OnSortCommand="gvBulkPOLineItem_SortCommand"
            GroupingEnabled="false" EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDLINEITEMID">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Item Name" AllowSorting="true" SortExpression="FLDLINEITEMNAME" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkItemName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Item Number" HeaderStyle-Width="70px" AllowSorting="true" SortExpression="FLDLINEITEMNUMBER">
                        <ItemTemplate>
                            <asp:Label ID="lblItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMNUMBER") %>'></asp:Label>
                            <asp:Label ID="lblLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Budget Code" AllowSorting="true" SortExpression="FLDBUDGETCODE" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Unit" AllowSorting="true" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Ordered Quantity" AllowSorting="true" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERQUANTITY") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Received Quantity" AllowSorting="true" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblReceivedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Price" AllowSorting="true" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE","{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="true" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
        <br />

        <eluc:TabStrip ID="MenuVessel" runat="server" OnTabStripCommand="MenuVessel_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvBulkPOLineItemByVessel" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvBulkPOLineItemByVessel" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvBulkPOLineItemByVessel_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemDataBound="gvBulkPOLineItemByVessel_ItemDataBound" OnItemCommand="gvBulkPOLineItemByVessel_ItemCommand" OnSortCommand="gvBulkPOLineItemByVessel_SortCommand"
            OnUpdateCommand="gvBulkPOLineItemByVessel_UpdateCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDLINEITEMBYVESSELID">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="true" SortExpression="FLDLINEITEMNAME">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                            <asp:Label ID="lblLineItemByVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMBYVESSELID") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Account Code" HeaderStyle-Width="115px" AllowSorting="true" SortExpression="FLDLINEITEMNUMBER">
                        <ItemTemplate>
                            <asp:Label ID="lblAccountCodeItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTDESCRIPTION") %>'></asp:Label>
                            <asp:Label ID="lblAccountId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" Width="100px"
                                DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" OnSelectedIndexChanged="ddlAccountDetails_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Budget Code" AllowSorting="true" SortExpression="FLDBUDGETCODE" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span id="spnPickListMainBudgetEdit">
                                <asp:TextBox ID="txtBudgetCodeEdit" runat="server" Width="60px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:TextBox>
                                <asp:ImageButton ID="btnShowBudgetEdit" runat="server" OnClick="cmdHiddenPick_Click" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="AbsMiddle" Text=".." />
                                <asp:TextBox ID="txtBudgetNameEdit" runat="server" CssClass="hidden" Enabled="False" Width="0px"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetIdEdit" runat="server" Width="0px" AutoPostBack="true" OnTextChanged="txtBudgetIdEdit_TextChanged" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></asp:TextBox>
                                <asp:TextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                            </span>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Owner Budget Code" AllowSorting="true" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblOwnerBudgetCodeItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></asp:Label>
                            <asp:Label ID="lblOwnerBudgetMapID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEMAPID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblOwnerBudgetCodeID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUPID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span id="spnPickListOwnerBudgetEdit">
                                <asp:TextBox ID="txtOwnerBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>' Width="60px"
                                    MaxLength="20" CssClass="input"></asp:TextBox>
                                <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." />
                                <asp:TextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"
                                    Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="hidden"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEMAPID") %>'></asp:TextBox>
                                <asp:TextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                            </span>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Form No" AllowSorting="true" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblFormNO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Order Quantity" AllowSorting="true" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucOrderQtyEdit" runat="server" CssClass="gridinput" MaxLength="7" IsInteger="true"
                                IsPositive="true" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Received Quantity" AllowSorting="true" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblReceivedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucReceivedQtyEdit" runat="server" CssClass="gridinput" MaxLength="7" IsInteger="true"
                                IsPositive="true" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Unit Price" AllowSorting="true" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE","{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblPriceEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE") %>' Visible="false"></asp:Label>
                            <eluc:Number ID="ucPriceEdit" runat="server" Width="120px" CssClass="input_mandatory txtNumber"
                                MaxLength="9" IsPositive="true" DecimalPlace="4" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE","{0:n2}") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="true" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
