<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminSoftwareDisposedAssetFilter.aspx.cs" Inherits="Registers_RegistersAdminSoftwareDisposedAssetFilter" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlTile" Text="Disposed Asset Search" ShowMenu="false"></eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="AssetSearchFilter" runat="server" OnTabStripCommand="AssetSearchFilter_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:UpdatePanel runat="server" ID="pnlAssetSearch">
            <ContentTemplate>
                <div id="divFind">
                    <table id="tblConfigureAdminAsset" width="100%">
                    <tr>
                        <td>
                                Location
                            </td>
                            <td>
                                <eluc:Zone ID="ddlLocation" runat="server" AppendDataBoundItems="true" CssClass="input" Width="120px" />
                            </td>
                        <td>
                            <asp:Literal ID="lblAssetType" runat="server" Text="Asset Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="input" Width="120px"
                                DataValueField="FLDASSETTYPEID" DataTextField="FLDNAME"  AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblDate" runat="server" Text="Disposal Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" CssClass="input" runat="server" DatePicker="true" />
                        </td>
                    </tr>
                </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
