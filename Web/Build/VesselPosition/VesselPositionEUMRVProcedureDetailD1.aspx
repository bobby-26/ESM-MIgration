<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVProcedureDetailD1.aspx.cs" Inherits="VesselPositionEUMRVProcedureDetailD1" %>

<%@ Import Namespace="SouthNests.Phoenix.VesselPosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVPRSLocation" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVPRSLocation" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlVPRSLocation">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="TabProcedure" runat="server" OnTabStripCommand="TabProcedure_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td style="width: 30%">
                        <telerik:RadLabel runat="server" ID="lblprocedure" Text="Procedure"></telerik:RadLabel>
                    </td>
                    <td style="width: 70%">
                        <b>
                            <telerik:RadLabel ID="lblProceduretxt" runat="server"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblBackupmethod" Text="Back-up monitoring method (A/B/C/D)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBackupmethod" runat="server" CssClass="input" Width="360px"></telerik:RadComboBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblFormulae" Text="Formulae"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFormulae" CssClass="input" Width="360px"></telerik:RadTextBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblMethodTreat" Text="Description of method to estimate fuel consumption"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMethodTreat" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Resize="Both"
                            Width="70%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblpersonreponsible" Text="Name of the Person Responsible"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtxpersonreponsible" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblDatatasource" Text="Data source"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtDatasource" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lbllocation" Text="Location where records are kept"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtlocation" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblSystemUsed" Text="Name of the IT System Used"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSystemUsed" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblVersion" Visible="false" Text="Version of Existing Procedure"></telerik:RadLabel>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel runat="server" Visible="false" ID="txtVersion" Text="0"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
