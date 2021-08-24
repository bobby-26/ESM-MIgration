<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateRenewal.aspx.cs"
    Inherits="PlannedMaintenanceVesselSurveyCertificateRenewal" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCLQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Certificate Details</title>
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

        <table id="CertifyDetail" width="100%" cellpadding="1px">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInitialAudit" runat="server" Text="Initial Survey/Audit" Visible="false"></telerik:RadLabel>
                </td>
                <td colspan="4">
                    <telerik:RadComboBox ID="ddlInitialSurvey" runat="server" CssClass="dropdown_mandatory"
                        Visible="false" Width="270px">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCertificate" runat="server" Text="Certificate"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCertificate" runat="server" Width="270px" CssClass="readonlytextbox"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblInitialDate" runat="server" Text="Anniversary /Initial Audit Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtInitialDate" runat="server"  AutoPostBack="true"
                        Width="120px" OnTextChangedEvent="chkNoExpiry_CheckedChange" />
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCategory" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="270px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtNumber" runat="server" Width="120px" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblIssueAuthority" runat="server" Text="Issued By"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:AddrType ID="ucIssuingAuthority" runat="server" AddressType="134,137,334,1600" CssClass="dropdown_mandatory" Width="270px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblIssueDate" runat="server" Text="Issue Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucIssueDate" runat="server" CssClass="input_mandatory" Width="120px" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UCPort ID="ddlPort" runat="server" AppendDataBoundItems="true" 
                        SeaportList='<%# PhoenixRegistersSeaport.ListSeaport() %>' Width="270px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucExpiryDate" runat="server"  Width="120px" />
                    &nbsp;&nbsp;<asp:CheckBox ID="chkNoExpiry" runat="server" AutoPostBack="true" OnCheckedChanged="chkNoExpiry_CheckedChange" Text="No Expiry" TextAlign="Left" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNotApplicable" runat="server" Text="Not applicable for this vessel"
                        Width="120px">
                    </telerik:RadLabel>
                </td>
                <td>
                    <asp:CheckBox ID="ChkNotApplicable" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblRemarksType" runat="server" Text="Certificate Status" Width="120px"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UCLQuick ID="ucRemarks" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                        QuickTypeCode="144" Width="120px" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReason" runat="server" Text="Reason why not applicable"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtReason" runat="server"  Width="270px" Height="30px"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblFileUpload" runat="server" Text="File Upload"></telerik:RadLabel>
                </td>
                <td>
                    <asp:FileUpload ID="txtFileUpload" runat="server"  Width="270px" />
                </td>
                <td></td>
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
                <td>
                    <telerik:RadLabel ID="lblAttachYN" runat="server" Text="Verified Y/N "
                        Width="120px">
                    </telerik:RadLabel>
                </td>
                <td>
                    <asp:CheckBox ID="chkVerifyYN" runat="server" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td colspan="5">
                    <hr />
                    <telerik:RadLabel ID="lblLastSurvey" runat="server"><b>Last Audit/Survey</b></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLastSurveyType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlLastSurveyType" runat="server" CssClass="dropdown_mandatory"
                        Width="270px">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lbllastSurveyDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucLastSurveyDate" runat="server" CssClass="input_mandatory" Width="120px" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td colspan="5">
                    <hr />
                    <telerik:RadLabel ID="lblNextSurvey" runat="server"><b>Next Audit/Survey</b></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSurveyType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSurveyType" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="270px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPlanedDate" runat="server" Text="Planned Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtPlanDate" runat="server"  Width="120px" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSurveyorName" runat="server" Text="Name of Auditor / Surveyor"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSurveyorName" runat="server"  Width="270px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSurveyDoneDate" runat="server" Text="Completion Date"
                        Width="120px">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDoneDate" runat="server"  Width="120px" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSurveyPort" runat="server" Text="Port"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UCPort ID="ddlSurveyPort" runat="server" AppendDataBoundItems="true" 
                        SeaportList='<%# PhoenixRegistersSeaport.ListSeaport() %>' Width="270px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPlanRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPlanRemarks" runat="server"  Width="270px" Height="30px"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
        </table>
    </form>
</body>
</html>
