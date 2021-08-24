<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseContractItemGeneral.aspx.cs"
    Inherits="PurchaseContractItemGeneral" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register Src="../UserControls/UserControlQuick.ascx" TagName="UserControlQuick"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Form</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseContractGeneral" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="General" ShowMenu="false"></eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuFormGeneral" runat="server" OnTabStripCommand="MenuFormGeneral_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlFormGeneral">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style=" margin-left: 0px; vertical-align: top; border:none;  width: 100%">
            <table cellpadding="1" cellspacing="1" width ="100%">
                <tr>
                    <td>
                     <asp:Literal ID="lblCode" runat="server" Text="Code"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemNumber" runat="server" Width="90px" MaxLength="20"
                            CssClass="input_mandatory"></asp:TextBox>
                    </td>
                    <td>
                       <asp:Literal ID="lblMinQuantity" runat="server" Text="Min Quantity"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMinQuantity" runat="server" Text=" " Width="90px" CssClass="input txtNumber"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderMinQuantity" 
                        runat="server" InputDirection="RightToLeft" Mask="99,999.99" 
                        MaskType="Number" TargetControlID="txtMinQuantity"></ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                      <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName" runat="server" Width="300px" MaxLength ="50"  CssClass="input_mandatory"></asp:TextBox>
                    </td>
                    <td>
                       <asp:Literal ID="lblMaxQuantity" runat="server" Text="Max Quantity"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxQuantity" runat="server" Text="" Width="90px" CssClass="input txtNumber"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderMaxQuantity" 
                        runat="server" InputDirection="RightToLeft" Mask="99,999.99" 
                        MaskType="Number" TargetControlID="txtMaxQuantity"></ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Literal ID="lblComments" runat="server" Text="Comments"></asp:Literal>
                    </td>
                    <td rowspan ="2">
                        <asp:TextBox ID="txtItemComments" runat="server" Width="300px" MaxLength ="800" CssClass="input" 
                            TextMode="MultiLine" Height="39px"></asp:TextBox>
                    </td>
                    <td >
                       <asp:Literal ID="lblDiscount" runat="server" Text="Discount"></asp:Literal></td>
                    <td>
                        <asp:RadioButtonList ID="rdlistDiscount" runat="server" DataTextField="FLDHARDNAME"
                            DataValueField="FLDHARDCODE" RepeatDirection="Horizontal">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                <td></td>
                <td><asp:Literal ID="lblFlatRateDiscount" runat="server" Text="Flat Rate Discount(%)"></asp:Literal> </td>
                <td>
                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="input" 
                        Style="text-align: right" Width="90px"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="txtDiscount_MaskedEditExtender" 
                        runat="server" InputDirection="RightToLeft" Mask="99.99" 
                        MaskType="Number" TargetControlID="txtDiscount">
                    </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
            </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
