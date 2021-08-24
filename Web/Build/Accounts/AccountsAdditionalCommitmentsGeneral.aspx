<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdditionalCommitmentsGeneral.aspx.cs" Inherits="AccountsAdditionalCommitmentsGeneral" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Projectcode" Src="~/UserControls/UserControlProjectCode.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuAdvancePayment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuAdvancePayment" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>


        <%-- <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>--%>


        <eluc:TabStrip ID="MenuAdvancePayment" runat="server" OnTabStripCommand="MenuAdvancePayment_TabStripCommand"></eluc:TabStrip>

        <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <asp:UpdatePanel runat="server" ID="pnlFormGeneral">
                <ContentTemplate>

                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <br />
                    <table cellpadding="2" cellspacing="1" style="width: 100%; z-index: 5">
                        <tr>
                            <td>Vessel
                            </td>
                            <td>
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" AutoPostBack="true"
                                    OnTextChangedEvent="ucVessel_TextChangedEvent" />
                            </td>
                            <td>Vessel Account
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" OnDataBound="ddlAccountDetails_DataBound"
                                    DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" 
                                   OnTextChanged="ddlAccountDetails_TextChanged" AutoPostBack="true"   >
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td>Budget Code
                            </td>
                            <td>
                                <span id="spnPickListMainBudget">
                                    <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input_mandatory"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </td>
                            <td>Owner Budget Code
                            </td>
                            <td>
                                <span id="spnPickListOwnerBudget">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCode" runat="server" Text="" MaxLength="20"
                                        CssClass="input_mandatory" Width="246">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="txtOwnerBudgetName" runat="server" Width="0px" CssClass="input"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetId" runat="server" Width="0px" CssClass="input" Text=""></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>PO Number 
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPONumber" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                            </td>
                            <td>Supplier Name
                            </td>
                            <td>
                                <span id="spnPickListMaker">
                                    <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input_mandatory"
                                        Width="60px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input_mandatory"
                                        Width="180px">
                                    </telerik:RadTextBox>
                                    <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                        style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                                    <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>PO Date / Receive Date
                            </td>
                            <td>
                                <eluc:Date ID="ucPODate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                            <td>Reversed Date
                            </td>
                            <td>
                                <eluc:Date ID="ucReversedDate" runat="server" CssClass="input" DatePicker="true" />
                            </td>

                        </tr>
                        <tr>
                            <td>Amount(USD)
                            </td>
                            <td>
                                <eluc:Number ID="ucAmount" runat="server" DecimalPlace="2" CssClass="input_mandatory" />
                                <%--<eluc:Decimal ID="ucAmount" runat="server" CssClass="input_mandatory" Mask="999999.99" />--%>
                            </td>
                            <td>Description
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="input_mandatory" Width="300px" Rows="4" TextMode="MultiLine"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Created Date
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCreatedDate" runat="server" Enabled="false" CssClass="input"></telerik:RadTextBox>
                            </td>
                            <td>Updated Date
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtUpdatedDate" runat="server" Enabled="false" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Created By
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCreatedBy" runat="server" Enabled="false" CssClass="input"></telerik:RadTextBox>
                            </td>
                            <td>Updated By
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtUpdatedBy" runat="server" Enabled="false" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>Project Code
                            </td>
                            <td>
                                <eluc:Projectcode ID="ucProjectcode" runat="server" AppendDataBoundItems="true" AutoPostBack="true"  />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </telerik:RadAjaxPanel>
        <eluc:Status runat="server" ID="ucStatus" />
        <%-- <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
