<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDebitNoteVoucherFilter.aspx.cs"
    Inherits="AccountsDebitNoteVoucherFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

            <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
      
         
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
       <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

        <telerik:RadAjaxPanel runat="server" ID="pnlAddressEntry">
                    <table id="tblExchangeRate">
                        <tr>
                            <td width="10%">
                                <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                            </td>
                            <td width="40%">
                                <telerik:RadTextBox ID="txtVoucherNumberSearch" runat="server" MaxLength="100" CssClass="input"
                                    Width="150px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text="Reference Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtReferenceNumberSearch" runat="server" MaxLength="100" CssClass="input"
                                    Width="150px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <telerik:RadLabel ID="lblVoucherFromDate" runat="server" Text="Voucher From Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="txtVoucherFromdateSearch" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVoucherToDate" runat="server" Text="Voucher To Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="txtVoucherTodateSearch" runat="server" CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <telerik:RadLabel ID="lblLongDescription" runat="server" Text="Long Description"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtLongDescription" runat="server" TextMode="MultiLine" Rows="2"
                                    CssClass="input"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Voucher Status"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ucInvoiceStatus" AppendDataBoundItems="true" CssClass="input" runat="server" />
                            </td>

                        </tr>
                    </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
