<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsChooseVessel.aspx.cs"
    Inherits="OptionsChooseVessel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>    
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <eluc:Title runat="server" ID="ucTitle" Text="Switch Vessel" />
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuOptionChooseVessel" runat="server" OnTabStripCommand="OptionChooseVessel_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="8">
            <tr>
                <td colspan="2">
                    <asp:Literal ID="lblClickontheVesselNamelinkbelowtoswitchTosearchkeyinpartofthevesselnameandtabouttoviewthefilteredlist" runat="server" Text="Click on the Vessel Name link below to switch. To search, key in part of the vessel name and tab out to view the filtered list."></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>&nbsp;<asp:TextBox ID="txtVesselName" runat="server" name="txtVesselName" MaxLength="200" CssClass="input" OnTextChanged="txtVesselName_TextChanged" AutoPostBack="true"></asp:TextBox>                    
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvVessel" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" OnRowCommand="gvVessel_RowCommand"
            ShowFooter="true" ShowHeader="true" EnableViewState="false">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <Columns>
                <asp:TemplateField HeaderText="Vessel Name">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblVesselID" visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                        <asp:Label ID="lblVesselName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                        <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="CHOOSEVESSEL" CommandArgument='<%# Container.DataItemIndex %>'
                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vessel Name">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPrincipalName" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALNAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        <eluc:Status runat="server" ID="ucStatus" />
    </div>
    </form>
</body>
</html>
