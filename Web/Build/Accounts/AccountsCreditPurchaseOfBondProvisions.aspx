<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditPurchaseOfBondProvisions.aspx.cs"
    Inherits="AccountsCreditPurchaseOfBondProvisions" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Not Yet Committed</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselCreditPurchase" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuGeneralSub" runat="server" OnTabStripCommand="MenuGeneralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" Width="180px" VesselsOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="true"
                                MaxLength="15" Width="90px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorName" runat="server" ReadOnly="true"
                                Width="200px">
                            </telerik:RadTextBox>
                            <img runat="server" id="ImgShowMakerVendor" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListVendor', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131,967', true); " />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="input hidden" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucCurrency" runat="server" Width="280px"
                            AppendDataBoundItems="true" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrderNo" runat="server" Text="Order No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtOrderNo" MaxLength="200" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrderStatus" runat="server" Text="Order Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" Width="300px"
                            HardTypeCode="41" ShortNameFilter="PEN,RCD,CNL" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="Order Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtfromdate" runat="server" />
                        <eluc:Date ID="ToDate" runat="server" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuOffice" runat="server" OnTabStripCommand="MenuOffice_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCreditPurchase" Height="78%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCreditPurchase_ItemCommand" OnItemDataBound="gvCreditPurchase_ItemDataBound" OnNeedDataSource="gvCreditPurchase_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Order" Name="Order" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Supplier" Name="Supplier" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Invoice" Name="Invoice" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Invoice Amount Discount" Name="Invoice Amount Discount" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStockType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselsupplierid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE")%>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Order">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkOrderNumber" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" ColumnGroupName="Order">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERSTATUSNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Order" AllowSorting="true" SortExpression="FLDORDERDATE">
                            <HeaderStyle Width="13%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsMapped" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMAPPED") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="Supplier">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME")%>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code" ColumnGroupName="Supplier">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupplierCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMappedSupplierCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE")%>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkSupplierCode" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Invoice">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" ColumnGroupName="Invoice">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Before" ColumnGroupName="Invoice Amount Discount">
                            <HeaderStyle Width="20%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmtBeforeDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMTBEFOREDISCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="After" ColumnGroupName="Invoice Amount Discount">
                            <HeaderStyle Width="20%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmtAfterDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMTAFTERDISCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Advance Payment" ImageUrl="<%$ PhoenixTheme:images/generate-po.png %>"
                                    CommandName="COMMITTPO" ID="cmdAdvancePayment"
                                    ToolTip="Committ PO"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Unmap Supplier" ImageUrl="<%$ PhoenixTheme:images/off-signer.png %>"
                                    CommandName="SUPPLIERUNMAP"
                                    ID="ImageButton1" ToolTip="Unmap Supplier" Visible="false"></asp:ImageButton>
                            </ItemTemplate>
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

