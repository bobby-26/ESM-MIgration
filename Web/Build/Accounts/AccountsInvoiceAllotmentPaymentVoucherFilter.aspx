<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceAllotmentPaymentVoucherFilter.aspx.cs" Inherits="AccountsInvoiceAllotmentPaymentVoucherFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Payment Voucher Filter</title>
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
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVoucherNumberSearch" CssClass="input" OnTextChanged="txtVoucherNumberSearch_TextChanged"
                            Width="200px" ></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" Text="Name" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="200px" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherFromDate" runat="server" Text="Voucher From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadTextBox ID="txtVoucherFromdateSearch" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox>--%>
                        <%--<ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                            Enabled="True" TargetControlID="txtVoucherFromdateSearch" PopupPosition="TopLeft">
                        </ajaxToolkit:CalendarExtender>--%>
                         <eluc:UserControlDate ID="CalendarExtender2" runat="server" ReadOnly="false" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoucherToDate" runat="server" Text="Voucher To Date"></telerik:RadLabel>
                    </td>
                    <td>
                       <%-- <telerik:RadTextBox ID="txtVoucherTodateSearch" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                            Enabled="True" TargetControlID="txtVoucherTodateSearch" PopupPosition="TopLeft">
                        </ajaxToolkit:CalendarExtender>--%>
                         <eluc:UserControlDate ID="CalendarExtender1" runat="server" ReadOnly="false" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherCurrency" runat="server" Text="Voucher Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

