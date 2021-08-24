<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPTankCleanandBunkerDetail.aspx.cs" Inherits="VesselPositionSIPTankCleanandBunkerDetail" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

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
                <tr id="trVessel" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblMethod" runat="server" Text="Method"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMethod" runat="server" CssClass="input" AutoPostBack="true" Width="180px" OnSelectedIndexChanged="ddlMethod_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblOption" runat="server" Text="Option"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlOption" runat="server" CssClass="input" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                    <td style="display: none;">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td style="display: none;">
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            VesselsOnly="true" AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>
                <tr id="tryardplace" runat="server" style="display: none;">
                    <td>
                        <telerik:RadLabel ID="lblregion" runat="server" Text="Region"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlregion" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlregion_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCountry" runat="server" Width="180px" CssClass="input"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYard" runat="server" Text="Yard"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtYard" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox></td>
                </tr>
                <tr align="left" id="trDate" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblcleanstart" runat="server" Text="Cleaning Start"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="UcCleaningstartDate" runat="server" CssClass="input" Width="180px" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcleanend" runat="server" Text="Cleaning End"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="UcCleaningEndDate" runat="server" CssClass="input" Width="180px" DatePicker="true" />
                    </td>
                </tr>
                <tr id="trInstruction" runat="server" style="color: blue">
                    <td colspan="6">
                        <asp:Label ID="lblInstruction" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr align="left" id="trSupplier" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblAmountAdditive" runat="server" Text="Amount additive req. (ltr)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmountAdditive" runat="server" CssClass="input" Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSupplier" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="lblSupplyNoLater" runat="server" Text="Supply. No Later Than"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="UcSupplyNoLater" runat="server" CssClass="input" Width="180px" DatePicker="true" />
                    </td>
                </tr>
                <tr id="trSupplyArea" runat="server" style="display: none;">
                    <td>
                        <telerik:RadLabel ID="lblSupplyregion" runat="server" Text="Supply region"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSupplyregion" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplyregion_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplycountry" runat="server" Text="Supply country"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSupplycountry" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplycountry_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplyport" runat="server" Text="Supply port"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSupplyport" runat="server" Width="180px" CssClass="input"></telerik:RadComboBox>
                    </td>
                </tr>

                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblComment" runat="server" Text="Comment"></telerik:RadLabel>
                    </td>
                </tr>
                <tr id="trOfficeComment" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtOfficeComment" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Resize="Both" Rows="3"
                            Width="98%" />
                    </td>
                </tr>
                <tr align="left" id="trComment" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtComment" runat="server" Width="98%" CssClass="input" TextMode="MultiLine" Resize="Both" Rows="3"></telerik:RadTextBox></td>
                </tr>
                <tr id="trbunkerHead" runat="server">
                    <td colspan="6">
                        <telerik:RadLabel ID="lblbunkerHead" runat="server"><b>First Bunkering, no later than</b></telerik:RadLabel>
                    </td>
                </tr>
                <tr align="left" id="trBunkerDate" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <eluc:Date ID="UcBunkerDate" runat="server" CssClass="input" Width="180px" DatePicker="true" />
                    </td>
                </tr>
                <tr id="trBunkerArea" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblBunkerRegion" runat="server" Text="Region"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBunkerRegion" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlBunkerRegion_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBunkerCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBunkerCountry" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlBunkerCountry_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBunkerPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBunkerPort" runat="server" Width="180px" CssClass="input"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr id="trflushingsystem" runat="server">
                    <td colspan="6">
                        <telerik:RadLabel ID="lblFlushing" runat="server"><b>Flushing of piping system</b></telerik:RadLabel>
                    </td>
                </tr>
                <tr align="left" id="trFlushing" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblFlushingDate" runat="server" Text="Flushing Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="UcFlushingDate" runat="server" CssClass="input" Width="180px" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFuelQtyNeed" runat="server" Text="Amount of Fuel needed (m3)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtFuelQtyNeed" runat="server" CssClass="input" Width="180px" />
                    </td>

                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
