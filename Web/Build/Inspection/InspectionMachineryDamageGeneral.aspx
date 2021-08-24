<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMachineryDamageGeneral.aspx.cs"
    Inherits="InspectionMachineryDamageGeneral" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlIncidentNearMissCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubCategory" Src="~/UserControls/UserControlIncidentNearMissSubCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
    <form id="frmMachineryDamage" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager runat="server" ID="RadSkinManager"></telerik:RadSkinManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false" />
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuMachineryDamage" runat="server" OnTabStripCommand="MenuMachineryDamage_TabStripCommand"
                TabStrip="true" />
            <eluc:TabStrip ID="MenuMachineryDamageGeneral" runat="server" OnTabStripCommand="MenuMachineryDamageGeneral_TabStripCommand"
                TabStrip="false" />
            <table id="tblConfigureInspectionIncident" width="100%">
                <tr>
                    <td colspan="5">
                        <telerik:RadLabel ID="lblIncidentDetails" runat="server" Text="Incident Details" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:VesselByCompany ID="ucVessel" runat="server" AppendDataBoundItems="true" AssignedVessels="true"
                            AutoPostBack="true" CssClass="input_mandatory" OnTextChangedEvent="ucVessel_Changed"
                            VesselsOnly="true" Width="217px" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <telerik:RadTextBox ID="txtReferenceNumber" runat="server" CssClass="readonlytextbox" MaxLength="500"
                            ReadOnly="true" Width="217px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input_mandatory" MaxLength="100"
                            Width="218px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <telerik:RadTextBox ID="txtStatus" runat="server" CssClass="readonlytextbox" MaxLength="500"
                            ReadOnly="true" Width="217px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofIncident" runat="server" Text="Incident Date [LT]"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfIncident" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtTimeOfIncident" runat="server" Width="80px" CssClass="input_mandatory"></telerik:RadTimePicker>
                        hrs
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportedDate" runat="server" Text="Reported Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <eluc:Date ID="txtReportedOfIncidentUTC" runat="server" CssClass="readonlytextbox"
                            DatePicker="true" ReadOnly="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtReportedTimeOfIncidentUTC" runat="server" ReadOnly="true" Enabled="false" Width="80px" CssClass="readonlytextbox"></telerik:RadTimePicker>
                        hrs
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselActivity" runat="server" Text="Vessel Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucQuickVesselActivity" runat="server" AppendDataBoundItems="true"
                            CssClass="input" QuickTypeCode="48" Width="217px" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported by"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <telerik:RadTextBox ID="txtReportedByName" runat="server" CssClass="readonlytextbox" MaxLength="500"
                            ReadOnly="true" Width="118px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtReportedByRank" runat="server" CssClass="readonlytextbox" MaxLength="500"
                            ReadOnly="true" Width="99px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                MaxLength="20" Width="70px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                MaxLength="20" Width="147px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgComponent" runat="server">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="input" Width="0px"></telerik:RadTextBox>
                            <asp:LinkButton ID="imgClearParentComponent" runat="server" OnClick="ClearComponent" ToolTip="Clear Value">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                        </span>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblModeofDiscovery" runat="server" Text="Mode of Discovery"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <telerik:RadTextBox ID="txtModeofDiscovery" runat="server" CssClass="input" MaxLength="500"
                            Visible="false" Width="50%">
                        </telerik:RadTextBox>
                        <eluc:Quick ID="ucModeofDiscovery" runat="server" AppendDataBoundItems="true" CssClass="input"
                            QuickTypeCode="139" Width="217px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="input" AutoPostBack="true"
                            Width="217px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Sub Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <telerik:RadComboBox ID="ddlSubCategory" runat="server" CssClass="input" Width="217px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWorkOrderNumber" runat="server" Text="Work Order"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:LinkButton ID="txtWorkOrderNumber" runat="server" Width="217px"></asp:LinkButton>
                        <asp:LinkButton ID="lnkWorkRequest" runat="server" Text="Create Work Request"></asp:LinkButton>
<%--                        <asp:LinkButton runat="server" ID="imgView" OnClick="imgView_Onlick" ToolTip="View Work Request">
                            <span class="icon"><i class="fas fa-search"></i></span>
                        </asp:LinkButton>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastOverhaulDate" runat="server" Text="Last Overhaul Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 100%">
                        <span>
                            <eluc:Date ID="ucLastOverhaulDate" runat="server" CssClass="input" DatePicker="true" />
                            <telerik:RadLabel ID="lblRunningHrs" runat="server" Text="Running Hours"></telerik:RadLabel>
                            <eluc:Number ID="ucRunningHrs" runat="server" CssClass="input" Width="60px" />
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadLabel ID="lblConsequences" runat="server" Text="Consequences" Font-Bold="true"></telerik:RadLabel>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblProcessLoss" runat="server" Text="Process Loss"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlProcessLoss" runat="server" CssClass="input" Width="217px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblConsequenceCategory" runat="server" Text="Consequence Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <span>
                            <telerik:RadTextBox ID="txtConsequenceCategory" runat="server" CssClass="readonlytextbox"
                                MaxLength="500" ReadOnly="true" Width="120px">
                            </telerik:RadTextBox>
                            <telerik:RadLabel ID="lblConsequenceGuidenceText" runat="server" Text="If the category is A or B, it should be reported under </br> Incident/Near Miss Module"
                                Visible="false" ForeColor="Red">
                            </telerik:RadLabel>
                        </span>
                    </td>
                    <%--<td style="width: 60%"></td>--%>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCost" runat="server" Text="Cost"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlCost" runat="server" CssClass="input" Width="217px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblnumberofhourslost" runat="server" Text="Number of Hours lost"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtnumberofhourslost" runat="server" CssClass="input txtNumber" MaxLength="5" Tooltip="Enter the Number of Hours lost in off hire cases under incidents" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNearmiss" runat="server" Text="Near Miss"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblNearmiss" RenderMode="Lightweight" runat="server" Direction="Horizontal">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadLabel ID="lblDescriptionOfIncident" runat="server" Font-Bold="true" Text="Description of the Incident"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblDescriptionGuidenceText" runat="server" ForeColor="Blue"
                            Text="(Enter as much detail as possible to understand the situation with action taken by the vessel. Attach supporting sketches and/or photographs, if applicable)">
                        </telerik:RadLabel>
                        <asp:LinkButton ID="imgEvidence" runat="server" ToolTip="Upload Evidence">
                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtDetialsOfIncident" runat="server" CssClass="input" Height="80px" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"
                            TextMode="MultiLine" Width="95%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="font-weight: 700">
                        <telerik:RadLabel ID="lblDirectCause" runat="server" Text="Immediate Cause"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td style="width: 35%" colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadCheckBoxList ID="cblImmediateCause" OnSelectedIndexChanged="cblImmediateCause_Changed"
                            AutoPostBack="true" runat="server" Columns="3" CssClass="input">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr id="trImmediateCauseOthers" runat="server">
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtImmediateCauseOthers" runat="server" CssClass="readonlytextbox"
                            Width="95%" Height="80px" TextMode="MultiLine" ReadOnly="true" onKeyUp="TxtMaxLength(this,1000)"
                            onChange="TxtMaxLength(this,1000)" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblRootCause" runat="server" Text="&lt;b&gt; Root Cause &lt;/b&gt;"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td style="width: 35%" colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadCheckBoxList ID="cblRootCause" runat="server" Columns="3" CssClass="input"
                            OnSelectedIndexChanged="cblRootCause_Changed" AutoPostBack="true">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr id="trRootCauseOthers" runat="server">
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtRootCauseOthers" runat="server" CssClass="readonlytextbox" Width="95%"
                            Height="80px" TextMode="MultiLine" ReadOnly="true" onKeyUp="TxtMaxLength(this,1000)"
                            onChange="TxtMaxLength(this,1000)" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="Immediate Corrective Action taken onboard" Font-Bold="true"></telerik:RadLabel>
                        &nbsp;
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td style="width: 35%" colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtCorrectiveAction" runat="server" CssClass="input" Height="80px" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"
                            MaxLength="2000" TextMode="MultiLine" Width="95%" Resize="Both">
                        </telerik:RadTextBox>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblRecommendations" Font-Bold="true" runat="server" Text="Recommendations / Suggestions for Improvement"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblRecommendationsGuidenceText" ForeColor="Blue" runat="server" Text="(for sister vessel or the entire fleet)"></telerik:RadLabel>
                        &nbsp;
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td style="width: 35%" colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtRecommendations" runat="server" CssClass="input" Height="80px" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"
                            MaxLength="2000" TextMode="MultiLine" Width="95%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lblReviewRemarks" runat="server" Text="Review Remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtReviewRemarks" runat="server" CssClass="input" Height="50px" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"
                            MaxLength="2000" TextMode="MultiLine" Width="90%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%;" valign="bottom">
                        <telerik:RadLabel ID="lblReviewedDate" runat="server" Text="Reviewed Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucReviewDate" runat="server" CssClass="readonlytextbox" DatePicker="true"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="ReviewBy" runat="server" Text="Reviewed By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtReviewByName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="340px">
                        </telerik:RadTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lblOfficeComments" runat="server" Text="Office Comments &amp; Closeout Remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtOfficeComments" runat="server" CssClass="input" Height="50px" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"
                            MaxLength="2000" TextMode="MultiLine" Width="90%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%;" valign="bottom">
                        <telerik:RadLabel ID="lblClosedDate" runat="server" Text="Closed Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucClosedDate" runat="server" CssClass="readonlytextbox" DatePicker="true"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblClosedBy" runat="server" Text="Closed By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtClosedByName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="160px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtClosedByRank" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="181px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" style="width: 15%;">
                        <telerik:RadLabel ID="lblCancelledRemarks" runat="server" Text="Cancelled Remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtCancelledRemarks" runat="server" CssClass="input" Height="50px"
                            MaxLength="2000" TextMode="MultiLine" Width="90%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%" valign="bottom">
                        <telerik:RadLabel ID="lblCancelledDate" runat="server" Text="Cancelled Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucCancelledDate" runat="server" CssClass="readonlytextbox" DatePicker="true"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblCancelledBy" runat="server" Text="Cancelled By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtCancelledByName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="160px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCancelledByRank" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="181px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblIncidentRaisedYN" runat="server" Text="Incident / Near Miss Raised YN"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkIncidentRaisedYN" runat="server" Enabled="false" />
                        <telerik:RadTextBox ID="txtIncidentorNearMissRefNo" runat="server" CssClass="readonlytextbox"
                            MaxLength="100" ReadOnly="true" Visible="false" Width="133px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblIncidentRaisedBy" runat="server" Text="Raised By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <telerik:RadTextBox ID="txtIncidentRaisedByName" runat="server" CssClass="readonlytextbox"
                            MaxLength="100" ReadOnly="true" Width="160px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtIncidentRaisedByRank" runat="server" CssClass="readonlytextbox"
                            MaxLength="100" ReadOnly="true" Width="181px">
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
                <tr>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td style="width: 35%">&nbsp;
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
