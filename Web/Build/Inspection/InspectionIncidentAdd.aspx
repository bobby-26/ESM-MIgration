<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentAdd.aspx.cs" Inherits="InspectionIncidentAdd" %>

<!DOCTYPE html>

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
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PortActivity" Src="~/UserControls/UserControlPortActivity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlIncidentNearMissCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubCategory" Src="~/UserControls/UserControlIncidentNearMissSubCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Time" Src="~/UserControls/UserControlTimeSlots.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuInspectionIncident" runat="server" OnTabStripCommand="InspectionIncident_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <table id="tblConfigureInspectionIncident" width="100%">
                <tr>
                    <td style="width: 15%" colspan="2">
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <telerik:RadRadioButtonList ID="rblIncidentNearmiss" runat="server" AutoPostBack="true"
                            Direction="Horizontal" OnSelectedIndexChanged="rblIncidentNearmiss_Changed">
                            <Items>
                                <telerik:ButtonListItem Text="Accident" Value="1" Selected="True" />
                                <telerik:ButtonListItem Text="Near Miss" Value="2" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblIncidentTitle" runat="server" Text="Incident Title"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="500"
                            Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" colspan="2">
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <telerik:RadTextBox runat="server" ID="txtRefNo" CssClass="readonlytextbox" ReadOnly="true"
                            MaxLength="200" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofIncident" runat="server" Text="Date of Incident"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfIncident" runat="server" CssClass="input_mandatory" DatePicker="true" DateTimeFormat="dd/mm/yyyy" AutoPostBack="true" OnTextChangedEvent="txtDateOfIncident_TextChanged"
                            Width="120px" />
                        &nbsp;
                        <telerik:RadTimePicker ID="txtTimeOfIncident" runat="server" Width="80px" CssClass="input_mandatory"></telerik:RadTimePicker>
                         hrs
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByCompany runat="server" ID="ucVessel" CssClass="input_mandatory"
                            AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" AppendDataBoundItems="true" Width="200px" />
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" Enabled="false" AppendDataBoundItems="true"
                            CssClass="input" OnTextChangedEvent="ucCompany_changed" Width="240px" />
                    </td>

                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDateofReport" runat="server">Date of Report</telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Date ID="txtDateOfReport" runat="server" CssClass="input" ReadOnly="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselActivity" runat="server" Text="Vessel Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucQuickVesselActivity" runat="server" AppendDataBoundItems="true"
                            CssClass="input" QuickTypeCode="48" Width="240px" />
                    </td>

                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblIncidentNearMissType" runat="server" Text="Incident Category"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Category ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AutoPostBack="true" OnTextChangedEvent="ucCategory_Changed" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncidneNearMissSubType" runat="Server" Text="Incident Subcategory"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SubCategory ID="ucSubcategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="240px" />
                    </td>


                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblOpenReportCategory" runat="server" Text="Open report category"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtReviewcategory" runat="server" CssClass="readonlytextbox" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblConsequencePotential" runat="server">Consequence Category</telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtConsequencePotential" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                        <eluc:Hard ID="ucConsequenceCategory" runat="server" AppendDataBoundItems="true"
                            CssClass="input" HardTypeCode="169" Visible="false" />
                        <eluc:Hard ID="ucPotentialCategory" runat="server" AppendDataBoundItems="true" CssClass="input"
                            HardTypeCode="169" ShortNameFilter="A,B,C,D,E" Visible="false" />
                    </td>


                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblLatitude" runat="server" Text="Latitude"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Latitude ID="ucLatitude" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLongitude" runat="Server" Text="Longitude"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Longitude ID="ucLongitude" runat="server" CssClass="input" />
                    </td>


                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCurrenct" runat="server" Text="Current"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="txtCurrent" CssClass="input" MaxLength="500" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVisibility" runat="server" Text="Visibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVisibility" CssClass="input" MaxLength="500" Width="240px"></telerik:RadTextBox>
                    </td>


                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Port ID="ucPort" runat="server" CssClass="input" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported By (Ship)"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnReportedByShip">
                            <telerik:RadTextBox ID="txtReportedByShipName" runat="server" CssClass="readonlytextbox"
                                Enabled="false" MaxLength="200" Width="120px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedbyDesignation" runat="server" CssClass="readonlytextbox"
                                Enabled="false" MaxLength="50" Width="160px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtReportedByShipId" CssClass="readonlytextbox" Width="75px"></telerik:RadTextBox>
                        </span>
                    </td>


                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblWindCondition" runat="server" Text="Wind Condition"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Quick ID="ucWindCondionScale" runat="server" AppendDataBoundItems="true" CssClass="input"
                            QuickTypeCode="42" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSeaCondition" runat="Server" Text="Sea Condition"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeaCondition" runat="server" CssClass="input" Width="240px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblSwellLength" runat="Server" Text="Swell Length"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Quick ID="ucSwellLength" runat="server" AppendDataBoundItems="true" CssClass="input"
                            QuickTypeCode="43" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWindDirection" runat="Server" Text="Wind Direction"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWindDirection" runat="server" CssClass="input" Width="240px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblSwellHeight" runat="server" Text="Swell Height"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Quick ID="ucSwellHeight" runat="server" AppendDataBoundItems="true" CssClass="input"
                            QuickTypeCode="44" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSwellDirection" runat="server" Text="Swell Direction"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSwellDirection" runat="server" CssClass="input" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCancelReason" runat="server" Text="Cancel Reason"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="txtCancelReason" CssClass="readonlytextbox" Enabled="false"
                            TextMode="MultiLine" Height="70px" Width="80%" Resize="Both">
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
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtCancelledByName" runat="server" Width="120px" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCancelledByDesignation" runat="server" Width="160px" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                        <span style="color: Black; font-weight: bold">Description of Incident</span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblOnboardLocation" runat="server" Text="Onboard Location"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Quick ID="ucOnboardLocation" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            QuickTypeCode="66" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActivityRelevanttotheEvent" runat="server" Text="Activity relevant to the Event"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucActivity" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            HardTypeCode="170" AutoPostBack="true" OnTextChangedEvent="Activity_Changed" Width="244px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPersonInChargeofActivity" runat="server" Text="Person In-Charge of Activity"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadComboBox ID="ddlPersonIncharge" runat="server" CssClass="input" Width="79%" Filter="Contains"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIFOthersSpecify" runat="server" Text="If others, specify"></telerik:RadLabel>
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
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="txtDescription" Resize="Both" CssClass="input_mandatory" TextMode="MultiLine"
                            Height="70px" Width="80%" onpaste="countDown(this, 500, 'desc', 'Description');"
                            onkeyup="countDown(this, 500, 'desc', 'Description');">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRemarks" CssClass="input" TextMode="MultiLine"
                            Height="70px" Width="80%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="top" colspan="2"></td>
                    <td colspan="4">
                        <span id="desc" style="display: none">
                            <telerik:RadLabel ID="lblCharacters" runat="server" Text="500 characters remaining"></telerik:RadLabel>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblComprehensiveDescription" runat="server" Text="Comprehensive Description"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtComprehensiveDescription" runat="server" CssClass="input" Height="70px"
                            TextMode="MultiLine" Width="92%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:Confirm ID="ucConfirmComplete" runat="server" OnConfirmMesage="btnComplete_Click"
                CancelText="OK" YesButtonVisible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
