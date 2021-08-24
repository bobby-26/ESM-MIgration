<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentDetails.aspx.cs" Inherits="InspectionIncidentDetails" %>

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
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident Details </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       
       <div id="IncidentDamagelink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmIncidentDamageGeneral" runat="server">
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status runat="server" ID="ucStatus" />
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <eluc:Title runat="server" ID="ucTitle" Text="Consequence"></eluc:Title>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuIncidentGeneral" runat="server" TabStrip="true" OnTabStripCommand="IncidentGeneral_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div class="subHeader" style="position: relative;">
            <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                <eluc:TabStrip ID="MenuIncidentReportGeneral" TabStrip="true" runat="server" OnTabStripCommand="IncidentReportGeneral_TabStripCommand">
                </eluc:TabStrip>
            </span>
        </div>
        <div class="subHeader" style="position: absolute">
            <span class="navSelect" style="margin-top: 0px; float: left; width: auto;">
                <eluc:Title runat="server" ID="Title1" Text="Details" ShowMenu="false"></eluc:Title>
            </span><span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand">
                </eluc:TabStrip>
            </span>
        </div>
        </br>
        </br>       
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlIncidentDamageGeneral">
            <ContentTemplate>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureIncidentDamage" width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblnotes" runat="server" 
                                    Text="Comprehensive Description of Incident" Font-Bold="True"></asp:Label>
                                <b>:</b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtComprehenciveDescription" runat="server" CssClass="input" Height="135px"
                                    Rows="3" TextMode="MultiLine" Width="687px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
