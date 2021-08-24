<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsBroadcast.aspx.cs"
    Inherits="Options_OptionsBroadcast" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuPhoenixBroadcast" runat="server" OnTabStripCommand="PhoenixBroadcast_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUser" runat="server" Text="User"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListUsers">
                            <telerik:RadTextBox CssClass="input" runat="server" ID="txtUser" MaxLength="200" Width="80%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="lblUserCode" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>
                        <asp:ImageButton ID="cmdShowUsers" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox CssClass="input" runat="server" ID="txtSubject" MaxLength="200" Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMessage" runat="server" Text="Message"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox CssClass="input" runat="server" ID="txtMessage" TextMode="MultiLine"
                            Rows="5" Columns="80" MaxLength="500" Width="80%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
