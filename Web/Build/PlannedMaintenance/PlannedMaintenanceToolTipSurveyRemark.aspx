<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceToolTipSurveyRemark.aspx.cs"
    Inherits="PlannedMaintenanceToolTipSurveyRemark" %>

<html>
<head>
    <title></title>
</head>
<body>
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <table style="border: 1px solid black; margin: 5px; border-collapse: collapse;" border="1">
        <tr>
            <td>
                <telerik:RadLabel ID="lblInitialAuditAnnDate" runat="server" Text="Ini. Audit / Ann. Date"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblInitialDate" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblCertificateStatus" runat="server" Text="Status"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblStatus" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblRemark" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblNotapplicableReason" runat="server" Text="Not applicable reason"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblNotApplicable" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblVerifiedByHeader" runat="server" Text="Verified By"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblVerifiedOnheader" runat="server" Text="Verified On"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadLabel ID="lblVerifiedOn" runat="server" Text=""></telerik:RadLabel>
            </td>
        </tr>
    </table>
</body>
</html>
