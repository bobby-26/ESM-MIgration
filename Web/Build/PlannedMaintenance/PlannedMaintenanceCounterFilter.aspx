<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceCounterFilter.aspx.cs"
    Inherits="PlannedMaintenanceCounterFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmJobFilter" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Counter Update Filter"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuCounterFilter" runat="server" OnTabStripCommand="JobFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlJobFilter">
        <ContentTemplate>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtComponentCode" CssClass="input" MaxLength="20"></asp:TextBox>
                         <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtComponentCode"
                            Mask="999.99.99" MaskType="Number" InputDirection="LeftToRight">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNameTitle" CssClass="input" MaxLength="200" Width="240px"></asp:TextBox>
                    </td>
                </tr>
             
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
