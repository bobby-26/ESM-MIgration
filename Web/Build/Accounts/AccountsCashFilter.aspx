<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCashFilter.aspx.cs"
    Inherits="AccountsCashFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
               <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
    
       <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
            
              
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblRemittanceNumber" runat="server" Text="Cash Request ID"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtRemittenceNumberSearch" MaxLength="200" CssClass="input"
                                    Width="150px"></telerik:RadTextBox>
                            </td>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Cash Account"></telerik:RadLabel>
                            </td>
                           
                            <td style="width: 50%">
                                <span id="spnPickListCashAccount">
                                    <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input" MaxLength="20"
                                        Width="20%"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input"
                                        MaxLength="50" Width="36%"></telerik:RadTextBox>
                                    <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                        src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListCashAccount', 'codehelp1', '', '../Common/CommonPickListCashAccount.aspx',true); " />
                                    <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblRemittanceCurrency" runat="server" Text="Cash Currency"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                    runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRemittanceStatus" runat="server" Text="Cash Status"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ucRemittanceStatus" AppendDataBoundItems="true" CssClass="input" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListFleetManager">
                                    <telerik:RadTextBox ID="txtMentorName" runat="server" CssClass="input" MaxLength="100"
                                        Width="80%"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtuserDesignation" runat="server" CssClass="hidden" Enabled="false"
                                        MaxLength="30" Width="5px" ReadOnly="true"></telerik:RadTextBox>
                                    <asp:ImageButton runat="server" ID="imguser" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListEmployeeAccount.aspx', true); "
                                        ToolTip="Select Mentor" />
                                    <telerik:RadTextBox runat="server" ID="txtuserid" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox runat="server" ID="txtuserEmailHidden" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                                </span>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblIsZeroAmount" runat="server" Text="Show Remittances With Zero Amount"
                                    Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkZeroAmount" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
