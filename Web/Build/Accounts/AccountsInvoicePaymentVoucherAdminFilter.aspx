<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoicePaymentVoucherAdminFilter.aspx.cs"
    Inherits="AccountsInvoicePaymentVoucherAdminFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <telerik:RadLabel runat="server" ID="lblCaption" Font-Bold="true" Text="Invoice Payment Voucher Filter " Visible="false"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand" Title="Invoice Payment Voucher Filter"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVoucherNumberSearch" MaxLength="200"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtMakerCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="80px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtMakerName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="200" Width="120px">
                            </telerik:RadTextBox>
                            <img runat="server" id="ImgShowMaker" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=', true); " />
                            <telerik:RadTextBox ID="txtMakerId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <%--                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Year"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ddlYearlist" runat="server" QuickTypeCode="55" AppendDataBoundItems="true"
                                AutoPostBack="true" CssClass="input_mandatory" OnTextChangedEvent="ucMonthHard_Changed" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblmonth" runat="server" Text="Month"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucMonthHard" runat="server" HardTypeCode="55" Width="20%" AppendDataBoundItems="true"
                                AutoPostBack="true" CssClass="input_mandatory" SortByShortName="true" OnTextChangedEvent="ucMonthHard_Changed" />
                        </td>
                    </tr>--%>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherFromDate" runat="server" Text="Voucher From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtVoucherFromdateSearch" runat="server" />
                        <%--                            <telerik:RadTextBox ID="txtVoucherFromdateSearch" runat="server" Width="57px" ></telerik:RadTextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtVoucherFromdateSearch" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoucherToDate" runat="server" Text="Voucher To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtVoucherTodateSearch" runat="server" />
                        <%--                            <telerik:RadTextBox ID="txtVoucherTodateSearch" runat="server" Width="57px" ></telerik:RadTextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtVoucherTodateSearch" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherCurrency" runat="server" Text="Voucher Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            runat="server" AppendDataBoundItems="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemittancenotGenerated" runat="server" Text="Remittance not Generated"></telerik:RadLabel>
                    </td>
                    <td>
                        <input type="checkbox" id="chkShowRemittancenotGenerated" value="0" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text="Voucher Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlVoucherStatus" runat="server" AppendDataBoundItems="true" Width="150px"
                            HardTypeCode="15" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Visible="false" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" MaxLength="200"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurchaseInvoiceVoucherNumber" runat="server" Text="Purchase Invoice Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPurchaseInvoiceVoucherNumber" runat="server"
                            MaxLength="200" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSource" runat="server" Text="Source"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlSource" runat="server" AppendDataBoundItems="true" Width="150px"
                            HardTypeCode="221" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentType" runat="server" Text="Payment Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlType" runat="server" AppendDataBoundItems="true" Width="150px"
                            HardTypeCode="213" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReportnottaken" runat="server" Text="Report not taken"></telerik:RadLabel>
                    </td>
                    <td>
                        <input type="checkbox" id="chkReportNotTaken" value="0" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
