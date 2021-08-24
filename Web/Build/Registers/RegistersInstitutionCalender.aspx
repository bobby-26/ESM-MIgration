<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersInstitutionCalender.aspx.cs" Inherits="Registers_RegistersInstitutionCalender" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Institute Calender generate</title>
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--    <eluc:title runat="server" id="ucTitle" text="Institute Calendar" showmenu="false" />--%>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtInstituteMappingId" runat="server" Width="100px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:CldMonth AppendDataBoundItems="true" ID="ddlCalenderMonth" runat="server"
                            CssClass="input" AutoPostBack="true" OnTextChangedEvent="ddlCalenderMonth_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:CldYear AppendDataBoundItems="true" ID="ddlCalenderYear" runat="server"
                            CssClass="input" AutoPostBack="true" OnTextChangedEvent="ddlCalenderYear_TextChangedEvent" />
                    </td>
                </tr>

            </table>
            <br />
            <table style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToViewtheGuidelines" runat="server" Text="Place the mouse pointer in this icon"></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadToolTip RenderMode="Lightweight" ID="btnTooltipHelp" runat="server" Height="100%" TargetControlID="btnHelp" HideDelay="10000">
                        </telerik:RadToolTip>
                        <asp:LinkButton runat="server" AlternateText="Guide" ID="btnHelp" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                        </asp:LinkButton>
                      <%--  <img id="imgnotes" runat="server" src="<%$ PhoenixTheme:images/54.png %>" style="vertical-align: top; cursor: pointer"
                            alt="NOTES" />
                        &nbsp;
                                    <telerik:RadLabel ID="lblbutton" runat="server" Text="button."></telerik:RadLabel>
                        <eluc:ToolTip ID="ucToolTipNW" runat="server" />--%>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="to view Guidelines"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <telerik:RadScheduler ID="cldInstitute" runat="server" SelectedView="MonthView" MonthView-UserSelectable="false" ShowNavigationPane="false"
                OnNavigationCommand="cldInstitute_NavigationCommand" OnAppointmentDelete="cldInstitute_AppointmentDelete"
                OnAppointmentDataBound="cldInstitute_AppointmentDataBound" OnFormCreating="cldInstitute_FormCreating"
                OnTimeSlotCreated="cldInstitute_TimeSlotCreated" MonthView-MinimumRowHeight="1" Height="70%">
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
