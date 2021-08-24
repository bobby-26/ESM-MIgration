<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaWeeklyPlanner.aspx.cs"
    EnableEventValidation="false" Inherits="PreSeaWeeklyPlanner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Faculty" Src="~/UserControls/UserControlFaculty.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreseaCurrentBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaSemester.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Week" Src="~/UserControls/UserControlPreSeaSemWeeks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Subject" Src="~/UserControls/UserControlPreSeaBatchSubject.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TimeSlots" Src="~/UserControls/UserControlTimeSlots.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Planner</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWeeklyPlanner" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWeeklyPlanner">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Label ID="lblCourseId" runat="server" Visible="false"></asp:Label>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ucTitle" Text="Weekly Planner - Proposal"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="PreSeaMenu" runat="server" OnTabStripCommand="PreSeaMenu_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="subHeader">
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MainMenuPreseaWeekPlanner" runat="server" OnTabStripCommand="MainMenuPreseaWeekPlanner_TabStripCommand" TabStrip="true">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%" id="tblCoursePlanner">
                        <tr style="color: Blue;" valign="top">
                            <td colspan="4">
                                1. To view the Weekly proposed plan, Select Batch,Semester,Section and Week
                                <br />
                                2. Once batch selected, based on the current date, the screen will automaticaly
                                select semester and week,
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;If the cuurent day is part of semester working day.
                            </td>
                            <td colspan="4">
                                3. To Clear selectecd data and add plan for another Day/Hour, Click Add button
                                <br />
                                4. Edit option will not be available for the plan,in which the actual status updated.
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Batch
                            </td>
                            <td>
                                <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    OnTextChangedEvent="Batch_Changed" AutoPostBack="true" />
                            </td>
                            <td>
                                Semester
                            </td>
                            <td>
                                <eluc:Semester ID="ucSemester" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    OnTextChangedEvent="Semester_Changed" AutoPostBack="true" />
                            </td>
                            <td>
                                Section
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSection" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    DataTextField="FLDSECTIONNAME" DataValueField="FLDSECTIONID" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSection_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2" rowspan="4" valign="top" id="PracticalDetails" runat="server">
                                <b>&nbsp;Practical Hour Details </b>
                                <br />
                                <div id="div1" runat="server" style="position: relative; z-index: 0; width: 95%;">
                                    <asp:GridView ID="grdPracticalTimetable" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false" EnableViewState="true"
                                        OnRowDataBound="grdPracticalTimetable_RowDataBound">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Division
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDivisionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRACTICALID")%>'></asp:Label>
                                                    <asp:Label ID="lblDivisionName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDGROUPNAME")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Subject
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Subject ID="ucPracSubject" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Instructor
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlInstructor" runat="server" CssClass="input" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Week
                            </td>
                            <td>
                                <eluc:Week ID="ucWeek" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnTextChangedEvent="Week_Changed" />
                            </td>
                            <td>
                                Day Start
                            </td>
                            <td>
                                <eluc:Number ID="txtDayStart" runat="server" Mask="99:99" CssClass="input_mandatory" />
                            </td>
                            <td>
                                Date
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDate" runat="server" DataTextField="FLDDATE" DataValueField="FLDDATE"
                                    AutoPostBack="true" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" rowspan="2">
                                Activity
                            </td>
                            <td valign="top" rowspan="2">
                                <asp:RadioButtonList ID="rdoActivity" runat="server" RepeatDirection="Horizontal"
                                    RepeatColumns="2" AutoPostBack="true" OnSelectedIndexChanged="rdoActivity_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </td>
                            <td valign="top">
                                Class Duration
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlDuration" runat="server" CssClass="input_mandatory">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="15 mins" Value="0.15"></asp:ListItem>
                                    <asp:ListItem Text="30 mins" Value="0.30"></asp:ListItem>
                                    <asp:ListItem Text="45 mins" Value="0.45"></asp:ListItem>
                                    <asp:ListItem Text="1 hr" Value="1.00"></asp:ListItem>
                                    <asp:ListItem Text="1.5 hr" Value="1.50"></asp:ListItem>
                                    <asp:ListItem Text="2 hrs" Value="2.00"></asp:ListItem>
                                    <asp:ListItem Text="2.5 hrs" Value="2.50"></asp:ListItem>
                                    <asp:ListItem Text="3 hrs" Value="3.00"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                Time&nbsp;from&nbsp;:&nbsp;
                            </td>
                            <td valign="top">
                                <eluc:TimeSlots ID="ucTimeSlotFrom" runat="server" AppendDataBoundItems="true" Width="75px"
                                    CssClass="input_mandatory" />
                                to&nbsp;:&nbsp;
                                <eluc:TimeSlots ID="ucTimeSlotTo" runat="server" AppendDataBoundItems="true" Width="75px"
                                    AutoPostBack="true" CssClass="input_mandatory" OnTextChangedEvent="TimeSlot_Changed" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Class Room
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlClassRoom" runat="server" CssClass="input" DataValueField="FLDROOMID"
                                    DataTextField="FLDROOMNAME" AppendDataBoundItems="true">                                 
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                &nbsp;
                            </td>
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="ClassDetails" runat="server">
                            <td valign="top">
                                Subject
                            </td>
                            <td valign="top">
                                <eluc:Subject ID="ucSubject" runat="server" CssClass="input" AppendDataBoundItems="true" />
                            </td>
                            <td valign="top">
                                Class taken by
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="input" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Break Description
                            </td>
                            <td>
                                <asp:TextBox ID="txtBreakDesc" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="PreparedBy" runat="server">
                            <td valign="top">
                                Prepared By
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlFacultyPreparedBy" runat="server" CssClass="input" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                            <td colspan="4"></td>
                        </tr>
                    </table>
                </div>
                <div style="position: relative; width: 100%">
                    <table width="100%" cellpadding="1" cellspacing="1" runat="server">
                        <tr>
                            <td colspan="2">
                                <div style="text-align: center; margin: 10px; vertical-align: bottom;">
                                    <asp:Button ID="btnPrevWeek" runat="server" CssClass="input" Text="View Previous Week"
                                        OnClick="PrevClick" />
                                    <asp:Button ID="btnNextWeek" runat="server" CssClass="input" Text="View Next Week"
                                        OnClick="NextClick" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                                    <eluc:TabStrip ID="MenuPreSeaWeekPlanner" runat="server" OnTabStripCommand="MenuPreSeaWeekPlanner_TabStripCommand">
                                    </eluc:TabStrip>
                                </div>
                                <div id="div2" runat="server" style="position: relative; z-index: 0; width: 100%;">
                                    <asp:GridView ID="gvPreseaWeeklyPlanner" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false" OnRowDataBound="gvPreseaWeeklyPlanner_RowDataBound"
                                        EnableViewState="false" OnDataBound="gvPreseaWeeklyPlanner_DataBound" OnRowCommand="gvPreseaWeeklyPlanner_RowCommand"
                                        OnRowDeleting="gvPreseaWeeklyPlanner_RowDeleting">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" VerticalAlign="Middle" />
                                        <Columns>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
