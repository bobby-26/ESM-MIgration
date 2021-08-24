<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogDeckLogConfiguration.aspx.cs" Inherits="Log_ElectricLogDeckLogConfiguration" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deck Log Configuration</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .container {
            padding: 10px 50px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <div class="container">
            <h3>Load Line and Draught Water Configuration</h3>
            <table style="width: 100%;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCenterOfDisc" runat="server" Text="Center of Disc is Placed at"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtCenterOfDiscMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>m</span>
                    </td>
                    <td>
                        <eluc:Number ID="txtCenterOfDiscCentiMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>cm</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineFreshWater" runat="server" Text="Maximum load-line in fresh water"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtMaxLoadLineFreshWaterMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>m</span>
                    </td>
                    <td>
                        <eluc:Number ID="txtMaxLoadLineFreshWaterCentiMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>cm below the center of the disc</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineIndianSummer" runat="server" Text="Maximum load-line in Indian summer"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtMaxLoadLineIndianSummerMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>m</span>
                    </td>
                    <td>
                        <eluc:Number ID="txtMaxLoadLineIndianSummerCentiMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>cm below the center of the disc</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineSummerCenterDisc" runat="server" Text="Maximum load-line in summer on the center of the disc"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtMaxLoadLineSummerCenterDisc" runat="server" MaxLength="3" Width="120px" />
                        <span>m</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLoadLineWinter" runat="server" Text="Maximum load-line in winter"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtLoadLineWinterMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>m</span>
                    </td>
                    <td>
                        <eluc:Number ID="txtLoadLineWinterCentiMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>cm below the center of the disc</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLoadLineWinterNorthAtlantic" runat="server" Text="Maximum load-line in winter North Atlantic"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtLoadLineWinterNorthAtlanticMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>m</span>
                    </td>
                    <td>
                        <eluc:Number ID="txtLoadLineWinterNorthAtlanticCentiMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>cm below the center of the disc</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDraughtWaterSummer" runat="server" Text="Maximum draught of water in summer"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDraughtWaterSummerMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>m</span>
                    </td>
                    <td>
                        <eluc:Number ID="txtDraughtWaterSummerCentiMeter" runat="server" MaxLength="3" Width="120px" />
                        <span>cm</span>
                    </td>
                </tr>
            </table>
            <h3>Radar Log Installation Data</h3>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblRadarNo" Text="Radar No:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtRadarNo" runat="server" MaxLength="3" Width="120px" />
                        <%--<telerik:RadTextBox runat="server" ID="txtRadarNo"></telerik:RadTextBox>--%>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblDateFitted" Text="Date Fitted"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtDateFitted" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblScannerHeight" Text="Height of Scanner above Summer Load Line:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtScannerHeight" runat="server" MaxLength="3" Width="120px" />
<%--                        <telerik:RadTextBox runat="server" ID="txtScannerHeight"></telerik:RadTextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblNorthStablization" Text="Is North Stablization Fitted ?"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkNorthStablization" AutoPostBack="false"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblPerformanceMonitor" Text="Normal Reading of Performance Monitor"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtPerformanceMonitor" runat="server" MaxLength="3" Width="120px" />
<%--                        <telerik:RadTextBox runat="server" ID="txtPerformanceMonitor" AutoPostBack="false"></telerik:RadTextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblMajorModification" Text="Major Modification since installation"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtMajorModification" runat="server" MaxLength="3" Width="120px" />
<%--                        <telerik:RadTextBox runat="server" ID="txtMajorModification" TextMode="MultiLine"></telerik:RadTextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblMakeType" Text="Type / Make"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtMakeType" runat="server" MaxLength="3" Width="120px" />
                      <%--  <telerik:RadTextBox runat="server" ID="txtMakeType"></telerik:RadTextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblWaveBand" Text="Wave Band (3/10cm)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtWaveBand" runat="server" MaxLength="3" Width="120px" />
                  <%--      <telerik:RadTextBox runat="server" ID="txtWaveBand"></telerik:RadTextBox>--%>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </form>
</body>
</html>
