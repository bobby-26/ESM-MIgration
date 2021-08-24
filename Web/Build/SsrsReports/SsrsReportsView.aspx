<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SsrsReportsView.aspx.cs" Inherits="SSRSReports_SsrsReportsView" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DisplayMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="../UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PurchaseConfirmMessage" Src="../UserControls/UserControlConfirmMessagePurchaseSendMail.ascx" %>
<%@ Register Src="../UserControls/UserControlCurrency.ascx" TagName="Currency" TagPrefix="eluc" %>
<%@ Register TagName="Status" TagPrefix="eluc" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Src="../UserControls/UserControlHard.ascx" TagName="UserControlHard"
    TagPrefix="eluc" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ReportView</title>
    <telerik:RadCodeBlock runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/PhoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/PhoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:TabStrip ID="OrderExportToPDF" runat="server" OnTabStripCommand="OrderExportToPDF_TabStripCommand" Title="Reports"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <telerik:RadAjaxPanel runat="server" ID="pnlOrderForm">

            <eluc:DisplayMessage ID="ucConfirm" runat="server" Text="" Visible="false" />

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td align="left">
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="100%"
                            ZoomMode="PageWidth" ShowBackButton="true" SizeToReportContent="True" OnLoad="ReportViewer_OnLoad" ShowRefreshButton="false">
                        </rsweb:ReportViewer>
                    </td>
                </tr>
            </table>
            <eluc:ConfirmMessage runat="server" ID="ucConfirmSent" Visible="false" Text="" OnConfirmMesage="ucConfirmSent_OnClick"></eluc:ConfirmMessage>
            <%--                <eluc:PurchaseConfirmMessage runat="server" ID="ucPurchaseConfirmSent" Visible="false" Text="" OnConfirmMesage="ucpurchaseConfirmSent_OnClick" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
