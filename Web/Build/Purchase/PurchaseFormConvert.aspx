<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormConvert.aspx.cs"
    Inherits="PurchaseFormConvert" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormType" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuStockItemGeneral">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuStockItemGeneral" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Confirm ID="ucConfirmMsg" runat="server" Visible="false" OnConfirmMesage="CheckMapping_Click"
                OKText="Yes" CancelText="No" />
                <eluc:TabStrip ID="MenuStockItemGeneral" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand"></eluc:TabStrip>
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormType" runat="server" Text="Form Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <%--<telerik:RadRadioButtonList ID="rdoListFormType" runat="server" DataTextField="FLDHARDNAME" Width ="420px"
                            DataValueField="FLDHARDCODE" RepeatDirection="Horizontal" ValidationGroup ="FormType">
                        </telerik:RadRadioButtonList>--%>
                            <telerik:RadRadioButtonList ID="rdoListFormType" runat="server" RepeatDirection="Horizontal" Direction="Horizontal" RepeatLayout="Table" Enabled="true" ValidationGroup="FormType">
                                <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                                <Items>
                                    <telerik:ButtonListItem Text="Unchange" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblFormStatus" runat="server" Text="Form Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rdoListFormStatus" runat="server" RepeatDirection="Horizontal" Direction="Horizontal" RepeatLayout="Table" Enabled="true">
                                <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                                <Items>
                                    <telerik:ButtonListItem Text="Unchange" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                </table>
    </form>
</body>
</html>
