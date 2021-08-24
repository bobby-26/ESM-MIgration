<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDeficiencyEdit.aspx.cs"
    Inherits="Inspection_InspectionDeficiencyEdit" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register Src="../UserControls/UserControlQuick.ascx" TagName="Quick" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection Deficiency</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function ConfirmClose(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmClose.UniqueID %>", "");
                }
            }
            function ConfirmCancel(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmCancel.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionDeficiency" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuInspectionDeficiency" runat="server" OnTabStripCommand="InspectionDeficiency_TabStripCommand"></eluc:TabStrip>
            <table id="tblInspectionNC" width="100%">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadRadioButtonList ID="rblDeficiencyType" runat="server" Direction="Horizontal"
                            OnTextChanged="DeficiencyType_TextChanged" AutoPostBack="true">
                            <Items>
                                <telerik:ButtonListItem Text="NC" Value="2" />
                                <telerik:ButtonListItem Text="Major NC" Value="1" />
                                <telerik:ButtonListItem Text="Observation" Value="3" />
                                <telerik:ButtonListItem Text="Hi Risk Observation" Value="4" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblIssuedDate" runat="server" Text="Issued"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" OnTextChangedEvent="ucIssueDateEdit_TextChanged"
                            DatePicker="true" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                            AssignedVessels="true" VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="vessel_TextChanged"
                            Width="270px" Enabled="false" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Company ID="ucCompany" runat="server" Enabled="false" Width="100px" 
                            AppendDataBoundItems="true" /> &nbsp;
                        <telerik:RadLabel ID="lblcode" runat="server" Text="Item Code" Width="70px"></telerik:RadLabel>
                        <telerik:RadTextBox ID="txtCompCode" runat="server" Text="" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblSource" runat="server" Text="Source"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlSchedule" runat="server" Width="270px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCheckListReferenceNo" runat="server" Text="CheckList Reference Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtChecklistRef" runat="server" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text="Deficiency Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Quick ID="ucNonConformanceCategory" runat="server" AppendDataBoundItems="true"
                            Width="270px" CssClass="dropdown_mandatory" QuickTypeCode="47" Visible="true" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblChapterNo" runat="server" Text="Chapter No"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtChapterNo" runat="server" CssClass="readonlytextbox" Width="270px"
                            Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblItem" runat="server" Text="Item"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtItem" runat="server" Height="60px" TextMode="MultiLine"
                            Width="270px" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblPSCActionCodeVIRCondition" runat="server" Text="PSC Action Code / VIR Condition"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtKey" runat="server" Width="70px" onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtKeyName" runat="server" CssClass="readonlytextbox" Width="198px"
                            ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtDesc" runat="server" CssClass="input_mandatory" Height="60px"
                            TextMode="MultiLine" Width="270px" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <%--Master's Comments--%>
                        <telerik:RadLabel ID="lblDeficiencyDetails" runat="server" Text="Deficiency Details"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtMasterComments" runat="server" Height="60px" TextMode="MultiLine" Width="270px" Visible="false" Resize="Both"></telerik:RadTextBox>
                        <div id="divDefDetails" runat="server" class="readonlytextbox" style="overflow: auto; width: 269px; height: 60px;">
                            <telerik:RadLabel ID="ltDefDetails" runat="server"></telerik:RadLabel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="Corrective Action"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCorrectiveAction" runat="server" Height="60px"
                            TextMode="MultiLine" Width="270px" Enabled="false" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblInspectorComments" runat="server" Text="Inspector Comments"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtInspectorComments" runat="server" Height="60px"
                            TextMode="MultiLine" Width="271px" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblRCAnotRequired" runat="server" Text="RCA not required"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkRCANotrequired" runat="server" AutoPostBack="true" OnCheckedChanged="chkRCANotrequired_CheckedChanged" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblRCATargetDate" runat="server" Text="RCA Target"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucRcaTargetDate" runat="server" COMMANDNAME="RCATARGETDATE"
                            DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblRCAisCompleted" runat="server" Text="RCA is Completed"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkRCAcompleted" runat="server" Enabled="false" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblRCACompletionDate" runat="server" Text="RCA Completion"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucCompletionDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCopyDetailstoCAR" runat="server" Text="Copy details to CAR"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkCopyCAR" runat="server" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtStatus" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCARNotRequired" runat="server" Text="CAR Not Required"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="4">
                        <telerik:RadCheckBox ID="chkCARNotRequiredYN" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            HardTypeCode="146" ShortNameFilter="OPN,CLD,CAD" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblAuditor" runat="server" Text="Auditor Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtAuditor" runat="server"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblAuditPlace" runat="server" Text="Place of Audit"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtAuditPlace" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblNCDuedate" runat="server" Text="NCN/Obs Due Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucNCDuedate" runat="server" CssClass="input_mandatory" COMMANDNAME="NCDUEDATE"
                            DatePicker="true" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDepartment" runat="server" Text="Auditee/Department"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Department ID="ucAuditDept" runat="server" AppendDataBoundItems="true" 
                            Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReviewRemarks" runat="server" Text="Review Remarks"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtReviewRemarks" runat="server" Height="60px"
                            TextMode="MultiLine" Width="270px" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReviewDate" runat="server" Text="Reviewed Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucReviewDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReviewedby" runat="server" Text="Reviewed By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtReviewedByName" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtReviewedByDesignation" runat="server" Width="108px" CssClass="readonlytextbox"
                            Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCloseOutRemarks" runat="server" Text="Close Out Remarks"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCloseOutRemarks" runat="server" Height="60px"
                            TextMode="MultiLine" Width="270px" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCloseOutDate" runat="server" Text="Close Out"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucCloseoutDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCloseOutBy" runat="server" Text="Close Out By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCloseOutByName" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCloseOutByDesignation" runat="server" Width="108px" CssClass="readonlytextbox"
                            Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCancelReason" runat="Server" Text="Cancel Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCancelReason" runat="server" Height="60px" Rows="4"
                            TextMode="MultiLine" Width="270px" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCancelDate" runat="server" Text="Cancel"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucCancelDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCancelledBy" runat="server" Text="Cancelled By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCancelledByName" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCancelledByDesignation" runat="server" Width="108px" CssClass="readonlytextbox"
                            Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
            <asp:Button ID="ucConfirmClose" runat="server" OnClick="ucConfirmClose_Click" CssClass="hidden" />
            <asp:Button ID="ucConfirmCancel" runat="server" OnClick="ucConfirmCancel_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
