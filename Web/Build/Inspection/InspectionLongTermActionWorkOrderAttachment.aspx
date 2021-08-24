<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionLongTermActionWorkOrderAttachment.aspx.cs" Inherits="InspectionLongTermActionWorkOrderAttachment" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Order Attachment</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
     
    </div> 
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionAttachment" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>     
    
    <asp:UpdatePanel runat="server" ID="pnlInspectionScheduleEntry">
        <ContentTemplate>  
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />
                        <eluc:Title runat="server" id="ucTitle" Text="Attachments" ShowMenu="false"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuInspectionGeneral" runat="server" TabStrip="true" OnTabStripCommand="InspectionGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>                   
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 550px; width: 100%">
                </iframe>
            </div>         
        </ContentTemplate>               
    </asp:UpdatePanel>      
    </form>
</body>
</html>
