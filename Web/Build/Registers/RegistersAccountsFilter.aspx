<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAccountsFilter.aspx.cs"
    Inherits="RegistersAccountsFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">    
            <eluc:Title runat="server" ID="ucTitleFilter" Text="Account Filter" ShowMenu="false"/>                 
        </div>
    </div>               
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <div id="divFind">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblAccountCode" runat="server" Text="Account Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtAccountCodeSearch" MaxLength="200" CssClass="input"
                                Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblGroup" runat="server" Text="Group"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucGroupSearch" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblActive" runat="server" Text="Active"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkActiveSearch" runat="server" Checked="true" ></asp:CheckBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblAccountLevel" runat="server" Text="Account Level"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAccountLevelSearch" runat="server" CssClass="input">
                                <asp:ListItem Text="--- Select---" Value="" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Level 1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Level 2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Level 3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Level 4" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
