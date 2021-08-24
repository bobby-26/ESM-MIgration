<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentTypeFilter.aspx.cs"
    Inherits="InventoryComponentTypeFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
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

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

        <script type="text/javascript">
            document.onkeydown = function(e) {
                var keyCode = (e) ? e.which : event.keyCode;
                if (keyCode == 13) {
                    __doPostBack('MenuComponentTypeFilter$dlstTabs$ctl00$btnMenu', '');
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentTypeFilter" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="div2" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="Component Type Filter" ShowMenu="True">
            </eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuComponentTypeFilter" runat="server" OnTabStripCommand="MenuComponentTypeFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlDiscussion">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumber" runat="server" CssClass="input" MaxLength="9"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="input" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <asp:Literal ID="lblMaker" runat="server" Text="Maker"></asp:Literal>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <asp:TextBox ID="txtMakerCode" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="txtMakerName" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="200" Width="120px"></asp:TextBox>
                            <img runat="server" id="ImgShowMaker" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true); " />
                            <asp:TextBox ID="txtMakerId" runat="server" CssClass="input readonlytextbox" Width="10px"></asp:TextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdMakerClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20">
                        <asp:Literal ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></asp:Literal>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <asp:TextBox ID="txtPreferredVendorCode" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="20" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="txtPreferredVendorName" runat="server" ReadOnly="true" CssClass="input readonlytextbox"
                                MaxLength="200" Width="120px"></asp:TextBox>
                            <img runat="server" id="ImgShowMakerVendor" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListVendor', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true); " />
                            <asp:TextBox ID="txtVendorId" runat="server" CssClass="input readonlytextbox" Width="10px"></asp:TextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblComponentClass" runat="server" Text="Component Class"></asp:Literal>
                    </td>
                    <td>
                        <eluc:UserControlQuick ID="ddlComponentClass" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtType" runat="server" CssClass="input" Width="180px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
