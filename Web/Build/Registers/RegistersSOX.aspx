<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSOX.aspx.cs" Inherits="RegistersSOX" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add/Edit SOx</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmSOxDetails" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <asp:Literal ID="lblSOx" runat="server" Text="SOx"></asp:Literal>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuSOx" runat="server" OnTabStripCommand="SOx_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" width="100%">
            <%--<tr>
                <td>
                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                </td>
                <td>
                    <eluc:Vessel ID="ucVesselName" runat="server" VesselsOnly="true" AppendDataBoundItems="true"
                        CssClass="input_mandatory" />
                </td>
            </tr>--%>
            <tr>
                <td>
                </td>
                <td>
                    <b><asp:Literal ID="lblBaselineSulphurlimitinfuelmm" runat="server" Text="Baseline Sulphur limit in fuel (% m/m)"></asp:Literal></b>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblOutsideECA" runat="server" Text="Outside ECA"></asp:Literal>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rblOutsideECA" AppendDataBoundItems="true" RepeatDirection="Horizontal" >
                        <asp:ListItem Text="4.5% m/m" Value="4.5"></asp:ListItem>
                        <asp:ListItem Text="3.5% m/m" Value="3.5"></asp:ListItem>
                        <asp:ListItem Text="0.5% m/m" Value="0.5"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Literal ID="lblOutWeightage" runat="server" Text="Weightage"></asp:Literal>
                </td>
                <td>
                    <eluc:Number runat="server" ID="txtOutWeightage" CssClass="input" IsInteger="true" IsPositive="true" MaxLength="3" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblInsideECA" runat="server" Text="Inside ECA"></asp:Literal>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rblInsideECA" AppendDataBoundItems="true" RepeatDirection="Horizontal" >
                        <asp:ListItem Text="1.0% m/m" Value="1.0"></asp:ListItem>
                        <asp:ListItem Text="0.1% m/m" Value="0.1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Literal ID="lblInWeightage" runat="server" Text="Weightage"></asp:Literal>
                </td>
                <td>
                    <eluc:Number runat="server" ID="txtInWeightage" CssClass="input" IsInteger="true" IsPositive="true" MaxLength="3" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblAtBerth" runat="server" Text="At Berth"></asp:Literal>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rblAtBerth" AppendDataBoundItems="true" RepeatDirection="Horizontal" >
                        <asp:ListItem Text="1.0% m/m" Value="1.0"></asp:ListItem>
                        <asp:ListItem Text="0.1% m/m" Value="0.1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Literal ID="lblAtBerthWeightage" runat="server" Text="Weightage"></asp:Literal>
                </td>
                <td>
                    <eluc:Number runat="server" ID="txtAtBerthWeightage" CssClass="input" IsInteger="true" IsPositive="true" MaxLength="3" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
