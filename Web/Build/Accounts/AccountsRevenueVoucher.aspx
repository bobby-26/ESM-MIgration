<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRevenueVoucher.aspx.cs"
    Inherits="AccountsRevenueVoucher" EnableEventValidation="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="split" Src="~/usercontrols/usercontrolsplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Voucher</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .rwContent {
                color: red !important;
            }

            .rwTitle {
                font-weight: 600 !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlVoucher">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuVoucher" runat="server" OnTabStripCommand="Voucher_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherNumber" runat="server" MaxLength="50" ReadOnly="true"
                            Width="240px" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtVoucherDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherType" runat="server" Text="Voucher Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVoucherType" Text="Revenue Voucher" CssClass="readonlytextbox"
                            ReadOnly="true" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNumber" runat="server" CssClass="input_mandatory" MaxLength="50"
                            Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocked" runat="server" Text="Locked"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkLocked" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDueDate" runat="server" Text="Due Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucDueDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtStatus" CssClass="readonlytextbox" Width="160px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLongDescription" runat="server" Text="Long Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLongDescription" TextMode="MultiLine" Width="270px" Resize="Both"
                            Height="75px" CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks to print in SOA Attachment"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" MaxLength="100" Width="270px" Resize="Both"
                            Height="65px" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastUpdatedBy" runat="server" Text="Last Updated By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtUpdatedBy" CssClass="readonlytextbox" ReadOnly="true"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastUpdatedDate" runat="server" Text="Last Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtUpdatedDate" CssClass="readonlytextbox" ReadOnly="true"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
