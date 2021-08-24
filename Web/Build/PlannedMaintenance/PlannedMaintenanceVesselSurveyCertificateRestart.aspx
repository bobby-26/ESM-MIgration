<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateRestart.aspx.cs"
    Inherits="PlannedMaintenanceVesselSurveyCertificateRestart" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Certificate Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="CertificatesRenewal" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuCertificatesRenewal" runat="server" OnTabStripCommand="MenuCertificatesRenewal_TabStripCommand"></eluc:TabStrip>
        </div>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCertificate" runat="server" Text="Certificate"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCertificate" runat="server" Width="270px" CssClass="readonlytextbox"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSurveyType" runat="server" Text="Type of Survey"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSurveyType" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="270px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDueDate" runat="server" Text="Due Date of Audit / Survey"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtDueDate" runat="server" CssClass="input_mandatory" Width="120px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPlanedDate" runat="server" Text="Planned Date of Audit / Survey"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtPlanDate" runat="server"  Width="120px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSurveyorName" runat="server" Text="Name of Auditor / Surveyor"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSurveyorName" runat="server"  Width="270px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSurveyPort" runat="server" Text="Port of Audit / Survey"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UCPort ID="ddlSurveyPort" runat="server" AppendDataBoundItems="true" 
                        SeaportList='<%# PhoenixRegistersSeaport.ListSeaport() %>' Width="270px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRemaks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRemarks" runat="server"  Width="270px" Height="30px"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
