<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchManageDetails.aspx.cs"
    Inherits="PreSeaBatchManageDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Date Of Availabilty</title>
  <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
      </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewDateOfAvailability" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDOA">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Batch Manager" ShowMenu="true" />
                    </div>
                    <div style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuBatchPlanner" runat="server" OnTabStripCommand="BatchPlanner_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
                <div class="subHeader" style="position: relative">
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <h4>
                    General Details</h4>
                <table cellpadding="1" cellspacing="1" width="90%">
                    <tr>
                        <td>
                            Batch
                        </td>
                        <td>
                            <asp:TextBox ID="txtBatchName" runat="server" CssClass="readonlytextbox" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            No of Students (Min)
                        </td>
                        <td>
                            <eluc:Number runat="server" ID="ucMinLimit" CssClass="readonlytextbox" IsInteger="true" />
                        </td>
                        <td>
                            No of Students (Max)
                        </td>
                        <td>
                            <eluc:Number runat="server" ID="ucMaxLimit" CssClass="readonlytextbox" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            No of Semester
                        </td>
                        <td valign="top">
                            <eluc:Number runat="server" ID="txtNoOfSem" CssClass="readonlytextbox" IsInteger="true" />
                        </td>
                        <td valign="top">
                            No of Section
                        </td>
                        <td valign="top">
                            <eluc:Number runat="server" ID="txtNoofSection" CssClass="input_mandatory" IsInteger="true" />
                        </td>
                        <td valign="top">
                            No of Practical div per sect
                        </td>
                        <td valign="top">
                            <eluc:Number runat="server" ID="txtNoDivperSection" CssClass="input_mandatory" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            TDC In-Charge
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlFacultyTDC" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                </asp:DropDownList>
                        </td>
                        <td valign="top">
                            Course in-charge & Acting Dean
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlFacultyCourse" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                </asp:DropDownList>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
