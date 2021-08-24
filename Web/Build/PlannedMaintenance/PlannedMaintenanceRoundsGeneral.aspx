<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceRoundsGeneral.aspx.cs"
    Inherits="PlannedMaintenanceRoundsGeneral" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HistoryTemplate" Src="~/UserControls/UserControlHistoryTemplate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPMSComponentJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />

        <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" CssClass="hidden" />
        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <eluc:TabStrip ID="MenuComponentGeneral" runat="server" OnTabStripCommand="PlannedMaintenanceComponent_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>

        <br clear="all" />
        <asp:UpdatePanel runat="server" ID="pnlComponentGeneral">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lbltitle" runat="server" Text="Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txttitle" runat="server" CssClass='input_mandatory' Width="300" MaxLength="200"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPickListJob">
                                <telerik:RadTextBox ID="txtJobCode" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="90px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtJobName" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="210px">
                                </telerik:RadTextBox>
                                <asp:LinkButton ID="imgJob" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click">
                                    <span class="icon"><i class="fas fa-paint-brush"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox ID="txtJobId" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                            </span>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Discipline ID="ucDiscipline" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
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
                            <telerik:RadTextBox runat="server" ID="txtFrequency" CssClass="input_mandatory" MaxLength="3"
                                IsInteger="true" Width="45px" OnTextChangedEvent="LastDoneDateCalculation" AutoPostBack="true" />
                            <eluc:Hard ID="ucFrequency" runat="server" CssClass="input_mandatory" AppendDataBoundItems="false"
                                OnTextChangedEvent="LastDoneDateCalculation" AutoPostBack="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblWindowDays" runat="server" Text="Window (Days)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtWindow" runat="server" CssClass="input" Text="0" MaxLength="2" IsInteger="true" Width="60px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text="Last Done Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtlastDoneDate" CssClass="input" OnTextChangedEvent="LastDoneDateCalculation" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblNextDueDate" runat="server" Text="Next Due Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNextDueDate" runat="server" CssClass="input readonlytextbox"
                                MaxLength="20" Width="80px" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
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
                            <eluc:Hard ID="ucPlanningMethod" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration(Hrs)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal runat="server" ID="txtDuration" MaxLength="8" CssClass="input" Width="60px" IsInteger="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtPriority" MaxLength="1" CssClass="input" Width="60px" IsInteger="true" />
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
                            <telerik:RadLabel ID="lblMaintenanceCause" runat="server" Text="Maintenance Cause"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucMainCause" runat="server" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <%--<telerik:RadLabel ID="lblHistoryTemplate" runat="server" Text="History Template"></telerik:RadLabel>--%>
                        </td>
                        <td>
                            <%--<eluc:HistoryTemplate ID="ucHistory" runat="server" AppendDataBoundItems="true" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPlannedforComponentStatus" runat="server" Text="Planned for Component Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <div runat="server" id="dvStatus" class="input" style="overflow: auto; width: 22%; height: 98px">
                                <telerik:RadCheckBoxList ID="cblJobStatus" runat="server" DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE" RepeatDirection="Horizontal">
                                </telerik:RadCheckBoxList>
                            </div>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblReference" runat="server" Text="Reference"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtReference" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
