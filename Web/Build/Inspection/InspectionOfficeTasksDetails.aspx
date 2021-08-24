<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOfficeTasksDetails.aspx.cs"
    Inherits="InspectionOfficeTasksDetails" %>

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
    <form id="frmPreventiveTasks" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuPreventiveGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuPreventiveGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuInspectionShipboard" runat="server" OnTabStripCommand="MenuInspectionShipboard_TabStripCommand"></eluc:TabStrip>
            <div id="divInspectionType" style="position: relative; z-index: 2">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblToReschedule" runat="server" ForeColor="Blue" Font-Bold="true" Text="To Reschedule,enter the Reschedule Date and Reschedule Reason before Saving."></telerik:RadLabel></font>
                        </td>
                    </tr>
                </table>
                <div style="position: relative">
                    <table cellpadding="4" width="100%">
                        <tr>
                            <td rowspan="2" style="width: 15%;" valign="top">
                                <telerik:RadLabel ID="lblPreventiveAction" runat="server" Text="Preventive Action"></telerik:RadLabel>
                            </td>
                            <td rowspan="2" style="width: 35%">
                                <telerik:RadTextBox ID="txtPreventiveAction" runat="server" CssClass="readonlytextbox" Height="50px"
                                    ReadOnly="true" Rows="4" TextMode="MultiLine" Width="97%" Resize="Both"></telerik:RadTextBox>
                            </td>
                            <td style="width: 15%;" valign="bottom">
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="bottom" colspan="2">
                                <telerik:RadComboBox ID="ddlTaskStatus" runat="server" AppendDataBoundItems="true" Width="240px"
                                    CssClass="input_mandatory" CommandName="TASKSTATUS" AutoPostBack="true" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                        <telerik:RadComboBoxItem Text="Open" Value="0" Selected="True" />
                                        <telerik:RadComboBoxItem Text="Completed" Value="2" />
                                        <telerik:RadComboBoxItem Text="Closed" Value="4" />
                                    </Items>
                                </telerik:RadComboBox>
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
                                <telerik:RadTextBox ID="txtRescheduleReason" runat="server" CssClass="input" Height="50px"
                                    CommandName="RESCHEDULEREASON" Enabled="false" Rows="4" TextMode="MultiLine"
                                    Width="97%" Resize="Both"></telerik:RadTextBox>
                            </td>
                            <td style="width: 15%" valign="bottom">
                                <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target Date"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="bottom" colspan="2">
                                <eluc:Date ID="ucTargetDate" runat="server" DatePicker="true" CssClass="input_mandatory"
                                    commandname="TARGETDATE" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" valign="top">
                                <telerik:RadLabel ID="lblRescheduleDate" runat="server" Text="Reschedule Date"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="top" colspan="2">
                                <eluc:Date ID="ucRescheduleDate" runat="server" DatePicker="true" CssClass="input"
                                    commandname="RESCHEDULEDATE" />
                                <asp:LinkButton ID="cmdReschedule" runat="server" AlternateText="Reschedule History"
                                    ToolTip="Reschedule History">
                                    <span class="icon"><i class="fas fa-history"></i></span>
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 15%;" valign="top">
                                <telerik:RadLabel ID="lblComletionRemarks" runat="server" Text="Completion Remarks"></telerik:RadLabel>
                            </td>
                            <td rowspan="2" style="width: 35%">
                                <telerik:RadTextBox ID="txtCompletionRemarks" runat="server" CssClass="input" Height="50px"
                                    CommandName="COMPLETIONREMARKS" Rows="4" TextMode="MultiLine" Width="97%" Resize="Both"></telerik:RadTextBox>
                            </td>
                            <td style="width: 15%" valign="bottom">
                                <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="bottom" colspan="2">
                                <eluc:Date ID="ucCompletionDate" runat="server" DatePicker="true" CssClass="input"
                                    commandname="COMPLETIONDATE" />
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
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblAttachment" runat="server" Text="Attachment"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <telerik:RadCheckBox ID="chkRescheduleRequired" runat="server" Visible="false" AutoPostBack="true"
                                    CommandName="RESCHEDULEREQUIRED" />
                                <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgEvidence"
                                    runat="server" ToolTip="Upload Evidence" />
                            </td>
                            <td>
                                <eluc:Hard ID="ucVerficationLevel" Visible="false" runat="server" AppendDataBoundItems="true"
                                    CssClass="input" commandname="VERFICATIONLEVEL" Width="250px" HardList='<%# PhoenixRegistersHard.ListHard(1, 195) %>'
                                    HardTypeCode="195" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <b>
                                    <telerik:RadLabel ID="lblDeficiencyDetails" runat="server" Text="Deficiency Details"></telerik:RadLabel></b>
                                <hr style="height: -12px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" valign="top">
                                <telerik:RadLabel ID="lblDeficiencyDescription" runat="server" Text="Deficiency Description"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <telerik:RadTextBox ID="txtDefDesc" runat="server" CssClass="readonlytextbox" Height="50px"
                                    ReadOnly="true" Rows="4" TextMode="MultiLine" Width="97%" Resize="Both"></telerik:RadTextBox>
                            </td>
                            <td style="width: 15%" valign="top">
                                <telerik:RadLabel ID="lblItem" runat="server" Text="Item"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <telerik:RadTextBox ID="txtItem" runat="server" CssClass="readonlytextbox" Height="50px"
                                    ReadOnly="true" Rows="4" TextMode="MultiLine" Width="97%" Resize="Both"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr style="height: -12px" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 15%;" valign="top">
                                <telerik:RadLabel ID="lblCloseOutRemarks" runat="server" Text="Close Out remarks"></telerik:RadLabel>
                            </td>
                            <td rowspan="2" style="width: 35%">
                                <telerik:RadTextBox ID="txtCloseOutRemarks" runat="server" CssClass="input" Height="50px"
                                    CommandName="CLOSEOUTREMARKS" Rows="4" TextMode="MultiLine" Width="97%" Resize="Both"></telerik:RadTextBox>
                            </td>
                            <td style="width: 15%" valign="bottom">
                                <telerik:RadLabel ID="lblCloseOutDate" runat="server" Text="Close Out Date"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="bottom" colspan="2">
                                <eluc:Date ID="ucCloseoutDate" runat="server" CssClass="input" />
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
                            <td colspan="2"></td>
                        </tr>
                    </table>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
