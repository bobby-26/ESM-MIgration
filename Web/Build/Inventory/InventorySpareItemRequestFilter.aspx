<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareItemRequestFilter.aspx.cs"
    Inherits="InventorySpareItemRequestFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStockItemFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuStockItemFilter" runat="server" OnTabStripCommand="MenuStockItemFilter_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChangeRequestType" runat="server" Text="Number"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox ID="txtNumber" runat="server" CssClass="input" MaxLength="13" Width="180px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Name"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="Maker"></telerik:RadLabel>

                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtMakerCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                            MaxLength="20" Width="90px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtMakerName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                            MaxLength="200" Width="150px">
                        </telerik:RadTextBox>
                        <img runat="server" id="ImgShowMaker" style="cursor: pointer; vertical-align: top" src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtMakerId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                    </span>&nbsp;
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdMakerClear_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 20">
                    <telerik:RadLabel ID="RadLabel3" runat="server" Text="Preferred Vendor"></telerik:RadLabel>

                </td>
                <td>
                    <span id="spnPickListVendor">
                        <telerik:RadTextBox ID="txtPreferredVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                            MaxLength="20" Width="90px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtPreferredVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                            MaxLength="200" Width="150px">
                        </telerik:RadTextBox>
                        <img runat="server" id="ImgShowMakerVendor" style="cursor: pointer; vertical-align: top" src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                    </span>&nbsp;
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel4" runat="server" Text="Critical"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadCheckBox ID="chkCritical" runat="server"></telerik:RadCheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel5" runat="server" Text="Maker Reference"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox ID="txtMakerReference" runat="server" CssClass="input" MaxLength="200"
                        Width="180px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%-- Drawing Number--%>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDrawing" runat="server" Visible="false" CssClass="input" MaxLength="200"
                        Width="180px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%-- Global Search--%>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkGlobalSearch" Checked="false" runat="server" Visible="false"
                        CssClass="input">
                    </telerik:RadCheckBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
