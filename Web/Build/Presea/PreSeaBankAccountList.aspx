<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBankAccountList.aspx.cs" Inherits="PreSeaBankAccountList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea Bank Account List</title>
  <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
   <form id="frmPreSeaBankAccountList" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaBankAccountListEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="PreSea Bank Account" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPreSeaBankAccountList" runat="server" OnTabStripCommand="PreSeaBankAccountList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            Account Type
                        </td>
                        <td>
                            <eluc:Hard runat="server" ID="ucAccountType" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            Beneficiary Name
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtAccountName" CssClass="input_mandatory" Width="60%"
                                MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Account Number
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtAccountNumber" CssClass="input_mandatory" Width="60%"
                                MaxLength="50"></asp:TextBox>
                        </td>
                        <td align="left">
                            Seafarer Bank
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSeafarerBank" CssClass="input_mandatory" Width="60%"
                                MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Short Code (Seafarer Bank)
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSeafarerBankSortCode" Width="60%" MaxLength="10"
                                CssClass="input"></asp:TextBox>
                        </td>
                        <td align="left">
                            Swift Code (Seafarer Bank)
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSeafarerBankSwiftCode" CssClass="input" Width="60%"
                                MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Branch (Seafarer Bank)
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSeafarerBankBranch" CssClass="input" Width="60%"
                                MaxLength="100"></asp:TextBox>
                        </td>
                        <td align="left">
                            Payment Percentage
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPaymentPercentage" Width="60%" MaxLength="50"
                                CssClass="input txtNumber"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="mexPaymentPercentage" runat="server" TargetControlID="txtPaymentPercentage"
                                Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <eluc:Address ID="ucAddress" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Intermediate Bank
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtIntermediateBank" CssClass="input" Width="60%"
                                MaxLength="100"></asp:TextBox>
                        </td>
                        <td align="left">
                            Address (Int. Bank)
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtIntermediateBankAddres" CssClass="input" TextMode="MultiLine"
                                Width="60%" Height="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Country (Int. Bank)
                        </td>
                        <td>
                            <eluc:Country runat="server" ID="ucIntermediateBankCountry" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                        <td align="left">
                            IBAN Number (Int. Bank)
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtIBANNumber" Width="60%" MaxLength="50" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Swift Code (Int. Bank)
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtIntermediateBankSwiftCode" CssClass="input" Width="60%"
                                MaxLength="100"></asp:TextBox>
                        </td>
                        <td align="left">
                            Type Of Remittance
                        </td>
                        <td>
                            <eluc:Quick runat="server" ID="ucRemittanceType" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Mode Of Payment
                        </td>
                        <td>
                            <eluc:Hard runat="server" ID="ucModeOfPayment" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
