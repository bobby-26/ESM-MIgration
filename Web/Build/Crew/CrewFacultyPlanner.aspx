<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewFacultyPlanner.aspx.cs" Inherits="Crew_CrewFacultyPlanner" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="cldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Faculty Planner</title>
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
            <eluc:TabStrip ID="MenuTitle" runat="server" OnTabStripCommand="MenuTitle_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <table id="tblFacultySearch" width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcourse" runat="server" Width="350px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCourseId" runat="server" Width="100px" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtinstitutefacultyId" runat="server" Width="100px" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstituteMappingId" runat="server" Width="100px"></telerik:RadTextBox>                        
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
                        <eluc:CldYear AppendDataBoundItems="true" ID="ddlCalenderYear" runat="server"
                            CssClass="input_mandatory" AutoPostBack="true" OnTextChangedEvent="ddlCalenderYear_TextChangedEvent" />
                    </td>
                </tr>
            </table>

            <br />
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
            <telerik:RadScheduler ID="cldFacultyPlanner" runat="server" SelectedView="MonthView" MonthView-UserSelectable="true"
                ShowNavigationPane="true" OnNavigationComplete="cldFacultyPlanner_NavigationComplete"
                OnNavigationCommand="cldFacultyPlanner_NavigationCommand" OnAppointmentDelete="cldFacultyPlanner_AppointmentDelete"
                OnAppointmentDataBound="cldFacultyPlanner_AppointmentDataBound" OnTimeSlotCreated="cldFacultyPlanner_TimeSlotCreated"
                 MonthView-MinimumRowHeight="1" Height="100%" OnFormCreating="cldFacultyPlanner_FormCreating">
                <AdvancedForm Modal="true"></AdvancedForm>
                <WeekView UserSelectable="false" />
                <DayView UserSelectable="false" />
                <MultiDayView UserSelectable="false" />
                <TimelineView UserSelectable="false" />
                <MonthView UserSelectable="false" />
                <TimeSlotContextMenuSettings EnableDefault="true"></TimeSlotContextMenuSettings>
                <AppointmentContextMenuSettings EnableDefault="true"></AppointmentContextMenuSettings>
            </telerik:RadScheduler>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
