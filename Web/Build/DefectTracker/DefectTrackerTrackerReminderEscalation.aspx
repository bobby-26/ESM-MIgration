<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerTrackerReminderEscalation.aspx.cs"
    Inherits="DefectTrackerTrackerReminderEscalation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.DefectTracker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPTeamMembers" Src="~/UserControls/UserControlSEPTeamMembers.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add ReminderEscalation</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
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
    <div class="subHeader">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader">
            <eluc:Title runat="server" ID="lblCaption" ShowMenu="false"></eluc:Title>
            <div style="position: absolute; top: 0px; right: 0px">
                <eluc:TabStrip ID="MenuReminderAdd" runat="server" TabStrip="true" OnTabStripCommand="MenuReminderAdd_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
        <div class="subHeader">
            <div style="position: absolute; right: 0px">
                <eluc:TabStrip ID="MenuReminderSave" runat="server" OnTabStripCommand="MenuReminderSave_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
        <table width="100%" border="1">
            <tr>
                <td>
                    <table width="90%">
                        <tr>
                            <td>
                                To
                            </td>
                            <td>
                                <div id="divTo" style="overflow-y:scroll;height:100px;">
                                <asp:CheckBoxList runat="server" ID="cblTo" DataTextField="FLDDESCRIPTION" DataValueField="FLDMAILFIELD"></asp:CheckBoxList>
                                </div>
                            </td>
                            <td>
                                Cc
                            </td>
                            <td>
                                <div id="divCc" style="overflow-y:scroll;height:100px;">
                                <asp:CheckBoxList runat="server" ID="cblCc" DataTextField="FLDDESCRIPTION" DataValueField="FLDMAILFIELD"></asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        Message
        <table width="100%">
            <tr>
                <td colspan="2" width="100%">
                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="20" Width="100%"
                        CssClass="input" Font-Size="Small" />
                </td>
            </tr>
        </table>
        <eluc:Status runat="server" ID="ucStatus" />
    </div>
    </form>
</body>
</html>
