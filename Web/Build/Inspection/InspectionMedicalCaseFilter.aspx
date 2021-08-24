<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMedicalCaseFilter.aspx.cs" Inherits="InspectionMedicalCaseFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <div id="DivHeader" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionScheduleFilter" runat="server">
    
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Filter"></asp:Label>
        </div>
    </div>
    
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuIncidentFilter" runat="server" OnTabStripCommand="MenuIncidentFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>
    
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlScheduleFilter">
        <ContentTemplate>
            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:literal ID="lblCaseNo" runat="server" Text="Case No"></asp:literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCaseNo" runat="server" CssClass="input" 
                                MaxLength="20" Width="35%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:literal ID="lblTypeOfCase" runat="server" Text="Type of Case"></asp:literal></td>
                        <td>
                            <eluc:Hard ID="ucTypeOfCase" runat="server" AppendDataBoundItems="true" 
                                CssClass="input" HardTypeCode="174" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal></td>
                        <td>
                            <eluc:Date ID="ucFromDate" CssClass="input" Width="80px" runat="server" />
                        </td>  
                         <td>
                             <asp:literal ID="lblToDate" runat="server" Text="To Date"></asp:literal></td>
                        <td>
                            <eluc:Date ID="ucToDate" CssClass="input" Width="80px" runat="server" />
                        </td>                      
                    </tr>
                    <tr>
                        <td>
                            <asp:literal ID="lblVessel" runat="server" Text="Vessel "></asp:literal></td>
                        <td>                            
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" 
                                CssClass="input" VesselsOnly="true" />
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
