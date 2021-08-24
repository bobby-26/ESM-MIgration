<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerBugAdd.aspx.cs"
    Inherits="DefectTrackerBugAdd" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="ModuleList" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugTypes" Src="~/UserControls/UserControlSEPBugType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugSeverity" Src="~/UserControls/UserControlSEPBugSeverity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugPriority" Src="~/UserControls/UserControlSEPBugPriority.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugStatus" Src="~/UserControls/UserControlSEPBugStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCode" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bug Insert</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
        <script type="text/javascript" language="javascript">
            function fnDefectTrackerBugEdit(dtkey) 
	    	{
                location.href = 'DefectTrackerBugEdit.aspx?dtkey=' + dtkey;
            }
        </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="pnlBugEntry" runat="server">
        <ContentTemplate>
            <div>
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        Add
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBugAdd" runat="server" OnTabStripCommand="MenuBugAdd_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table width="100%">
                    <tr>
                        <td colspan="4">
                            <font color="blue" size="0"><b>On Change Requests/Requirements</b>
                                <li>Users other than persons-in-charge of the requirements can log only Change Requests</li>
                                <li>Persons-in-charge of the requirements are required to "Approve" to make it as Requirement</li>
                            </font>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Project
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlProject" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                               OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" MaxLength="100" Width="150px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            For Vessel
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlvesselcode" runat="server" CssClass="input" MaxLength="100"
                                Width="150px">
                            </asp:DropDownList>                            
                        </td>
                        <td>
                            Module
                        </td>
                        <td>
                            <eluc:ModuleList ID="ddlModuleList" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Issue Type
                        </td>
                        <td>
                            <eluc:BugTypes ID="ddlBugType" runat="server" MaxLength="100" Width="360px" AppendDataBoundItems="true"
                                CssClass="dropdown_mandatory" />
                        </td>
                        <td valign="top">
                            Status
                        </td>
                        <td>
                            <eluc:BugStatus ID="ddlSEPBugStatus" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Severity
                        </td>
                        <td>
                            <eluc:BugSeverity ID="ddlBugSeverity" runat="server" MaxLength="100" Width="360px"
                                AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                        </td>
                        <td>
                            Priority
                        </td>
                        <td>
                            <eluc:BugPriority ID="ddlBugPriority" runat="server" MaxLength="100" Width="360px"
                                AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Subject
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtSubject" runat="server" MaxLength="200" CssClass="input_mandatory"
                                Width="90%"></asp:TextBox>
                        </td>
                        <td valign="top">
                            Reported By
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtReportedBy" runat="server" MaxLength="100" CssClass="input" Width="50%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Description
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" TextMode="MultiLine"
                                CssClass="input_mandatory" Rows="8" Columns="100"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
