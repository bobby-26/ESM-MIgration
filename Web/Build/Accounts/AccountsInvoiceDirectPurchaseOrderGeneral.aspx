<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceDirectPurchaseOrderGeneral.aspx.cs"
    Inherits="AccountsInvoiceDirectPurchaseOrder" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Projectcode" Src="~/UserControls/UserControlProjectCode.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Order</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoiceDirctPurchaseOrder" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="pnlInvoice" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenPick" CssClass="hidden" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuDirectPO" runat="server" OnTabStripCommand="MenuDirectPO_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1" style="z-index: 2;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlInvoiceType" runat="server" CssClass="dropdown_mandatory" HardTypeCode="59"
                            AutoPostBack="TRUE" HardList='<%# PhoenixRegistersHard.ListHard(1, 59) %>' AppendDataBoundItems="true"
                            Width="290px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPI" runat="server" Text="Is P&I"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlispni" CssClass="dropdown_mandatory" runat="server">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Yes" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="No" Value="0"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlVessel_Changed" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPONumber" runat="server" Text="PO Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPONumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        <eluc:ToolTip ID="ucToolTip" runat="server" />
                        <%--<ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtPONumber"
                                Mask="LLL-99-99" MaskType="None" Filtered="0123456789" InputDirection="LeftToRight" ClearMaskOnLostFocus="false">
                            </ajaxToolkit:MaskedEditExtender>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input" Width="400px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency runat="server" ID="ddlCurrency" AppendDataBoundItems="true" CssClass="dropdown_mandatory" ActiveCurrency="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucETA" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblETD" runat="server" Text="ETD"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucETD" runat="server" CssClass="input" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEstimateAmount" runat="server" Text="Estimate Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEstimateAmount" runat="server" Style="text-align: right;" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListBudgetCode">
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" CssClass="input" MaxLength="20" Enabled="false"
                                Width="20%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetCodeDescription" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <img runat="server" id="imgShowBudgetCode" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" />
                            <telerik:RadTextBox ID="txtBudgetCodeId" runat="server" CssClass="hidden" MaxLength="20"
                                Width="10px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceRefereceNo" runat="server" Text="Invoice Referece No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNo" runat="server" CssClass="readonlytextbox" Width="290px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                    </td>

                    <td>
                        <span id="spnPickListOwnerBudgetCode">
                            <telerik:RadTextBox ID="txtOwnerBudgetCode" runat="server" CssClass="input" MaxLength="20" Enabled="false"
                                Width="20%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOwnerBudgetName" runat="server" Width="0PX" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtOwnerBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOwnerBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
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
                            <telerik:RadTextBox ID="txtVenderName" runat="server" Width="197px" CssClass="input_mandatory"
                                Enabled="false">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="ImgSupplierPickList" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <br />
                        <font color="blue">* If Vendor is selected as "Not Receiving Invoices from Supplier"
                                in Address register,
                                <br />
                            it will not participate in the vendor pick list.</font>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblProjectCode" runat="server" Text="Project Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Projectcode ID="ucProjectcode" runat="server" AppendDataBoundItems="true" AutoPostBack="true" />
                        <%--  </td>
                    <td>--%>
                        <telerik:RadLabel ID="lblBillToCompany" runat="server" Text="Bill To Company"></telerik:RadLabel>
                        <%--</td>
                    <td>--%>
                        <eluc:Company ID="ddlCompany" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        <telerik:RadLabel ID="lblBillToCompanyName" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAdvanceAmount" runat="server" Text="Advance Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAdvanceAmount" runat="server" CssClass="readonlytextbox" Width="90px"
                            Enabled="false" />
                        <img runat="server" id="imgAddAdvanceAmount" style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGST" runat="server" Text="GST"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkGSTOffset" runat="server" />
                        <%--<telerik:RadCheckBox ID="chkGSTOffset1" runat="server"></telerik:RadCheckBox>
                    </td> --%>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="input_mandatory" Width="291px" MaxLength="400"
                            Height="60px" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDiscount" runat="server" Text="Discount"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" Width="60px" ID="txtDiscount" MaskText="###.##">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblPercentage" runat="server" Text="%"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddlAccountDetails_SelectedIndexChanged" AutoPostBack="true"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME"
                            DataValueField="FLDACCOUNTID" Width="290px">
                        </telerik:RadDropDownList>
                        <telerik:RadLabel ID="lblVesselAccountName" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApprovedon" runat="server" Text="Approved on"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtApprovedOn" runat="server" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblApprovedby" runat="server" Text="Approved by"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtApprovedBy" runat="server" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                        <telerik:RadLabel ID="lblApprovedStatus" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReceivedDate" runat="server" Text="Received Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtReceivedDate" runat="server"  />
                    </td>


                    <td colspan="2"></td>
                </tr>
            </table>
            <eluc:Confirm ID="ucConfirmMsg" runat="server" Visible="false" OnConfirmMesage="CheckMapping_Click"
                OKText="Yes" CancelText="No" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
