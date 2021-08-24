<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceLedgerPostingConfirmationcheck.aspx.cs"
    Inherits="AccountsInvoiceLedgerPostingConfirmationcheck" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <style type="text/css">
        .style1
        {
            height: 20px;
        }
    </style>
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
       
          
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
             
                
                    <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
             
                <br />
                <br />
             
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblckInvoicenumber" runat="server" Text="Invoice number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtckInvoiceNumber" runat="server" MaxLength="25" ReadOnly="true"
                                CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                        </td>
                    
                        <td>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" />
                               <eluc:Number  CssClass="input_mandatory txtNumber" ID="ucInvoiceAmoutEdit" runat="server" Mask="9,999.999999" Width="120px"></eluc:Number>

                       <%--     <telerik:RadTextBox ID="ucInvoiceAmoutEdit" runat="server" CssClass="input_mandatory txtNumber"
                                DecimalPlace="2" IsPositive="true" Width="120px"></telerik:RadTextBox>--%>
                                <telerik:RadTextBox ID="txtExchangeRateEdit" runat="server" CssClass="readonlytextbox txtNumber"
                                Visible="false" Wrap="False" Width="150px" ReadOnly="true"></telerik:RadTextBox>
                                  <eluc:Number Visible="false" ID="MaskedEditExchangeRate" runat="server"  Width="120px"></eluc:Number>
                                  <eluc:Number Visible="false" ID="MaskedEditExtender1" runat="server"  Width="120px"></eluc:Number>
                                         
                         <%--   <ajaxToolkit:MaskedEditExtender ID="MaskedEditExchangeRate" runat="server" AutoComplete="true"
                                InputDirection="RightToLeft" Mask="9,999.999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                TargetControlID="txtExchangeRateEdit" />
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="true"
                                InputDirection="RightToLeft" Mask="999999999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                TargetControlID="ucInvoiceAmoutEdit" />--%>
                            <span id="spnPickListMaker0">
                                <telerik:RadTextBox ID="txtInvoiceAdjustmentAmount" runat="server" CssClass="input" ReadOnly="false"
                                    Visible="False" Width="60px"></telerik:RadTextBox>
                            </span>
                        </td>
                    </tr>
                    </table>
             
                <br />
                <br />
                <br />
                <div>
                    <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 550px;
                        width: 100%" frameborder="0"></iframe>
                    <asp:HiddenField ID="hdnScroll" runat="server" />
                </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
