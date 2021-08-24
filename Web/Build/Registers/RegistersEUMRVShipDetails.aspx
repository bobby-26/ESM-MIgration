<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersEUMRVShipDetails.aspx.cs"
    Inherits="RegistersEUMRVShipDetails" %>

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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCityEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlCityEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />

            <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" Title="Ship Details" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="TabProcedure" runat="server" OnTabStripCommand="TabProcedure_TabStripCommand"></eluc:TabStrip>


            <table width="100%">
                <tr>
                    <td style="width: 20%"><b>
                        <telerik:RadLabel ID="lblvessel" Text="Vessel" runat="server"></telerik:RadLabel></b></td>
                    <td style="width: 80%">
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" OnTextChangedEvent="VesselChange" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblIMOIdentificationNumber" runat="server" Text="IMO Identification Number"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtIMOIdentificationNumber" Width="80%" CssClass="readonlytextbox" Enabled="false" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblPortofRegistry" runat="server" Text="Port of Registry"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtPortofRegistry" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblHomePort" runat="server" Text="Home Port"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtHomePort" Width="80%" CssClass="input" runat="server" Text=""></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblNameoftheShipOwner" runat="server" Text="Name of the Ship Owner"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtNameoftheShipOwner" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblIMOUniqueCompany" runat="server" Text="IMO Unique Company"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtIMOUniqueCompany" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblTypeoftheShip" runat="server" Text="Type of the Ship"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtTypeoftheShip" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblDeadweight" runat="server" Text="Deadweight"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtDeadweight" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblGrosstonnage" runat="server" Text="Gross tonnage"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtGrosstonnage" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblClassificationSociety" runat="server" Text="Classification Society"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtClassificationSociety" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblIceClass" runat="server" Text="Ice Class"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtIceClass" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblFlagState" runat="server" Text="Flag State"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtFlagState" Width="80%" CssClass="readonlytextbox" runat="server" Enabled="false"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblAdditionalDescription" runat="server" Text="Additional Description"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtAdditionalDescription" Width="80%" CssClass="input" TextMode="MultiLine" Resize="Both" Text="" runat="server"></telerik:RadTextBox></td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
