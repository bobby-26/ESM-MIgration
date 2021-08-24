<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditPurchaseMoreInfo.aspx.cs" Inherits="AccountsCreditPurchaseMoreInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Credit Purchase More Info</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmVesselCreditPurchaseMoreInfo" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMoreInfoEntry">
        <ContentTemplate>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="div1">
                        <eluc:Title runat="server" ID="ucTitle" Text="Credit Purchase More Info" ShowMenu="false" />                        
                    </div>
                </div>
                <div>
                    <table id="tblMoreInfo">
                       <tr>
                            <td>
                                <asp:Literal ID="lblEmail" runat="server" Text="Email"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="input" MaxLength="300" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                       </tr> 
                       <tr>
                            <td>
                                <asp:Literal ID="lblExchangeRate" runat="server" Text="Exchange Rate"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>
                            </td>
                       </tr> 
                       <tr>
                            <td>
                                <asp:Literal ID="lblTotalAmountUSD" runat="server" Text="Total Amount(USD)"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>
                            </td>
                       </tr> 
                    </table>
                </div>                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
