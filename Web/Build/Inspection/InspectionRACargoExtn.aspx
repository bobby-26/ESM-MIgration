<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRACargoExtn.aspx.cs" Inherits="InspectionRACargoExtn" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubHazard" Src="~/UserControls/UserControlRASubHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Miscellaneous" Src="~/UserControls/UserControlRAMiscellaneous.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Frequency" Src="~/UserControls/UserControlRAFrequency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cargo</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            table.Hazard {
                border-collapse: collapse;
            }

                table.Hazard td, table.Hazard th {
                    border: 1px solid black;
                    padding: 5px;
                }
        </style>

        <script language="javascript" type="text/javascript">
            function TxtMaxLength(text, maxLength) {
                text.value = text.value.substring(0, maxLength);
            }
        </script>
        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvHealthSafetyRisk.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>

        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvEnvironmentalRisk.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>

        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvEconomicRisk.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>

        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvevent.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>

        <style type="text/css">
            .checkboxstyle tbody tr td {
                width: 550px;
                vertical-align: top;
            }
        </style>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCargo" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblcargo" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuCargo" runat="server" OnTabStripCommand="MenuCargo_TabStripCommand" Title="Cargo"></eluc:TabStrip>
            <table id="tblcargo" width="100%" cellspacing="3">
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref Number"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <telerik:RadTextBox runat="server" Width="300px" CssClass="readonlytextbox" ID="txtRefNo"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblRevisionNo" runat="server" Text="Revision No"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <telerik:RadTextBox runat="server" Width="65px" CssClass="readonlytextbox" ID="txtRevNO"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPreparedBy" runat="server" Text="Prepared By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="300px" CssClass="readonlytextbox" ID="txtpreparedby"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucCreatedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="300px" CssClass="readonlytextbox" ID="txtApprovedby"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate1" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucApprovedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIssuedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="300px" CssClass="readonlytextbox" ID="txtIssuedBy"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate2" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIssuedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblrejectedby" runat="server" Text="Rejected By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="300px" CssClass="readonlytextbox" ID="txtrejectedby"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblrejecteddate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucrejecteddate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="300px"></telerik:RadTextBox>
                        <eluc:Date ID="txtDate" Visible="false" CssClass="readonlytextbox" ReadOnly="false"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofIntendedWorkActivity" runat="server" Text="Date of intended Work"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtIntendedWorkDate" CssClass="input_mandatory" runat="server" DatePicker="true" />
                        &nbsp;&nbsp;&nbsp;&nbsp 
                        <asp:LinkButton ID="lnkcomment" runat="server" ToolTip="Comments" Visible="false">
                                        <span class="icon"><i class="fa fa-comments"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofcompletion" runat="server" Text="Target Date for completion of the Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtcompletiondate" CssClass="input_mandatory" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActivityConditionsEquipment" runat="server" Text="Activity / Conditions"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtActivityCondition" runat="server" CssClass="input_mandatory"
                            Width="360px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNoofPeopleInvolvedInActivity" runat="server" Text="No of people affected"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="rblPeopleInvolved" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblpersons" runat="server" Text="Persons carrying out job"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="divpersonsinvolved" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 420px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="Chkpersonsinvolved" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2"
                                AutoPostBack="false">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDurationofWorkActivity" runat="server" Text="Duration"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="rblWorkDuration" runat="server" AppendDataBoundItems="true" Enabled="false" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" />
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblReasonForAssessment" runat="server" Text="Reason for Assessment"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="divReason" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 420px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="cblReason" runat="server" RepeatDirection="Horizontal" RepeatColumns="2"
                                AutoPostBack="true" OnTextChanged="OtherDetailClick">
                            </asp:CheckBoxList>
                        </div>
                        <br />
                        <telerik:RadTextBox ID="txtOtherReason" runat="server" CssClass="readonlytextbox" Width="94%"
                            Height="80px" TextMode="MultiLine" ReadOnly="true" onKeyUp="TxtMaxLength(this,700)"
                            onChange="TxtMaxLength(this,700)">
                        </telerik:RadTextBox>
                    </td>
                    <td rowspan="6" colspan="2" style="vertical-align: top">
                        <telerik:RadLabel ID="lblFrequencyofWorkActivity" runat="server" Text="Frequency" Width="19%"></telerik:RadLabel>
                        <telerik:RadComboBox ID="rblWorkFrequency" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" />
                        <br />
                        <table class="Hazard">
                            <tr>
                                <td align="center" id="Td" width="24%"></td>
                                <td align="center" style="background-color: rgb(255,230,110);" width="19%">
                                    <telerik:RadLabel ID="lblHealthSafety" runat="server">Health and Safety</telerik:RadLabel>
                                </td>
                                <td align="center" style="background-color: rgb(155,255,166);" width="19%">
                                    <telerik:RadLabel ID="lblEnviormental" runat="server">Environmental</telerik:RadLabel>
                                </td>
                                <td align="center" style="background-color: rgb(251,255,225);" width="19%">
                                    <telerik:RadLabel ID="lblEconomic" runat="server">Economic/Process Loss</telerik:RadLabel>
                                </td>
                                <td align="center" style="background-color: rgb(255,216,44);" width="19%">
                                    <telerik:RadLabel ID="lblWorst" runat="server">Worst Case</telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="impact" runat="server">
                                    <telerik:RadLabel ID="lblimpact" runat="server" Text="Impact"></telerik:RadLabel>
                                </td>
                                <td align="center" id="impacthealth" runat="server">
                                    <telerik:RadLabel ID="lblimpacthealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="impactenv" runat="server">
                                    <telerik:RadLabel ID="lblimpactenv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="impacteco" runat="server">
                                    <telerik:RadLabel ID="lblimpacteco" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="impactws" runat="server">
                                    <telerik:RadLabel ID="lblimpactws" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="POO" runat="server">
                                    <telerik:RadLabel ID="lblPOO" runat="server" Text="POO"></telerik:RadLabel>
                                </td>
                                <td align="center" id="POOhealth" runat="server">
                                    <telerik:RadLabel ID="lblPOOhealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="POOenv" runat="server">
                                    <telerik:RadLabel ID="lblPOOenv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="POOeco" runat="server">
                                    <telerik:RadLabel ID="lblPOOeco" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="POOws" runat="server">
                                    <telerik:RadLabel ID="lblPOOws" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="loh" runat="server">
                                    <telerik:RadLabel ID="lblloh" runat="server" Text="LOH"></telerik:RadLabel>
                                </td>
                                <td align="center" id="lohhealth" runat="server">
                                    <telerik:RadLabel ID="lbllohhealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="lohenv" runat="server">
                                    <telerik:RadLabel ID="lbllohenv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="loheco" runat="server">
                                    <telerik:RadLabel ID="lblloheco" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="lohws" runat="server">
                                    <telerik:RadLabel ID="lbllohws" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="tdControls" runat="server">
                                    <telerik:RadLabel ID="lblControlstxt" runat="server" Text="Controls"></telerik:RadLabel>
                                </td>
                                <td align="center" id="Controlshealth" runat="server">
                                    <telerik:RadLabel ID="lblControlshealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="Controlsenv" runat="server">
                                    <telerik:RadLabel ID="lblControlsenv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="Controlseco" runat="server">
                                    <telerik:RadLabel ID="lblControlseco" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="Controlsws" runat="server">
                                    <telerik:RadLabel ID="lblControlsws" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="level" runat="server">
                                    <telerik:RadLabel ID="lblLevel" runat="server" Text="Risk Level"></telerik:RadLabel>
                                </td>
                                <td align="center" id="levelofriskhealth" runat="server">
                                    <telerik:RadLabel ID="lblLevelofRiskHealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="levelofriskenv" runat="server">
                                    <telerik:RadLabel ID="lblLevelofRiskEnv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="levelofriskeco" runat="server">
                                    <telerik:RadLabel ID="lblLevelofRiskEconomic" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="levelofriskworst" runat="server">
                                    <telerik:RadLabel ID="lblLevelofRiskWorst" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="Td1" runat="server">
                                    <telerik:RadLabel ID="lblcontrols" runat="server" Text="Additional Controls due to Supervision / Checklist"></telerik:RadLabel>
                                </td>
                                <td align="center" id="tdhscontrols" runat="server">
                                    <telerik:RadLabel ID="lblhscontrols" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="tdencontrols" runat="server">
                                    <telerik:RadLabel ID="lblencontrols" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="tdecocontrols" runat="server">
                                    <telerik:RadLabel ID="lbleccontrols" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="tdwscontrols" runat="server">
                                    <telerik:RadLabel ID="lblwscontrols" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="tdresrisk" runat="server">
                                    <telerik:RadLabel ID="lblresrisk" runat="server" Text="Residual Risk"></telerik:RadLabel>
                                </td>
                                <td align="center" id="tdreshsrisk" runat="server">
                                    <telerik:RadLabel ID="lblreshsrisk" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="tdresenrisk" runat="server">
                                    <telerik:RadLabel ID="lblresenrisk" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="tdresecorisk" runat="server">
                                    <telerik:RadLabel ID="lblresecorisk" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="tdreswsrisk" runat="server">
                                    <telerik:RadLabel ID="lblreswsrisk" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td align="center" id="Description" runat="server"></td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblHealthDescription" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblEnvDescription" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblEconomicDescription" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblWorstDescription" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblEquipmentBeingAssessed" runat="server" Text="Equipment"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input"
                                Width="120px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input"
                                Width="240px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowComponents" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                            <asp:LinkButton ID="lnkComponentAdd" runat="server" OnClick="lnkComponentAdd_Click" ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtjobid" runat="server" Visible="false" CssClass="input" Width="10px"></telerik:RadTextBox>
                            <div id="divComponents" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 420px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                <table id="tblcomponents" runat="server">
                                </table>
                            </div>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblActivity" runat="server" Text="Activities Affected"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="divActivities" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 420px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="chkActivities" runat="server" RepeatDirection="Horizontal" RepeatColumns="2">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblprocessra" runat="server" Text="Process RA"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="divprocessra" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 420px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <table id="tbldivprocessra" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblAternativeMethod" runat="server" Text="Alternative Method Considered for Work"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAternativeMethod" runat="server" CssClass="input" Width="94%"
                            Height="80px" TextMode="MultiLine" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblppe" runat="server" Text="PPE"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadTextBox ID="txtppe" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="4" Width="94%" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone2" runat="server" BorderStyle="None" FitDocks="true" Orientation="Horizontal" Width="99%">
                            <telerik:RadDock ID="RadDock1" runat="server" Closed="false" CssClass="higherZIndex" EnableAnimation="true" EnableDrag="false" EnableRoundedCorners="true" RenderMode="Lightweight" Resizable="true" Width="100%">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblaspect" Text="Aspects" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkaspectcomment" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuOperationalHazard" runat="server" TabStrip="false" />
                                    <telerik:RadGrid ID="gvRAOperationalHazard" runat="server" AllowCustomPaging="false" AllowPaging="false" AllowSorting="true" CellPadding="3" EnableHeaderContextMenu="true" EnableViewState="false" Font-Size="11px" GroupingEnabled="false" OnItemCommand="gvRAOperationalHazard_ItemCommand" OnItemDataBound="gvRAOperationalHazard_ItemDataBound" OnNeedDataSource="gvRAOperationalHazard_NeedDataSource" RenderMode="Lightweight" ShowFooter="false" ShowHeader="true" Width="100%">
                                        <SortingSettings EnableSkinSortStyles="false" SortedBackColor="#FFF6D6" />
                                        <MasterTableView AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDRACARGOOPERATIONID" EditMode="InPlace" ShowHeadersWhenNoRecords="true">
                                            <NoRecordsTemplate>
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Font-Bold="true" Font-Size="Larger" Text="No Records Found">
                                                            </telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <HeaderStyle Width="102px" />
                                            <CommandItemSettings ShowAddNewRecordButton="true" ShowExportToExcelButton="true" ShowExportToPdfButton="false" ShowPrintButton="true" ShowRefreshButton="true" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSNO") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aspect" HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOperationalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRACARGOOPERATIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblOperationalHazard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazards / Risks" HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazards" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONALHAZARD") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Controls / Precautions" HeaderStyle-Width="40%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblControls" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLPRECAUTIONS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Task Activity" HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:RadioButtonList ID="rblOptionEdit" runat="server" RepeatColumns="3" RepeatDirection="Vertical" OnSelectedIndexChanged="rblOptionEdit_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                            <asp:ListItem Value="0" Text="Before"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="During"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="After"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="ASPECTEDIT" ID="cmdAspectEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Map Hazard" CommandName="MAPIMPACT" ID="cmdEdit" ToolTip="Map Hazard">
                                                            <span class="icon"><i class="fas fa-biohazard"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="ASPECTDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings AllowColumnHide="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder" EnableRowHoverStyle="true" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" ScrollHeight="" UseStaticHeaders="true" />
                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" EnableRealTimeResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock2" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblhealthcomments" Text="Health and Safety Risk" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkhealthcomments" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvHealthSafetyRisk" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" EnableViewState="false" OnNeedDataSource="gvHealthSafetyRisk_NeedDataSource"
                                        Width="100%" CellPadding="3" OnItemCommand="gvHealthSafetyRisk_ItemCommand" OnItemDataBound="gvHealthSafetyRisk_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDCATEGORYID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="4%">
                                                    <ItemStyle HorizontalAlign="Left" Width="4%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aspects" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAspects" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard / Risk" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardRisk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDRISK") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard Type" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Undesirable Event" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblUndesirableEvent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNDISIRABLEEVENTLIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHealthid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Impact" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Operational Control" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOperational" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONAL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="PPE" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="RadLabel4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPPELIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="8%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="HMAPPING" ID="cmdEquipment" ToolTip="Controls Mapping">
                                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="HDELETE" ID="cmddelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone3" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock3" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblEnvironmental" Text="Environmental Risk" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkEnvironmental" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvEnvironmentalRisk" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" OnNeedDataSource="gvEnvironmentalRisk_NeedDataSource"
                                        CellPadding="3" OnItemCommand="gvEnvironmentalRisk_ItemCommand" OnItemDataBound="gvEnvironmentalRisk_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDCATEGORYID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="4%">
                                                    <ItemStyle HorizontalAlign="Left" Width="4%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aspects" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAspects" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard / Risk" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardRisk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDRISK") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard Type" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Undesirable Event" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblUndesirableEvent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNDISIRABLEEVENTLIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Impact Type" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblImpactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHealthid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Impact" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Operational Control" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOperational" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONAL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="8%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="ENVMAPPING" ID="cmdEquipment" ToolTip="Controls Mapping">
                                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="EDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone4" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock4" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblEconomicrisk" Text="Economic Risk" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkEconomic" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvEconomicRisk" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" OnNeedDataSource="gvEconomicRisk_NeedDataSource"
                                        CellPadding="3" OnItemCommand="gvEconomicRisk_ItemCommand" OnItemDataBound="gvEconomicRisk_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false"  DataKeyNames="FLDCATEGORYID">                                            
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="4%">
                                                    <ItemStyle HorizontalAlign="Left" Width="4%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aspects" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAspects" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard / Risk" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardRisk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDRISK") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard Type" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Undesirable Event" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblUndesirableEvent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNDISIRABLEEVENTLIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHealthid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Impact" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Operational Control" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOperational" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONAL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="ECOMAPPING" ID="cmdEquipment" ToolTip="Controls Mapping">
                                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="CDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone5" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock5" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblUndesirable" Text="Undesirable Event / Worst Case Scenario" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkUndesireable" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvevent" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" Width="100%"
                                        CellPadding="3" OnNeedDataSource="gvevent_NeedDataSource" OnItemCommand="gvevent_ItemCommand" OnItemDataBound="gvevent_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false"  DataKeyNames="FLDWORSTCASEID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="5%">
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Undesirable Event">
                                                    <ItemStyle Wrap="true"   HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblevent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>   
                                                <telerik:GridTemplateColumn HeaderText="Risks">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRisks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKESCALATION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Procedures">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <div id="divProcedures" runat="server" style="width: auto; border-width: 1px; border-style: solid; border: 0px solid #9f9fff">
                                                            <table id="tblProcedures" runat="server">
                                                            </table>
                                                        </div>
                                                        <%--<telerik:RadLabel ID="lblProcedures" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURES") %>'></telerik:RadLabel>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="PPE">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPPE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPPELIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Worst Case">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblWorstCaseid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASEID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblWorstCase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="EMAPPING" ID="cmdEquipment" ToolTip="Controls Mapping">
                                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="UDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone6" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock6" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblPersonal" Text="Personal" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkPersonal" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPersonal" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" OnNeedDataSource="gvPersonal_NeedDataSource"
                                        CellPadding="3" OnItemCommand="gvPersonal_ItemCommand" OnItemDataBound="gvPersonal_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false" EnableHeaderContextMenu="true" EnableViewState="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDCONTROLID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="80%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblItemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEM") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Option" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOption" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTION") %>'></telerik:RadLabel>
                                                        <asp:RadioButtonList ID="rblOptionEdit" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                                            AutoPostBack="true" OnSelectedIndexChanged="rblOptions_SelectedIndexChanged">
                                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="N/A"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone7" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock7" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblAdditionalControls" Text="Additional Controls and Supervision Level" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                   <table>
                                        <tr>
                                             <td valign="top" width="10%">
                                                <telerik:RadLabel ID="lblForms" runat="server" Text="Forms and Checklists"></telerik:RadLabel>
                                            </td>                                            
                                            <td valign="top" width="40%">



                                                <span id="spnPickListDocument">
                                                    <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="338px" Enabled="False" Style="font-weight: bold">
                                                    </telerik:RadTextBox>
                                                    <asp:LinkButton ID="btnShowDocuments" runat="server" ToolTip="Select Forms and Checklists">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                </span>
                                                <asp:LinkButton ID="lnkFormAdd" runat="server" OnClick="lnkFormAdd_Click" ToolTip="Add">
                            <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                                </asp:LinkButton>
                                                <br />
                                                <div id="divForms" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                                    <table id="tblForms" runat="server">
                                                    </table>
                                                </div>
                                            </td>
                                            <td valign="top" width="10%">
                                                <telerik:RadLabel ID="lbldocumentprocedures" runat="server" Text="Procedures"></telerik:RadLabel>
                                            </td>                                            
                                            <td valign="top" width="40%">
                                                <span id="spnPickListdocumentprocedure">
                                                    <telerik:RadTextBox ID="txtdocumentProcedureName" runat="server" Width="338px" Enabled="False" Style="font-weight: bold">
                                                    </telerik:RadTextBox>
                                                    <asp:LinkButton ID="btnShowdocumentProcedure" runat="server" ToolTip="Select Procedures">
                                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadTextBox ID="txtdocumentProcedureId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                </span>
                                                <asp:LinkButton ID="lnkdocumentProcedureAdd" runat="server" OnClick="lnkdocumentProcedureAdd_Click" ToolTip="Add">
                                                <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                                </asp:LinkButton>
                                                <br />
                                                <div id="divdocumentProcedure" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                                    <table id="tbldocumentprocedure" runat="server">
                                                    </table>
                                                </div>
                                            </td>                                              
                                        </tr>
                                        <tr>
                                            <td valign="top" width="15%">
                                                <telerik:RadLabel ID="lblSupervision" runat="server" Text="Supervision Level"></telerik:RadLabel>
                                            </td>
                                            <td valign="top" width="25%">
                                                <telerik:RadComboBox ID="ddlsupervisionlist" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="270px" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                                            </td>
                                            <td colspan="2"></td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone8" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock8" runat="server" Title="<b>Tasks Prior Commencement</b>" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MachinerySafety" runat="server" TabStrip="false" />
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvMachinerySafety" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" OnNeedDataSource="gvMachinerySafety_NeedDataSource"
                                        CellPadding="3" OnItemCommand="gvMachinerySafety_ItemCommand" OnItemDataBound="gvMachinerySafety_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false" EnableHeaderContextMenu="true" EnableViewState="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDMACHINERYSAFETYID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Task" HeaderStyle-Width="38%">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" Width="38%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASK") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMachinerySafetyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINERYSAFETYID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Responsibility" HeaderStyle-Width="24%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="24%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Target Date" HeaderStyle-Width="14%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEstimatedFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDFINISHDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Completed Date" HeaderStyle-Width="14%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblActualFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="SEDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="SDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone11" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock11" runat="server" Title="<b>Tasks During the activity</b>" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MachinerySafetyDuring" runat="server" TabStrip="false" />
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvMachinerySafetyDuring" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" OnNeedDataSource="gvMachinerySafetyDuring_NeedDataSource"
                                        CellPadding="3" OnItemCommand="gvMachinerySafetyDuring_ItemCommand" OnItemDataBound="gvMachinerySafetyDuring_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false" EnableHeaderContextMenu="true" EnableViewState="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDMACHINERYSAFETYID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Task" HeaderStyle-Width="38%">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" Width="38%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASK") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMachinerySafetyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINERYSAFETYID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Responsibility" HeaderStyle-Width="24%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="24%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Target Date" HeaderStyle-Width="14%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEstimatedFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDFINISHDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Completed Date" HeaderStyle-Width="14%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblActualFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="SDEDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="SDDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone9" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock9" runat="server" Title="<b>Tasks Upon Completion</b>" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MachinerySafetyAfter" runat="server" TabStrip="false" />
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvMachinerySafetyAfter" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" OnNeedDataSource="gvMachinerySafetyAfter_NeedDataSource"
                                        CellPadding="3" OnItemCommand="gvMachinerySafetyAfter_ItemCommand" OnItemDataBound="gvMachinerySafetyAfter_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false" EnableHeaderContextMenu="true" EnableViewState="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDMACHINERYSAFETYID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Task" HeaderStyle-Width="38%">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" Width="38%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASK") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMachinerySafetyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINERYSAFETYID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Responsibility" HeaderStyle-Width="24%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="24%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Target Date" HeaderStyle-Width="14%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEstimatedFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDFINISHDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Completed Date" HeaderStyle-Width="14%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblActualFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="SAEDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="SADELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone10" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock10" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="RadLabel1" Text="Approval and Verfication Remarks" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <table>
                                        <tr valign="middle">
                                            <td valign="top" width="15%">
                                                <telerik:RadLabel ID="lblRemarks" runat="server" Text="Approval Remarks"></telerik:RadLabel>
                                            </td>
                                            <td valign="top" width="35%">
                                                <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="readonlytextbox" Width="360px"
                                                    TextMode="MultiLine" Rows="4" ReadOnly="true" Resize="Both">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td valign="top" width="15%">
                                                <telerik:RadLabel ID="lblOverridebyMaster" runat="server" Text="Override by Master"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadLabel ID="lblReasonsforOverride" runat="server" Text="Reasons for Override"></telerik:RadLabel>
                                            </td>
                                            <td valign="top" width="35%">
                                                <asp:CheckBox ID="chkOverrideByMaster" runat="server" Enabled="false" />
                                                <br />
                                                <telerik:RadTextBox ID="txtMasterRemarks" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                                    Width="280px" Rows="4" ReadOnly="true">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblVerifcationRemarks" runat="server" Text="Verification Remarks"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtVerificationRemarks" runat="server" CssClass="input" Width="360px" ReadOnly="true"
                                                    Height="80px" TextMode="MultiLine" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)" Resize="Both">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblverifiedby" runat="server" Text="Verified By"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtverifiedby" runat="server" Width="270px"></telerik:RadTextBox>
                                                &nbsp;&nbsp;
                        <eluc:Date ID="ucverifieddate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="700px" Height="650px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
            VisibleStatusbar="false" KeepInScreenBounds="true">
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1" OnAjaxRequest="RadAjaxPanel2_AjaxRequest">
                    <table id="tbl1" border="0" style="width: 100%" runat="server" visible="false" cellspacing="5">
                        <tr>
                            <td valign="top" width="10%">
                                <telerik:RadLabel ID="lblequipnet" runat="server" Text="Equipment"></telerik:RadLabel>
                            </td>
                            <td valign="top" width="50%">
                                <telerik:RadCheckBoxList ID="chkEquipment" runat="server" CssClass="checkboxstyle" DataBindings-DataTextField="FLDCOMPONENTNAME" DataBindings-DataValueField="FLDCOMPONENTID" Style="height: 120px; width: 99%; overflow-y: auto; overflow-x: auto; border-width: 1px; border-style: solid; border: 1px solid #c3cedd"></telerik:RadCheckBoxList>


                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="10%">
                                <telerik:RadLabel ID="lblEvent" runat="server" Text="Undesirable Event"></telerik:RadLabel>
                            </td>
                            <td valign="top" width="50%">
                                <telerik:RadCheckBoxList ID="chkEvent" runat="server" AutoPostBack="false" CssClass="checkboxstyle" DataBindings-DataTextField="FLDUNDESIRABLEEVENTNAME" DataBindings-DataValueField="FLDUNDESIRABLEEVENTID" Style="height: 120px; width: 99%; overflow-y: auto; overflow-x: auto; border-width: 1px; border-style: solid; border: 1px solid #c3cedd"></telerik:RadCheckBoxList>
                            </td>
                        </tr>

                        <tr id="trPPE" runat="server" visible="false">
                            <td valign="top">
                                <telerik:RadLabel ID="lblPPETEXT" runat="server" Text="PPE"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:CheckBoxList ID="cblRecomendedPPE" runat="server" CssClass="checkboxstyle" RepeatDirection="Vertical" RepeatColumns="3" DataTextField="FLDNAME" DataValueField="FLDMISCELLANEOUSID" Style="height: 120px; width: 99%; overflow-y: auto; overflow-x: auto; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                </asp:CheckBoxList>


                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <telerik:RadButton ID="btnCreate" Text="Save" runat="server" OnClick="btnCreate_Click" Font-Bold="true"></telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                    <table id="tbl2" border="0" style="width: 100%" runat="server" visible="false" cellspacing="5">
                        <tr>
                            <td valign="top" width="20%">
                                <telerik:RadLabel ID="lblProcedure" runat="server" Text="Procedure"></telerik:RadLabel>
                            </td>
                            <td width="80%">
                                <span id="spnPickListProcedure">
                                    <telerik:RadTextBox ID="txtProcedure" runat="server" Width="403px" Style="font-weight: bold"
                                        CssClass="input">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="lnkProcedureList" runat="server" ToolTip="Select Procedures, Forms and Checklists">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtProcedureid" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                                <asp:LinkButton ID="lnkProcedureAdd" runat="server" OnClick="lnkProcedureAdd_Click" ToolTip="Add">
                                    <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                </asp:LinkButton>
                                <br />
                                <asp:CheckBoxList ID="cbProcedure" runat="server" DataTextField="FLDPROCEDURENAME" Style="width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd"
                                    DataValueField="FLDPROCEDUREID" AutoPostBack="true" CssClass="checkboxstyle" OnSelectedIndexChanged="cbProcedure_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <telerik:RadLabel ID="lblEquipment" runat="server" Text="Equipment"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListEquipment" runat="server" visible="false">
                                    <telerik:RadTextBox ID="txtEquipmentCode" runat="server" CssClass="input"
                                        Width="100px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtEquipment" runat="server" CssClass="input"
                                        Width="300px">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="lnkEquipmentList" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtEquipmentid" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                    <asp:LinkButton ID="lnkEquipmentAdd" runat="server" OnClick="lnkEquipmentAdd_Click" ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </span>
                                <asp:CheckBoxList ID="cbEventEquipment" runat="server" DataTextField="FLDCOMPONENTNAME"
                                    DataValueField="FLDCOMPONENTID" RepeatDirection="Vertical" CssClass="checkboxstyle" RepeatColumns="2" Style="width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <telerik:RadLabel ID="lblEventPPE" runat="server" Text="PPE"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:CheckBoxList ID="cbEventPPE" runat="server" RepeatDirection="Vertical" RepeatColumns="3"
                                    DataTextField="FLDNAME" DataValueField="FLDMISCELLANEOUSID" CssClass="checkboxstyle" Style="width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <telerik:RadLabel ID="lblWorstCase" runat="server" Text="Worst Case"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlWorstCase" runat="server" AppendDataBoundItems="true" EmptyMessage="Type to select" Filter="Contains"
                                    MarkFirstMatch="true" Width="270px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="Center">
                                <telerik:RadButton ID="btnEventsave" Text="Save" runat="server" OnClick="btnEventsave_Click"></telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
