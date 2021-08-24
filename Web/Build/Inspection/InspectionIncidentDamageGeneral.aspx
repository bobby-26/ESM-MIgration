<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentDamageGeneral.aspx.cs" Inherits="InspectionIncidentDamageGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident Damage General</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       
       <div id="IncidentDamagelink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmIncidentDamageGeneral" runat="server">                       
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%"> 
            <div class="subHeader" style="position: relative">
                <div id="divHeading">
                    <eluc:Title runat="server" id="ucTitle" Text="Damage" ShowMenu="false"></eluc:Title>                     
                </div>
            </div>
             <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="IncidentDamageGeneralMain" runat="server" OnTabStripCommand="IncidentDamageGeneralMain_TabStripCommand" TabStrip="true">
                </eluc:TabStrip> 
            </div>
                
            <div class="subHeader" style="position: relative;">
                        <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                <eluc:TabStrip ID="MenuIncidentDamageGeneral" runat="server" OnTabStripCommand="IncidentDamageGeneral_TabStripCommand">
                </eluc:TabStrip> 
                </span>
            </div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel runat="server" ID="pnlIncidentDamageGeneral">
                <ContentTemplate>                                                        
                    <div id="divFind" style="position: relative; z-index: 2">
                        <table id="tblConfigureIncidentDamage" width="100%">
                            <tr>
                                <td style="width: 15%">
                                   <asp:Literal ID="lblDamagedComponent" runat="server" Text="Damaged Component"></asp:Literal>
                                </td>
                                <td style="width: 35%">
                                    <span id="spnPickListComponent">
                                        <asp:TextBox ID="txtComponentCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                            Enabled="false" Width="60px"></asp:TextBox>
                                        <asp:TextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="20"
                                            Enabled="false" Width="210px"></asp:TextBox>
                                        <img id="imgComponent" runat="server" onclick="return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx', true); "
                                            src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />
                                        <asp:TextBox ID="txtComponentId" runat="server" CssClass="input" Width="10px"></asp:TextBox>
                                    </span>
                                </td>
                                <td style="width: 15%">
                                    <asp:literal ID="lblTypeOfDamage" runat="server" Text="Type of Damage "></asp:literal>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Quick runat="server" ID="ucTypeOfDamage" AppendDataBoundItems="true" CssClass="input_mandatory"
                                         QuickTypeCode="71" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <asp:Literal ID="lblEstimatedCostinUSD" runat="server" Text="Estimated cost in USD "></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Number runat="server" ID="txtEstimatedCost" Mask="99999999.99" CssClass="input" MaxLength="4" />                                    
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    Time of Incident
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTimeOfIncident" runat="server" CssClass="input_mandatory" Width="50px" />  
                                    <ajaxToolkit:MaskedEditExtender ID="txtTimeMask" runat="server" TargetControlID="txtTimeOfIncident"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="false" UserTimeFormat="TwentyFourHour" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true"/>
                                </td>
                            </tr>--%>
                        </table>
                    </div>       
                </ContentTemplate>      
            </asp:UpdatePanel>  
            <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />                       
        </div>
    </form>
</body>
</html>
