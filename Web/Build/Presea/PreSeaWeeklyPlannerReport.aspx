<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaWeeklyPlannerReport.aspx.cs" Inherits="PreSeaWeeklyPlannerReport" %>

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
<head runat="server">
    <title>Weekly Planner Report</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

   </telerik:RadCodeBlock>
</head>
<body>
     <form id="frmWeeklyPlannerReport" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWeeklyPlannerReport">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Label ID="lblCourseId" runat="server" Visible="false"></asp:Label>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ucTitle" Text="Weekly Planner Report"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="" OnClick="cmdHiddenSubmit_Click" />
                </div>
<%--                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="PreSeaMenu" runat="server" OnTabStripCommand="PreSeaMenu_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>--%>
      <%--          <div class="subHeader">
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MainMenuPreseaWeekPlannerReport" runat="server" OnTabStripCommand="MainMenuPreseaWeekPlannerReport_TabStripCommand" TabStrip="true">
                        </eluc:TabStrip>
                    </div>
                </div>--%>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="80%" id="tblCoursePlanner">
                       
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
                        
                        </tr>
                        <tr>
                            <td>
                                Section
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSection" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    DataTextField="FLDSECTIONNAME" DataValueField="FLDSECTIONID" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSection_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Week
                            </td>
                            <td colspan=''>
                                <eluc:Week ID="ucWeek" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnTextChangedEvent="Week_Changed" />
                            </td>
                           
                        </tr>
                      
                    </table>
                </div>
                <div style="position: relative; width: 100%">
                    <table id="Table1" width="100%" cellpadding="1" cellspacing="1" runat="server">
                       <%-- <tr>
                            <td colspan="2">
                                <div style="text-align: center; margin: 10px; vertical-align: bottom;">
                                    <asp:Button ID="btnPrevWeek" runat="server" CssClass="input" Text="View Previous Week"
                                        OnClick="PrevClick" />
                                    <asp:Button ID="btnNextWeek" runat="server" CssClass="input" Text="View Next Week"
                                        OnClick="NextClick" />
                                </div>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="2">
                                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                                    <eluc:TabStrip ID="MenuPreSeaWeekPlannerReport" runat="server" OnTabStripCommand="MenuPreSeaWeekPlannerReport_TabStripCommand">
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
