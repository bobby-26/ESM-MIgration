<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfareCreditNote.aspx.cs" Inherits="AccountsAirfareCreditNote" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Credit Note</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCreditNote" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="ucTitle" Text="Credit Note" ShowMenu="false" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuCreditNote" runat="server" OnTabStripCommand="MenuCreditNote_TabStripCommand" Title="Credit Note"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCreditNoteRegisterNo" runat="server" Text="Airfare Credit Note Register No:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRegisterNo" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtStatus" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateReceived" runat="server" Text="Date Received:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucReceivedDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier:"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListSupplier">
                            <telerik:RadTextBox ID="txtSupplierCode" runat="server" Width="100px" CssClass="readonlytextbox"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSupplierName" runat="server" BorderWidth="1px" Width="250px"
                                CssClass="readonlytextbox">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnPickSupplier" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClick="btnPickSupplier_Click" />
                            <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucDate" runat="server" CssClass="input_mandatory" ReadOnly="false" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVendorCreditNoteNo" runat="server" Text="Vendor Credit Note No:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVendorCreditNoteNo" runat="server" CssClass="input_mandatory" MaxLength="50"
                            Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyAmount" runat="server" Text="Currency/ Amount:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency runat="server" ID="ucCurrency" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />

                     <%--   <telerik:RadTextBox runat="server" ID="txtAmount" CssClass="input_mandatory" Style="text-align: right;" Width="133px">
                            <ClientEvents OnKeyPress="keyPress" />
                        </telerik:RadTextBox>--%>
                        <%--        <ajaxtoolkit:maskededitextender id="MaskNumber" runat="server" targetcontrolid="txtAmount"
                            mask="999,999,999.99" masktype="Number" inputdirection="RightToLeft">
                            </ajaxtoolkit:maskededitextender>--%>
                        <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory"
                            IsInteger="true" IsPositive="true" Width="133px">
                        </eluc:Number>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCreditNoteVoucherNo" runat="server" Text="Credit Note Voucher No:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCreditNoteVoucherNo" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Width="270px" Height="75px" Resize="Both"
                            CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentVoucherNo" runat="server" Text="Payment Voucher No:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPaymentVoucherNo" CssClass="readonlytextbox" Width="350px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBilltoCompany" runat="server" Text="Paying Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
