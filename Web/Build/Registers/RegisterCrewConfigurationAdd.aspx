<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewConfigurationAdd.aspx.cs" Inherits="RegisterCrewConfigurationAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Configuration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frm1" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuTab" runat="server" OnTabStripCommand="MenuTab_TabStripCommand"></eluc:TabStrip>
        <table id="table1" width="100%">
             <tr>
                <td>
                    <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCode" runat="server" MaxLength="50"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDesc" runat="server" Text="Description"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDesc" runat="server" MaxLength="50"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblchkactive" runat="server" Text="IsActive"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkisactive" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                </td>
            </tr>
           
        </table>
    </form>
</body>
</html>

