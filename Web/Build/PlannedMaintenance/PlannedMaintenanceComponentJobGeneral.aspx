<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobGeneral.aspx.cs"
    Inherits="PlannedMaintenanceComponentJobGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HistoryTemplate" Src="~/UserControls/UserControlHistoryTemplate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="../UserControls/UserControlRank.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPMSComponentJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="ajxPanel" runat="server">
            <eluc:TabStrip ID="MenuComponentGeneral" runat="server" OnTabStripCommand="PlannedMaintenanceComponent_TabStripCommand" Title="General"></eluc:TabStrip>

            <br clear="all" />

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Literal ID="lblJobDescription" runat="server" Text="Job Description"></asp:Literal>
                    </td>
                    <td>
                        <span id="spnPickListJob">
                            <asp:TextBox ID="txtJobCode" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                ReadOnly="false" Width="60px"></asp:TextBox>
                            <asp:TextBox ID="txtJobName" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                ReadOnly="false" Width="210px"></asp:TextBox>
                            <img id="imgJob" runat="server" onclick="return showPickList('spnPickListJob', 'codehelp1', '', '../Common/CommonPickListJob.aspx', true); "
                                src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />
                            <asp:TextBox ID="txtJobId" runat="server" CssClass="input" Width="10px"></asp:TextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click" />
                    </td>
                    <td>
                        <asp:Literal ID="lblResponsibility" runat="server" Text="Responsibility"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Discipline ID="ucDiscipline" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkCheckList" Text="Checklist Reporting" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <b>
                            <asp:Literal ID="lblCalendar" runat="server" Text="Calendar :"></asp:Literal></b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblFrequency" runat="server" Text="Frequency"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtFrequency" CssClass="input" MaxLength="3" IsInteger="true"
                            Width="45px" OnTextChangedEvent="LastDoneDateCalculation" AutoPostBack="true" />
                        <eluc:Hard ID="ucFrequency" runat="server" CssClass="input" AppendDataBoundItems="true"
                            OnTextChangedEvent="LastDoneDateCalculation" AutoPostBack="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblWindowDays" runat="server" Text="Window (Days)"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtWindow" runat="server" CssClass="input" Text="0" MaxLength="2"
                            IsInteger="true" Width="60px" />
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkAttachment" Text="Attachment Required" runat="server" OnCheckedChanged="chkAttachment_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblLastDoneDate" runat="server" Text="Last Done Date"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="txtlastDoneDate" OnSelectedDateChanged="txtlastDoneDate_SelectedDateChanged" AutoPostBack="true"></telerik:RadDatePicker>
                        
                    </td>
                    <td>
                        <asp:Literal ID="lblNextDueDate" runat="server" Text="Next Due Date"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNextDueDate" runat="server" CssClass="input readonlytextbox"
                            MaxLength="20" Width="80px" ReadOnly="true"></asp:TextBox>
                        <%-- <ajaxToolkit:CalendarExtender ID="ceTxtNextDueDate" runat="server" Format="dd/MMM/yyyy"
                            Enabled="True" TargetControlID="txtNextDueDate" PopupPosition="TopLeft">
                        </ajaxToolkit:CalendarExtender>--%>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="txtInstructions" TextMode="MultiLine" Rows="2" EmptyMessage="Type Attachment Instructions" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                        <b>
                            <asp:Literal ID="lblRunHour" runat="server" Text="Run Hour :"></asp:Literal>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblCounterFrequency" runat="server" Text="Frequency"></asp:Literal>
                    </td>
                    <td colspan="5">
                        <eluc:Hard ID="ucCounterType" runat="server" CssClass="input" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ucCounterType_OnTextChangedEvent" />
                        <eluc:Decimal runat="server" ID="ucCounterFrequency" CssClass="input" MaxLength="6"
                            IsInteger="true" Width="60px" AutoPostBack="true" OnTextChangedEvent="ucCounterFrequency_TextChanged" />
                        <eluc:Decimal runat="server" ID="txtAverage" CssClass="input" MaxLength="14" Visible="false" />
                        <asp:TextBox ID="txtCounterDate" runat="server" CssClass="input" Width="80px" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtCounterValue" runat="server" CssClass="input" Width="80px" Visible="false"></asp:TextBox>
                    </td>
                    <%--     <td>
                        Next Due Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtCouterNextDueDate" runat="server" CssClass="input readonlytextbox" MaxLength="20" Width="80px" ReadOnly="true"></asp:TextBox>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblLastDoneHrs" runat="server" Text="Last Done (Hrs)"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtLastDoneHours" CssClass="input" Width="90px"
                            MaxLength="7" IsInteger="true" AutoPostBack="true" OnTextChangedEvent="ucCounterFrequency_TextChanged" />
                    </td>
                    <td>
                        <asp:Literal ID="lblNextDueHrs" runat="server" Text="Next Due (Hrs)"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="ucNextDueHours" CssClass="readonlytextbox" Width="60px"
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
                        <telerik:RadLabel ID="lblMapRA" runat="server" Text="Templates"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <span id="spnRA">
                            <telerik:RadTextBox ID="txtRANumber" runat="server" CssClass="input readonlytextbox"
                                MaxLength="50" Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtRA" runat="server" CssClass="input readonlytextbox"
                                MaxLength="50" Width="200px">
                            </telerik:RadTextBox>
                            <%--<asp:ImageButton ID="imgShowRA" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." ToolTip="Pick RA Template" OnClick="imgShowRA_Click" />--%>
                            <telerik:RadTextBox ID="txtRAId" runat="server" CssClass="hidden" MaxLength="20" Width="50px"
                                Text="">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtRaType" runat="server" CssClass="hidden" MaxLength="2" Width="50px"
                                Text=''>
                            </telerik:RadTextBox>
                        </span>

                        <asp:ImageButton runat="server" AlternateText="Show RA Details" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>"
                            ID="cmdRA" ToolTip="Show PDF" OnClick="cmdRA_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblHistoryMandatory" runat="server" Text="History Mandatory"></asp:Literal>
                    </td>
                    <td>
                        <eluc:YesNo ID="ucMandatory" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <asp:Literal ID="lblPlanningMethod" runat="server" Text="Planning Method"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Hard ID="ucPlanningMethod" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPostOverhaulcheck" runat="server" Text="Post Overhaul Operational Test and Check"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPostOverHaulCheck" runat="server" OnCheckedChanged="chkPostOverHaulCheck_CheckedChanged"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="EstimatedDurationHrs" runat="server" Text="Estimated Duration(Hrs)"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtDuration" MaxLength="8" CssClass="input" Width="60px"
                            IsInteger="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblPriority" runat="server" Text="Priority"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtPriority" MaxLength="1" CssClass="input" Width="60px"
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
                        <asp:Literal ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainType" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMaintClass" runat="server" CssClass="input" AppendDataBoundItems="true" />
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
                        <asp:Literal ID="lblMaintenanceClaim" runat="server" Text="Maintenance Claim"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainCause" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblHistoryTemplate" runat="server" Text="History Template"></asp:Literal>
                    </td>
                    <td>
                        <eluc:HistoryTemplate ID="ucHistory" runat="server" CssClass="input" AppendDataBoundItems="true" />
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
                        <asp:Literal ID="lblPlannedforComponentStatus" runat="server" Text="Planned for Component Status"></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="cblJobStatus" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"
                            RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </td>
                    <td>
                        <asp:Literal ID="lblReference" runat="server" Text="Reference"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReference" runat="server" CssClass="input"></asp:TextBox>
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
