<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobCRAdd.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceComponentJobCRAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentJob" Src="../UserControls/UserControlMultiColumnComponentJob.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Job Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenPick" runat="server" CssClass="hidden" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuComponentGeneral" runat="server" OnTabStripCommand="PlannedMaintenanceComponent_TabStripCommand"></eluc:TabStrip>
            <b>Request Values</b>
            <br clear="all" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponentJob" runat="server" Text="Component Job"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:ComponentJob ID="txtComponentJobId" runat="server" OnTextChangedEvent="cmdHiddenPick_Click" AutoPostBack="true" />
                        <%-- <span id="spnPickCompJob">
                        <telerik:RadTextBox ID="txtCompJobCode" runat="server" CssClass="input_mandatory"
                            MaxLength="20" ReadOnly="false" Width="60px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCompJobName" runat="server" CssClass="input_mandatory"
                            MaxLength="200" ReadOnly="false" Width="210px"></telerik:RadTextBox>
                        <img id="img1" runat="server" onclick="return showPickList('spnPickCompJob', 'codehelp1', '', '../Common/CommonPickListComponentJob.aspx', true); "
                            src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />
                        <telerik:RadTextBox ID="txtComponentJobId" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                    </span>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChangeRequestType" runat="server" Text="Request Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlChangeReqType" CssClass="input_mandatory" runat="server">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="" />
                                <telerik:DropDownListItem Text="Update" Value="1" />
                                <telerik:DropDownListItem Text="Delete" Value="2" />
                            </Items>

                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarksChange" runat="server" Rows="2" TextMode="MultiLine" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListJob">
                            <telerik:RadTextBox ID="txtJobCode" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                ReadOnly="false" Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtJobName" runat="server" CssClass="input readonlytextbox" MaxLength="200"
                                ReadOnly="false" Width="210px">
                            </telerik:RadTextBox>
                            <%-- <img id="imgJob" runat="server" onclick="return showPickList('spnPickListJob', 'codehelp1', '', '../Common/CommonPickListJob.aspx', true); "
                                    src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />--%>
                            <telerik:RadTextBox ID="txtJobId" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                        </span>&nbsp;
                         <%--   <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click" />--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Discipline ID="ucDiscipline" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
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
                        <eluc:Decimal runat="server" ID="txtFrequency" MaxLength="3" IsInteger="true"
                            Width="45px" />
                        <eluc:Hard ID="ucFrequency" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWindowDays" runat="server" Text="Window (Days)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtWindow" runat="server" Text="0" MaxLength="2"
                            IsInteger="true" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="lblRunHour" runat="server" Text="Run Hour :"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCounterFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Hard ID="ucCounterType" runat="server" AppendDataBoundItems="true" />
                        <eluc:Decimal runat="server" ID="ucCounterFrequency" MaxLength="6"
                            IsInteger="true" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHistoryMandatory" runat="server" Text="History Mandatory"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:YesNo ID="ucMandatory" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlanningMethod" runat="server" Text="Planning Method"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucPlanningMethod" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration(Hrs)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtDuration" MaxLength="8" Width="60px"
                            IsInteger="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtPriority" MaxLength="1" Width="60px"
                            IsInteger="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainType" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMaintClass" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceClaim" runat="server" Text="Maintenance Claim"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainCause" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <%--<telerik:RadLabel ID="lblHistoryTemplate" runat="server" Text="History Template"></telerik:RadLabel>--%>
                    </td>
                    <td>
                        <%--<eluc:HistoryTemplate ID="ucHistory" runat="server"  AppendDataBoundItems="true" />--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlannedforComponentStatus" runat="server" Text="Planned for Component Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadListBox ID="cblJobStatus" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" CheckBoxes="true"
                            RepeatDirection="Horizontal">
                        </telerik:RadListBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReference" runat="server" Text="Reference"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReference" runat="server" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <div id="divOldValues" runat="server">
                <hr />
                <b>Actual Values</b>
                <br clear="all" />
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnOPickListJob">
                                <telerik:RadTextBox ID="txtOJobCode" runat="server" CssClass="readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="60px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtOJobName" runat="server" CssClass="readonlytextbox " MaxLength="200"
                                    ReadOnly="false" Width="210px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtOJobId" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                            </span>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Discipline ID="ucODiscipline" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <b>
                                <telerik:RadLabel ID="lblOCalendar" runat="server" Text="Calendar :"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal runat="server" ID="txtOFrequency" CssClass="readonlytextbox" MaxLength="3" IsInteger="true"
                                Width="45px" Enabled="false" />
                            <eluc:Hard ID="ucOFrequency" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOWindowDays" runat="server" Text="Window (Days)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtOWindow" runat="server" CssClass="readonlytextbox" Text="0" MaxLength="2"
                                IsInteger="true" Width="60px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <b>
                                <telerik:RadLabel ID="lblORunHour" runat="server" Text="Run Hour :"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOCounterFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Hard ID="ucOCounterType" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Enabled="false" />
                            <eluc:Decimal runat="server" ID="ucOCounterFrequency" CssClass="readonlytextbox" MaxLength="6"
                                IsInteger="true" Width="60px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOHistoryMandatory" runat="server" Text="History Mandatory"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOHistoryMandatory" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOPlanningMethod" runat="server" Text="Planning Method"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucOPlanningMethod" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOEstimatedDurationHrs" runat="server" Text="Estimated Duration(Hrs)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal runat="server" ID="txtODuration" MaxLength="8" CssClass="readonlytextbox" Width="60px"
                                IsInteger="true" Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOPriority" runat="server" Text="Priority"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal runat="server" ID="txtOPriority" MaxLength="1" CssClass="readonlytextbox" Width="60px"
                                IsInteger="true" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucOMainType" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucOMaintClass" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOMaintenanceClaim" runat="server" Text="Maintenance Claim"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucOMainCause" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Enabled="false" />
                        </td>
                        <td>
                            <%--<telerik:RadLabel ID="lblOHistoryTemplate" runat="server" Text="History Template"></telerik:RadLabel>--%>
                        </td>
                        <td>
                            <%--<eluc:HistoryTemplate ID="ucOHistory" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Enabled="false" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOPlannedforComponentStatus" runat="server" Text="Planned for Component Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadListBox ID="cblOJobStatus" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" CheckBoxes="true"
                                RepeatDirection="Horizontal" Enabled="false">
                            </telerik:RadListBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOReference" runat="server" Text="Reference"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOReference" runat="server" CssClass="readonlytextbox" Enabled="false" Width="200px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:Status ID="ucStatus" runat="server" />
        </div>
    </form>
</body>
</html>
