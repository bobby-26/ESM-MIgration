<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentGeneral.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="InspectionIncidentGeneral" %>

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
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Incident General</title>
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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="80%" >
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuInspectionIncident" runat="server" OnTabStripCommand="InspectionIncident_TabStripCommand" Title="Details"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureInspectionIncident" width="100%">
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblIncidentNearmiss" runat="server" AutoPostBack="true"
                            Direction="Horizontal" OnSelectedIndexChanged="rblIncidentNearmiss_Changed">
                            <Items>
                                <telerik:ButtonListItem Text="Accident" Value="1" Selected="True" />
                                <telerik:ButtonListItem Text="Near Miss" Value="2" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncidentTitle" runat="server" Text="Incident Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="500"
                            Width="80%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRefNo" CssClass="readonlytextbox" ReadOnly="true"
                            MaxLength="200" Width="242px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofIncident" runat="server" Text="Date of Incident"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfIncident" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        &nbsp;
                        <telerik:RadTimePicker ID="txtTimeOfIncident" runat="server" Width="80px" CssClass="input_mandatory"></telerik:RadTimePicker>
                        hrs
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AssignedVessels="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width="240px" />--%>
                        <telerik:RadComboBox ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"  EmptyMessage="--Select--" Filter="Contains" MarkFirstMatch="true"
                           Width="240px"  DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" OnTextChangedEvent="ucVessel_Changed" >
                            </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company runat="server" ID="ucCompany"  AppendDataBoundItems="true"
                            Enabled="false" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDateofReport" runat="server" Text="Date of Report"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfReport" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAcitivity" runat="server" Text="Vessel Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucQuickVesselActivity" runat="server" AppendDataBoundItems="true"
                             QuickTypeCode="48" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblIncidentNearMissType" runat="server" Text="Incident Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Category ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AutoPostBack="true" OnTextChangedEvent="ucCategory_Changed" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncidentNearMissSubType" runat="server" Text="Incident Subcategory"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SubCategory ID="ucSubcategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblOpenReportCategory" runat="server" Text="Open report category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReviewcategory" runat="server" CssClass="readonlytextbox" Width="242px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblConsequencePotential" runat="server">Consequence Category</telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtConsequencePotential" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true" Width="242px">
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
                        <eluc:Latitude ID="ucLatitude" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLogitude" runat="server" Text="Longitude"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Longitude ID="ucLongitude" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCurrent" runat="server" Text="Current"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCurrent"  MaxLength="500" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblvisibility" runat="server" Text="Visibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVisibility"  MaxLength="500" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ucPort" runat="server"  AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported By (Ship)"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnReportedByShip">
                            <telerik:RadTextBox ID="txtReportedByShipName" runat="server" CssClass="readonlytextbox"
                                Enabled="false" MaxLength="200" Width="150px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedbyDesignation" runat="server" CssClass="readonlytextbox"
                                Enabled="false" MaxLength="50" Width="88px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtReportedByShipId" CssClass="readonlytextbox" Width="75px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblwindCondition" runat="server" Text="Wind Condition"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucWindCondionScale" runat="server" AppendDataBoundItems="true" 
                            QuickTypeCode="42" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSeaCondition" runat="server" Text="Sea Condition"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeaCondition" runat="server"  Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblSwellLength" runat="server" Text="Swell Length"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucSwellLength" runat="server" AppendDataBoundItems="true" 
                            QuickTypeCode="43" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWindDirection" runat="server" Text="Wind Direction"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWindDirection" runat="server"  Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblSwellHeight" runat="server" Text="Swell Height"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucSwellHeight" runat="server" AppendDataBoundItems="true" 
                            QuickTypeCode="44" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSwellDirection" runat="server" Text="Swell Direction"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSwellDirection" runat="server"  Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCriticalEquipment" runat="server" Text="Critical Equipment Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkCriticalEquipmentYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkCriticalEquipmentYN_Checked" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNoRoutineRA" runat="server" Text="Old Non Routine RA"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnRA">
                            <telerik:RadTextBox ID="txtRANumber" runat="server"  Enabled="false" MaxLength="50"
                                Width="120px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtRA" runat="server"  Enabled="false" MaxLength="50"
                                Width="118px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowRA" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="ImageButton1" ImageAlign="AbsMiddle" OnClick="cmdRAClear_Click" Text="..">
                            <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtRAId" runat="server" CssClass="hidden" MaxLength="20" Width="0px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtRaType" runat="server" CssClass="hidden" MaxLength="2" Width="0px"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton runat="server" AlternateText="Risk Assesment PDF" CommandName="RA"
                            Visible="false" ID="cmdRA" ToolTip="Show PDF">
                            <span class="icon"><i class="fas fa-chart-bar"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCancelReason" runat="server" Text="Cancel Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCancelReason" CssClass="readonlytextbox" Enabled="false" 
                            Height="50px" Width="80%" TextMode="MultiLine" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNewNoRoutineRA" runat="server" Text="New Non Routine RA"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnNewRA">
                            <telerik:RadTextBox ID="txtNewRANumber" runat="server"  Enabled="false" MaxLength="50"
                                Width="120px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtNewRA" runat="server"  Enabled="false" MaxLength="50"
                                Width="118px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowNewRA" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="imgNewClear" ImageAlign="AbsMiddle" OnClick="cmdNEWRAClear_Click" Text="..">
                            <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtNewRAId" runat="server" CssClass="hidden" MaxLength="20" Width="0px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtNewRaType" runat="server" CssClass="hidden" MaxLength="2" Width="0px"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton runat="server" AlternateText="Risk Assesment PDF" CommandName="NEWRA"
                            Visible="false" ID="cmdNewRA" ToolTip="Show PDF">
                            <span class="icon"><i class="fas fa-chart-bar"></i></span>
                        </asp:LinkButton>
                    </td>
                    
                </tr>
                <tr>

                    <td colspan="2">
                        <telerik:RadLabel ID="lblCancelledBy" runat="server" Text="Cancelled By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCancelledByName" runat="server" Width="185px" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCancelledByDesignation" runat="server" Width="118px" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReviewRemarks" runat="server" Text="Review Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReviewRemarks" runat="server" CssClass="readonlytextbox" Enabled="false" Height="50px"
                            CommandName="SUPERINTENDENTCOMMENTS" Rows="4" TextMode="MultiLine" Width="78%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCancelDate" runat="server" Text="Cancel Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucCancelDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                    </td>                    
                    <td>
                        <telerik:RadLabel ID="lblReviewedBy" runat="server" Text="Reviewed By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReviewedBy" runat="server" Width="185px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtReviewedDateByDesignation" runat="server" Width="118px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblmachinery" runat="server" Text="Machinery Damage / Failure"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtmachinerynumber" runat="server" Width="240px" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblReviewedDate" runat="server" Text="Reviewed Date"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <eluc:Date ID="ucReviewedDate" runat="server" DatePicker="true" 
                            CommandName="APPROVEDDATE" Enabled="false" ReadOnly="true" />
                    </td>
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
                        <telerik:RadLabel ID="lblOnboardLoacation" runat="server" Text="Onboard Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucOnboardLocation" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            QuickTypeCode="66" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActivityRelevantTotheEvent" runat="server" Text="Activity relevant to the Event"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucActivity" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            HardTypeCode="170" AutoPostBack="true" OnTextChangedEvent="Activity_Changed" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPersonInChargeofActivity" runat="server" Text="Person In-Charge of Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPersonInCharge" runat="server">
                            <telerik:RadTextBox ID="txtCrewName" runat="server"  Enabled="false" MaxLength="50"
                                Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtCrewRank" runat="server"  Enabled="false" MaxLength="50"
                                Width="29%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgPersonInCharge" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtCrewId" runat="server"  MaxLength="20" Width="10px"></telerik:RadTextBox>
                        </span><span id="spnPersonInChargeOffice" runat="server">
                            <telerik:RadTextBox ID="txtOfficePersonName" runat="server"  Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOfficePersonDesignation" runat="server"  Enabled="false"
                                MaxLength="50" Width="23%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgPersonInChargeOffice" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtPersonInChargeOfficeId"  Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtPersonInChargeOfficeEmail"  Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIfothersSpecify" runat="server" Text="If others, specify"></telerik:RadLabel>
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
                        <telerik:RadTextBox runat="server" ID="txtDescription" CssClass="input_mandatory" TextMode="MultiLine"
                            Height="70px" Width="80%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRemarks"  TextMode="MultiLine"
                            Height="70px" Width="80%" Resize="Both">
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
                        <telerik:RadLabel ID="lblComprehesiveDescription" runat="server" Text="Comprehensive Description"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtComprehensiveDescription" runat="server"  Height="70px"
                            TextMode="MultiLine" Width="92%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <br />
                        <span style="color: Black; font-weight: bold">
                            <telerik:RadLabel ID="lblImmediateActionTakenOnboard" runat="server" Text="Immediate Action Taken Onboard" ></telerik:RadLabel>
                        </span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblImmediateAction" runat="server" Text="Immediate Action"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtImmediateActionTaken"  TextMode="MultiLine"
                            Height="70px" Width="92%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="spnAssignedTo">
                            <telerik:RadTextBox ID="txtImmediateAssignedToName" runat="server"  Enabled="false"
                                MaxLength="50" Width="50%" Visible="false">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtImmediateAssignedToRank" runat="server"  Enabled="false"
                                MaxLength="50" Width="23%" Visible="false">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgImmediateAssignedTo" Style="cursor: pointer; vertical-align: top" Visible="false">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtImmediateAssignedToId" runat="server"  MaxLength="20"
                                Width="10px" Visible="false">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMaintenanceRequired" runat="server" Enabled="false" Visible="false" />
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadTextBox ID="txtWorkOrderNumber" runat="server" CssClass="readonlytextbox" Height="20px"
                            Rows="3" ReadOnly="true" TextMode="MultiLine" Width="80%" Visible="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <br />
                        <span style="color: Black; font-weight: bold">
                            <telerik:RadLabel ID="lblOnboardInvestigation" runat="server" Text="Onboard Investigation"></telerik:RadLabel>
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
                        <span id="spnReportedByShip1" runat="server">
                            <telerik:RadTextBox ID="txtReportedByShipname1" runat="server"  Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByShipRank1" runat="server"  Enabled="false"
                                MaxLength="50" Width="30%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShip1" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtReportedByShipId1" runat="server"  MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                        </span><span id="spnReportedByOffice1" runat="server">
                            <telerik:RadTextBox ID="txtReportedByOfficeByName1" runat="server"  Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByOfficeByDesignation1" runat="server" 
                                Enabled="false" MaxLength="50" Width="30%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByOffice1" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtReportedByOfficeId1"  Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtReportedByOfficeEmail1"  Width="0px"
                                MaxLength="20">
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
                        <span id="spnReportedByShip2" runat="server">
                            <telerik:RadTextBox ID="txtReportedByShipname2" runat="server"  Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByShipRank2" runat="server"  Enabled="false"
                                MaxLength="50" Width="30%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShip2" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtReportedByShipId2" runat="server"  MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                        </span><span id="spnReportedByOffice2" runat="server">
                            <telerik:RadTextBox ID="txtReportedByOfficeByName2" runat="server"  Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByOfficeByDesignation2" runat="server" 
                                Enabled="false" MaxLength="50" Width="30%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByOffice2" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtReportedByOfficeId2"  Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtReportedByOfficeEmail2"  Width="0px"
                                MaxLength="20">
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
                        <span id="spnReportedByShip3" runat="server">
                            <telerik:RadTextBox ID="txtReportedByShipname3" runat="server"  Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByShipRank3" runat="server"  Enabled="false"
                                MaxLength="50" Width="30%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShip3" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtReportedByShipId3" runat="server"  MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                        </span><span id="spnReportedByOffice3" runat="server">
                            <telerik:RadTextBox ID="txtReportedByOfficeByName3" runat="server"  Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByOfficeByDesignation3" runat="server" 
                                Enabled="false" MaxLength="50" Width="23%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByOffice3" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtReportedByOfficeId3"  Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtReportedByOfficeEmail3"  Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblStatusofVentilation" runat="server" Text="Status of Ventilation"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucVentilation" runat="server" AppendDataBoundItems="true" 
                            QuickTypeCode="60" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLightingCondition" runat="server" Text="Lighting Conditions"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucLightingCond" runat="server" AppendDataBoundItems="true" 
                            QuickTypeCode="49" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblcontractorRelatedIncident" runat="server" Text="Contractor related Incident"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkContractorIncident" runat="server" AutoPostBack="true" OnCheckedChanged="chkContractorIncident_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContractorDetails" runat="server" Text="Contractor Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContractorDetails" runat="server"  Height="60px"
                            Width="80%" TextMode="MultiLine" Resize="Both">
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
                        <eluc:Date ID="txttestDate" runat="server"  ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadGrid ID="gvInvestigation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="98%" CellPadding="3" OnItemCommand="gvInvestigation_ItemCommand" OnItemDataBound="gvInvestigation_ItemDataBound"
                            ShowHeader="true" EnableViewState="false" ShowFooter="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDQUESTIONID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Question">
                                        <HeaderStyle Width="85%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
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
                                    <telerik:GridTemplateColumn HeaderText="Answer">
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAnswer" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWERNAME") %>'></telerik:RadLabel>
                                            <telerik:RadRadioButtonList ID="rblAnswer" runat="server" Columns="3" Direction="Horizontal"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="rblAnswer_SelectedIndexChanged">
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
                            <telerik:RadLabel ID="lblwhatwentwrong" runat="server" Text="What went wrong"></telerik:RadLabel>
                        </span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadGrid ID="gvFindings" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="98%" CellPadding="3" OnItemCommand="gvFindings_ItemCommand" OnItemDataBound="gvFindings_ItemDataBound"
                            ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvFindings_NeedDataSource"
                            OnUpdateCommand="gvFindings_UpdateCommand" OnDeleteCommand="gvFindings_DeleteCommand">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDINCIDENTFINDINGSID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="S.No" AllowSorting="true" SortExpression="FLDSERIALNUMBER">
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSNO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblFindingsId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTFINDINGSID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblFindingsIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTFINDINGSID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:Number ID="ucSNOEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="3"
                                                IsPositive="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></eluc:Number>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Number ID="ucSNOAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="3"
                                                IsPositive="true"></eluc:Number>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Findings">
                                        <HeaderStyle Width="40%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblFindings" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINDINGS") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtFindingsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINDINGS") %>'
                                                CssClass="gridinput_mandatory" Width="98%" Resize="Both" Height="40px" TextMode="MultiLine">
                                            </telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtFindingsAdd" runat="server" CssClass="gridinput_mandatory" Width="98%" Resize="Both" Height="40px" TextMode="MultiLine"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Contact Type">
                                        <HeaderStyle Width="45%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblContactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTACTTYPE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="ddlContactTypeEdit" runat="server" CssClass="input_mandatory" Width="100%" Filter="Contains">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlContactTypeAdd" runat="server" CssClass="input_mandatory" EnableDirectionDetection="true" Width="100%"
                                                Filter="Contains">
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                                ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                                ToolTip="Delete">
                                                <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                                ToolTip="Save">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                                ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd"
                                                ToolTip="Add New">
                                                <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <asp:Button ID="ucConfirm" runat="server" OnClick="ucConfirm_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
