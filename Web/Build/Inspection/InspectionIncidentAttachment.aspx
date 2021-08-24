<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentAttachment.aspx.cs" Inherits="InspectionIncidentAttachment" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionAttachment" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />
            <%--<eluc:TabStrip ID="MenuInspectionGeneral" runat="server" TabStrip="true" OnTabStripCommand="InspectionGeneral_TabStripCommand"></eluc:TabStrip>--%>
            <eluc:TabStrip ID="MenuTitle" runat="server" Title="Attachment"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblGuidance" runat="server">
                <tr>
                    <td>
                        <font color="blue"><b><asp:Literal ID="lblNote" runat="server" Text="*Note: Following to be added here as &#39;Attachments&#39;:
                                <br />&nbsp; Ship staff Training record ( Q19 ), Rest Hour record ( CR 6B ), 
                            Sickness report ( CR 11 ), Safety Alerts / Moments if issued, Ships Medical Log, 
                            Crew Statements, Photographs or sketches"></asp:Literal> </font>
                    </td>
                </tr>
            </table>
            <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 480px; width: 99%"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
