<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseAmosFormGeneral.aspx.cs"
    Inherits="PurchaseAmosFormGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="../UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
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
    <form id="frmOrderForm" runat="server" autocomplete="off">
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
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlFormGeneral">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                border: none; width: 100%">
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormNumber" runat="server" Width="120px" CssClass="input_mandatory"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblStockType" runat="server" Text="Stock Type"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtType" runat="server" Text=" " Width="90px" CssClass="input"
                                Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTitle" runat="server" Text="Title"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromTitle" runat="server" Width="330px" CssClass="input_mandatory" Enabled="false"
                                MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblOrderedDate" runat="server" Text="Ordered Date"></asp:Literal>
                         
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderDate" runat="server" Width="90px" Enabled="false" CssClass="input" ReadOnly="true"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ceOrderDate" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtOrderDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                            
                        </td>
                        <td>
                            <span id="spnPickListMaker">
                                <asp:TextBox ID="txtVendorNumber" runat="server" Width="60px" CssClass="input" Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtVenderName" runat="server" Width="180px" CssClass="input" Enabled="False"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132', true);"
                                    Text=".." />
                                <asp:TextBox ID="txtVendor" runat="server" Width="1px" CssClass="input"></asp:TextBox>
                            </span>
                            &nbsp;
                             <asp:Image runat="server" ID="cmdvendorAddress" ImageUrl="<%$ PhoenixTheme:images/supplier-address.png %>" 
                                 ToolTip="Address" style="cursor: pointer; vertical-align: top"></asp:Image>
                        </td>
                        <td>
                           <%-- Confirmed--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmDate"  Visible="false"  runat="server" Width="90px" CssClass="input"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ceConfirmDate" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtConfirmDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
               
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" CssClass="input" runat="server" />
                        </td>
                         <td>
                            <asp:Label ID="lblFinalTotal" runat="server">   Final Total</asp:Label>s
                        </td>
                        <td>
                            <eluc:Decimal ID="txtFinalTotal" runat="server" Width="90px" Mask="99,999,999.99"
                                CssClass="input" />
                        </td>
                      
                    </tr>
                    <tr id="trPay" runat="server">
                       <%-- <td>
                            <asp:Label ID="lblVenderEsmeted" runat="server" Visible ="false" >  Vendor Estimate </asp:Label>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtVenderEsmeted"  Visible ="false"  runat="server" Width="120px" Mask="99,999,999.99"
                                CssClass="input" ReadOnly="true" />
                        </td>--%>
                       
                        <td></td>
                        <td></td>
                          <td>
                          <%--  Bill To--%>
                        </td>
                        <td>
                            <eluc:Company ID="ucPayCompany" AppendDataBoundItems="true" Visible ="false"  runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Literal ID="lblPOHavingIssues" runat="server" Text="PO Having Issues"></asp:Literal>
                            
                        </td>
                        <td valign="top">
                            <asp:CheckBox ID="chkIssues" runat="server" />
                        </td>
                        <td valign="top">
                            <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="input" Width="360px" MaxLength="400"
                                Height="40px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Status runat="server" ID="ucStatus" />
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
