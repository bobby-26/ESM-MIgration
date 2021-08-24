<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfareManualBillingDetails.aspx.cs" Inherits="Accounts_AccountsAirfareManualBillingDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>

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
            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" Visible="false" />
            <eluc:TabStrip ID="MenuAirfareBillingMain" runat="server" OnTabStripCommand="MenuAirfareBillingMain_TabStripCommand" Title="Airfare Manual Billing"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuAirfareBilling" runat="server" OnTabStripCommand="MenuAirfareBilling_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuBill" runat="server" OnTabStripCommand="MenuBill_TabStripCommand"></eluc:TabStrip>
            <table id="tblAirfare" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployee" runat="server" Text="Employee/File Number:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployee" runat="server" CssClass="readonlytextbox" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice No:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassengerName" runat="server" Text="Passenger Name:"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkPassengerName" runat="server"></asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceDate" runat="server" Text="Invoice Date:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceDate" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepartureDate" runat="server" Text="Departure Date:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDepartureDate" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAgentName" runat="server" Text="Agent Name:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAgentName" runat="server" CssClass="readonlytextbox" Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSector1" runat="server" Text="Sector 1:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSector1" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTicketNo" runat="server" Text="Ticket No:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTicketNo" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSector2" runat="server" Text="Sector 2:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSector2" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselChargeable" runat="server" Text="Vessel Chargeable:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselChargeable" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSector3" runat="server" Text="Sector 3:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSector3" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSector4" runat="server" Text="Sector 4:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSector4" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <table id="tblChargingDetails" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWriteOff" runat="server" Text="Write-off to Aviation Income"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkWriteOff" runat="server" AutoPostBack="true" OnCheckedChanged="chkWriteOff_OnCheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <telerik:RadLabel ID="lblAccount" runat="server" Text="Account to be Charged"></telerik:RadLabel>
                        </br>
                                    <br />
                        <telerik:RadLabel ID="lblSubAccount" runat="server" Text="Sub Account"></telerik:RadLabel>
                        </br>
                    </td>

                    <td rowspan="2">
                        <span id="spnPickListExpenseAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                MaxLength="20" Width="20%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory" OnTextChanged="txtAccountDescription_OnTextChanged"
                                ReadOnly="false" MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx',true); " />
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountSource" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountUsage" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox>
                            <br />
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListSubAccount.aspx?mode=custom',true); " />
                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td rowspan="2">
                        <telerik:RadLabel ID="lblOwnerBudget" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                        </br>
                                    <br />
                        <telerik:RadLabel ID="lblBillToCompany" runat="server" Text="Bill To Company"></telerik:RadLabel>
                        </br>
                    </td>
                    <td rowspan="2">
                        <span id="spnPickListOwnerBudgetEdit">
                            <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" CssClass="input"
                                MaxLength="20" Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                            </br>
                                        </br>
                                        <eluc:UserControlCompany ID="ddlBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                            CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                        </span>

                    </td>
                </tr>
            </table>
            <hr />
            <telerik:RadLabel ID="lbllist" runat="server" Font-Bold="true" Text=" List of Flight"></telerik:RadLabel>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvFlightList" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvFlightList_ItemCommand"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvFlightList_NeedDataSource">
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
                        <telerik:GridTemplateColumn HeaderText="Origin">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblOrigin" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDestination" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDepDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblArrivalDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceNo" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblInvDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblTicket" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Agent Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblAgent" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceStatus" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Billing Voucher">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblBillingVoucher" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETCOPURCHASEINVOICENO") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Cancelled">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceCancelled" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECANCELYN") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="false" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
