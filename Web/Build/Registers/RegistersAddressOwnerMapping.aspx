<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddressOwnerMapping.aspx.cs" Inherits="RegistersAddressOwnerMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplier Mapping</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
    </eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="div1">
            <eluc:Title runat="server" ID="ucTitle" Text="Supplier Mapping" ShowMenu="false" />
        </div>
    </div>
<%--    <div runat="server" id="divSubHeader" class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="" Width="360px"></asp:Label>
        </div>
    </div>--%>
    <div class="navSelectHeader" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAddressRelation" runat="server" OnTabStripCommand="AddressRelation_TabStripCommand" TabStrip="true">
        </eluc:TabStrip>
    </div>
    <asp:UpdatePanel runat="server" ID="pnlAddressRelation">
        <ContentTemplate>
            <div id="divGrid" style="position: relative;">
                <asp:GridView ID="gvAddressRelaion" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Font-Size="11px" AllowSorting="true" ShowFooter="false" ShowHeader="true" Width="100%" OnRowCommand="gvAddressRelation_RowCommand" 
                    OnRowDataBound="gvAddressRelation_RowDataBound" 
                    onrowdeleting="gvAddressRelation_RowDeleting" >
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblCode" runat="server" Text="Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                <asp:Label ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIER") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="false" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
