<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlanSchedule.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDailyWorkPlanSchedule" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function pageLoad() {
                var spn = document.querySelector(".rsPrevDay");
                spn.title = "previous month";
                
                spn = document.querySelector(".rsNextDay");
                spn.title = "next month";  
                setTimeout(function () { setSchedulerHeight() }, 200);
            }            
            function OnClientAppointmentMoveStart(sender, eventArgs) {
                eventArgs.set_cancel(true);
            } 
            function resize() {
                setTimeout(function () { setSchedulerHeight() }, 200);
            }
            function setSchedulerHeight() {
                var $ = $telerik.$;
                var height = $(window).height();
                var scheduler = $find('<%=schDWP.ClientID %>');
                scheduler.set_height("" + height + "px");
                document.querySelector(".rsContentTable").style.height = "100%";
                document.querySelector(".rsContentWrapper").style.height = (height - 75) + "px";
                document.querySelector(".rsContentScrollArea").style.height = (height - 75) + "px"; 
            }
            window.onresize = window.onload = resize;
        </script>
        <style type="text/css">
            .item {
                position: relative;
                padding-top: 20px;
                display: inline-block;
            }
            .item a {
                color: black !important;
                font-weight: bold !important;
            }
            .rsMonthView .rsTodayCell
            {
                border: 1px solid red !important;
            }  
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="schDWP">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="schDWP" UpdatePanelHeight="100%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="cmdHiddenSubmit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="schDWP" UpdatePanelHeight="100%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button runat="server" ID="cmdHiddenSubmit" Text="Refresh" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadScheduler runat="server" ID="schDWP" DataKeyField="FLDDAILYWORKPLANID" DataSubjectField="FLDNAME" DataStartField="FLDACTUALDATE" DataEndField="FLDACTUALDATE"
            OnNavigationComplete="schDWP_NavigationComplete" ShowAllDayRow="true" StartEditingInAdvancedForm="false" AllowInsert="false" AllowEdit="false"
            ReadOnly="true" MonthView-VisibleAppointmentsPerDay="10" OverflowBehavior="Expand" Height="100%" OnClientAppointmentMoveStart="OnClientAppointmentMoveStart"
            AllowDelete="false" CustomAttributeNames="DWPID, COUNT, CATEGORYID" 
            SelectedView="MonthView" OnTimeSlotCreated="schDWP_TimeSlotCreated">
            <WeekView UserSelectable="false" />
            <DayView UserSelectable="false" />
            <MultiDayView UserSelectable="false" />
            <TimelineView UserSelectable="false" />
            <MonthView UserSelectable="false" />           
            <AdvancedForm Enabled="false" />
            <%--<AppointmentTemplate>
                <div class="item">
                    <a href="javascript:top.openNewWindow('dp','Daily Work Plan','PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetail.aspx?p=<%# (Container.Appointment.Attributes["DWPID"]) %>&eid=<%# (Container.Appointment.Attributes["CATEGORYID"]) %>')"
                         title='<%#Eval("Subject") %> - <%# (Container.Appointment.Attributes["COUNT"]) %>'>
                        <%#Eval("Subject") %> - <%# (Container.Appointment.Attributes["COUNT"]) %>
                    </a>
                </div>
            </AppointmentTemplate>
            <InlineEditTemplate>
                <table border="0" style="width: 100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                        </td>
                        <td>

                            <telerik:RadDatePicker ID="txtDate" runat="server" Width="120px" DateInput-ReadOnly="true">
                                <DateInput Enabled="false" runat="server"></DateInput>
                                <DatePopupButton Enabled="false" runat="server" />
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselStatus" runat="server" Text="Vessel Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblVesselStatus" runat="server" CssClass="input_mandatory"
                                Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="In Port" Value="1" />
                                    <telerik:ButtonListItem Text="At Sea" Value="2" />
                                    <telerik:ButtonListItem Text="Both" Value="3" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblChangeTime" runat="server" Text="Change Time"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTimePicker ID="tpChangeTime" runat="server"></telerik:RadTimePicker>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <telerik:RadButton ID="btnSave" Text="Save" runat="server" CommandName="Insert"></telerik:RadButton>
                            <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </InlineEditTemplate>
            <InlineInsertTemplate>
                <div id="InlineInsertTemplate" class="rsCustomAppointmentContainer">
                    <div>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date" CssClass="inline-label"></telerik:RadLabel>

                        <telerik:RadDatePicker ID="txtDate" runat="server" Width="120px" DateInput-ReadOnly="true">
                            <DateInput Enabled="false" runat="server"></DateInput>
                            <DatePopupButton Enabled="false" runat="server" />
                        </telerik:RadDatePicker>
                    </div>
                    <div>
                        <telerik:RadLabel ID="lblVesselStatus" runat="server" Text="Vessel Status" CssClass="inline-label"></telerik:RadLabel>

                        <telerik:RadRadioButtonList ID="rblVesselStatus" runat="server" CssClass="input_mandatory"
                            Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="In Port" Value="1" Selected="true" />
                                <telerik:ButtonListItem Text="At Sea" Value="2" />
                                <telerik:ButtonListItem Text="Both" Value="3" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </div>
                    <div>
                        <telerik:RadLabel ID="lblChangeTime" runat="server" Text="Change Time" CssClass="inline-label"></telerik:RadLabel>

                        <telerik:RadTimePicker ID="tpChangeTime" runat="server"></telerik:RadTimePicker>
                    </div>
                    <div>
                        <telerik:RadButton ID="btnSave" Text="Save" runat="server" CommandName="Insert"></telerik:RadButton>
                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></telerik:RadButton>
                    </div>
                </div>
            </InlineInsertTemplate>
            <AdvancedInsertTemplate>
                <div class="qsfexAdvEditControlWrapper">
                    <div id="qsfexAdvEditInnerWrapper">
                        <div class="qsfexAdvEditControlWrapper">
                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="" Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Copy To" CssClass="inline-label"></telerik:RadLabel>

                            <telerik:RadDatePicker ID="txtDate" runat="server" Width="120px">
                            </telerik:RadDatePicker>
                        </div>
                        <div class="qsfexAdvEditControlWrapper">
                            <telerik:RadButton ID="btnSave" Text="Copy" runat="server" CommandName="Update"></telerik:RadButton>
                            <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></telerik:RadButton>
                        </div>
                    </div>
                </div>
            </AdvancedInsertTemplate>
            <TimeSlotContextMenus>
                <telerik:RadSchedulerContextMenu runat="server" ID="SchedulerTimeSlotContextMenu">
                    <Items>
                        <telerik:RadMenuItem Text="New" ImageUrl="../css/Theme1/images/add.png" Value="CommandAddAppointment">
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Copy" ImageUrl="../css/Theme1/images/copy.png" Value="CommandAddRecurringAppointment">
                        </telerik:RadMenuItem>
                    </Items>
                </telerik:RadSchedulerContextMenu>
            </TimeSlotContextMenus>
            <AppointmentContextMenuSettings EnableDefault="true"></AppointmentContextMenuSettings>--%>
        </telerik:RadScheduler>
    </form>
</body>
</html>
