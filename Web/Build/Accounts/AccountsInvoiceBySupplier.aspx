<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceBySupplier.aspx.cs" Inherits="AccountsInvoiceBySupplier" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tax" Src="~/UserControls/UserControlTaxMaster.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselList" Src="../UserControls/UserControlVesselList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="DivHeader" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
            <script type="text/javascript" language="javascript">
                function SetPONumber(pickspan, url) {
                    if (document.getElementById(pickspan) != null) {
                        var elem = document.getElementById(pickspan).childNodes;
                        var args = 'ponumber=';
                        for (var i = 0; i < elem.length; i++) {
                            if (elem[i].type == 'text') {
                                args += elem[i].value;
                            }
                        }
                        parent.Openpopup('codehelp1', '', url + args);
                        return;
                    }
                }
            </script>
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="pnlInvoice" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuInvoice1" runat="server" OnTabStripCommand="Invoice_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblESMRegisterNumber" runat="server" Text="Register Number"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" MaxLength="25" ReadOnly="true"
                            CssClass="readonlytextbox" Width="150px">
                        </telerik:RadTextBox>
                        <asp:HyperLink ID="imgAttachment" runat="server" ImageUrl="<%$ PhoenixTheme:images/attachment.png%>"
                            Visible="false" ToolTip="Attachments"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" TextMode="SingleLine" ReadOnly="true"
                            CssClass="readonlytextbox" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr style="height: -15px" />
                    </td>
                </tr>
                <tr valign="top">
                    <td width="15%">
                        <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                                Width="88px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                                Width="200px">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="ImgSupplierPickList" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtInvoiceDateEdit" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVendorInvoiceNumber" runat="server" Text="Vendor Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSupplierRefEdit" runat="server" CssClass="input_mandatory" MaxLength="200"
                            Width="290px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            OnTextChangedEvent="Invoice_SetExchangeRate" />
                        <eluc:Number ID="ucInvoiceAmoutEdit" runat="server" CssClass="input_mandatory" MaskText="###.##" Width="152px"></eluc:Number>
                        <eluc:Number ID="txtExchangeRateEdit" runat="server" CssClass="readonlytextbox" DecimalPlace="6" Visible="false" ReadOnly="true" />

                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3" Width="290px"
                            CssClass="input">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtDTKey" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="lblForOfficeUse" runat="server" Text="For Office Use"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateReceived" runat="server" Text="Date Received"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtInvoiceReceivedDateEdit" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlInvoiceType" runat="server" CssClass="dropdown_mandatory" HardTypeCode="59"
                            AutoPostBack="TRUE" HardList='<%# PhoenixRegistersHard.ListHard(1, 59) %>' AppendDataBoundItems="true"
                            Width="300px" />
                        <%--OnTextChangedEvent="ddlInvoice_SelectedIndexChanged"--%>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
