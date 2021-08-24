<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSupplierStoreTypeMapping.aspx.cs" Inherits="RegistersSupplierStoreTypeMapping_aspx" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .RadCheckBoxList span.rbText.rbToggleCheckbox {
                text-align: left;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRestHourWorkCalenderRemarks" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:TabStrip ID="MenuStockType" runat="server" OnTabStripCommand="MenuStockType_TabStripCommand"></eluc:TabStrip>
        <br />
        <table width="95%" cellpadding="1" cellspacing="1">
            <tr valign="top">
                <td>
                    <telerik:RadCheckBoxList runat="server" ID="chkStoreTypeMap" Columns="5" AutoPostBack="false">
                    </telerik:RadCheckBoxList>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
