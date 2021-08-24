<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareItemMove.aspx.cs" Inherits="InventorySpareItemMove" %>

<!DOCTYPE html>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Spare Item Move</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSpareMove" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuInventorySpareMove" runat="server" OnTabStripCommand="MenuInventorySpareMove_TabStripCommand"></eluc:TabStrip>
                </div>
                <div id="divField" style="position: relative; z-index: 2">
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSpareItem" runat="server" Text="Spare Item"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtSpareItem" CssClass="readonlytextbox" Width="200px" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblCurrentLocation" runat="server" Text="Current Location"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCurrentLocation" runat="server" ReadOnly="true" Width="200px" CssClass="readonlytextbox"></telerik:RadTextBox>
                                <telerik:RadLabel runat="server" ID="lblLocationId" Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblSpareItemId" Visible="false"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblInStock" runat="server" Text="In Stock"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtInStockQuantity" runat="server" ReadOnly="true" Width="200px" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblMoveto" runat="server" Text="Move to"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="ddlLocationList" runat="server" CssClass="dropdown_mandatory" Visible="false"
                                    DataTextField="FLDLOCATIONNAME" DataValueField="FLDLOCATIONID" />--%>
                                <telerik:RadDropDownList runat="server" ID="ddlLocationList" CssClass="input" Width="200px"></telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMoveQuantity" runat="server" Text="Move Quantity"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtMoveQuantity" runat="server" MaxLength="7" IsInteger="true" Width="200px" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <eluc:Status runat="server" ID="ucStatus" Text="" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
