<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVPrimaryManager.aspx.cs" Inherits="VesselPositionEUMRVPrimaryManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Check</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageList" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlVoyageList">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />


            <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" Title="Company Information" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuCompanyInformation" runat="server" OnTabStripCommand="MenuCompanyInformation_TabStripCommand"></eluc:TabStrip>

            <table width="80%">
                <tr>
                    <td style="width: 20%"><b>
                        <telerik:RadLabel ID="lblNameofthecompany" runat="server" Text="Name of the company"></telerik:RadLabel></b></td>
                    <td style="width: 80%">
                        <telerik:RadTextBox ID="txtNameofthecompany" Width="80%" Enabled="false" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td style="width: 20%"><b>
                        <telerik:RadLabel ID="lblIMONo" runat="server" Text="IMO No"></telerik:RadLabel></b></td>
                    <td style="width: 80%">
                        <telerik:RadTextBox ID="txtIMONo" Width="80%" Enabled="false" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblAddress1" runat="server" Text="Address 1"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress1" Width="80%" Enabled="false" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblAddress2" runat="server" Text="Address 2"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress2" Width="80%" Enabled="false" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtCity" Width="80%" Enabled="false" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblStateProvinceRegion" runat="server" Text="State/Province/Region"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtStateProvinceRegion" Width="80%" Enabled="false" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblPostalCode" runat="server" Text="Postal Code"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtPostalCode" Width="80%" Enabled="false" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel></b></td>
                    <td>
                        <telerik:RadTextBox ID="txtCountry" Width="80%" Enabled="false" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblContactPerson" runat="server" Text="Contact Person"></telerik:RadLabel></b>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContactPerson" runat="server" Width="80%" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblTelephoneNumber" runat="server" Text="Telephone Number"></telerik:RadLabel></b>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTelephoneNumber" runat="server" Width="80%" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblEmailAddress" runat="server" Text="Email Address"></telerik:RadLabel></b>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmailAddress" runat="server" Width="80%" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
