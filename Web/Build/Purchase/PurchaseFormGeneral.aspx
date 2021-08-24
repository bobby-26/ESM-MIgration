<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormGeneral.aspx.cs"
    Inherits="PurchaseFormGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="../UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HardExtn" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Projectcode" Src="~/UserControls/UserControlProjectCode.ascx" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Form</title>
    <style type="text/css">
        .input {}
    </style>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOrderForm" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuFormGeneral">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuFormGeneral" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>

        
        <%--<div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">--%>
        
        
    
        <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuFormGeneral" runat="server" OnTabStripCommand="MenuFormGeneral_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
        <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtFormNumber" runat="server" Width="120px" CssClass="input_mandatory"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblFormTypeCaption" runat="server" Text="Form Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtType" runat="server" Text=" " Width="190px" CssClass="input"
                        Enabled="False">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblStockType" runat="server" Text="Stock Type / Department"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList RenderMode="Lightweight" runat="server" ID="ddlStockType" CssClass="input" Width="80px">
                        <Items>
                            <telerik:DropDownListItem Text="Spares" Value="SPARE" />
                            <telerik:DropDownListItem Text="Stores" Value="STORE" />
                            <telerik:DropDownListItem Text="Service" Value="SERVICE" />
                        </Items>
                    </telerik:RadDropDownList>
                    /
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtDepartment" CssClass="readonlytextbox" ReadOnly="true" Width="90px">
                            </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                </td>
                <td>

                        <span id="spantitle">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtFromTitle" runat="server" Width="240px" CssClass="input_mandatory"
                                MaxLength="50">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="lnkWorkorder" runat="server" Visible="false" ToolTip="Work order" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                        </span>

                </td>
                <td>
                    <telerik:RadLabel ID="lblCreated" runat="server" Text="Created"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtCreatedDate" runat="server" Width="190px" CssClass="input" Enabled="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblOrdered" runat="server" Text="Ordered"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtOrderDate" runat="server" Width="190px" CssClass="input" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorNumber" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                        <asp:LinkButton ID="cmdShowMaker" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', 'Common/CommonPickListAddressOwner.aspx?addresstype=130,131,132&windowname=detail&framename=ifMoreInfo&POPUP=Y', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>

                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendor" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton ID="cmdvendorAddress" ToolTip="Address" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-address-book"></i></span>
                    </asp:LinkButton>
                </td>
                <td>
                    <telerik:RadLabel ID="lblImportedDate" runat="server" Text="Imported"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtRecivedDate" runat="server" Width="190px" CssClass="input" Enabled="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblConfirmed" runat="server" Text="Confirmed"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtConfirmDate" runat="server" Width="190px" CssClass="input" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDeliveryLocation" runat="server" Text="Delivery Location"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnDLocation">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDeliveryLocationCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDeliveryLocationName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                        <asp:LinkButton ID="cmdShowLocation" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnDLocation', 'codehelp1', '', 'Common/CommonPickListDeliveryLocation.aspx', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDeliveryLocationId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton runat="server" ID="cmdClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click">
                            <span class="icon"><i class="fas fa-paint-brush"></i></span>
                    </asp:LinkButton>
                </td>
                <td>
                    <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtBugetDate" runat="server" Width="190px" CssClass="input" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDelivered" runat="server" Text="Delivered"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtlLastDeliveryDate" runat="server" Width="190px" CssClass="input" Enabled="false" />
                    <telerik:RadLabel ID="lblFormType" runat="server" Visible="false"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDeliveryAddress" runat="server" Text="Delivery Address"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnDAddress">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDeliveryAddressCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDeliveryAddressName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                        <asp:LinkButton ID="cmdShowAddress" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnDAddress', 'codehelp1', '', 'Common/CommonPickListAddress.aspx?addresstype=141&framename=ifMoreInfo', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>

                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDeliveryAddressId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton runat="server" ID="imgClear2" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearAddress_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="cmdDeliveryAddress" ToolTip="Address" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-address-book"></i></span>
                    </asp:LinkButton>


                </td>
                <td>
                    <telerik:RadLabel ID="lblApproved" runat="server" Text="Approved"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtApproveDate" runat="server" Width="190px" CssClass="input" Enabled="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblReceived" runat="server" Text="Received"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtVenderDelveryDate" runat="server" Width="190px" CssClass="input" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblContact" runat="server" Text="Contact"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnDeliveryAddress">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddressName" runat="server" Text="" MaxLength="30" CssClass="input"
                            Width="246">
                        </telerik:RadTextBox>
                        <asp:LinkButton ID="cmdAddressPurpose" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddressPurposeId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton runat="server" ID="ImageButton2" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearDeliveryContact_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                    </asp:LinkButton>
                </td>
                <td>
                    <telerik:RadLabel ID="lblComponentClass" runat="server" Text="Component"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListComponent">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentNo" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentName" runat="server" Width="95px" CssClass="input"></telerik:RadTextBox>
                        <asp:LinkButton ID="IbtnPickListComponent" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentID" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox>
                    </span>
                    <%--<eluc:Quick ID="ddlComponentClass" runat="server" CssClass="input" AppendDataBoundItems="true" />--%>
                    <eluc:Hard ID="ddlStockClassType" runat="server" CssClass="input" AppendDataBoundItems="true" Width="190px"
                        Visible="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtStatus" runat="server" Text="" Width="190px" CssClass="input"
                        ReadOnly="True" Enabled="False">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMainBudget">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                        <asp:LinkButton ID="btnShowBudget" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton runat="server" ID="imgClear1" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearBudget_Click">
                            <span class="icon"><i class="fas fa-paint-brush"></i></span>
                    </asp:LinkButton>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPaymentTerms" runat="server" Text="Payment Terms"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="UCPaymentTerms" AppendDataBoundItems="true" Enabled="false" CssClass="input" Width="190px"
                        runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="UCPeority" AppendDataBoundItems="true" CssClass="input" runat="server" Width="190px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAgentAddress" runat="server" Text="Agent Address"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListForwarder">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtForwarderCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtForwarderName" runat="server" BorderWidth="1px" Width="180px"
                            CssClass="input">
                        </telerik:RadTextBox>
                        <asp:LinkButton ID="btnPickForwarder" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListForwarder', 'codehelp1', '', 'Common/CommonPickListAddress.aspx?addresstype=135&framename=ifMoreInfo', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtForwarderId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton ID="cmdForwarderAddress" ToolTip="Address" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-address-book"></i></span>
                    </asp:LinkButton>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDeliveryTerms" runat="server" Text="Delivery Terms"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="UCDeliveryTerms" AppendDataBoundItems="true" Enabled="false" CssClass="input" Width="190px"
                        runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblBillTo" runat="server" Text="Bill To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Company ID="ucPayCompany" AppendDataBoundItems="true" runat="server" CssClass="input_mandatory" Width="190px" />
                    <telerik:RadLabel ID="lblBillToCompanyName" runat="server" Text=""></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOwnerbudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListOwnerBudget">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetCode" runat="server" Text="" MaxLength="20" CssClass="input"
                            Width="246">
                        </telerik:RadTextBox>
                        <asp:LinkButton ID="btnShowOwnerBudget" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="imgClearOwnerBudget" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearOwnerBudget_Click">
                                    <span class="icon"><i class="fas fa-paint-brush"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetName" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>

                </td>
                <td>
                    <telerik:RadLabel ID="lblPartPaid" runat="server">Part Paid  </telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPicPartPaid">
                        <eluc:Decimal ID="txtPartPaid" runat="server" Width="190px" CssClass="input" ReadOnly="true" />
                        <asp:LinkButton ID="cmdPicPartPaid" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                    </span>
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                </td>
                <td>
                    <%--<asp:DropDownList ID="ddlVesselAccount" runat="server" CssClass="input" DataTextField="FLDDESCRIPTION" AutoPostBack="true"
                                    DataValueField="FLDACCOUNTID" OnTextChanged="ddlVeselAccount_TextChanged" Width="190px"></asp:DropDownList> --%>

                    <telerik:RadComboBox EnableLoadOnDemand="true" ID="ddlVesselAccount" runat="server" CssClass="input" DataTextField="FLDDESCRIPTION" AutoPostBack="true"
                        DataValueField="FLDACCOUNTID" OnSelectedIndexChanged="ddlVeselAccount_TextChanged" Width="190px">
                    </telerik:RadComboBox>
                    <asp:HiddenField ID="hdnPrincipalId" runat="server" />
                </td>
                
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReason4Requisition" runat="server" Text="Reason for Requisition"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucReason4Requisition" runat="server" CssClass="input_mandatory"
                        AppendDataBoundItems="true" QuickTypeCode="147" Width="250px" ExpandDirection="Up" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblStandardComments" runat="server" Text="PO Status"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucPOStatus" runat="server" CssClass="input" ExpandDirection="Up"
                        AppendDataBoundItems="true" QuickTypeCode="150" Width="190px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblFinalTotal" runat="server">Final Total</telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtFinalTotal" runat="server" Width="190px" Mask="99,999,999.99"
                        ReadOnly="true" CssClass="input" />
                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lbldeliveryto" runat="server" Text="Delivery To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:HardExtn ID="ddlDeliveryto" runat="server" CssClass="input" HardTypeCode="267" HardList='<%# PhoenixRegistersHardExtn.ListHardExtn(267,1,null, null) %>' Width="250px" Enabled="false"
                                    AutoPostBack="true" OnTextChangedEvent="ddlDeliveryto_TextChangedEvent" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblReadinessdate" runat="server" Text="Readiness Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtReadinessDate" runat="server" Width="190px" CssClass="input" Enabled="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblProjectCode" runat="server" Text="Project Code"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Projectcode ID="ucProjectcode" runat="server" Width="190px" AppendDataBoundItems="true" AutoPostBack="true" />
                </td>
            </tr>
            <%-- <tr>
                       <td>
                            <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtDepartment" CssClass="readonlytextbox" ReadOnly="true" >
                            </telerik:RadTextBox>
                        </td>
                    </tr>--%>
            <tr id="trPay2" runat="server">
                <td colspan="6">
                    <hr />
                </td>
            </tr>
            <tr id="trPay1" runat="server">
                <td>
                    <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" CssClass="input" Enabled="False"
                        runat="server" Width="250px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblInvoiceNo" runat="server" Text="Invoice No."></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtInvoiceNo" CssClass="readonlytextbox" Width="190px"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                    <eluc:Decimal ID="txtEstimeted" runat="server" Visible="false" Width="0px" Mask="99,999,999.99"
                        CssClass="input" />
                    <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClip" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtInvoiceStatus" CssClass="readonlytextbox" Width="190px"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr id="trPay" runat="server">
                <td>
                    <telerik:RadLabel ID="lblVenderEsmeted" runat="server">Vendor Estimate </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtVenderEsmeted" runat="server" Width="250px" Mask="99,999,999.99"
                        CssClass="input" ReadOnly="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblInvoiceAmount" runat="server" Text="Invoice Amount"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtInvoiceAmount" runat="server" Width="190px" CssClass="input"
                        ReadOnly="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblInvoiceCurrency" runat="server" Text="Invoice Currency"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtInvoiceCurrency" CssClass="input" Width="190px"></telerik:RadTextBox>
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
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtFormCreatedBy" runat="server" Width="250px" CssClass="input"
                        Enabled="False">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPOOrderedBy" runat="server" Text="PO Ordered By"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtPOorderedBy" runat="server" Width="190px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPurchaseApprovedBy" runat="server" Text="Purchase Approved By"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtPurchaseAppovedBy" runat="server" Width="190px" CssClass="input"
                        Enabled="False">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%-- Requisition Approved By--%>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtReqApprovedBy" runat="server" Visible="false" Width="120px" CssClass="input"
                        Enabled="False">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <%-- Accumulated Budget--%>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtAccumulatedBudget" Visible="false" runat="server" Width="90px"
                        Style="text-align: right" CssClass="input" Enabled="False">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <%--Accumulated Total--%>
                </td>
                <td>
                    <eluc:Decimal ID="txtAccumulatedTotal" runat="server" Width="90px" CssClass="input" Visible="false" />

                </td>
            </tr>
        </table>
        </telerik:RadAjaxPanel>
        
     <%--       </div>--%>
    </form>
</body>
</html>
