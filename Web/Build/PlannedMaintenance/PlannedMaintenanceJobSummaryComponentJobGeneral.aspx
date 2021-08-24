<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobSummaryComponentJobGeneral.aspx.cs"
    Inherits="PlannedMaintenanceJobSummaryComponentJobGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HistoryTemplate" Src="~/UserControls/UserControlHistoryTemplate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="../UserControls/UserControlRank.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component Job</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

    <style type="text/css">
        .display {
            display: initial;
        }
    </style>
</head>
<body>
    <form id="frmPMSComponentJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <%--<telerik:RadSkinManager ID="RadSkinManager1" runat="server" />--%>

        <eluc:TabStrip ID="MenuComponentGeneral" runat="server" OnTabStripCommand="PlannedMaintenanceComponent_TabStripCommand"></eluc:TabStrip>

        <br clear="all" />
        <telerik:RadAjaxPanel ID="Panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                    </td>
                    <td>
                            <telerik:RadTextBox ID="txtJobCode" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                ReadOnly="false" Width="70px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtJobName" runat="server" CssClass="input readonlytextbox" MaxLength="200"
                                ReadOnly="false" Width="210px">
                            </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Discipline ID="ucDiscipline" runat="server" CssClass="input" Width="210px" AppendDataBoundItems="true" />
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkCheckList" Text="Checklist Reporting" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <b>
                            <telerik:RadLabel ID="lblCalendar" runat="server" Text="Calendar :"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFrequency" MaxLength="3" IsInteger="true" InputType="Number"
                            Width="70px" AutoPostBack="true">
                        </telerik:RadTextBox>
                        <eluc:Hard ID="ucFrequency" runat="server" AppendDataBoundItems="true" Width="210px"
                             AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWindowDays" runat="server" Text="Window (Days)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWindow" runat="server" CssClass="input" Text="0" MaxLength="2"
                            IsInteger="true" Width="40px" />
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkAttachment" Text="Attachment Required" runat="server" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text="Last Done Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="txtlastDoneDate" AutoPostBack="true"></telerik:RadDatePicker>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNextDueDate" runat="server" Text="Next Due Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNextDueDate" runat="server" CssClass="input readonlytextbox"
                            MaxLength="20" Width="90px" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="txtInstructions" TextMode="MultiLine" Rows="2" EmptyMessage="Type Attachment Instructions" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                        <b>
                            <telerik:RadLabel ID="lblRunHour" runat="server" Text="Run Hour :"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCounterFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCounterType" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" />
                        <%--<eluc:Decimal runat="server" ID="ucCounterFrequency" CssClass="input" MaxLength="6"
                            IsInteger="true" Width="60px" AutoPostBack="true" OnTextChangedEvent="ucCounterFrequency_TextChanged" />--%>

                        <telerik:RadTextBox runat="server" ID="ucCounterFrequency" CssClass="input" MaxLength="6"
                            IsInteger="true" Width="60px" AutoPostBack="true">
                        </telerik:RadTextBox>
                        <eluc:Decimal runat="server" ID="txtAverage" CssClass="input" MaxLength="14" Visible="false" />
                        <telerik:RadTextBox ID="txtCounterDate" runat="server" CssClass="input" Width="80px" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCounterValue" runat="server" CssClass="input" Width="80px" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastDoneHrs" runat="server" Text="Last Done (Hrs)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtLastDoneHours" CssClass="input" Width="90px"
                            MaxLength="7" IsInteger="true" AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNextDueHrs" runat="server" Text="Next Due (Hrs)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="ucNextDueHours" CssClass="readonlytextbox" Width="60px"
                            Style="text-align: right;" Enabled="false" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                        <b>
                            <telerik:RadLabel ID="lblRA" runat="server" Text="Risk Assessment :"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadCheckBox ID="chkRAMandatory" Text="Required" AutoPostBack="false" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMapRA" runat="server" Text="Template"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                            <telerik:RadTextBox ID="txtRANumber" runat="server" CssClass="input readonlytextbox"
                                MaxLength="50" Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtRA" runat="server" CssClass="input readonlytextbox"
                                MaxLength="50" Width="200px">
                            </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHistoryMandatory" runat="server" Text="History Mandatory"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:YesNo ID="ucMandatory" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlanningMethod" runat="server" Text="Planning Method"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucPlanningMethod" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPostOverhaulcheck" runat="server" Text="Post Overhaul Operational Test and Check"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPostOverHaulCheck" runat="server" ></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration(Hrs)"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:Decimal runat="server" ID="txtDuration" MaxLength="8" CssClass="input" Width="60px"
                            IsInteger="true" />--%>
                        <telerik:RadTextBox runat="server" ID="txtDuration" MaxLength="8" CssClass="input" Width="70px"
                            IsInteger="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:Decimal runat="server" ID="txtPriority" MaxLength="1" CssClass="input" Width="60px"
                            IsInteger="true" />--%>
                        <telerik:RadTextBox runat="server" ID="txtPriority" MaxLength="1" CssClass="input" Width="70px"
                            IsInteger="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDetailofCheck" runat="server" Text="Details of Check"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDetilsofCheck" runat="server" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainType" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td style="width: 102px">
                        <telerik:RadLabel ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMaintClass" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInterval" runat="server" Text="Interval"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblInterval" runat="server">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="Hours" />
                                <telerik:ButtonListItem Value="1" Text="Days" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceClaim" runat="server" Text="Maintenance Claim"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainCause" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHistoryTemplate" runat="server" Text="History Template" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:HistoryTemplate ID="ucHistory" runat="server" CssClass="input" AppendDataBoundItems="true" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIntervalValue" runat="server" Text="Interval after the Overhaul "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInterValue" runat="server">
                            <ClientEvents OnKeyPress="keyPress" />
                        </telerik:RadTextBox>
                        <script type="text/javascript">
                            function keyPress(sender, args) {
                                var text = sender.get_value() + args.get_keyCharacter();
                                if (!text.match('^[0-9]+$'))
                                    args.set_cancel(true);
                            }
                        </script>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblPlannedforComponentStatus" runat="server" Text="Planned for Component Status"></telerik:RadLabel>
                    </td>
                    <td style="display: initial">
                        <telerik:RadCheckBoxList CssClass=".display" ID="cblJobStatus" runat="server" DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE"
                            RepeatDirection="Horizontal">
                        </telerik:RadCheckBoxList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReference" runat="server" Text="Reference"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReference" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCeVerify" runat="server" Text="Vessel Verification Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupntVerify" runat="server" Text="Supnt Verification Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkSupntVerification" runat="server"></telerik:RadCheckBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
