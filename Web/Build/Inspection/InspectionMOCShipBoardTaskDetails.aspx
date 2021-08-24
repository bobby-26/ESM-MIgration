<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCShipBoardTaskDetails.aspx.cs"
    Inherits="InspectionMOCShipBoardTaskDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ship Board Tasks</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmShipBoardTasks" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:TabStrip ID="MenuShipboardGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuShipboardGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuInspectionShipboard" runat="server" OnTabStripCommand="MenuInspectionShipboard_TabStripCommand"></eluc:TabStrip>
            <div id="divInspectionType" style="position: relative; z-index: 2">
                <table width="100%">
                    <tr>
                        <td>
                            <font color="blue">
                                <telerik:RadLabel ID="lblToReschedule" runat="server" Text="To Reschedule,enter the Reschedule Date and Reschedule Reason before Saving.">
                                </telerik:RadLabel>
                            </font>
                        </td>
                    </tr>
                </table>
                <div style="position: relative">
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td rowspan="2" style="width: 15%;" valign="top">
                                <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="Corrective Action">
                                </telerik:RadLabel>
                            </td>
                            <td rowspan="2" style="width: 35%">
                                <telerik:RadTextBox ID="txtCorrectiveAction" runat="server" CssClass="readonlytextbox"
                                    Height="50px" ReadOnly="true" Rows="4" TextMode="MultiLine" Width="95%">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 15%;" valign="bottom">
                                <telerik:RadLabel runat="server" ID="lblStatus" Text="Status">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="bottom" colspan="2">
                                <eluc:Hard ID="ucTaskStatus" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    HardTypeCode="146" Width="150px" ShortNameFilter="OPN,CMP,CLD" CommandName="TASKSTATUS" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" valign="top">
                                <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported By">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="top" colspan="2">
                                <telerik:RadTextBox ID="txtReportedByName" runat="server" Width="150px" CssClass="readonlytextbox"
                                    Enabled="false" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 15%;" valign="top">
                                <telerik:RadLabel ID="lblRescheduleReason" runat="server" Text="Reschedule Reason">
                                </telerik:RadLabel>
                            </td>
                            <td rowspan="2" style="width: 35%">
                                <telerik:RadTextBox ID="txtRescheduleReason" runat="server" CssClass="input" Height="50px"
                                    CommandName="RESCHEDULEREASON" Rows="4" TextMode="MultiLine" Width="90%">
                                </telerik:RadTextBox>
                                <asp:LinkButton runat="server" ID="cmdReschedule" ToolTip="Reschedule History"><span class="icon"><i class="fas fa-user"></i></span>
                                </asp:LinkButton>
                                <%--                            <asp:ImageButton ID="cmdReschedule" runat="server" AlternateText="Reschedule History"
                                ToolTip="Reschedule History" ImageUrl="<%$ PhoenixTheme:images/showlist.png %>" />--%>
                            </td>
                            <td style="width: 15%" valign="bottom">
                                <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target Date">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="bottom" colspan="2">
                                <eluc:Date ID="ucTargetDate" runat="server" DatePicker="true" CssClass="input_mandatory"
                                    Enabled="false" CommandName="TARGETDATE" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" valign="top">
                                <telerik:RadLabel ID="lblRescheduleDate" runat="server" Text="Reschedule Date">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="top" colspan="2">
                                <eluc:Date ID="ucRescheduleDate" runat="server" DatePicker="true" CssClass="input"
                                    CommandName="RESCHEDULEDATE" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 15%;" valign="top">
                                <telerik:RadLabel ID="lblCompletionRemarks" runat="server" Text="Completion Remarks">
                                </telerik:RadLabel>
                            </td>
                            <td rowspan="2" style="width: 35%">
                                <telerik:RadTextBox ID="txtCompletionRemarks" runat="server" CssClass="input" Height="50px"
                                    Rows="4" TextMode="MultiLine" Width="95%" CommandName="COMPLETIONREMARKS">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 15%" valign="bottom">
                                <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="Completion Date">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="bottom" colspan="2">
                                <eluc:Date ID="ucCompletionDate" runat="server" DatePicker="true" CssClass="input"
                                    CommandName="COMPLETIONDATE" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" valign="top">
                                <telerik:RadLabel ID="lblCompletedBy" runat="server" Text="Completed By">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="top" colspan="2">
                                <telerik:RadTextBox ID="txtCompletedByName" runat="server" Width="150px" CssClass="readonlytextbox"
                                    Enabled="false" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblVerificationLevel" runat="server" Text="Verification Level">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <eluc:Hard ID="ucVerficationLevel" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    CommandName="VERFICATIONLEVEL" Width="220px" HardList='<%# PhoenixRegistersHard.ListHard(1, 195) %>'
                                    HardTypeCode="195" />
                                <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgEvidence"
                                    runat="server" ToolTip="Upload Evidence" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblRescheduleRequired" runat="server" Visible="false" Text="Reschedule required">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <asp:CheckBox ID="chkRescheduleRequired" runat="server" Visible="false" AutoPostBack="false"
                                    OnCheckedChanged="RescheduleRequired_CheckedChanged" CommandName="RESCHEDULEREQUIRED" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr style="height: -12px" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 15%;" valign="top">
                                <telerik:RadLabel ID="lbLCloseOutRemarks" runat="server" Text="Close Out remarks">
                                </telerik:RadLabel>
                            </td>
                            <td rowspan="2" style="width: 35%">
                                <telerik:RadTextBox ID="txtCloseOutRemarks" runat="server" CssClass="input" Height="50px"
                                    Rows="4" TextMode="MultiLine" Width="95%">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 15%" valign="bottom">
                                <telerik:RadLabel ID="lblCloseOutDate" runat="server" Text="Close Out Date">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="bottom" colspan="2">
                                <eluc:Date ID="ucCloseoutDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" valign="top">
                                <telerik:RadLabel ID="lblCloseOutBy" runat="server" Text="Close Out by">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 35%" valign="top" colspan="2">
                                <telerik:RadTextBox ID="txtCloseOutByName" runat="server" Width="150px" CssClass="readonlytextbox"
                                    Enabled="false">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
