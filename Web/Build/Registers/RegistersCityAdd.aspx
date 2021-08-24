<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCityAdd.aspx.cs" Inherits="Registers_RegistersCityAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>City</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuCity" runat="server" OnTabStripCommand="MenuCity_TabStripCommand"></eluc:TabStrip>
        </div>
        <table cellpadding="3" cellspacing="3">
            <tr>
                <td>
                    <telerik:RadLabel ID="ltCountry" runat="server" Text="Country"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Country ID="ddlcountrylist" Visible="false" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ddlcountrylist_TextChangedEvent"
                        AutoPostBack="true" />
                    <telerik:RadTextBox ID="txtCountry" runat="server" Enabled="false" ReadOnly="true" CssClass="readonlytextbox" Width="110%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="ltState" runat="server" Text="State"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:State runat="server" ID="ucState" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="110%"/>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCity" runat="server" MaxLength="100" CssClass="input" Width="110%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkInactive" runat="server" AutoPostBack="false" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
