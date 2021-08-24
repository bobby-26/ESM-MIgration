<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVettingAttachment.aspx.cs" Inherits="InspectionVettingAttachment" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
                        <eluc:Title runat="server" id="ucTitle" Text="Attachments" ShowMenu="false"></eluc:Title>
                    </div>
                </div>             
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table id="tblGuidance" runat="server">
                    <tr>
                        <td style="width:15%">
                            <asp:Literal ID="lblAttachmentType" runat="server" Text="Attachment type"></asp:Literal>
                        </td>
                        <td> 
                            <asp:RadioButtonList ID="rblAttachmentType" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" 
                                OnSelectedIndexChanged="SetValue" RepeatLayout="Table" AutoPostBack="true" CssClass="readonlytextbox" Enabled="true">
                                <asp:ListItem Text="Inspection Report" Value="INSPECTIONREPORT" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="Operator's Comments" Value="OPERATORCOMMENTS"></asp:ListItem>
                                <asp:ListItem Text="Approval Letter" Value="APPROVALLETTER"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 550px; width: 100%">
                </iframe>
            </div>         
        </ContentTemplate>            
    </asp:UpdatePanel>      
    </form>
</body>
</html>
