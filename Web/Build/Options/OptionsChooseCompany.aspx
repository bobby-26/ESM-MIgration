<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsChooseCompany.aspx.cs"
    Inherits="OptionsChooseCompany" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Company</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
         <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
         <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

         <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

         <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

         <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCompany" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <eluc:Title runat="server" ID="ucTitle" Text="Switch Company" />
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuOptionChooseCompany" runat="server" OnTabStripCommand="OptionChooseCompany_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="8">
            <tr>
                <td colspan="2">
                    <asp:Literal ID="lblClickontheCompanynamelinkbelowtoswitch" runat="server" Text="Click on the Company name link below to switch."></asp:Literal>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvCompany" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" OnRowCommand="gvCompany_RowCommand"
            ShowFooter="true" ShowHeader="true" EnableViewState="false">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <Columns>
                <asp:TemplateField HeaderText="Company Prefix">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblCompanyPrefix" runat="server" Text="Company Prefix"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCompanyID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></asp:Label>
                        <asp:Label ID="lblCompanyPrefix" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYPREFIX") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Company Prefix">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblCompanyCode" runat="server" Text="Company Code"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCompanyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Company Name">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblCompanyName" runat="server" Text="Company Name"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCompanyName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></asp:Label>
                        <asp:LinkButton ID="lnkCompanyName" runat="server" CommandName="CHOOSECOMPANY" CommandArgument='<%# Container.DataItemIndex %>'
                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        <eluc:Status runat="server" ID="ucStatus" />
    </div>
    </form>
</body>
</html>
