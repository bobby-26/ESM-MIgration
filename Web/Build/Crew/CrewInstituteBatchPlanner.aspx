<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInstituteBatchPlanner.aspx.cs" Inherits="Crew_CrewInstituteBatchPlanner" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="cldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Course Plan</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function btnconfirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" OnClick="confirm_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuTitle" runat="server" OnTabStripCommand="MenuTitle_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <table id="tblFacultySearch">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCourse" runat="server" Width="350px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtcourseId" runat="server" Width="0px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBatchId" runat="server" Width="0px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstituteMappingId" runat="server" Width="0px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="txtPlan" runat="server" Text="Plan"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                                </td>
                                <td></td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <eluc:Date ID="txtplanFrom" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDash" runat="server" Text="-"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtPlanTo" runat="server" CssClass="input" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <br />
            <table style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToviewtheGuidelinesplacethemouseonthe" runat="server" Text="Place the mouse pointer in this icon"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadToolTip RenderMode="Lightweight" ID="btnTooltipHelp" runat="server" Height="100%" TargetControlID="btnHelp" HideDelay="10000">
                        </telerik:RadToolTip>
                        <asp:LinkButton runat="server" AlternateText="Guide" ID="btnHelp" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                        </asp:LinkButton>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="to view Guidelines"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvplan" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvplan_ItemCommand" OnNeedDataSource="gvplan_NeedDataSource"
                OnItemDataBound="gvplan_ItemDataBound" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <img id="imgPlan" alt="" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Month" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProposedDate" runat="server" CommandName="Select"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblMonthNumber" runat="server" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHNUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Year" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYear" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="110px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Plan"
                                    CommandName="PLAN" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPlan"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-arrow-circle-right"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
             <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDocumentsExpired" runat="server" Text="Course Planned"></asp:Literal>
                    </td>                  
                </tr>
            </table>
            <br />
            <telerik:RadLabel ID="lblMonthPlan" runat="server" Text="Month Plan"></telerik:RadLabel>
            <telerik:RadScheduler ID="cldBathPlanner1" runat="server" SelectedView="MonthView" MonthView-UserSelectable="false" ShowNavigationPane="false"
                OnNavigationCommand="cldBathPlanner1_NavigationCommand" OnAppointmentDelete="cldBathPlanner1_AppointmentDelete"
                OnAppointmentDataBound="cldBathPlanner1_AppointmentDataBound" OnFormCreating="cldBathPlanner1_FormCreating"
                OnTimeSlotCreated="cldBathPlanner1_TimeSlotCreated" MonthView-MinimumRowHeight="1" Height="70%">
                <AdvancedForm Modal="true"></AdvancedForm>

                <WeekView UserSelectable="false" />
                <DayView UserSelectable="false" />
                <MultiDayView UserSelectable="false" />
                <TimelineView UserSelectable="false" />
                <MonthView UserSelectable="false" />
                <TimeSlotContextMenuSettings EnableDefault="true"></TimeSlotContextMenuSettings>
                <AppointmentContextMenuSettings EnableDefault="true"></AppointmentContextMenuSettings>
            </telerik:RadScheduler>

            <%--  <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_ConfirmMesage"
                OKText="Yes" CancelText="No" Visible="false" />--%>
            <asp:HiddenField ID="hdnCourseCalendarId" runat="server" />
            <asp:HiddenField ID="hdnMonth" runat="server" />
            <asp:HiddenField ID="hdnYear" runat="server" />
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
