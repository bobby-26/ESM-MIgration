<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentProcessLossGeneral.aspx.cs" Inherits="InspectionIncidentProcessLossGeneral" %>

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
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Incident General</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       
       <div id="InspectionIncidentlink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmIncidentInjuryGeneral" runat="server">
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status runat="server" ID="ucStatus" />
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <eluc:Title runat="server" ID="ucTitle" Text="Process Loss" ShowMenu="false"></eluc:Title>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuIncidentInjuryGeneral" runat="server" OnTabStripCommand="MenuIncidentInjuryGeneral_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlIncidentInjuryGeneral">
            <ContentTemplate>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureIncidentInjury" width="100%">
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lblTypeOfProcessLoss" runat="server" Text="Type of Process Loss"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:DropDownList ID="ddlTypeOfProcessLoss" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeOfProcessLoss_Changed" 
                                    DataTextField="FLDNAME" DataValueField="FLDHAZARDID" Width="150px"></asp:DropDownList>
                            </td>
                            <td style="width: 15%">
                                <asp:Literal ID="lblSubtypeofProcessLoss" runat="server" Text="Subtype of Process Loss"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:DropDownList ID="ddlSubProcessLoss" Width="250px" AppendDataBoundItems="true" runat="server" 
                                    CssClass="input_mandatory" DataTextField="FLDNAME" DataValueField="FLDSUBHAZARDID"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>                            
                            <td style="width: 15%">
                                <asp:Literal ID="lblEstimatedCostInUSD" runat="server" Text="Estimated Cost in USD"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <eluc:Number ID="ucExtimatedCost" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                    MaxLength="10" />
                            </td>
                            <td style="width: 15%"> 
                                <asp:Literal ID="lblConsequenceCategory" runat="server" Text="Consequence Category"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtCategory" runat="server" Width="30px" ReadOnly="true" Enabled="false" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <b><asp:Literal ID="lblOffHire" runat="server" Text="Offhire"></asp:Literal></b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lbldays" runat="server" Text="Days"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <eluc:Number runat="server" ID="txtOffhiredays" CssClass="input" MaxLength="4" IsInteger="true" />
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lblHours" runat="server" Text="Hours"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <eluc:Number ID="txtOffhirehours" runat="server" CssClass="input" MaxLength="4" IsInteger="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lblMinutes" runat="server" Text="Minutes"></asp:Literal>
                            </td>    
                            <td style="width: 35%">
                                <eluc:Number runat="server" ID="txtOffhiremins" CssClass="input" MaxLength="4" IsInteger="true" />
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
