<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaWeeklyPlannerCopyHours.aspx.cs"
    EnableEventValidation="false" Inherits="PreSeaWeeklyPlannerCopyHours" %>

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
<%@ Register TagPrefix="eluc" TagName="Subject" Src="~/UserControls/UserControlPreSeaBatchSubject.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreseaCurrentBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaSemester.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Week" Src="~/UserControls/UserControlPreSeaSemWeeks.ascx" %>
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
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Weekly Planner - Proposal"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MainMenuPreseaWeekPlanner" runat="server" OnTabStripCommand="MainMenuPreseaWeekPlanner_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%" id="tblCoursePlanner">
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
                                <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                    ReadOnly="true" />
                            </td>
                            <td>
                                Semester
                            </td>
                            <td>
                                <eluc:Semester ID="ucSemester" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                    ReadOnly="true" />
                            </td>
                            <td>
                                Section
                            </td>
                            <td>
                                <asp:Label ID="lblSectionId" runat="server" Visible="false" Text=""></asp:Label>
                                <asp:TextBox ID="txtSection" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td colspan="2" rowspan="4" valign="top" id="PracticalDetails" runat="server">
                                <b>&nbsp;Practical Hour Details </b>
                                <br />
                                <div id="div1" runat="server" style="position: relative; z-index: 0; width: 95%;">
                                    <asp:Literal ID="ltrlPracticalDetails" runat="server"></asp:Literal>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Week
                            </td>
                            <td>
                                <eluc:Week ID="ucWeek" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    AppendDataBoundItems="true" />
                            </td>
                            <td>
                                Day Start
                            </td>
                            <td>
                                <eluc:Number ID="txtDayStart" runat="server" Mask="99:99" CssClass="readonlytextbox"
                                    ReadOnly="true" />
                            </td>
                            <td>
                                Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
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
                        <tr>
                            <td valign="top">
                                Activity
                            </td>
                            <td valign="top">
                                <asp:RadioButtonList ID="rdoActivity" runat="server" RepeatDirection="Horizontal"
                                    RepeatColumns="2">
                                </asp:RadioButtonList>
                            </td>
                            <td valign="top">
                                Copy to date
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlCopyDate" runat="server" DataTextField="FLDDATE" DataValueField="FLDDATE"
                                    AutoPostBack="true" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    OnSelectedIndexChanged="ddlCopyDate_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                Copy to timeslot
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlCopyTimeSlots" runat="server" DataTextField="FLDHOURSLOT"
                                    DataValueField="FLDHOURSLOT" AppendDataBoundItems="true" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
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
