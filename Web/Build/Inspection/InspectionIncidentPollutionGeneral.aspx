<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentPollutionGeneral.aspx.cs" Inherits="InspectionIncidentPollutionGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Environment Release General</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="InspectionIncidentlink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
     <form id="frmRegistersInspectionIncident" runat="server" submitdisabledcontrols="true">                         
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%"> 
            <div class="subHeader" style="position: relative">
                <div id="divHeading">
                    <eluc:Title runat="server" id="ucTitle" Text="Environment Release" ShowMenu="false"></eluc:Title>                     
                </div>
            </div>
                
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuInspectionIncident" runat="server" OnTabStripCommand="InspectionIncident_TabStripCommand">
                </eluc:TabStrip> 
            </div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel runat="server" ID="pnlInspectionPollutionEntry">
                <ContentTemplate>
                    <div id="divFind" style="position: relative; z-index: 2">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblNameofSubstance" runat="server" Text="Name of Substance"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSubstanceName" runat="server" CssClass="input_mandatory"></asp:TextBox>
                                </td>
                                <td>
                                   <asp:Literal ID="lblReleaseType" runat="server" Text="Release Type"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Quick runat="server" ID="ucReleaseType" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        QuickTypeCode="45" />
                                </td>
                            </tr>
                            <tr>
                                <%--<td>
                                    Qty Released
                                </td>
                                <td>
                                    <eluc:Quick runat="server" ID="ucQuantity" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        Width="200px" QuickTypeCode="41" />
                                </td>--%>
                                <td>
                                   <asp:Literal ID="lblReleaseCategory" runat="server" Text=" Release Category"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hazard ID="ucReleaseCategory" Type="2" runat="server" AutoPostBack="true" Width="200px" 
                                        AppendDataBoundItems="true" OnTextChangedEvent="ucReleaseCategory_Changed" CssClass="input_mandatory" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblReleaseSubCategory" runat="server" Text="Release Subcategory"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReleaseSubCategory" Width="250px" AppendDataBoundItems="true" runat="server" 
                                        CssClass="input_mandatory" DataTextField="FLDNAME" DataValueField="FLDSUBHAZARDID"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <asp:Literal ID="lblEstimatedCostinUSD" runat="server" Text="Estimated cost in USD"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Number ID="ucExtimatedCost" runat="server" CssClass="input" MaxLength="10" DecimalPlace="2" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblConsequenceCategory" runat="server" Text="Consequence Category"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCategory" runat="server" Width="30px" ReadOnly="true" Enabled="false"
                                        CssClass="input"></asp:TextBox>
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
