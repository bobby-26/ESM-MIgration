<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreItemWantedFilter.aspx.cs"
    Inherits="InventoryStoreItemWantedFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Store Wanted Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStockItemFilter" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div style="font-weight: 600; font-size: 12px;" runat="server">
        <eluc:TabStrip ID="MenuStockItemFilter" runat="server" OnTabStripCommand="MenuStockItemFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNumber" runat="server" CssClass="input" Width="203px" MaxLength="8"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="203px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20">
                        <telerik:RadLabel ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtPreferredVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="80px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtPreferredVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="200" Width="120px"></telerik:RadTextBox>
                            <img runat="server" id="ImgShowMakerVendor" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>"/>&nbsp;
                             <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click" />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStoreType" runat="server" Text="Store Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlStockClass" runat="server" Visible="true" CssClass="input" Width="203px"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- Global Search--%>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkGlobalSearch" Checked="false" Visible="false" runat="server"
                            CssClass="input"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
