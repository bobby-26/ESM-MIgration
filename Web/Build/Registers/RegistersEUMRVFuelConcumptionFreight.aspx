<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersEUMRVFuelConcumptionFreight.aspx.cs"
    Inherits="RegistersEUMRVFuelConcumptionFreight" %>

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
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
            <eluc:TabStrip ID="MenuProcedureDetailList" Title="Procedure Details" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="TabProcedure" runat="server" OnTabStripCommand="TabProcedure_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td><b><span>C.2.9. Method for determining the split of fuel consumption into freight and passenger part (for ro-pax ships only)</span></b></td>
                </tr>
            </table>
            <br />
            <table id="tblConfigureCity" width="90%">

                <tr>
                    <td style="width: 50%;">
                        <b>Title of method</b>
                    </td>
                    <td>Determining the split of fuel consumption into freight and passenger part
                    </td>
                </tr>

                <tr>
                    <td style="width: 50%;">Applied allocation method according to EN 16258
                                
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAppliedAllocation" runat="server" CssClass="input" Width="50%">
                        </telerik:RadComboBox>

                    </td>
                </tr>
                <tr>
                    <td>Description of method to determine the mass of freight and passengers including the possible use of default values for the weight of cargo units / lane meters(if mass method is used)
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMassFreight" runat="server" CssClass="input" Text="" Width="98%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Description of method to determine the deck area assigned to freight and passengers including the Consideration of hanging decks and of passenger cars on freight decks (if area method is used)
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAreaFreight" runat="server" CssClass="input" Text="" Width="98%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Split of fuel Consumption into  freight and passengers part (if area method is used only)
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFuelConsumption" runat="server" CssClass="input" Text="" Width="98%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Name of the Person or position responsible for this method
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPostionResponsible" runat="server" CssClass="input" Text="" Width="98%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Formulae and data source
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFormulae" runat="server" CssClass="input" Text="" Width="98%"></telerik:RadTextBox>
                    </td>
                </tr>
                <%--<tr>
                            <td>Data sources 
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDataSources" runat="server" CssClass="input" Text="" Width="98%"></telerik:RadTextBox>
                            </td>
                        </tr>--%>
                <tr>
                    <td>Location where records are kept 
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLocationRecord" runat="server" CssClass="input" Text="" Width="98%"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>Name of it System Used (where applicable)
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNameSystem" runat="server" CssClass="input" Text="" Width="98%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <table width="100%">
                <tr>
                    <th style="width: 50%">
                        <telerik:RadLabel ID="lblapplicableyn" runat="server" Text="Company does not wish to report on this procedure"></telerik:RadLabel>
                    </th>
                    <td style="width: 50%">
                        <telerik:RadCheckBox ID="chkapplicableYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkapplicableYN_CheckedChanged" />
                    </td>
                </tr>
            </table>
            <br />
            <eluc:Status ID="ucstatus" runat="server" Text="" />

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
