<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentCorrectiveAction.aspx.cs" Inherits="InspectionIncidentCorrectiveAction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Incident CAR</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="divInspectionNonConformity" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmIncidentCAR" runat="server">
    
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlInspectionNC">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblCorrectiveAction" runat="server" Text="Corrective Action"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCARGeneral" runat="server" OnTabStripCommand="MenuCARGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table id="tblImmediateAction" runat="server" width="100%">
                    <tr>
                        <td style="width:10px">
                            <b><asp:Literal ID="lblImmediateAction" runat="server" Text="Immediate Action"></asp:Literal></b>
                        </td>
                        <td style="width:90px">
                            <asp:TextBox runat="server" ID="txtImmediateaction" CssClass="input" TextMode="MultiLine"
                                Height="70px" Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                </table>                
            </div> 
        </ContentTemplate>
    </asp:UpdatePanel>         
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />  
    </form>
</body>
</html>
