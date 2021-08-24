<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreBulkSignOnOff.aspx.cs"
    Inherits="CrewOffshoreBulkSignOnOff" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bulk Sign On Off</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmNoonReport" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNoonReportData">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="false" />
                        <eluc:Status runat="server" ID="ucStatus" />
                    </div>
                </div>
                <div class="Header">
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuTab" TabStrip="false" runat="server" OnTabStripCommand="MenuTab_TabStripCommand"></eluc:TabStrip>
                    </div>
                </div>
                <div>
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <asp:Literal ID="lblSignOnPort" runat="server" Text="Port"></asp:Literal>
                            </td>
                            <td>
                                <eluc:SeaPort runat="server" ID="ddlSeaPort" CssClass="input_mandatory" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
