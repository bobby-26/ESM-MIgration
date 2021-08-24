<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentTypeJobGeneral.aspx.cs"
    Inherits="PlannedMaintenanceComponentTypeJobGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HistoryTemplate" Src="~/UserControls/UserControlHistoryTemplate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPMSComponentTypeJob" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="General" ShowMenu="false"></eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuComponentTypeGeneral" runat="server" OnTabStripCommand="PlannedMaintenanceComponentType_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlComponentTypeGeneral">
        <ContentTemplate>
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
                        <asp:Literal ID="lblWindowsDays" runat="server" Text="Windows(Days)"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtWindow" runat="server" CssClass="input" Mask="999" Text="0"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblFrequency" runat="server" Text="Frequency"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtFrequency" Mask="999" Text="0" CssClass="input"
                            Width="45px" />
                        <eluc:Hard ID="ucFrequency" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblPlanningMethod" runat="server" Text="Planning Method"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Hard ID="ucPlanningMethod" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblHistoryTemplate" runat="server" Text="History Template"></asp:Literal>
                    </td>
                    <td>
                        <eluc:HistoryTemplate ID="ucHistory" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblMandatory" runat="server" Text="Mandatory"></asp:Literal>
                    </td>
                    <td>
                        <eluc:YesNo ID="ucMandatory" runat="server" CssClass="input" />
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
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblMaintenanceClaim" runat="server" Text="Maintenance Claim"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainCause" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblPriority" runat="server" Text="Priority"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtPriority" Mask="9" CssClass="input" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblResponsibility" runat="server" Text="Responsibility"></asp:Literal>
                    </td>
                    <td colspan="3">
                        <eluc:Discipline ID="ucDiscipline" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblCounterType" runat="server" Text="Counter Type"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCounterType" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblCounterValues" runat="server" Text="Counter Values"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="ucCounterValues" Mask="999999999" CssClass="input"
                            Width="60px" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
