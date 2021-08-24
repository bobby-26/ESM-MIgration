<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdvancePaymentVoucherFilter.aspx.cs"
    Inherits="AccountsAdvancePaymentVoucherFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <br />
        <table width="100%">
            <tr>
                <td>

                    <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtVoucherNumberSearch" MaxLength="200" CssClass="input"
                        Width="150px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMakerId" runat="server" Text="MakerId"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtMakerId" MaxLength="100" CssClass="input"
                        Width="150px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVoucherFromDate" runat="server" Text="Voucher From Date"></telerik:RadLabel>
                </td>
                <td>

                    <eluc:UserControlDate ID="txtVoucherFromdateSearch" runat="server"
                        CssClass="input" />

                </td>
                <td>
                    <telerik:RadLabel ID="lblVoucherToDate" runat="server" Text="Voucher To Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtVoucherTodateSearch" runat="server"
                        CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPaymentCurrency" runat="server" Text="Payment Currency"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                        runat="server" AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text="Supplier Name"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="60px" CssClass="input" ReadOnly="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" Width="200px" CssClass="input" ReadOnly="false"></telerik:RadTextBox>
                        <img runat="server" id="ImgSupplierPickList" style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="40px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
