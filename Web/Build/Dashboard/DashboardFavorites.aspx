<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardFavorites.aspx.cs" Inherits="Dashboard_DashboardFavorites" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDashboard">
        <ContentTemplate>
               <div id="navigation" style="top: 0px; vertical-align: top; width: 100%; postion:absolute; margin-bottom:0px; bottom:0px;">                
                <table width="50%" style="height: 100%; margin-top: 0px;  vertical-align:top;">
                    <tr>
                        <td>
                            <div style="
                                width: 100%; overflow-x: hidden; overflow-y: auto;">
                                <div class="dashboard_section" style="position: relative">
                                    <div id="div1">
                                        <a id="A1" onclick="javascript:parent.Openpopup('codehelp1', '', '../Options/OptionsMenuFavorites.aspx');">
                                            <img id="imgNews" runat="server" src="<%$ PhoenixTheme:images/28.png %>" alt="Dashboard"
                                                title="Dashboard" />Favorites</a>
                                    </div>
                                </div>
                                <asp:GridView GridLines="None" ID="gvFavorites" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" ShowHeader="false" EnableViewState="false" BorderColor="Transparent"
                                    OnRowDataBound="gvFavorites_RowDataBound">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUNAME") %>'
                                                    CommandName="TASK" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
