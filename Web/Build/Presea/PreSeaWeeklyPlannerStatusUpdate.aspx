<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaWeeklyPlannerStatusUpdate.aspx.cs"
    EnableEventValidation="false" Inherits="PreSeaWeeklyPlannerStatusUpdate" %>

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

        <%--<script type="text/javascript">
            function CalculateEndtime()
            {
              var duration = document.getElementById('<%=txtDuration.userCtlClientId %>');
              var timefrom = document.getElementById('<%=txtTimeFrom.userCtlClientId %>');
              var timeto = document.getElementById('<%=txtTimeFrom.userCtlClientId %>');
              
              timeto.value = (parseInt(timefrom.value)+parseInt(duration.value+"00")).toString();
            
            }
        </script>--%>
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
                    <eluc:Title runat="server" ID="ucTitle" Text="Weekly Planner - Actual"></eluc:Title>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%" id="tblCoursePlanner">
                        <tr style="color: Blue;" valign="top">
                            <td colspan="4">
                                1. To view the Weekly proposed plan, Select Batch,Semester,Section and Week
                                <br />
                                2. Once batch selected, based on the current date, the screen will automaticaly
                                select semester and week,
                            </td>
                            <td colspan="4">
                                3. To upadte the status(Class Conducted as per planned), check the checkbox in each hour slot.
                                <br />
                                4. The check box will not be available future days plan
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
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
                            <td>
                                Week
                            </td>
                            <td>
                                <eluc:Week ID="ucWeek" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnTextChangedEvent="Week_Changed" />
                            </td>
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
                                        EnableViewState="false" OnDataBound="gvPreseaWeeklyPlanner_DataBound">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                        <Columns>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
