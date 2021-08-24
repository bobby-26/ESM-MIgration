<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersIMODCSShipParticulars.aspx.cs" Inherits="RegistersIMODCSShipParticulars" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EUMRV Ship Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCity" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCityEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlCityEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="MenuProcedureDetailList" Title="Ship Particulars" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="TabProcedure" runat="server" OnTabStripCommand="TabProcedure_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td style="width: 20%"><b>
                        <telerik:RadLabel ID="lblvessel" Text="Name of the ship" runat="server"></telerik:RadLabel></b></td>
                    <td style="width: 80%">
                        <telerik:RadTextBox ID="txtvessel" Width="80%" CssClass="readonlytextbox" Enabled="false" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblIMOIdentificationNumber" runat="server" Text="IMO Number"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtIMOIdentificationNumber" Width="80%" CssClass="readonlytextbox" Enabled="false" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblIMOUniqueCompany" runat="server" Text="Company"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtIMOUniqueCompany" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblFlagState" runat="server" Text="Flag"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtFlagState" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblshiptype" runat="server" Text="Ship Type"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtshiptype" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblGrosstonnage" runat="server" Text="Gross Tonnage"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtGrosstonnage" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblnt" runat="server" Text="NT"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtnt" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblDeadweight" runat="server" Text="DWT"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtDeadweight" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lbleedi" runat="server" Text="EEDI (if applicable)"></telerik:RadLabel></b></td>
                    <td>
                        <eluc:Number ID="txtEEDI" runat="server" MaxLength="11" CssClass="input" style="text-align: right;" Width="70px"
                            DecimalPlace="6" />
                        &nbsp;&nbsp;&nbsp;
                                    <b>
                                        <telerik:RadLabel ID="lblEIV" runat="server" Text="EIV"></telerik:RadLabel></b> &nbsp;&nbsp;&nbsp;
                                    <eluc:Number ID="txtEIV" runat="server" MaxLength="11" CssClass="input" style="text-align: right;" Width="70px"
                                        DecimalPlace="6" />
                    </td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblIceClass" runat="server" Text="Ice Class"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtIceClass" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
