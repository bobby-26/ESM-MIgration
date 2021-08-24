<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceManualsComponentTreeView.aspx.cs"
    Inherits="PlannedMaintenanceManualsComponentTreeView" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentTreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Tree View</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="Component" ShowMenu="false" />
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlComponet">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table>
                <tr>
                    <td align="left">
                        <asp:TextBox ID="txtComponentName" CssClass="input" runat="server" Text="" MaxLength="100"></asp:TextBox>&nbsp;<asp:ImageButton
                            runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png %>" ID="cmdSearch"
                            OnClick="cmdSearch_Click" ToolTip="Search" />
                    </td>
                    <td>
                        <b>
                            <asp:Literal ID="Literal1" runat="server" Text="Mappend Component"></asp:Literal></b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <eluc:ComponentTreeView ID="tvwComponent" runat="server" ShowCheckBoxes="All" />
                    </td>
                    <td valign="top">
                        <asp:ListView ID="lstMappedComponent" runat="server">
                            <LayoutTemplate>
                                <ol>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </ol>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li><b></b>
                                    <%# Eval("FLDCOMPONENTNAME") %></b></li>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
