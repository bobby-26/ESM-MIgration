<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerPatchProjectAddEdit.aspx.cs"
    Inherits="DefectTrackerPatchProjectAddEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.DefectTracker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPTeamMembers" Src="~/UserControls/UserControlSEPTeamMembers.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Module" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselList" Src="~/UserControls/UserControlVesselList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Patch Add/Edit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <div class="subHeader">
            <div id="divHeading" class="divFloatLeft">
                <eluc:Title runat="server" ID="ucTitle" ShowMenu="false" Text="Patch Project Add/Edit">
                </eluc:Title>
            </div>
            <div style="position: absolute; top: 0px; right: 0px">
                <eluc:TabStrip ID="MenuPatchProjectSave" runat="server" OnTabStripCommand="MenuPatchProjectSave_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
        <eluc:Error ID="ucError" runat="server" Visible="false" />
    </div>
    <table width="100%">
        <tr>
            <td>
                Title
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtTitle" CssClass="input_mandatory" Width="70%" />
            </td>
        </tr>
        <tr>
            <td>
                Catalog Number
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCatalog" CssClass="input_mandatory" />
            </td>
        </tr>
        <tr>
            <td>
                Subject
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSubject" CssClass="input" Width="70%" />
            </td>
        </tr>
        <tr>
            <td>
                Type
            </td>
            <td>
                <asp:CheckBoxList runat="server" ID="ddlPatchType" CssClass="input_mandatory" RepeatDirection="Horizontal"
                    CellPadding="5">
                    <asp:ListItem Value="LINKS" Text="Links"></asp:ListItem>
                    <asp:ListItem Value="DATAEXTRACT" Text="Data Extract"></asp:ListItem>
                    <asp:ListItem Value="HOTFIX" Text="Hot Fix"></asp:ListItem>
                    <asp:ListItem Value="PATCH" Text="Patch"></asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td>
                Call Number / Date
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCallNumber" CssClass="input" ReadOnly="true" />
                /
                <eluc:Date runat="server" ID="ucCallDate" CssClass="input" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td>
                Created By
            </td>
            <td>
                <eluc:SEPTeamMembers ID="txtCreatedby" AppendDataBoundItems="true" runat="server"
                    CssClass="input_mandatory"></eluc:SEPTeamMembers>
            </td>
        </tr>
        <tr>
            <td>
                To
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtTo" CssClass="input" Width="40%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Cc
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCc" CssClass="input" Width="70%"></asp:TextBox>
            </td>
        </tr>
    </table>
    <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
