<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreItemMove.aspx.cs" Inherits="InventoryStoreItemMove" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Store Item Move</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStoreMove" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlStoreItemMove">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Store Item Move" ShowMenu="false"></eluc:Title>
                    </div>
                    <div>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuInventoryStoreMove" runat="server" OnTabStripCommand="MenuInventoryStoreMove_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divField" style="position: relative; z-index: 2">
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblStoreItem" runat="server" Text="Store Item"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStoreItem" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lblCurrentLocation" runat="server" Text="Current Location"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurrentLocation" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                                <asp:Label runat="server" ID="lblLocationId" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblStoreItemId" Visible="false"></asp:Label>                                
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lblInStock" runat="server" Text="In Stock"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInStockQuantity" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>                                
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <asp:Literal ID="lblMoveto" runat="server" Text="Move to"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLocationList" runat="server" CssClass="dropdown_mandatory" Visible="false" 
                                    DataTextField="FLDLOCATIONNAME" DataValueField="FLDLOCATIONID" />                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblMoveQuantity" runat="server" Text="Move Quantity"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMoveQuantity" CssClass="input_mandatory" ></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender runat="server" ID="mskMoveQuantity" Mask="9999999" MaskType="Number"
                                     TargetControlID="txtMoveQuantity" AutoComplete="false"></ajaxToolkit:MaskedEditExtender>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <eluc:Status runat="server" ID="ucStatus" Text="" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="cmdHiddenSubmit" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
