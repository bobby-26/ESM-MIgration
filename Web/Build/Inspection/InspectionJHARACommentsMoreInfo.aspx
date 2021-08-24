<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionJHARACommentsMoreInfo.aspx.cs" Inherits="Inspection_InspectionJHARACommentsMoreInfo" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Comments More Info</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCommentsMore" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <br />
            <table cellpadding="2" cellspacing="1" style="width: 94%" border="1">
                <tr>
                    <td style="width: 25%">
                        <b>
                            <asp:Literal ID="lblArchivedByCap" runat="server" Text="Archived By"></asp:Literal></b>
                    </td>
                    <td>
                        <asp:Literal ID="lblArchivedBy" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <asp:Literal ID="lblArchivedOnCap" runat="server" Text="Archived On"></asp:Literal></b>
                    </td>
                    <td>
                        <asp:Literal ID="lblArchivedOn" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
