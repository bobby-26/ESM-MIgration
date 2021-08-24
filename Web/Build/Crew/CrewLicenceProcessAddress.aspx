<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceProcessAddress.aspx.cs"   Inherits="CrewLicenceProcessAddress" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="pnlAddress" runat="server">
        <ContentTemplate>           
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                 <div class="subHeader">
                    <eluc:Title runat="server" ID="Title3" Text="" ShowMenu="false"></eluc:Title>                    
                </div>   
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                  <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                       <eluc:TabStrip ID="MenuAddressMain" runat="server" OnTabStripCommand="AddressMain_TabStripCommand">
                    </eluc:TabStrip>
                    </span>
                </div>                            
                
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td><asp:Literal ID="lblAddress" runat="server" Text="Address"></asp:Literal></td>
                        <td>
                            <asp:DropDownList ID="ddlFlagAddress" runat="server" OnDataBound="ddlFlagAddress_DataBound" OnTextChanged="ddlFlagAddress_TextChanged"
                                CssClass="input" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    
                    <td>
                        <asp:Literal ID="lblBankName" runat="server" Text="Bank Name"></asp:Literal>
                    </td>
                        <td>
                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="dropdown_mandatory"  AppendDataBoundItems="true" OnTextChanged="BankCurrency" AutoPostBack="true">
                        <asp:ListItem Value="Dummy">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="30px" ></asp:TextBox>
                        <asp:TextBox ID="txtBeneficiaryName" runat="server" CssClass="readonlytextbox" ReadOnly="true" ToolTip="Beneficiary Name"></asp:TextBox>
                        <asp:TextBox ID="txtBankAccount" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            ToolTip="Bank Account"></asp:TextBox>
                           <%-- <span id="spnPickListBank">
                                <asp:TextBox ID="txtAccountNo" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                    Width="60px"></asp:TextBox>
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                    Width="60px"></asp:TextBox>
                                <img id="imgBankPicklist" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                    style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                                <asp:TextBox ID="txtBankID" runat="server" Width="10px"></asp:TextBox>
                            </span>--%>
                             
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <eluc:Address runat="server" ID="ucAddress"></eluc:Address>
                            <eluc:Status runat="server" ID="ucStatus" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
 
    </form>
</body>
</html>
