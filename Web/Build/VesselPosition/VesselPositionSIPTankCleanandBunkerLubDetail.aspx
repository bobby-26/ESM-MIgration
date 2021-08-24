<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPTankCleanandBunkerLubDetail.aspx.cs" Inherits="VesselPositionSIPTankCleanandBunkerLubDetail" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tank cleaning & bunkering date</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSIPTanksConfuguration" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlSIPTanksConfuguration" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlSIPTanksConfuguration">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:TabStrip ID="MenuRiskassessmentplan" runat="server" OnTabStripCommand="MenuRiskassessmentplan_TabStripCommand" />
            <table width="98%" runat="server" id="tbltest">
                <tr id="trVessel" runat="server" style="display: none;">
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Name"></telerik:RadLabel></td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            VesselsOnly="true" AppendDataBoundItems="true" Width="180px" />

                    </td>

                </tr>

                <tr id="trbunkerHead" runat="server">
                    <td colspan="6">
                        <telerik:RadLabel ID="lblbunkerHead" runat="server"> <b>Supply of Lubricating oil, no later than:</b></telerik:RadLabel></td>
                </tr>
                <tr align="left" id="trBunkerDate" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel></td>
                    <td colspan="5">
                        <eluc:Date ID="UcBunkerDate" runat="server" CssClass="input" Width="180px" DatePicker="true" />
                    </td>
                </tr>
                <tr id="trBunkerArea" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblBunkerRegion" runat="server" Text="Region"></telerik:RadLabel></td>
                    <td>
                        <telerik:RadComboBox ID="ddlBunkerRegion" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlBunkerRegion_SelectedIndexChanged"></telerik:RadComboBox></td>
                    <td>
                        <telerik:RadLabel ID="lblBunkerCountry" runat="server" Text="Country"></telerik:RadLabel></td>
                    <td>
                        <telerik:RadComboBox ID="ddlBunkerCountry" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlBunkerCountry_SelectedIndexChanged"></telerik:RadComboBox></td>
                    <td>
                        <telerik:RadLabel ID="lblBunkerPort" runat="server" Text="Port"></telerik:RadLabel></td>
                    <td>
                        <telerik:RadComboBox ID="ddlBunkerPort" runat="server" Width="180px" CssClass="input"></telerik:RadComboBox></td>
                </tr>
                <tr id="trflushingsystem" runat="server">
                    <td colspan="6">
                        <telerik:RadLabel ID="lblFlushing" runat="server"> <b>Flushing of piping system</b></telerik:RadLabel></td>
                </tr>
                <tr align="left" id="trFlushing" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblFlushingDate" runat="server" Text="Flushing Date"></telerik:RadLabel></td>
                    <td>
                        <eluc:Date ID="UcFlushingDate" runat="server" CssClass="input" Width="180px" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFuelQtyNeed" runat="server" Text="Amount of Fuel needed (m3)"></telerik:RadLabel></td>
                    <td colspan="3">
                        <eluc:Number ID="txtFuelQtyNeed" runat="server" CssClass="input" Width="180px" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
