<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPOfficeConfiguration.aspx.cs" Inherits="VesselPositionSIPOfficeConfiguration" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bunker Receipt</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
     </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSIPTanksConfuguration" runat="server">
        <telerik:RadScriptManager  ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"/>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlSIPTanksConfuguration" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlSIPTanksConfuguration">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" TabStrip="false" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td><telerik:RadLabel ID="lblriskassesmentHead" runat="server" Text="<b>Risk assessment & mitigation plan</b>"></telerik:RadLabel></td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblriskassesment" runat="server" Text="Risk assessment (impact of new fuels) performed Details"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtriskassesment" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Resize="Vertical" 
                                Width="98%" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblstrucHead" runat="server" Text="<b>Fuel oil system modifications</b>"></telerik:RadLabel></td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblstruc" runat="server" Text="Structural modifications Details"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtstructuralmodifiation" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Resize="Vertical" 
                                Width="98%" />
                </td>
            </tr>

            <tr>
                <td><telerik:RadLabel ID="lblscruber" runat="server" Text="Scrubbers installed Details"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtscrubber" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Resize="Vertical" 
                                Width="98%" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblbunker" runat="server" Text="Dedicated FO sampling Details"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtbunkercomment" runat="server" CssClass="input" Height="70px" TextMode="MultiLine"  Resize="Vertical"
                                Width="98%" />
                </td>
            </tr>
             <tr>
                <td><telerik:RadLabel ID="lbltankcleanHead" runat="server" Text="<b>Tank cleaning & bunkering date</b>"></telerik:RadLabel></td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lbltankcoment" runat="server" Text="Tank Cleaning comment"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txttankclean" runat="server" CssClass="input" Height="70px" TextMode="MultiLine"   Resize="Vertical"
                                Width="98%" />
                </td>
            </tr>
            
            <tr>
                <td><telerik:RadLabel ID="lblprocureHead" runat="server" Text="<b>Procurement of compliant fuel oil</b>"></telerik:RadLabel></td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblpurchaseprocedure" runat="server" Text="Details of fuel purchasing procedure to source compliant fuels, including procedures in cases where compliant fuel oil is not readily available"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtpurchaseprocedure" runat="server" CssClass="input" Height="70px" TextMode="MultiLine"  Resize="Vertical"
                                Width="98%" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblalternate" runat="server" Text="Details of alternate steps taken"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtalternate" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" 
                                Width="98%"  Resize="Vertical" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblcomplientfuel" runat="server" Text="Details of alternate steps taken to ensure timely availability of compliant fuel oil"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtcomplientfuel" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" 
                                Width="98%"  Resize="Vertical" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblnoncomplient" runat="server" Text="Details of arrangements (if any planned) to dispose of any remaining non-compliant fuel oil"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtnoncomplient" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" 
                                Width="98%"  Resize="Vertical" />
                </td>
            </tr>
             <tr>
                <td><telerik:RadLabel ID="lblfuelhcangeHead" runat="server" Text="<b>Fuel oil changeover plan</b>"></telerik:RadLabel></td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblfuelhcange" runat="server" Text="Ship-specific fuel changeover plan"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtfuelhcange" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" 
                                Width="98%"  Resize="Vertical" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lbltrainingdetails" runat="server" Text="Training details"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txttrainingdetails" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" 
                                Width="98%"  Resize="Vertical" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblDocumentationHead" runat="server" Text="<b>Documentation & reporting</b>"></telerik:RadLabel></td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblDocumentation" runat="server" Text="If when following the implementation plan the ship has to bunker and use non-compliant fuel oil due to unavailability of compliant fuel oil safe for use on board the ship, steps to limit the impact of using non-compliant fuel oil could be"></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtDocumentation" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" 
                                Width="98%"  Resize="Vertical" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblnonavailability" runat="server" Text="The ship should have a procedure for Fuel Oil Non-Availability Reporting (FONAR). The master and chief engineer should be conversant about when and how FONAR should be used and who it should be reported to."></telerik:RadLabel></td>
            </tr>
            <tr>                    
                <td>
                    <telerik:RadTextBox ID="txtnonavailability" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" 
                                Width="98%"  Resize="Vertical" />
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
