<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentSecurityGeneral.aspx.cs" Inherits="InspectionIncidentSecurityGeneral" %>

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
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Incident Security General</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       
       <div id="IncidentSecuritylink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmIncidentSecurityGeneral" runat="server">                       
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%"> 
            <div class="subHeader" style="position: relative">
                <div id="divHeading">
                    <eluc:Title runat="server" id="ucTitle" Text="Security" ShowMenu="false"></eluc:Title>                     
                </div>
            </div>
                
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuIncidentSecurityGeneral" runat="server" OnTabStripCommand="IncidentSecurityGeneral_TabStripCommand">
                </eluc:TabStrip> 
            </div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel runat="server" ID="pnlIncidentSecurityGeneral">
                <ContentTemplate>
                    <div id="divFind" style="position: relative; z-index: 2">
                        <table id="tblConfigureIncidentSecurity" width="100%">
                            <tr>
                                <td style="width: 15%">
                                   <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Hard runat="server" ID="ucTypeOfSecurity" AppendDataBoundItems="true" CssClass="input_mandatory"
                                        Width="300px" HardTypeCode="205" />
                                </td>
                                <td>
                                   <asp:Literal ID="lblEstimatedCostinUSD" runat="server" Text="Estimated cost in USD"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Number ID="ucExtimatedCost" runat="server" CssClass="input" MaxLength="10"
                                        DecimalPlace="2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblConsequenceCategory" runat="server" Text="Consequence Category"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCategory" Width="30px" CssClass="input" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>  
            <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />                       
        </div>
    </form>
</body>
</html>
