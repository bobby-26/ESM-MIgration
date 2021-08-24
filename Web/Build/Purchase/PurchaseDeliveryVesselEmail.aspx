<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDeliveryVesselEmail.aspx.cs"
    Inherits="PurchaseDeliveryVesselEmail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Email</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCorrespondenceEmail" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadAjaxPanel runat="server" ID="pnlCorrespondenceEmail">

                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <asp:Button ID="cmdHiddenSubmit" Title="Vessel Email" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />
                    <eluc:TabStrip ID="MenuCorrespondenceEmail" runat="server" OnTabStripCommand="CorrespondenceEmail_TabStripCommand">
                    </eluc:TabStrip>

                <br />
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr id="trFrom" runat="server">
                        <td>
                            
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtFrom" Width="500px" runat="server" CssClass="input" Visible="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trTO" runat="server">
                        <td>
                            To
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtTO" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trCC" runat="server">
                        <td>
                            Cc
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trBCC" runat="server">
                        <td>
                            Bcc
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtBCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Subject
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtSubject" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadEditor ID="txtBody" runat="server" Width="100%" Height="275px" RenderMode="Lightweight">
                                <Modules>
                                    <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                    <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                    <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                    <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                                </Modules>
                            </telerik:RadEditor>
                        </td>
                    </tr>
                </table>
            <eluc:Status ID="ucStatus" runat="server" />

    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
