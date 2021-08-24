<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderLogFilter.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderLogFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Order Log Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
      <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Work Order Log Filter"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip id="MenuWorkOrderLogFilter" runat="server" OnTabStripCommand="WorkOrderLog_TabStripCommand">
        </eluc:TabStrip>
    </div>
    
    <asp:UpdatePanel runat="server" ID="pnlJobFilter">
        <ContentTemplate>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Literal ID="lblWorkOrderNumber" runat="server" Text="Work Order Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtWorkOrderNumber" CssClass="input" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblWorkOrderTitle" runat="server" Text="Work Order Title"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtWorkOrderTitle" CssClass="input" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Literal ID="lblWorkDoneDateFrom" runat="server" Text="Work Done Date From"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtWorkDoneDateFrom" CssClass="input" MaxLength="10"></asp:TextBox>
                         <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="txtWorkDoneDateFrom" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblWorkDoneDateTo" runat="server" Text="Work Done Date To"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtWorkDoneDateTo" CssClass="input" MaxLength="10"></asp:TextBox>
                         <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="txtWorkDoneDateTo" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Literal ID="lblWorkDoneBy" runat="server" Text="Work Done By"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Discipline ID="ucWorkDoneBy" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
