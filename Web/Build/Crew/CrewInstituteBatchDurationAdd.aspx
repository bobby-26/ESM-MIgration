<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInstituteBatchDurationAdd.aspx.cs" Inherits="Crew_CrewInstituteBatchDurationAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="cldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Plan Edit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="Title" runat="server" />
            <table id="tblFacultySearch" width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCourse" runat="server" Width="350px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtcourseId" runat="server" Width="100px" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBatchId" runat="server" Width="100px" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstituteId" runat="server" Width="100px" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:cldMonth AppendDataBoundItems="true" ID="ddlMonth" runat="server"
                            CssClass="input_mandatory" AutoPostBack="true" OnTextChangedEvent="ddlMonth_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:CldYear AppendDataBoundItems="true" ID="ddlYear" runat="server"
                            CssClass="input_mandatory" AutoPostBack="true" OnTextChangedEvent="ddlYear_TextChangedEvent" />
                    </td>
                </tr>
            </table>

            <br />
            <table style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToviewtheGuidelinesplacethemouseonthe" runat="server" Text="To view the Guidelines, place the mouse on the"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadToolTip RenderMode="Lightweight" ID="btnTooltipHelp" runat="server" Height="100%" TargetControlID="btnHelp" HideDelay="10000">
                        </telerik:RadToolTip>
                        <asp:LinkButton runat="server" AlternateText="Guide" ID="btnHelp" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadScheduler ID="cldBathPlanner" runat="server" SelectedView="MonthView" MonthView-UserSelectable="true" 
                ShowNavigationPane="true" OnNavigationComplete="cldBathPlanner_NavigationComplete"
                OnNavigationCommand="cldBathPlanner_NavigationCommand" OnAppointmentDelete="cldBathPlanner_AppointmentDelete"
                OnAppointmentDataBound="cldBathPlanner_AppointmentDataBound" OnFormCreating="cldBathPlanner_FormCreating"
                OnTimeSlotCreated="cldBathPlanner_TimeSlotCreated" MonthView-MinimumRowHeight="1" Height="100%">
                <AdvancedForm Modal="true"></AdvancedForm>

                <WeekView UserSelectable="false" />
                <DayView UserSelectable="false" />
                <MultiDayView UserSelectable="false" />
                <TimelineView UserSelectable="false" />
                <MonthView UserSelectable="false"  />
                <TimeSlotContextMenuSettings EnableDefault="true"></TimeSlotContextMenuSettings>
                <AppointmentContextMenuSettings EnableDefault="true"></AppointmentContextMenuSettings>
            </telerik:RadScheduler>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
