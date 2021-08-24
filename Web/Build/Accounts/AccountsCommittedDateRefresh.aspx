<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCommittedDateRefresh.aspx.cs" Inherits="AccountsCommittedDateRefresh" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiOwnerBudgetCode" Src="~/UserControls/UserControlMultipleColumnOwnerBudgetCode.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>


        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />

        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

        <eluc:TabStrip ID="MenuAdvancePayment" runat="server" OnTabStripCommand="MenuAdvancePayment_TabStripCommand"></eluc:TabStrip>

        <table cellpadding="2" cellspacing="1" style="width: 100%; z-index: 5">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEffectiveDate" runat="server" Text="Effective Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucEffectiveDate" runat="server" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCommitteddate" runat="server" Text="New Committed Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucCommittedDate" runat="server" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReason" runat="server" Text="Reason for Change in Committed Date"></telerik:RadLabel>
                </td>
                <td>
                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" CssClass="input_mandatory"></asp:TextBox>
                </td>

            </tr>
        </table>
      
    </form>
</body>
</html>
