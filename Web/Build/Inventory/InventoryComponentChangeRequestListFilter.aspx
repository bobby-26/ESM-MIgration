<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentChangeRequestListFilter.aspx.cs" Inherits="Inventory_InventoryComponentChangeRequestListFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            document.onkeydown = function (e) {
                var keyCode = (e) ? e.which : event.keyCode;
                if (keyCode == 13) {
                    __doPostBack('MenuComponentFilter$dlstTabs$ctl00$btnMenu', '');
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <eluc:TabStrip ID="MenuComponentFilter" runat="server" OnTabStripCommand="MenuComponentFilter_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>
        <br clear="all" />
        <asp:UpdatePanel runat="server" ID="pnlDiscussion">
            <ContentTemplate>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNumber" runat="server" CssClass="input" MaxLength="9" Width="239px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="239px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlAddress ID="txtMakerId" AddressType="130,131" runat="server" />
                           <%-- <span id="spnPickListMaker">
                                <telerik:RadTextBox ID="txtMakerCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                    MaxLength="20" Width="85px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtMakerName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                    MaxLength="200" Width="150px">
                                </telerik:RadTextBox>
                                <img runat="server" id="ImgShowMaker" style="cursor: pointer; vertical-align: top"
                                    src="<%$ PhoenixTheme:images/picklist.png %>" />
                                <telerik:RadTextBox ID="txtMakerId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                            </span>&nbsp;--%>
                        <asp:LinkButton ID="cmdClear" runat="server" 
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click" >
                             <span class="ïcon"><i class="fas fa-broom"></i></span>
                        </asp:LinkButton>
                            <%--   <span id="spnPickListMaker">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtMakerCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtMakerName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                                <asp:ImageButton runat="server" ID="ImgShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', 'Common/CommonPickListAddressOwner.aspx?addresstype=130,131,132', true);"
                                    Text=".." />
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtMakerId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                            </span>&nbsp;
                            <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClick="cmdMakerClear_Click" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></telerik:RadLabel>
                        </td>
                        <td style="width: 100%">
                         <%--   <span id="spnPickListVendor">
                                <telerik:RadTextBox ID="txtPreferredVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                    MaxLength="20" Width="85px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtPreferredVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                    MaxLength="200" Width="150px">
                                </telerik:RadTextBox>
                                <img runat="server" id="ImgShowMakerVendor" style="cursor: pointer; vertical-align: top"
                                    src="<%$ PhoenixTheme:images/picklist.png %>" />
                                <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                            </span>&nbsp;--%>
                             <eluc:UserControlAddress ID="txtVendorId" AddressType="130,131" runat="server" />
                        <asp:LinkButton ID="ImageButton1" runat="server" 
                            ImageAlign="AbsMiddle" Text=".." OnClick="ImageButton1_Click">
                            <span class="ïcon"><i class="fas fa-broom"></i></span>
                        </asp:LinkButton>
                        </td>
                    </tr>
                    <%-- <tr>
                    <td>
                        Component Class
                    </td>
                    <td>
                        <eluc:UserControlQuick ID="ddlComponentClass" runat="server" CssClass="input" AppendDataBoundItems="true" QuickTypeCode="28" />
                    </td>
                </tr>--%>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblClassCode" runat="server" Text="Class Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtClassCode" runat="server" CssClass="input" MaxLength="200"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtType" runat="server" CssClass="input" MaxLength="50"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <%--<tr>
                    <td>
                        Global Search
                    </td>
                    <td>
                        <asp:CheckBox ID="chkGlobalSearch" Checked ="false" runat="server" ></asp:CheckBox>
                    </td>
                </tr>--%>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCritical" runat="server" Text="Critical"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkCritical" runat="server"></telerik:RadCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblComponentCategory" runat="server" Text="Component Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlQuick ID="ucComponentCategory" runat="server" QuickTypeCode="166" AppendDataBoundItems="true" />
                        </td>

                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
