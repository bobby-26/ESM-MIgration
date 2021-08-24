<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogInitializeROB.aspx.cs" Inherits="Log_ElectricLogInitializeROB" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:radcodeblock id="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:radcodeblock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:TabStrip ID="logROBEdit" runat="server" OnTabStripCommand="logROBEdit_TabStripCommand"></eluc:TabStrip>
        <table cellspacing="1" width="100%">
            <tr>
                <br />
            </tr>
            <tr>
                <td>
                    <telerik:radlabel rendermode="Lightweight" id="lblRob" runat="server" text="ROB"></telerik:radlabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtRob" runat="server" Width="150px"  CssClass="input_mandatory"/>
                </td>

            </tr>
        </table>

    </form>
</body>
</html>
