<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBankAdd.aspx.cs" Inherits="Registers_RegistersBankAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bank</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersBank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <%--<eluc:TabStrip ID="MenuBank" runat="server" OnTabStripCommand="MenuBank_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>--%>
            <eluc:TabStrip ID="MenuBank1" runat="server" OnTabStripCommand="MenuBank1_TabStripCommand"></eluc:TabStrip>
        </div>
        <table id="tblBank" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblbname" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtname" runat="server" MaxLength="200" CssClass="input_mandatory" Width="60%"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblscode" runat="server" Text="Short Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtShortcode" runat="server" MaxLength="50" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblaccnopattern" runat="server" Text="A/C No Digits"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtacnoDigits" runat="server" CssClass="input_mandatory" Text="" MaskText="##" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblremitancepattern" runat="server" Text="A/C No Format"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtacnopattern" runat="server" MaxLength="50" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblswiftnodigits" runat="server" Text="Swift Code Digits"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtswiftcodedigits" runat="server" CssClass="input_mandatory" Text="" MaskText="##" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblAllowCharacterYN" runat="server" Text="Allow Character YN"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkAllowCharacterYN" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblactiveyn" runat="server" Text="Active YN"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkActiveYN" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
