<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfareManualBillingBilledDetails.aspx.cs" Inherits="AccountsAirfareManualBillingBilledDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AIRFARE MANUAL BILLING</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAirfareManualBilling" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Airfare Manual Billing" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuAirfareBillingMain" runat="server" OnTabStripCommand="MenuAirfareBillingMain_TabStripCommand" Title="Airfare Manual Billing"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuAirfareBilling" runat="server" OnTabStripCommand="MenuAirfareBilling_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table id="Table2" width="100%" style="color: Blue">
                <tr>
                    <td>&nbsp;
                        <telerik:RadLabel ID="lblplease" runat="server" Font-Bold="true"
                            Text=" * Please remember to manually change the Voucher Line Items and the Attachment under Target Company">
                        </telerik:RadLabel>

                    </td>
                </tr>
            </table>
            <table id="tblAirfare" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Purchase Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassengerName" runat="server" Text="Passenger Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassengerName" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuAirfareBillinglist" runat="server" OnTabStripCommand="MenuAirfareBillinglist_TabStripCommand"></eluc:TabStrip>
            <asp:HiddenField ID="hdnScroll" runat="server" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAirfareBilling" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Height="69%" Width="100%" CellPadding="3" OnRowDataBound="gvAirfareBilling_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnRowUpdating="gvAirfareBilling_RowUpdating" OnItemCommand="gvAirfareBilling_ItemCommand"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvAirfareBilling_NeedDataSource"
                EnableViewState="false" AllowSorting="true" OnSorting="gvAirfareBilling_Sorting">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Invoice Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPurchaseInvoiceNumberHeader" Visible="true" runat="server">
                                    <asp:ImageButton runat="server" ID="cmdPurchaseInvoiceNumber" OnClick="cmdSearch_Click"
                                        CommandName="FLDVOUCHERNUMBER" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                        CommandArgument="1" />
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkPurchaseInvoiceNumberHeader" runat="server" CommandName="Sort"
                                    CommandArgument="FLDVOUCHERNUMBER">Purchase Invoice Number&nbsp;</asp:LinkButton>
                                <img id="FLDVOUCHERNUMBER" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurchaseInvoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblInvoiceDateHeader" Visible="true" runat="server">
                                    <asp:ImageButton runat="server" ID="cmdInvoiceDate" OnClick="cmdSearch_Click" CommandName="FLDINVOICEDATE"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkInvoiceDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDINVOICEDATE">Invoice Date&nbsp;</asp:LinkButton>
                                <img id="FLDINVOICEDATE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAgentInvoiceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTINVOICEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInvoiceDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblAgentInvoiceIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTINVOICEID") %>'></telerik:RadLabel>
                                <eluc:Date ID="ucInvoiceDateEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE")  %>'></eluc:Date>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDepartureDateEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE")  %>'></eluc:Date>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passenger Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPassengerNameHeader" Visible="true" runat="server">
                                    <asp:ImageButton runat="server" ID="cmdPassengerName" OnClick="cmdSearch_Click" CommandName="FLDPASSENGERNAME"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkPassengerNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDPASSENGERNAME">Passenger Name&nbsp;</asp:LinkButton>
                                <img id="FLDPASSENGERNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassengerName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPassengerNameEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME")  %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sector 1">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSector1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTOR1") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSector1Edit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTOR1")  %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sector 2">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSector2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTOR2") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSector2Edit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTOR2")  %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sector 3">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSector3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTOR3") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSector3Edit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTOR3")  %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sector 4">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSector4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTOR4") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSector4Edit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTOR4")  %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Code – Account Code Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODEDESCRIPTIONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrdernumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Tax (USD)">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTaxEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX","{0:n2}")  %>'></telerik:RadTextBox>
                                <%--      <ajaxtoolkit:maskededitextender id="MaskTax" runat="server" targetcontrolid="txtTaxEdit"
                                    oninvalidcssclass="MaskedEditError" mask="999,999,999.99" masktype="Number" inputdirection="RightToLeft"
                                    autocomplete="false">
                                        </ajaxtoolkit:maskededitextender>--%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charged Fare">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChargedFare" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARGEDFARE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revenue Voucher No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevenueVoucherNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVENUEVOUCHERNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target Company">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTargetCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETCOMPANY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target Company Purchase Invoice Voucher">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTargetCompanyPurchase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETCOPURCHASEINVOICENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="Edit" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Bill" ImageUrl="<%$ PhoenixTheme:images/priority_invoice.png %>"
                                    CommandName="Bill" CommandArgument='<%# Container.DataItem %>' ID="cmdBill"
                                    ToolTip="Generate Billing"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItem %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
