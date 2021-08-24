<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReconcilationStagingDiscountUpdatePopUp.aspx.cs" Inherits="AccountsReconcilationStagingDiscountUpdatePopUp" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Update All Cr Note Discount</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function ucConfirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmApproval" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuStaging" runat="server" OnTabStripCommand="MenuStaging_TabStripCommand"></eluc:TabStrip>
        <br />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPONumber" runat="server" Text="PO Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPONumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblNewCrNoteDiscount" runat="server" Text="New Cr. Note Discount (%)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtDiscountPercentage" runat="server" CssClass="input_mandatory" Width="150px"
                        Mask="999.9999999" />
                </td>
            </tr>
        </table>
        <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="ucConfirm_Click" />
        <%-- <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_Click" OKText="Yes"
                CancelText="No" />--%>
    </form>
</body>
</html>
