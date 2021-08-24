<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersEUMRVExemptionArticle.aspx.cs"
    Inherits="RegistersEUMRVExemptionArticle" %>

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
    <title>EUMRV Fuel Monitoring</title>
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
            <%--<eluc:Title ID="ucTitle" Text="" runat="server" />--%>
            <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" Title="Conditions of exemption" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="TabProcedure" runat="server" OnTabStripCommand="TabProcedure_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td colspan="2"><b><span>C.1 Conditions of exemption related to Article 9(2).</span></b></td>
                </tr>               
            </table>
            <br />
            <table id="tblConfigureCity" width="100%">
                 <tr>
                    <td style="width: 65%;">
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>&nbsp;&nbsp;
                    </td>
                    <td style="width: 35%;">
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" AppendDataBoundItems="true"
                            Width="120px" AutoPostBack="true" OnTextChangedEvent="ucVessel_TextChanged" />
                    </td>
                </tr>
                <tr>
                    <td>Mininum number of expected voyages per reporing 
                                period falling under the scope of the EV MRV Regulation
                                according to the ship's Schedule?
                                
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtexpectedvoyage" runat="server" CssClass="input" Text="" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Are there expected voyages per reporing period not falling under the scope of the EV MRV Regulation
                                according to the ship's Schedule?
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlexpectedvoyagenot" runat="server" CssClass="input" Width="120px">
                            <Items>
                                <telerik:RadComboBoxItem Text="--select--" Value="Dummy"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Yes" Value="Yes"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="No" Value="No"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>Conditions of Article 9(2) fulfiled?
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlConditions" runat="server" CssClass="input" Width="120px" OnTextChanged="ddlConditions_TextChanged" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--select--" Value="Dummy"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Yes" Value="Yes"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="No" Value="No"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>If yes,do you intend to make use of the derogation for 
                                monitoring the amount of fuel consumed on a per-voyage basis?
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlconsumed" runat="server" CssClass="input" Width="120px">
                            <Items>
                                <telerik:RadComboBoxItem Text="--select--" Value="Dummy"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Yes" Value="Yes"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="No" Value="No"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Not Applicable" Value="NA"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
