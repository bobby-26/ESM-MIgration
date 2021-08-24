<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentView.aspx.cs"
    Inherits="InspectionIncidentView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PortActivity" Src="~/UserControls/UserControlPortActivity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlIncidentNearMissCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubCategory" Src="~/UserControls/UserControlIncidentNearMissSubCategory.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Incident General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript" language="javascript">
            function countDown(control, maxLen, counter, typeName) {
                var len = control.value.length;
                var txt = control.value;
                var span = document.getElementById(counter);
                span.style.display = '';
                span.innerHTML = (maxLen - len) + ' characters remaining';
                if (len >= (maxLen - 10)) {
                    span.style.color = 'red';
                    if (len > maxLen) {
                        control.innerHTML = txt.substring(0, maxLen);
                        span.innerHTML = (maxLen - control.value.length) + ' characters remaining';
                        alert(typeName + ' text exceeds the maximum allowed!');
                    }
                }
                else {
                    span.style.color = '';
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersInspectionIncident" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Title runat="server" ID="ucTitle" Text="Incident" ShowMenu="false" Visible="false"></eluc:Title>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table id="tblConfigureInspectionIncident" width="100%">
            <tr>
                <td style="width: 15%" colspan="2">
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadRadioButtonList ID="rblIncidentNearmiss" runat="server" AutoPostBack="true" Direction="Horizontal"
                        RepeatDirection="Horizontal" OnSelectedIndexChanged="rblIncidentNearmiss_Changed">
                        <Items>
                            <telerik:ButtonListItem Text="Incident" Value="1" Selected="True"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="Near Miss" Value="2"></telerik:ButtonListItem>
                            <%--<asp:ListItem Text="Serious Near Miss" Value="3"></asp:ListItem>--%>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblIncidentTitle" runat="server" Text="Incident Title"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadTextBox runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="500"
                        Width="80%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 15%" colspan="2">
                    <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadTextBox runat="server" ID="txtRefNo" CssClass="readonlytextbox" ReadOnly="true" Width="200px"
                        MaxLength="200">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDateofIncident" runat="server" Text="Date of Incident"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtDateOfIncident" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    &nbsp;
                                <telerik:RadTextBox ID="txtTimeOfIncident" runat="server" CssClass="input_mandatory" Width="105px" />
                    <%-- <ajaxToolkit:MaskedEditExtender ID="txtTimeMask" runat="server" AcceptAMPM="false"
                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                        TargetControlID="txtTimeOfIncident" UserTimeFormat="TwentyFourHour" />--%>
                    hrs
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory" Width="200px"
                        AssignedVessels="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported By (Ship)"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnReportedByShip">
                        <telerik:RadTextBox ID="txtReportedByShipName" runat="server" CssClass="readonlytextbox"
                            Enabled="false" MaxLength="200" Width="45%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtReportedbyDesignation" runat="server" CssClass="readonlytextbox"
                            Enabled="false" MaxLength="50" Width="35%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtReportedByShipId" CssClass="readonlytextbox" Width="75px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblDateofReport" runat="server" Text="Date of Report"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtDateOfReport" runat="server" ReadOnly="true" Width="118px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselActivity" runat="server" Text="Vessel Activity"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucQuickVesselActivity" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="48" Width="200px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblIncidentNearMissType" runat="server" Text="Incident / Near Miss Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Category ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="200px"
                        AutoPostBack="true" OnTextChangedEvent="ucCategory_Changed" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblIncidentNearMisssubType" runat="server" Text="Incident / Near Miss Subtype"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:SubCategory ID="ucSubcategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="200px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblOpenreportCategory" runat="server" Text="Open report category"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtReviewcategory" runat="server" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblConsequencePotential" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtConsequencePotential" runat="server" CssClass="readonlytextbox" Width="200px"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                    <eluc:Hard ID="ucConsequenceCategory" runat="server" AppendDataBoundItems="true"
                        HardTypeCode="169" Visible="false" />
                    <eluc:Hard ID="ucPotentialCategory" runat="server" AppendDataBoundItems="true"
                        HardTypeCode="169" ShortNameFilter="A,B,C,D,E" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblLatitude" runat="server" Text="Latitude"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Latitude ID="ucLatitude" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblLongitude" runat="server" Text="Longitude"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Longitude ID="ucLongitude" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblCurrent" runat="server" Text="Current"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtCurrent" CssClass="input" MaxLength="500" Width="200px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblVisibility" runat="server" Text="Visibility"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtVisibility" CssClass="input" MaxLength="500" Width="80%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Port ID="ucPort" runat="server" AppendDataBoundItems="true"
                        Width="200px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblSeaCondition" runat="server" Text="Sea Condition"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSeaCondition" runat="server" CssClass="input" Width="80%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblWindCondition" runat="server" Text="Wind Condition"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucWindCondionScale" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="42" Width="200px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblWindDirection" runat="server" Text="Wind Direction"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWindDirection" runat="server" CssClass="input" Width="80%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblSwellLength" runat="server" Text="Swell Length"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucSwellLength" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="43" Width="200px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblSwellDirection" runat="server" Text="Swell Direction"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSwellDirection" runat="server" Width="80%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblSwellHeight" runat="server" Text="Swell Height"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucSwellHeight" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="44" Width="200px" />
                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblCancelReason" runat="server" Text="Cancel Reason"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtCancelReason" CssClass="readonlytextbox" Enabled="false" Resize="Both"
                        TextMode="MultiLine" Height="70px" Width="80%">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCancelDate" runat="server" Text="Cancel Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucCancelDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblCancelledBy" runat="server" Text="Cancelled By"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCancelledByName" runat="server" Width="145px" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtCancelledByDesignation" runat="server" Width="100px" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td colspan="5">
                    <br />
                    <span style="color: Black; font-weight: bold">
                        <telerik:RadLabel ID="lblDescriptionofIncident" runat="server" Text="Description of Incident"></telerik:RadLabel>
                    </span>
                    <hr style="height: -12px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblOnboardLoctation" runat="server" Text="Onboard Location"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucOnboardLocation" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                        QuickTypeCode="66" Width="200px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblActivityrelavanttotheEvent" runat="server" Text="Activity relevant to the Event"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard runat="server" ID="ucActivity" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="80%"
                        HardTypeCode="170" AutoPostBack="true" OnTextChangedEvent="Activity_Changed" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblPersonInChargeofActivity" runat="server" Text="Person In-Charge of Activity"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPersonInCharge">
                        <telerik:RadTextBox ID="txtCrewName" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                            Width="50%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCrewRank" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                            Width="20%">
                        </telerik:RadTextBox>
                        <img runat="server" id="imgPersonInCharge" style="cursor: pointer; vertical-align: top"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtCrewId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>
                <td>
                    <telerik:RadLabel ID="lblIfOthersSpecify" runat="server" Text="If others, specify"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtOtherActivity" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="80%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="2">
                    <telerik:RadLabel ID="lblBriefDescription" runat="server" Text="Brief Description"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtDescription" CssClass="input_mandatory" TextMode="MultiLine" Resize="Both"
                        Height="70px" Width="80%" onpaste="countDown(this, 500, 'desc', 'Description');"
                        onkeyup="countDown(this, 500, 'desc', 'Description');">
                    </telerik:RadTextBox>
                </td>
                <td valign="top">
                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtRemarks" CssClass="input" TextMode="MultiLine" Resize="Both"
                        Height="70px" Width="80%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td align="top" colspan="2"></td>
                <td colspan="3">
                    <span id="desc" style="display: none">
                        <telerik:RadLabel ID="lbl500CharactersRemaining" runat="server" Text="500 characters remaining"></telerik:RadLabel>
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblComprehensiveDescription" runat="server" Text="Comprehensive Description"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtComprehensiveDescription" runat="server" CssClass="input" Height="70px" Resize="Both"
                        TextMode="MultiLine" Width="92%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <br />
                    <span style="color: Black; font-weight: bold">
                        <telerik:RadLabel ID="lblImmediateActionTakenOnboard" runat="server" Text="Immediate Action Taken Onboard"></telerik:RadLabel>
                    </span>
                    <hr style="height: -12px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblImmediateAction" runat="server" Text="Immediate Action"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox runat="server" ID="txtImmediateActionTaken" CssClass="input" TextMode="MultiLine" Height="70px" Resize="Both"
                        Width="92%">
                    </telerik:RadTextBox>
                </td>
                <%--<td>
                                Assigned to
                            </td>
                            <td>
                                
                            </td>--%>
            </tr>
            <tr>
                <td colspan="2">
                    <%--Maintenance Required--%>
                    <span id="spnAssignedTo">
                        <telerik:RadTextBox ID="txtImmediateAssignedToName" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="50%" Visible="false">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtImmediateAssignedToRank" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="23%" Visible="false">
                        </telerik:RadTextBox>
                        <img runat="server" id="imgImmediateAssignedTo" style="cursor: pointer; vertical-align: top"
                            src="<%$ PhoenixTheme:images/picklist.png %>" visible="false" />
                        <telerik:RadTextBox ID="txtImmediateAssignedToId" runat="server" CssClass="input" MaxLength="20"
                            Width="10px" Visible="false">
                        </telerik:RadTextBox>
                    </span>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkMaintenanceRequired" runat="server" Enabled="false" Visible="false" />
                </td>
                <td>
                    <%--WO Number--%>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWorkOrderNumber" runat="server" CssClass="readonlytextbox" Height="20px" Resize="Both"
                        Rows="3" ReadOnly="true" TextMode="MultiLine" Width="80%" Visible="false">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <br />
                    <span style="color: Black; font-weight: bold">
                        <telerik:RadLabel ID="lblOnBoardInvestigation" runat="server" Text="Onboard Investigation"></telerik:RadLabel>
                    </span>
                    <hr style="height: -12px" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <telerik:RadLabel ID="lblInvestigatedBy" runat="server" Text="Investigated By"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLead" runat="server" Text="Lead Investigator"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lbl1" runat="server" Text="1"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnReportedByShip1">
                        <telerik:RadTextBox ID="txtReportedByShipname1" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="50%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtReportedByShipRank1" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="23%">
                        </telerik:RadTextBox>
                        <img runat="server" id="imgReportedByShip1" style="cursor: pointer; vertical-align: top"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtReportedByShipId1" runat="server" CssClass="input" MaxLength="20"
                            Width="0px">
                        </telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <telerik:RadLabel ID="lbl2" runat="server" Text="2"></telerik:RadLabel>
                </td>

                <td>
                    <span id="spnReportedByShip2">
                        <telerik:RadTextBox ID="txtReportedByShipname2" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="50%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtReportedByShipRank2" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="23%">
                        </telerik:RadTextBox>
                        <img runat="server" id="imgReportedByShip2" style="cursor: pointer; vertical-align: top"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtReportedByShipId2" runat="server" CssClass="input" MaxLength="20"
                            Width="0px">
                        </telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <telerik:RadLabel ID="lbl3" runat="server" Text="3"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnReportedByShip3">
                        <telerik:RadTextBox ID="txtReportedByShipname3" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="50%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtReportedByShipRank3" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="23%">
                        </telerik:RadTextBox>
                        <img runat="server" id="imgReportedByShip3" style="cursor: pointer; vertical-align: top"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtReportedByShipId3" runat="server" CssClass="input" MaxLength="20"
                            Width="0px">
                        </telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblStatusOfVentilation" runat="server" Text="Status of Ventilation"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucVentilation" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="60" Width="200px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblLightingConditions" runat="server" Text="Lighting Conditions"></telerik:RadLabel>
                </td>
                <td>
                    <%--<eluc:Hard runat="server" ID="ucLightingCond" CssClass="input" AppendDataBoundItems="true"
                                    HardTypeCode="177" ShortNameFilter="ADE,IAD" />--%>
                    <eluc:Quick ID="ucLightingCond" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="49" Width="200px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblContractorRelatedIncident" runat="server" Text="Contractor related Incident"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkContractorIncident" runat="server" AutoPostBack="true" OnCheckedChanged="chkContractorIncident_CheckedChanged" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblContractorDetails" runat="server" Text="Contractor Details"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtContractorDetails" runat="server" CssClass="input" Height="60px" Resize="Both"
                        Width="80%" TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblDrugAlcoholTestDone" runat="server" Text="Drug/Alcohol Test Done?"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkAlcoholtest" runat="server" Enabled="false" />
                    &nbsp;
                </td>
                <td>
                    <telerik:RadLabel ID="lblTestDate" runat="server" Text="Test Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txttestDate" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvInvestigation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="98%" CellPadding="3" OnRowCommand="gvInvestigation_RowCommand" OnItemDataBound="gvInvestigation_ItemDataBound"
                        ShowHeader="true" EnableViewState="false" ShowFooter="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDQUESTIONID">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="545px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblQuestionHeader" runat="server">Question</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblInvestigationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTINVESTIGATIONID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblQuestionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="150px">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblAnswerHeader" runat="server">Answer</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAnswer" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWERNAME") %>'></telerik:RadLabel>
                                        <telerik:RadRadioButtonList ID="rblAnswer" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" DataTextField="FLDHARDNAME"
                                            Direction="Horizontal" DataValueField="FLDHARDCODE" AutoPostBack="true" OnSelectedIndexChanged="rblAnswer_SelectedIndexChanged">
                                        </telerik:RadRadioButtonList>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <br />
                    <span style="color: Black; font-weight: bold">
                        <telerik:RadLabel ID="lblWhatWentWrong" runat="server" Text="What went wrong"></telerik:RadLabel>
                    </span>
                    <hr style="height: -12px" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <%--<div id="divGrid" style="position: relative; z-index: +5; width: 97%;">--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvFindings" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="98%" CellPadding="3" OnRowCommand="gvFindings_RowCommand" OnItemDataBound="gvFindings_ItemDataBound"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnSorting="gvFindings_Sorting">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDINCIDENTFINDINGSID">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="S.No">
                                    <itemstyle wrap="False" horizontalalign="Center" width="45px"></itemstyle>
                                    <headertemplate>
                                    <asp:LinkButton ID="lblSNOHeader" runat="server" CommandName="Sort" CommandArgument="FLDSERIALNUMBER">S.No&nbsp;</asp:LinkButton>
                                    <img id="FLDSERIALNUMBER" runat="server" visible="false" />
                                </headertemplate>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblSNO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblFindingsId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTFINDINGSID") %>'
                                        Visible="false"></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Findings">
                                    <itemstyle wrap="true" horizontalalign="Left" width="350px"></itemstyle>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblFindings" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINDINGS") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Contact Type">
                                    <itemstyle wrap="true" horizontalalign="Left" width="150px"></itemstyle>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblContactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTACTTYPE") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <%-- </div>--%>
                </td>
            </tr>
        </table>
        <eluc:Confirm ID="ucConfirmComplete" runat="server" OnConfirmMesage="btnComplete_Click"
            CancelText="OK" YesButtonVisible="false" />
    </form>
</body>
</html>
