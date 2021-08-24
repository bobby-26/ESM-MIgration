<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfareInvoiceAdminFilter.aspx.cs" Inherits="AccountsAirfareInvoiceAdminFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Title runat="server" ID="lblCaption" Text="Airfare Invoice Admin Filter" ShowMenu="True" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand" Title="Airfare Invoice Admin Filter"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequisitionNo" runat="server" Text="Request No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRequestNo" runat="server" ></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNo" runat="server" Text="Invoice No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNo" runat="server" ></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTicketNo" runat="server" Text="Ticket No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtTicketNo" ></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassengerName" runat="server" Text="Passsenger Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPassengerName" ></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepartureFrom" runat="server" Text="Departure Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucDepartureFrom" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblArrivalFrom" runat="server" Text="Arrival Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucArrivalFrom" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepartureTo" runat="server" Text="Departure Date TO"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucDepartureTO" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblArrivalTO" runat="server" Text="Arrival Date TO"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucArrivalTO" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server"  ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorName" runat="server"  ReadOnly="false"
                                Width="150px">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="imgbtnShowSupplier" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true);" />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListCreditAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" 
                                MaxLength="20" Width="60px">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="hidden" Enabled="false"
                                MaxLength="50" Width="0px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="hidden" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
