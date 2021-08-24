<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateExtentionUpdate.aspx.cs"
    Inherits="PlannedMaintenanceVesselSurveyCertificateExtentionUpdate" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Extension Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="CertificatesRenewal" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnablePageMethods="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />

        <eluc:TabStrip ID="MenuCertificatesRenewal" runat="server" OnTabStripCommand="MenuCertificatesRenewal_TabStripCommand"></eluc:TabStrip>

        <table id="CertifyDetail" width="70%" cellpadding="1px">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDueDate" runat="server" Text="Extn. Expiry Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtExtnExpiryDate" runat="server" Width="120px" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadLabel ID="lblexpirydate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <eluc:Date ID="txtExpirydate" runat="server" Width="120px" />

                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblExtnSurveyDue" runat="server" Text=" Extn. Survey Due"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtExtnSurveyDue" runat="server" Width="120px" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadLabel ID="lblSurveyDue" runat="server" Text="Survey Due"></telerik:RadLabel>
                    &nbsp;&nbsp;&nbsp;
                    <eluc:Date ID="txtSurveyDue" runat="server" Width="120px" />

                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReason" runat="server" Text="Reason For Extension"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRemarks" runat="server" Width="300px" Height="100px"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFileUpload" runat="server" Text="File Upload"></telerik:RadLabel>
                </td>
                <td>
                    <asp:FileUpload ID="txtFileUpload" runat="server" Width="270px" />
                    <asp:LinkButton runat="server" ID="cmdExtnAtt" Visible="false"
                        ToolTip="Extension Certificate"><span class="icon"><i class="fas fa-paperclip"></i></span></asp:LinkButton>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
