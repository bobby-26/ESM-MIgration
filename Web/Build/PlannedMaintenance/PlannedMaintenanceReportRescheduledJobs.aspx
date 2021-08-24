<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceReportRescheduledJobs.aspx.cs"
    Inherits="PlannedMaintenanceReportRescheduledJobs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rescheduled Jobs</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRescheduledJobsFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />


        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <eluc:TabStrip ID="MenuReportRescheduledJobs" runat="server" OnTabStripCommand="MenuReportRescheduledJobs_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>

        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text="Workorder Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWorkOrderNumber" runat="server" CssClass="input" MaxLength="20"
                        Width="120px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWorkOrderDescription" runat="server" CssClass="input" MaxLength="20"
                        Width="160px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtComponentNumber" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                        MaxLength="20" Width="120px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtComponentName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                        MaxLength="20" Width="160px">
                    </telerik:RadTextBox>
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
                        <telerik:RadCheckBoxList ID="chkClasses" runat="server" DataBindings-DataTextField="FLDQUICKNAME" DataBindings-DataValueField="FLDQUICKCODE"
                            RepeatDirection="Vertical" Height="100%">
                        </telerik:RadCheckBoxList>
                    </div>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPlannedDueDateBetween" runat="server" Text="Planned Due Date Between"></telerik:RadLabel>
                    <br />
                    <br />
                    <telerik:RadLabel ID="lblPostponedDate" runat="server" Text="Postponed Date"></telerik:RadLabel>
                </td>
                <td>
                    <%-- <asp:TextBox ID="txtDateFrom" runat="server" CssClass="input" Width="60px"></asp:TextBox>
                    <ajaxtoolkit:calendarextender id="CalendarExtender2" runat="server" format="dd/MMM/yyyy"
                        enabled="True" targetcontrolid="txtDateFrom" popupposition="TopLeft">
                        </ajaxtoolkit:calendarextender>--%>
                    <eluc:Date runat="server" ID="txtDateFrom" CssClass="input" />
                    -
                        <%--<asp:TextBox ID="txtDateTo" runat="server" CssClass="input" Width="60px"></asp:TextBox>--%>
                    <%--<ajaxtoolkit:calendarextender id="CalendarExtender1" runat="server" format="dd/MMM/yyyy"
                        enabled="True" targetcontrolid="txtDateTo" popupposition="TopLeft">
                        </ajaxtoolkit:calendarextender>--%>
                    <eluc:Date runat="server" ID="txtDateTo" CssClass="input" />
                    <br />
                    <br />
                    <%-- <asp:TextBox ID="txtPostponedDate" runat="server" CssClass="input" Width="60px"></asp:TextBox>
                    <ajaxtoolkit:calendarextender id="CalendarExtender3" runat="server" format="dd/MMM/yyyy"
                        enabled="True" targetcontrolid="txtPostponedDate" popupposition="TopLeft">
                        </ajaxtoolkit:calendarextender>--%>
                    <eluc:Date runat="server" ID="txtPostponedDate" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <telerik:RadLabel ID="lblWorkorderStatus" runat="server" Text="Workorder Status"></telerik:RadLabel>
                </td>
                <td>
                    <div runat="server" id="dvStatus" class="input" style="overflow: auto; width: 70%; height: 100px">
                        <telerik:RadCheckBoxList ID="chkStatus" runat="server" DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE"
                            RepeatDirection="Vertical" Height="60%">
                            <Items>
                                <telerik:ButtonListItem Text="Completed" Value="1"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="NotCompleted" Value="2"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadCheckBoxList>
                    </div>
                </td>
                <td valign="top">
                    <telerik:RadLabel ID="lblWorkorderType" runat="server" Text="Workorder Type"></telerik:RadLabel>
                </td>
                <td valign="top">
                    <div runat="server" id="dvWoType" class="input" style="overflow: auto; width: 70%; height: 100px">
                        <telerik:RadCheckBoxList ID="ckWotype" runat="server" DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE"
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
                    <telerik:RadLabel ID="lblPriorityBetween" runat="server" Text="Priority Between"></telerik:RadLabel>
                </td>
                <td valign="top">
                    <%--<eluc:Decimal runat="server" ID="txtPriorityFrom" Mask="9" CssClass="input" Width="20px" />--%>
                    <telerik:RadTextBox runat="server" ID="txtPriorityFrom" MaxLength="1" EnabledStyle-HorizontalAlign="Right" CssClass="input" Width="20px"></telerik:RadTextBox>
                    -
                        <%--<eluc:Decimal runat="server" ID="txtPriorityTo" Mask="9" CssClass="input" Width="20px" />--%>
                    <telerik:RadTextBox runat="server" ID="txtPriorityTo" MaxLength="1" EnabledStyle-HorizontalAlign="Right" CssClass="input" Width="20px"></telerik:RadTextBox>
                </td>
                <td valign="top">
                    <telerik:RadLabel ID="lblDurationHoursBetween" runat="server" Text="Duration Hours Between"></telerik:RadLabel>
                </td>
                <td valign="top">
                    <%--<eluc:Decimal runat="server" ID="txtDurationFrom" Mask="99999999" CssClass="input"
                        Width="50px" />--%>
                    <telerik:RadTextBox runat="server" ID="txtDurationFrom" MaxLength="8" EnabledStyle-HorizontalAlign="Right" CssClass="input" Width="50px"></telerik:RadTextBox>
                    -
                       <%-- <eluc:Decimal runat="server" ID="txtDurationTo" Mask="99999999" CssClass="input"
                            Width="50px" />--%>
                    <telerik:RadTextBox runat="server" ID="txtDurationTo" MaxLength="8" EnabledStyle-HorizontalAlign="Right" CssClass="input" Width="50px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
