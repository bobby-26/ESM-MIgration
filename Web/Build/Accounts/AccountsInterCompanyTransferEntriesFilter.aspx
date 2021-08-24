<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInterCompanyTransferEntriesFilter.aspx.cs" Inherits="AccountsInterCompanyTransferEntriesFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <%: Scripts.Render("~/bundles/js") %>
      <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
  
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

    <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">

          
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
   
        
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtVoucherNumber" MaxLength="200" CssClass="input"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text="Reference Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRefenceNumber" MaxLength="100" CssClass="input"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVoucherLongDescription" runat="server" Text="Voucher Long Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtVoucherLongDescription" MaxLength="200" CssClass="input"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVoucherLineItemLongDescription" runat="server" Text="Voucher Line Item Long Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtLineItemLongDescription" MaxLength="100" CssClass="input"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVoucherFromDate" runat="server" Text="Voucher From Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtVoucherFromdate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVoucherToDate" runat="server" Text="Voucher To Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtVoucherTodate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAmountFrom" runat="server" Text="Amount From"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtAmountFrom" runat="server"  Width="120px"></eluc:Number>
                                         
                                        
                           <%-- <telerik:RadTextBox ID="txtAmountFrom" runat="server" CssClass="input" Width="100px"></telerik:RadTextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditInvoiceAmout" runat="server" AutoComplete="true"
                                InputDirection="RightToLeft" Mask="999,999,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                TargetControlID="txtAmountFrom" />--%>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAmountTo" runat="server" Text="Amount To"></telerik:RadLabel>
                        </td>
                        <td>
                             <eluc:Number ID="txtAmountTo" runat="server"  Width="120px"></eluc:Number>
                                         
                          <%--  <telerik:RadTextBox ID="txtAmountTo" runat="server" CssClass="input" Width="100px"></telerik:RadTextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="true"
                                InputDirection="RightToLeft" Mask="999,999,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                TargetControlID="txtAmountTo" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="dvCurrencyList" runat="server" class="input" style="overflow: auto; width: 33%;
                                height: 100px">
                                <asp:CheckBoxList ID="chkCurrencyList" runat="server" Height="100%" RepeatColumns="1"
                                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                       
                        <td>
                            <telerik:RadLabel ID="lblUserID" runat="server" Text="User ID"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="dvUsersList" runat="server" class="input" style="overflow: auto; width: 67%;
                                height: 100px">
                                <asp:CheckBoxList ID="chkUserList" runat="server" Height="100%" RepeatColumns="1"
                                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <telerik:RadLabel ID="lblAccount" runat="server" Text="Account"></telerik:RadLabel>  
                        </td>
                        <td>
                            <div runat="server" id="dvaccount" class="input" style="overflow: auto; width: 70%;
                                height: 100px">
                                <asp:CheckBoxList runat="server" ID="chkAccountList" Height="100%" RepeatColumns="1"
                                    AutoPostBack="True" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text="Voucher Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlVoucherStatus" runat="server" AppendDataBoundItems="true" 
                                AutoPostBack="TRUE" CssClass="input" Width="300px" />
                            &nbsp;
                        </td>
                    </tr>
                </table>
           </telerik:RadAjaxPanel>
    </form>
</body>
</html>
