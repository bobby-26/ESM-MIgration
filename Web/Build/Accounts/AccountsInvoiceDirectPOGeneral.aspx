<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceDirectPOGeneral.aspx.cs"
    Inherits="AccountsInvoiceDirectPO" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoiceDirctPO" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuDirectPO" runat="server" OnTabStripCommand="MenuDirectPO_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" Width="291px"
                            AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPONumber" runat="server" Text="PO Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPONumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        <%--<ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtPONumber"
                                Mask="LLL-99-99" MaskType="None" Filtered="0123456789" InputDirection="LeftToRight" ClearMaskOnLostFocus="false">
                            </ajaxToolkit:MaskedEditExtender>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="90px" CssClass="input_mandatory"
                                Enabled="false">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" Width="200px" CssClass="input_mandatory"
                                Enabled="false">
                            </telerik:RadTextBox>
                            <img runat="server" id="ImgSupplierPickList" style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;"
                                src="<%$ PhoenixTheme:images/picklist.png %>" />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency runat="server" ID="ddlCurrency" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPOReceivedDate" runat="server" Text="PO Received Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPoReceivedDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAdvanceAmount" runat="server" Text="Advance Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAdvanceAmount" runat="server" CssClass="input" Width="90px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGST" runat="server" Text="GST"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkGSTOffset" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPOHavingIssues" runat="server" Text="PO Having Issues"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIssues" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="input" Width="360px" MaxLength="500"
                            Height="60px" TextMode="MultiLine"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDiscount" runat="server" Text="Discount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtDiscount" CssClass="input txtNumber" />
                        <telerik:RadLabel runat="server" ID="lblPercentage" Text="%"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
