<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantAppraisalSeamanComment.aspx.cs" Inherits="CrewNewApplicantAppraisalSeamanComment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Appraisal Seafarer  Comment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
          <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewAppraisalSeamanComment" runat="server">   
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">   
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>     
            <eluc:TabStrip ID="AppraisalTabs" runat="server" OnTabStripCommand="AppraisalTabs_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>             
            <eluc:TabStrip ID="MenuSeamanComment" runat="server" OnTabStripCommand="SeamanComment_TabStripCommand">
            </eluc:TabStrip>       
        
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtvessel" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtdate" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOccassionForReport" runat="server" Text="Occassion For Report"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtoccassion" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <hr />
        <br clear="all" />
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSeafarerComment" runat="server" Text="Seafarer  Comment"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtSeaman" CssClass="input_mandatory" Width="400px" TextMode="MultiLine"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
