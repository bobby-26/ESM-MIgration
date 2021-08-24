<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaWeeklyPlannerIndividualReport.aspx.cs"
    Inherits="PreSeaWeeklyPlannerIndividualReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreseaCurrentBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaSemester.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Week" Src="~/UserControls/UserControlPreSeaSemWeeks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Faculty" Src="~/UserControls/UserControlFaculty.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Individual Week Plan Report</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmIndividualWeekPlanReport" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWeeklyPlannerReport">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" style="position: relative">
                <eluc:Title runat="server" ID="ucTitle" Text="Weekly Planner Report - Individual"
                    ShowMenu="false"></eluc:Title>
            </div>
            <table width="60%">
                <tr>
                    <td>
                       <asp:Literal ID="lblBatch" runat="server" Text="Batch"> </asp:Literal>
                    </td>
                    <td>
                        <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            OnTextChangedEvent="Batch_Changed" AutoPostBack="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblSemester" runat="server" Text="Semester"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Semester ID="ucSemester" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            OnTextChangedEvent="Semester_Changed" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>                    
                    <td>
                        <asp:Literal ID="lblWeek" runat="server" Text="Week"></asp:Literal>
                    </td>
                    <td colspan=''>
                        <eluc:Week ID="ucWeek" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="Week_Changed" />
                    </td>
                     <td>
                       <asp:Literal ID="lblFaculty" runat="server" Text="Faculty"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <div class="navSelect" style="position: relative; clear: both; width: 15px">
                <eluc:TabStrip ID="MenuPreSeaWeekPlannerReport" runat="server" OnTabStripCommand="MenuPreSeaWeekPlannerReport_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="div2" runat="server" style="position: relative; z-index: 0; width: 100%;">
                <asp:GridView ID="gvPreseaWeeklyPlannerReport" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false" OnRowDataBound="gvPreseaWeeklyPlannerReport_RowDataBound"
                    EnableViewState="false" OnDataBound="gvPreseaWeeklyPlannerReport_DataBound" OnRowCommand="gvPreseaWeeklyPlannerReport_RowCommand">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
