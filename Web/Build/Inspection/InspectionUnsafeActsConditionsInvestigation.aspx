<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionUnsafeActsConditionsInvestigation.aspx.cs"
    Inherits="InspectionUnsafeActsConditionsInvestigation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report an Incident</title>
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
        <style type="text/css">
        .RadCheckBox {
            width: 99% !important;
        }
        .rbText {
            text-align: left;
            width: 89% !important;
        }

        .rbVerticalList {
            width: 32% !important;
        }
    </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuInspectionGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuInspectionGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuInspectionIncident" runat="server" OnTabStripCommand="InspectionIncident_TabStripCommand"></eluc:TabStrip>
            <table runat="server" cellpadding="4" width="100%">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="input" Enabled="false" Width="240px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input" Enabled="false" Width="240px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucDate" runat="server" DatePicker="true" CssClass="input" ReadOnly="true"
                            Enabled="false" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTime" runat="server" Text="Time"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTimePicker ID="txtTimeOfIncident" runat="server" Width="75px" Enabled="false"></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Hard ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            HardTypeCode="208" AutoPostBack="true" OnTextChangedEvent="ucCategory_TextChanged" Width="240px" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Sub-category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlSubcategory" runat="server" CssClass="input_mandatory" Width="240px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSubcategory_SelectedIndexChanged" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported by"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Enabled="false" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="input" Enabled="false" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Hard ID="ucActStatus" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            HardTypeCode="146" ShortNameFilter="OPN,CMP,CLD,CAD" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGFTBasicFactor" runat="server" Text="GFT Basic Factor"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlGFTBasicFactor" runat="server" CssClass="input" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlGFTBasicFactor_Changed" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGFTRootCause" runat="server" Text="GFT Root Cause"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlGFTRootCause" runat="server" CssClass="input" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                    <td></td>
                    <td style="width: 35%">
                        <asp:LinkButton ID="lnkQ5Report" runat="server" Text="Q5 Report"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblHeader" runat="server" Font-Bold="true" Text="Comprehensive Description of Near Miss"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtInvestigationAndEvidence" runat="server" CssClass="input" ReadOnly="true"
                            Enabled="false" Height="150px" Rows="50" TextMode="MultiLine" Width="100%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblHeader1" runat="server" Font-Bold="true" Text="Root Cause (To be filled in by the Safety Officer in agreement with Master)"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadCheckBoxList ID="cblRootCause" runat="server" Columns="3">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCorrectiveActionTaken" runat="server" Text="Corrective Action Taken"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCorrectiveActionTaken" runat="server" CssClass="input" Height="50px"
                            Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblMasterComments" runat="server" Text="Master Comments"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtMasterComments" runat="server" CssClass="input" Height="50px"
                            Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblOfficeComments" runat="server" Text="Office Comments"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtOfficeComments" runat="server" CssClass="input" Height="50px"
                            Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblMasterName" runat="server" Text="Master Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtMasterName" runat="server" Width="150px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblOfficeCommentsUpdatedBy" runat="server" Text="Office comments updated by"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtOfficeUserName" runat="server" Width="150px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtOfficeUserDesignation" runat="server" Width="130px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblOfficeCommentsCloseOutRemarks" runat="server" Text="Office comments & Close Out remarks"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCloseOutRemarks" runat="server" CssClass="input" Height="50px"
                            Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCloseOutDate" runat="server" Text="Close Out Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucCloseoutDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCloseOutBy" runat="server" Text="Close Out by"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCloseOutByName" runat="server" Width="180px" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCloseOutByDesignation" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
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
                        <telerik:RadLabel ID="lblCancelReason" runat="server" Text="Cancel Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCancelReason" runat="server" CssClass="input" Height="50px" Rows="4"
                            TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCancelDate" runat="server" Text="Cancel Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucCancelDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCancelledBy" runat="server" Text="Cancelled By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCancelledByName" runat="server" Width="180px" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCancelledByDesignation" runat="server" Width="160px" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
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
                        <telerik:RadLabel ID="lblIncidentRaisedYN" runat="server" Text="Incident / Near Miss Raised YN"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkIncidentRaisedYN" runat="server" Enabled="false" />
                        <telerik:RadTextBox ID="txtIncidentorNearMissRefNo" runat="server" CssClass="readonlytextbox"
                            MaxLength="100" ReadOnly="true" Visible="false" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblIncidentRaisedBy" runat="server" Text="Raised By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <telerik:RadTextBox ID="txtIncidentRaisedByName" runat="server" CssClass="readonlytextbox"
                            MaxLength="100" ReadOnly="true" Width="180px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtIncidentRaisedByRank" runat="server" CssClass="readonlytextbox"
                            MaxLength="100" ReadOnly="true" Width="160px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblIncidentRaisedDate" runat="server" Text="Raised Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucIncidentRaisedDate" runat="server" CssClass="readonlytextbox" DatePicker="true"
                            ReadOnly="true" Enabled="false" />
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td style="width: 35%" colspan="2">&nbsp;
                    </td>
                </tr>
            </table>
            <asp:Button ID="ucConfirm" runat="server" OnClick="ucConfirm_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
