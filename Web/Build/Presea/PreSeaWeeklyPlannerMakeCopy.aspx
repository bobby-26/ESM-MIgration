<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaWeeklyPlannerMakeCopy.aspx.cs"
    EnableEventValidation="false" Inherits="PreSeaWeeklyPlannerMakeCopy" %>

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
    <form id="frmCoursePlanner" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionRecordAndResponseEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Label ID="lblCourseId" runat="server" Visible="false"></asp:Label>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ucTitle" Text="Weekly Planner - Proposal"></eluc:Title>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="PreSeaMenu" runat="server" OnTabStripCommand="PreSeaMenu_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="subHeader">
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MainMenuPreseaWeekPlanner" runat="server" OnTabStripCommand="MainMenuPreseaWeekPlanner_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%" id="tblCoursePlanner">
                        <tr style="color: Blue;" valign="top">
                            <td colspan="3">
                                1. First select Batch, Semester,Section and Week, then
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;planned dates and unplaned dates will be loaded.
                                <br />
                                2. To copy a plan, select a planned date,
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;After selecting date, you can see plan details.
                            </td>
                            <td colspan="3">
                                3. Then select unplanned date in 'Copy to' drop down
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;To make a copy, click 'Make Copy' button
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
                                    DataTextField="FLDSECTIONNAME" DataValueField="FLDSECTIONID">
                                </asp:DropDownList>
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
                                Planned Date
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPlanned" runat="server" DataTextField="FLDDATE" DataValueField="FLDDAYID" AutoPostBack="true"
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddlPlanned_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Copy to
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUnPlanned" runat="server" DataTextField="FLDDATE" DataValueField="FLDDATE"
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <br style="clear: both;" />
                    <table width="100%" id="PlanDetails" runat="server">
                        <tr>
                            <td colspan="6" style="font-weight: bold;">
                                Plan Details for the date of :&nbsp;
                                <asp:Label ID="lblPlanDate" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:GridView ID="gvPlanDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="60%" CellPadding="3" EnableViewState="false">
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>
                                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                        <asp:TemplateField HeaderText="Time Slot">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                Time Slot
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDHOURSLOTS")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IMU Exam">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                Class Details
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDCLASSDETAILS")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
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
