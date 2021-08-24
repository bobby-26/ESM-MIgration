<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBuddyPlanner.aspx.cs"
    Inherits="Presea_PreSeaBuddyPlanner" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Training Staff</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTrainingStaff" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlTrainingStaff">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Buddy Planner"></eluc:Title>
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBuddyPlanner" runat="server" OnTabStripCommand="MenuBuddyPlanner_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2;">
                    <table id="tblConfigureDesignation" width="100%">
                        <tr>
                            <td>
                                Faculty
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="input_mandatory" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <%--<td>
                                Year
                            </td>
                            <td>
                                <eluc:Quick ID="ucQuick" runat="server" CssClass="input" QuickTypeCode="55" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                Date
                            </td>
                            <td>
                                <eluc:Date ID="ucDate" runat="server" CssClass="input" />
                            </td>--%>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <table width="100%" runat="server">
                    <tr>
                        <td align="center">
                            <asp:Calendar ID="ucCalender" runat="server" Height="350px" Width="350px" 
                                Font-Size="Small" BackColor="White" 
                                NextPrevFormat="ShortMonth" OnDayRender="ucCalender_OnDayRender" ShowGridLines="true"
                                OnSelectionChanged="ucCalender_SelectedIndexChanged" 
                                onvisiblemonthchanged="ucCalender_VisibleMonthChanged">
                                <OtherMonthDayStyle ForeColor="InactiveCaptionText" />
                                <DayHeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <DayStyle CssClass="Datagrid_alternatingstyle" />
                                <TitleStyle CssClass="DataGrid-HeaderStyle" />
                                <NextPrevStyle CssClass="DataGrid-HeaderStyle" BackColor="#5588BB"  />
                                <SelectedDayStyle CssClass="datagrid_selectedstyle" />
                            </asp:Calendar>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
