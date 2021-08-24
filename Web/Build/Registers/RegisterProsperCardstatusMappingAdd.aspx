<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterProsperCardstatusMappingAdd.aspx.cs" Inherits="RegisterProsperCardstatusMappingAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CardstatusMapping</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmcardstatus" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuCardstatusMapping" runat="server" OnTabStripCommand="MenuCardstatusMapping_TabStripCommand"></eluc:TabStrip>
        </div>
        <table cellpadding="3" cellspacing="3">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblcardstatus" runat="server" Text="Cardstatus Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlcardstatus" runat="server" Visible="false" AllowCustomText="true" Width="65%" EmptyMessage="Type to Select" OnTextChanged="ddlcardstatus_OnTextChanged" AutoPostBack="true">
                                    </telerik:RadComboBox>
                    <telerik:RadTextBox ID="txtcardstatus" runat="server" Enabled="false" ReadOnly="true" CssClass="readonlytextbox" Text="<%# ddlcardstatus.SelectedItem.Text.ToString() %>" Width="110%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblrank" runat="server" Text="Rank"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="false" AutoPostBack="false"
                            CssClass="dropdown_mandatory" Width="110%" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblminpointsrequired" runat="server" Text="Min Points Required"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtminpointsrequired" runat="server" MaxLength="100" CssClass="input_mandatory" Width="110%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblmaxpointsrequired" runat="server" Text="Max Points Required"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtmaxpointsrequired" runat="server" MaxLength="100" CssClass="input_mandatory" Width="110%"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
