<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBuddyPlannerReport.aspx.cs"
    Inherits="PreSeaBuddyPlannerReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Buddy Planner Report</title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaPlannerReport" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaPlannerReport">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Buddy Planner Report"></eluc:Title>
                    </div>
                </div>
                <div>
                    <table id="tblFind" runat="server">
                        <tr>
                            <td>
                                <asp:Literal ID="lblFromDate" Text="From Date" runat="server"> </asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <asp:Literal ID="lblToDate" Text="To Date" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblFaculty" runat="server" Text="Faculty"> </asp:Literal>
                            </td>
                            <td>
                                <div id="divFacultyID" runat="server" class="input" style="overflow: auto; height: 100px;
                                    width: 200px;">
                                    &nbsp;<asp:CheckBox ID="chkFacultyIDAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkFacultyIDAll_Changed"
                                        Text="---SELECT ALL---" />
                                    <asp:CheckBoxList ID="chkFacultyID" runat="server"  AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList AppendDataBoundItems="true" CssClass="input" runat="server" ID="ddlStatus" AutoPostBack="true">
                                    <asp:ListItem Text="--Select" Value="">--Select--</asp:ListItem>
                                    <asp:ListItem Text="Completed" Value="1">Completed </asp:ListItem>
                                    <asp:ListItem Text="Not Completed" Value="0">Not Completed</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaPlannerReport" runat="server" OnTabStripCommand="MenuPreSeaPlannerReport_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table width="100%">
                    <td>
                        <asp:Literal runat="server" ID="ltGrid" Text=""></asp:Literal>
                    </td>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
