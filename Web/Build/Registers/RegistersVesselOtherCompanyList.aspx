<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselOtherCompanyList.aspx.cs"
    Inherits="VesselOtherCompanyList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EngineType" Src="~/UserControls/UserControlEngineType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmVesselOtherCompany" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <asp:Literal ID="lblVesselOtherCompany" runat="server" Text="Vessel Other Company"></asp:Literal>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuSecurityVesselOtherCompanyList" runat="server" OnTabStripCommand="SecurityVesselOtherCompanyList_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <asp:Literal ID="lblCompanyName" runat="server" Text="Company Name"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtCompanyName" MaxLength="100" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtVesselName" MaxLength="100" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                </td>
                <td>
                    <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblDWT" runat="server" Text="DWT"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtDWT" MaxLength="100" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Literal ID="lblGRT" runat="server" Text="GRT"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtGRT" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Literal ID="lblEngineType" runat="server" Text="Engine Type"></asp:Literal>
                </td>
                <td>
                    <eluc:EngineType id="ucEngineType" runat="server" AppendDataBoundItems="true"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblModel" runat="server" Text="Model"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtModel" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Literal ID="lblKW" runat="server" Text="KW"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtKW" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Literal ID="lblBHP" runat="server" Text="BHP"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtBHP" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblNoOfUnits" runat="server" Text="No Of Units"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtNoOfUnits" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                   <%-- <ajaxToolkit:MaskedEditExtender ID="txtNumberMask" runat="server" TargetControlID="txtNoOfUnits"
                        Mask="999" MaskType="Number" InputDirection="RightToLeft">
                    </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <asp:Literal ID="lblFlag" runat="server" Text="Flag"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtFlag" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Literal ID="lblNationalityOfCrew" runat="server" Text="Nationality Of Crew"></asp:Literal>
                </td>
                <td>
                    <eluc:Nationality ID="ucNationality" runat="server"  AppendDataBoundItems="true"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblUMS" runat="server" Text="UMS"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtUMS" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Literal ID="lblIMONumber" runat="server" Text="IMO Number"></asp:Literal>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtIMONumber" MaxLength="100" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
