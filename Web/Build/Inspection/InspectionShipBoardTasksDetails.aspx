<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionShipBoardTasksDetails.aspx.cs"
    Inherits="InspectionShipBoardTasksDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmShipBoardTasks" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"/>
            <eluc:TabStrip ID="MenuShipboardGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuShipboardGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuInspectionShipboard" runat="server" OnTabStripCommand="MenuInspectionShipboard_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToReschedule" runat="server" ForeColor="Blue" Font-Bold="true" Text="To Reschedule,enter the Reschedule Date and Reschedule Reason before Saving."></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="Corrective Action"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtCorrectiveAction" runat="server" CssClass="readonlytextbox" Height="75px"
                            ReadOnly="true" Rows="6" TextMode="MultiLine" Width="95%" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%;" valign="bottom">
                        <telerik:RadLabel runat="server" ID="lblStatus" Text="Status"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Hard ID="ucTaskStatus" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            HardTypeCode="146" Width="240px" ShortNameFilter="OPN,EXR,PSA,CMP,CLD" CommandName="TASKSTATUS" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtReportedByName" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtReportedByDesignation" runat="server" Width="130px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lblRescheduleReason" runat="server" Text="Reschedule Reason"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtRescheduleReason" runat="server" CssClass="input" Height="75px"
                            CommandName="RESCHEDULEREASON" Rows="6" TextMode="MultiLine" Width="90%" Resize="Both"></telerik:RadTextBox>
                        <asp:LinkButton ID="cmdReschedule" runat="server" AlternateText="Reschedule History"
                            ToolTip="Reschedule History">
                            <span class="icon"><i class="fas fa-history"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td style="width: 15%" valign="bottom">
                        <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucTargetDate" runat="server" DatePicker="true" CssClass="input_mandatory"
                            CommandName="TARGETDATE" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblRescheduleDate" runat="server" Text="Reschedule Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <eluc:Date ID="ucRescheduleDate" runat="server" DatePicker="true"
                            CommandName="RESCHEDULEDATE" />
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lblSuperintendentcomments" runat="server" Text="Reschedule Approver Comments"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtSuperintendentcomments" runat="server" CssClass="input" Height="75px"
                            CommandName="SUPERINTENDENTCOMMENTS" Rows="6" TextMode="MultiLine" Width="95%" Resize="Both"></telerik:RadTextBox>
                    </td>

                    <td style="width: 15%" valign="bottom">
                        <telerik:RadLabel ID="lblRescheduleApprovedDate" runat="server" Text="Reschedule Approved Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucApprovedDate" runat="server" DatePicker="true" 
                            CommandName="APPROVEDDATE" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtApprovedByName" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtApprovedByDesignation" runat="server" Width="130px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lblSecondaryApproverComments" runat="server" Text="Secondary Approver Comments"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtSecondaryApproverComments" runat="server" CssClass="input" Height="75px"
                            CommandName="SUPERINTENDENTCOMMENTS" Rows="6" TextMode="MultiLine" Width="95%" Resize="Both"></telerik:RadTextBox>
                    </td>

                    <td style="width: 15%" valign="bottom">
                        <telerik:RadLabel ID="lblSecondaryApprovedDate" runat="server" Text="Secondary Approved Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucSecondaryApprovedDate" runat="server" DatePicker="true"
                            CommandName="APPROVEDDATE" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblSecondaryApprovedBy" runat="server" Text="Secondary Approved By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtSecondaryApprovedByName" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtSecondaryApprovedByDesignation" runat="server" Width="130px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lblCompletionRemarks" runat="server" Text="Completion Remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtCompletionRemarks" runat="server" CssClass="input" Height="75px"
                            Rows="6" TextMode="MultiLine" Width="95%" CommandName="COMPLETIONREMARKS" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%" valign="bottom">
                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucCompletionDate" runat="server" DatePicker="true"
                            CommandName="COMPLETIONDATE" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblCompletedBy" runat="server" Text="Completed By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtCompletedByName" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCompletedByDesignation" runat="server" Width="130px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblVerificationLevel" runat="server" Text="Verification Level"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Hard ID="ucVerficationLevel" runat="server" AppendDataBoundItems="true"
                            CommandName="VERFICATIONLEVEL" Width="220px" HardList='<%# PhoenixRegistersHard.ListHard(1, 195) %>'
                            HardTypeCode="195" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgEvidence"
                            runat="server" ToolTip="Upload Evidence" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblRescheduleRequired" runat="server" Visible="false" Text="Reschedule required"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkRescheduleRequired" runat="server" Visible="false" AutoPostBack="false"
                            OnCheckedChanged="RescheduleRequired_CheckedChanged" CommandName="RESCHEDULEREQUIRED" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblreopentask" runat="server" Text="Re-open Task"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkreopentask" runat="server" AutoPostBack="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblreopenreason" runat="server" Text="Reason for Re-opening the Task"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtreopenreason" runat="server" CssClass="input" Height="75px" Rows="6"
                            TextMode="MultiLine" Width="90%" CommandName="REOPENREASON" Resize="Both"></telerik:RadTextBox>
                        <asp:LinkButton ID="imgReopenhistory" runat="server" AlternateText="Re-Open Task History"
                            ToolTip="Re-Open Task History">
                            <span class="icon"><i class="fas fa-history"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblnewduedate" runat="server" Text="New Due Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtnewduedate" runat="server" DatePicker="true" CommandName="NEWTARGETDATE" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr style="height: -12px" />
                        <b>
                            <telerik:RadLabel ID="lblDeficiencyDetails" runat="server" Text="Deficiency Details"></telerik:RadLabel></b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblDeficiencyDescription" runat="server" Text="Deficiency Description"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtDefDesc" runat="server" CssClass="readonlytextbox" Height="75px"
                            ReadOnly="true" Rows="6" TextMode="MultiLine" Width="95%" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblItem" runat="server" Text="Item"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtItem" runat="server" CssClass="readonlytextbox" Height="75px"
                            ReadOnly="true" Rows="6" TextMode="MultiLine" Width="95%" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblDeficiencyDetail" runat="server" Text="Deficiency Details"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtDefDetails" runat="server" CssClass="readonlytextbox" Height="75px"
                            ReadOnly="true" Rows="6" TextMode="MultiLine" Width="95%" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCheckListRefNo" runat="server" Text="Checklist Ref No."></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtChecklistRefNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="132px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lbLCloseOutRemarks" runat="server" Text="Close Out remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtCloseOutRemarks" runat="server" CssClass="input" Height="75px"
                            CommandName="CLOSEOUTREMARKS" Rows="6" TextMode="MultiLine" Width="95%" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%" valign="bottom">
                        <telerik:RadLabel ID="lblCloseOutDate" runat="server" Text="Close Out Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucCloseoutDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblCloseOutBy" runat="server" Text="Close Out by"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtCloseOutByName" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCloseOutByDesignation" runat="server" Width="130px" CssClass="readonlytextbox"
                            Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
