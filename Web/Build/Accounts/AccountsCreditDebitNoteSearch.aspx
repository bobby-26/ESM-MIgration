<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditDebitNoteSearch.aspx.cs"
    Inherits="AccountsCreditDebitNoteSearch" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        </telerik:RadCodeBlock>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="pnlAddressEntry" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuFilterMain" runat="server" OnTabStripCommand="MenuFilterMain_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1" style="z-index: 2;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false" Width="60px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false" Width="180px"></telerik:RadTextBox>
                            <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCreditRegisterNo" runat="server" Text="Credit Note Register No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCreditRegisterNo" MaxLength="200" CssClass="input"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVendorCreditNoteNo" runat="server" Text="Vendor Credit Note No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVendorCreditNoteNo" MaxLength="200" CssClass="input" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" AutoPostBack="TRUE"
                            CssClass="input" HardTypeCode="237" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReceivedFromDate" runat="server" Text="Received From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucReceivedFromDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReceivedToDate" runat="server" Text="ReceivedToDate"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucReceivedToDate" runat="server" CssClass="input" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucFromDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucToDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVoucherNumber" MaxLength="200" CssClass="input" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td />
                    <td />
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
