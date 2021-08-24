<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceReportMaintenanceDue.aspx.cs"
    Inherits="PlannedMaintenanceReportMaintenanceDue" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />


        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%-- <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Maintenance Due" ShowMenu="True"></eluc:Title>
                    </div>
                </div>--%>
        <%--<div class="navSelect" style="top: 0px; right: 0px; position: absolute">--%>
        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <eluc:TabStrip ID="MenuReportMaintenanceDue" runat="server" OnTabStripCommand="MenuReportMaintenanceDue_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>
        <%--</div>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWorkOrderNumber" runat="server" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponentType" runat="server" Text="Component Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadMaskedTextBox RenderMode="Lightweight" Mask="###.##.##" runat="server" ID="txtComponentNumber"></telerik:RadMaskedTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input" MaxLength="50"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblResponsibleDiscipline" runat="server" Text="Responsible Discipline"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="Div3" class="input" style="overflow: auto; width: 70%; height: 100px">
                            <telerik:RadCheckBoxList ID="chkDiscipline" runat="server"
                                DataBindings-DataTextField="FLDDISCIPLINENAME" DataBindings-DataValueField="FLDDISCIPLINEID"
                                RepeatDirection="Vertical" Height="100%">
                            </telerik:RadCheckBoxList>
                        </div>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDueDateBetween" runat="server" Text="Due Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtDateFrom" CssClass="input" />
                        -
                        <eluc:Date runat="server" ID="txtDateTo" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblJobClasses" runat="server" Text="Job Classes"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="dvClass" class="input" style="overflow: auto; width: 70%; height: 100px">
                            <telerik:RadCheckBoxList ID="chkClasses" runat="server"
                                DataBindings-DataTextField="FLDQUICKNAME" DataBindings-DataValueField="FLDQUICKCODE"
                                RepeatDirection="Vertical" Height="100%">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPlanning" runat="server" Text="Planning"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div runat="server" id="dvPlaning" class="input" style="overflow: auto; width: 70%; height: 100px">
                            <telerik:RadCheckBoxList ID="ckPlaning" runat="server"
                                DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE"
                                RepeatDirection="Vertical" Height="100%">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainType" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMaintClass" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceCause" runat="server" Text="Maintenance Cause"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainCause" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUnplannedWork" runat="server" Text="Unplanned Work"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkUnexpected" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="dvStatus" class="input" style="overflow: auto; width: 70%; height: 100px">
                            <telerik:RadCheckBoxList ID="chkStatus" runat="server" DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE"
                                RepeatDirection="Vertical" Height="100%">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <eluc:Decimal runat="server" ID="txtPriority" Mask="9" CssClass="input" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClassCode" runat="server" Text="Class Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtClassCode" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
