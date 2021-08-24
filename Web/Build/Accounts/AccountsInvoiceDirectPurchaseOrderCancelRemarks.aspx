<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceDirectPurchaseOrderCancelRemarks.aspx.cs" Inherits="AccountsInvoiceDirectPurchaseOrderCancelRemarks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cancel Remarks</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDeliveryInstruction" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <eluc:TabStrip ID="MenuDelInstruction" runat="server" OnTabStripCommand="MenuDelInstruction_TabStripCommand"></eluc:TabStrip>
            <br clear="all" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Width="300px" Height="120px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 500px; height: 550px; width: 100%" frameborder="0"></iframe>
        </div>
    </form>
</body>
</html>
