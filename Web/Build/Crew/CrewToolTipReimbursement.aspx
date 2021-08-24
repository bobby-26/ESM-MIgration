<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewToolTipReimbursement.aspx.cs"
    Inherits="PlannedMaintenanceToolTipSurveyRemark" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html>
<head>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
    </telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
    </telerik:RadSkinManager>
    <table style="border: 1px solid black; margin: 5px; border-collapse: collapse;" border="1" cellspacing="2">
        <tr>
            <td>
                <telerik:RadLabel ID="lblHeaderReimbursementDescription" runat="server" Text="Description"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblDescription" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblHeaderChargeableVessel" runat="server" Text="Chargeable Vessel"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblChargeableVessel" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblHeaderBudgetcode" runat="server" Text="Budget Code"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblBudgetCode" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblHeaderModeofPayment" runat="server" Text="Mode of Payment"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblPaymentMode" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblHeaderUnpaid" runat="server" Text="Un Paid"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblUnPaid" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblHeaderVoucher" runat="server" Text="Settlement Voucher Reference"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblVoucher" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>

    </table>
</body>
</html>
