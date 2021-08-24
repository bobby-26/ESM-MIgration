<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDirectIncidentGeneral.aspx.cs"
    Inherits="InspectionDirectIncidentGeneral" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlInspectionDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersInspectionIncident" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Title runat="server" ID="ucTitle" Text="General" ShowMenu="true" Visible="false"></eluc:Title>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuInspectionGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuInspectionGeneral_TabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuInspectionIncident" runat="server" OnTabStripCommand="InspectionIncident_TabStripCommand"></eluc:TabStrip>
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNote" runat="server" Text="Note:<br>1. Review category cannot be changed once an 'Incident' or 'Near Miss' or 'Non conformity' or 'Observation' or 'Crew Complaint' is raised."
                        CssClass="guideline_text">
                    </telerik:RadLabel>
                </td>
            </tr>
        </table>
        <table cellpadding="4" style="width: 100%">
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel / Office"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadTextBox ID="txtVesselName" Width="150px" runat="server" CssClass="readonlytextbox"
                        Enabled="false">
                    </telerik:RadTextBox>
                </td>
                <td style="width: 20px"></td>
                <td style="width: 30px"></td>
            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lbLReviewCategory" runat="server" Text="Review Category"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <eluc:Quick ID="ucReviewCategory" Width="150px" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="89" CssClass="input_mandatory" />
                </td>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status  "></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadTextBox ID="txtStatus" runat="server" ReadOnly="true" Width="150px" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblDetails" runat="server" Text="Details"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtInvestigationAndEvidence" runat="server" CssClass="input" ReadOnly="true" Resize="Both"
                        Height="150px" Rows="50" TextMode="MultiLine" Width="95%" OnTextChanged="txtInvestigationAndEvidence_TextChanged">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblActiontobetaken" runat="server" Text="Action to be taken"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtAction" runat="server" CssClass="input" Height="50px" Rows="50" Resize="Both"
                        TextMode="MultiLine" Width="95%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Height="50px" Rows="50" Resize="Both"
                        TextMode="MultiLine" Width="95%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblEvidenceRequired" runat="server" Text="Evidence Required"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadCheckBox ID="chkEvidenceRequired" runat="server" />
                    <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgEvidence"
                        Visible="false" runat="server" ToolTip="Upload Evidence" />
                </td>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblAssignedTo" runat="server" Text="Assigned to"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <eluc:Department ID="ucDept" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                        AutoPostBack="true" OnTextChangedEvent="ucDept_Changed" />
                    <span id="spnPIC" visible="false">
                        <telerik:RadTextBox ID="txtPICName" runat="server" CssClass="input" Width="100px" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtPICDesignation" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                        <img id="imgPIC" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;"
                            visible="false" />
                        <telerik:RadTextBox ID="txtPIC" runat="server" CssClass="hidden" MaxLength="50" Width="0px"></telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtPICEmailHiddenAdd" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton ID="imgSupdtEventFeedback" OnClick="SupdtFeedback_Click"
                        Visible="true" CommandName="SUPDTEVENTFEEDBACK" runat="server">
                    <span class="icon"><i class="fas fa-user-tie"></i></span>
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblDueDate" runat="server" Text="Due Date"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <eluc:Date ID="ucDueDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblCOmpletionDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <eluc:Date ID="ucCompletiondate" runat="server" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblClosedNoFutherActionRequired" runat="server" Text="Closed, No further action required"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkClosed" runat="server" />
                </td>
                <td style="width: 30px">
                    <telerik:RadLabel ID="lblCloseOutDate" runat="server" Text="Close Out Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucCloseOutDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblCloaseOutRemarks" runat="server" Text="Close Out Remarks"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadTextBox ID="txtCloseOutRemarks" runat="server" CssClass="input" Height="50px" Resize="Both"
                        Rows="4" TextMode="MultiLine" Width="97%">
                    </telerik:RadTextBox>
                </td>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblCloseOutBy" runat="server" Text="Close Out By"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadTextBox ID="txtCloseOutByName" runat="server" Width="150px" CssClass="readonlytextbox"
                        Enabled="false">
                    </telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtCloseOutByDesignation" runat="server" Width="130px" CssClass="readonlytextbox"
                        Enabled="false">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblCancelReason" runat="server" Text="Cancel Reason"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadTextBox ID="txtCancelReason" runat="server" CssClass="input" Height="50px" Rows="4" Resize="Both"
                        TextMode="MultiLine" Width="97%">
                    </telerik:RadTextBox>
                </td>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblCancelDate" runat="server" Text="Cancel Date"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <eluc:Date ID="ucCancelDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblCancelledBy" runat="server" Text="Cancelled By"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadTextBox ID="txtCancelledByName" runat="server" Width="145px" CssClass="readonlytextbox"
                        Enabled="false">
                    </telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtCancelledByDesignation" runat="server" Width="100px" CssClass="readonlytextbox"
                        Enabled="false">
                    </telerik:RadTextBox>
                </td>
                <td colspan="2"></td>
            </tr>
        </table>
        <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
    </form>
</body>
</html>
