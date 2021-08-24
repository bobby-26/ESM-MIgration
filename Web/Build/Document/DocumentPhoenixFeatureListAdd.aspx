<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentPhoenixFeatureListAdd.aspx.cs" Inherits="DocumentPhoenixFeatureListAdd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Feature Add" ShowMenu ="false" />
                    </div>
                </div>
                 <table width="80%">
                    <tr>
                        <td>
                            <asp:Label ID = "MenuPath" runat = "server" Font-Size ="Large" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvMenu" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowCommand="gvMenu_RowCommand" OnRowDataBound="gvMenu_RowDataBound"
                                OnRowEditing="gvMenu_RowEditing" ShowHeader="true" EnableViewState="false" ShowFooter ="true" 
                                OnRowCancelingEdit ="gvMenu_RowCancelingEditing" OnRowUpdating = "gvMenu_RowUpdating">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Feature">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblMenuFeatureHeader" runat="server">Features&nbsp;
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFeatureID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEATUREID") %>'></asp:Label>
                                            <asp:Label ID="lblMenuValue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUVALUE") %>'></asp:Label>
                                            <asp:Label ID="lblMenuCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUCODE") %>'></asp:Label>
                                            <asp:Label ID="lblFeature" runat="server" Width= "60%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEATURE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate >
                                        <asp:Label ID="lblFeatureIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEATUREID") %>'></asp:Label>
                                            <asp:Label ID="lblMenuValueEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUVALUE") %>'></asp:Label>
                                            <asp:Label ID="lblMenuCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUCODE") %>'></asp:Label>
                                            <asp:TextBox ID ="txtFeatureEdit" runat ="server" CssClass ="input_mandatory" Width= "60%" Text = '<%# DataBinder.Eval(Container,"DataItem.FLDFEATURE") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate >
                                            <asp:TextBox ID ="txtFeatureAdd" runat ="server" CssClass ="input_mandatory" Width= "60%"></asp:TextBox>
                                        </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblActionHeader" runat="server">
                                                    Action
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                                    ToolTip="Edit Feature List"></asp:ImageButton>
                                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                                    ToolTip="Delete"></asp:ImageButton>                                                        
                                            </ItemTemplate>
                                            <EditItemTemplate >
                                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                    CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                                    ToolTip="Save"></asp:ImageButton>
                                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                    width="3" />
                                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                    ToolTip="Cancel"></asp:ImageButton>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                    CommandName="ADD" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                                    ToolTip="Add"></asp:ImageButton>
                                            </FooterTemplate>
                                        </asp:TemplateField>                                    
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>     
    </form>
</body>
</html>
