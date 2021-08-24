<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreTypeFilter.aspx.cs"
    Inherits="InventoryStoreTypeFilter" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStockItemFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuStoreTypeFilter" runat="server" OnTabStripCommand="MenuStoreTypeFilter_TabStripCommand"></eluc:TabStrip>
        <br clear="all" />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtNumber" runat="server" CssClass="input" MaxLength="8"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 20">
                    <telerik:RadLabel ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListVendor">
                        <telerik:RadTextBox ID="txtPreferredVendorCode" runat="server" ReadOnly="false" CssClass="input"
                            MaxLength="20" Width="80px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtPreferredVendorName" runat="server" ReadOnly="false" CssClass="input"
                            MaxLength="200" Width="120px"></telerik:RadTextBox>
                         <asp:LinkButton ID="ImgShowMakerVendor" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="cmdClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click">
                            <span class="icon"><i class="fas fa-paint-brush"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
