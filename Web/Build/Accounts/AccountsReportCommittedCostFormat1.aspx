<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportCommittedCostFormat1.aspx.cs" Inherits="AccountsReportCommittedCostFormat1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Committed Cost</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

        <script type="text/javascript">
        function resizeFrame() {
                                var obj = document.getElementById("ifMoreInfo");
                                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
                               }
        </script>

    </div>
</telerik:RadCodeBlock></head>
<body onload="resizeFrame()">
    <form id="frmReportCommittedCost" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />
    <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title3" Text="Committed Cost Report" ShowMenu="true">
            </eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
        </div>
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="ucFromdate" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                            Enabled="True" TargetControlID="ucFromdate" PopupPosition="TopLeft">
                        </ajaxToolkit:CalendarExtender>
                    </td>
                    <td>
                        <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="ucToDate" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                            Enabled="True" TargetControlID="ucToDate" PopupPosition="TopLeft">
                        </ajaxToolkit:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true"
                            CssClass="dropdown_mandatory" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px;
                width: 100%;"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
