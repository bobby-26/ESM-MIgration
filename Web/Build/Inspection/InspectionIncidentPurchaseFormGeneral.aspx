<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentPurchaseFormGeneral.aspx.cs" Inherits="InspectionIncidentPurchaseFormGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="../UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Form</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOrderForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:TabStrip ID="MenuFormGeneral" runat="server" OnTabStripCommand="MenuFormGeneral_TabStripCommand"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text=""></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFormNumber" runat="server" Width="120px" CssClass="input_mandatory"
                            Enabled="False">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFormType" runat="server" Text="Form Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtType" runat="server" Text=" " Width="134px" CssClass="input"
                            Enabled="False">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlStockType" CssClass="input" Width="160px" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" Selected="True" />
                                <telerik:RadComboBoxItem Text="Spares" Value="SPARE" />
                                <telerik:RadComboBoxItem Text="Stores" Value="STORE" />
                                <telerik:RadComboBoxItem Text="Service" Value="SERVICE" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFromTitle" runat="server" Width="265px" CssClass="input_mandatory"
                            MaxLength="50">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCreated" runat="server" Text="Created"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCreatedDate" runat="server" CssClass="input" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrdered" runat="server" Text="Ordered"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtOrderDate" runat="server" CssClass="input" Enabled="false" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorNumber" runat="server" Width="82px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" Width="180px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="cmdShowMaker" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtVendor" runat="server" Width="1px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton runat="server" ID="cmdvendorAddress" ToolTip="Address" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fa-address-book"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblImportedDate" runat="server" Text="Imported"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtRecivedDate" runat="server" CssClass="input" ReadOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblConfirmed" runat="Server" Text="Confirmed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtConfirmDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeliveryLocation" runat="Server" Text="Delivery Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnDLocation">
                            <telerik:RadTextBox ID="txtDeliveryLocationCode" runat="server" Width="82px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtDeliveryLocationName" runat="server" Width="180px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="cmdShowLocation" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtDeliveryLocationId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtBugetDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDelivered" runat="server" Text="Delivered"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtlLastDeliveryDate" runat="server" CssClass="input"
                            ReadOnly="false" />
                        <telerik:RadLabel ID="lblFormType1" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeliveryAddress" runat="server" Text="Delivery Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnDAddress">
                            <telerik:RadTextBox ID="txtDeliveryAddressCode" runat="server" Width="82px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtDeliveryAddressName" runat="server" Width="180px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="cmdShowAddress" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtDeliveryAddressId" runat="server" Width="1px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton runat="server" ID="cmdDeliveryAddress" ToolTip="Address" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fa-address-book"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblApproved" runat="server" Text="Approved"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtApproveDate" runat="server" CssClass="input" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReceived" runat="server" Text="Received"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtVenderDelveryDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetCode" runat="Server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMainBudget">
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="82px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowBudget" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentClass" runat="server" Text=" Component Class "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ddlComponentClass" runat="server" CssClass="input" AppendDataBoundItems="true" Width="180px" />
                        <eluc:Hard ID="ddlStockClassType" runat="server" CssClass="input" AppendDataBoundItems="true"
                            Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="Server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" Text="" Width="135px" CssClass="input"
                            ReadOnly="True" Enabled="False">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAgentAddress" runat="Server" Text="Agent Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListForwarder">
                            <telerik:RadTextBox ID="txtForwarderCode" runat="server" Width="82px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtForwarderName" runat="server" BorderWidth="1px" Width="180px" Enabled="False"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnPickForwarder" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtForwarderId" runat="server" Width="1" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton runat="server" ID="cmdForwarderAddress" ToolTip="Address" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fa-address-book"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentTerms" runat="server" Text="Payment Terms"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UCPaymentTerms" AppendDataBoundItems="true" Enabled="false" CssClass="input"
                            runat="server" Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UCPeority" AppendDataBoundItems="true" CssClass="input" runat="server" Width="160px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListOwnerBudget">
                            <telerik:RadTextBox ID="txtOwnerBudgetCode" runat="server" Text="" Enabled="false" MaxLength="20"
                                CssClass="input" Width="265px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOwnerBudgetName" runat="server" Width="0px" CssClass="hidden"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowOwnerBudget" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtOwnerBudgetId" runat="server" Width="0px" CssClass="input" Text=""></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOwnerBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDeliveryTerms" runat="server" Text="Delivery Terms"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UCDeliveryTerms" AppendDataBoundItems="true" Enabled="false" CssClass="input"
                            runat="server" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" CssClass="input" Enabled="False"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBillTO" runat="server" Text="Bill To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucPayCompany" AppendDataBoundItems="true" runat="server" />
                    </td>
                    <td>
                        <%-- Estimate<telerik:RadLabel ID="lblCurrencyEstimet" runat="server" BorderWidth="1px" Text="USD"
                                CssClass="input" Visible="False"></telerik:RadLabel>--%>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtEstimeted" runat="server" Visible="false" Width="120px" Mask="99,999,999.99"
                            CssClass="input" />
                    </td>
                </tr>
                <tr id="trPay" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblVenderEsmeted" runat="server">Vendor Estimate </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtVenderEsmeted" runat="server" Width="120px" Mask="99,999,999.99"
                            CssClass="input" ReadOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPartPaid" runat="server">Part Paid  </telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPicPartPaid">
                            <eluc:Decimal ID="txtPartPaid" runat="server" Width="120px" CssClass="input" ReadOnly="true" />
                            <asp:LinkButton ID="cmdPicPartPaid" runat="server" Visible="false"
                                ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFinalTotal" runat="server">Final Total</telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtFinalTotal" runat="server" Width="120px" Mask="99,999,999.99"
                            ReadOnly="true" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFormCreatedBy" runat="server" Text="Form Created By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFormCreatedBy" runat="server" Width="180px" CssClass="input"
                            Enabled="False">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPOOrderedBy" runat="server" Text="PO Ordered By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPOorderedBy" runat="server" Width="180px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurchaseApprovedBy" runat="server" Text="Purchase Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPurchaseAppovedBy" runat="server" Width="180px" CssClass="input"
                            Enabled="False">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- Requisition Approved By--%>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReqApprovedBy" runat="server" Visible="false" Width="120px" CssClass="input"
                            Enabled="False">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <%-- Accumulated Budget--%>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccumulatedBudget" Visible="false" runat="server" Width="90px"
                            Style="text-align: right" CssClass="input" Enabled="False">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <%--Accumulated Total--%>
                    </td>
                    <td>
                        <eluc:Date ID="txtAccumulatedTotal" runat="server" Style="text-align: right"
                            CssClass="input" Visible="false" Enabled="False" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
