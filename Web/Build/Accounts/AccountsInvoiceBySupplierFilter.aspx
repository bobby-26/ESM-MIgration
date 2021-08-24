<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceBySupplierFilter.aspx.cs" Inherits="AccountsInvoiceBySupplierFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice By Supplier Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvBySupplierFilter" runat="server">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="pnlAddressEntry" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtInvoiceNumberSearch" MaxLength="200" CssClass="input"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceReference" runat="server" Text="Invoice Reference"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSupplierReferenceSearch" MaxLength="100" CssClass="input"
                            Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceFromDate" runat="server" Text="Invoice From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtInvoiceFromdateSearch" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceToDate" runat="server" Text="Invoice To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtInvoiceTodateSearch" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlInvoiceStatus" CssClass="input" AppendDataBoundItems="true" runat="server">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Draft" Value="DRAFT"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Approved" Value="APPROVED"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                                Width="180px">
                            </telerik:RadTextBox>
                            <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <%--<tr>
                        <td>
                            Invoice Currency
                        </td>
                        <td>
                            <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                runat="server" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            Invoice Type
                        </td>
                        <td>
                            <eluc:Hard ID="ddlInvoiceType" runat="server" AppendDataBoundItems="true" AutoPostBack="TRUE"
                                CssClass="input" HardTypeCode="59" Width="300px" />
                        </td>
                    </tr>--%>
                <%--<tr>
                        <td>
                            Received From Date
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtReceivedFromdateSearch" runat="server" CssClass="input" />
                        </td>
                        <td>
                            Received To Date
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtReceivedTodateSearch" runat="server" CssClass="input" />
                        </td>
                    </tr>--%>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

