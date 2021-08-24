<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSOxCO2.aspx.cs" Inherits="RegistersSOxCO2" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OilType" Src="~/UserControls/UserControlOilType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add/Edit ESI_SOx CO2</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                <asp:Literal ID="lblESISOxCO2" runat="server" Text="ESI_SOx CO2"></asp:Literal>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuSOx" runat="server" OnTabStripCommand="SOx_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <asp:Literal ID="lblLocation" runat="server" Text="Location"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLocation" CssClass="dropdown_mandatory" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Outside ECA" Value="Outside ECA"></asp:ListItem>
                        <asp:ListItem Text="Inside ECA" Value="Inside ECA"></asp:ListItem>
                        <asp:ListItem Text="At Berth" Value="At Berth"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblTypeofFuel" runat="server" Text="Type of Fuel"></asp:Literal>
                </td>
                <td>
                    <eluc:OilType runat="server" ID="ucTypeOfFuel" CssClass="dropdown_mandatory" IsFuelOil="1" IsOil="1"
                        AppendDataBoundItems="true"  />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblReference" runat="server" Text="Reference"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtReference" CssClass="input" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblBaselineSulphurlimitinfuelmm" runat="server" Text="Baseline Sulphur limit in fuel (% m/m)"></asp:Literal>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rblBSLimit" AppendDataBoundItems="true" RepeatDirection="Horizontal" >
                        <asp:ListItem Text="4.5% m/m" Value="4.5"></asp:ListItem>
                        <asp:ListItem Text="3.5% m/m" Value="3.5"></asp:ListItem>
                        <asp:ListItem Text="1.0% m/m" Value="1.0"></asp:ListItem>
                        <asp:ListItem Text="0.5% m/m" Value="0.5"></asp:ListItem>
                        <asp:ListItem Text="0.1% m/m" Value="0.1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblWeightage" runat="server" Text="Weightage"></asp:Literal>
                </td>
                <td>
                    <eluc:Number runat="server" ID="txtWeightage" CssClass="input" IsInteger="true" IsPositive="true" MaxLength="3" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCarbonContent" runat="server" Text="Carbon Content"></asp:Literal>
                </td>
                <td>
                    <eluc:Number runat="server" ID="txtCarbonContent" CssClass="input" DecimalPlace="3" MaxLength="5" IsInteger="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCf" runat="server" Text="Cf"></asp:Literal>
                </td>
                <td>
                    <eluc:Number runat="server" ID="txtCf" CssClass="input" DecimalPlace="6" MaxLength="8" IsInteger="false" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
