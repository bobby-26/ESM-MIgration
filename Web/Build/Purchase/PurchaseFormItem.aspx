<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormItem.aspx.cs"
    Inherits="PurchaseFormItem" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit"  Src="~/UserControls/UserControlPurchaseUnit.ascx"%>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Form Line Item</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItem" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuLineItemGeneral">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuLineItemGeneral" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
         <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuLineItemGeneral" runat="server" OnTabStripCommand="MenuLineItemGeneral_TabStripCommand" TabStrip="true"></eluc:TabStrip>

        <br clear="all" />
        <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" CssClass="hidden"/>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPickItem">
                                <eluc:MaskNumber runat="server" ID="txtItemNumber" MaxLength="20" ReadOnly="true" Width="90px"/>
                                <%--<telerik:RadTextBox runat="server" ID="txtItemNumber" MaxLength="20" ReadOnly="true" Width="90px" RenderMode="Lightweight"></telerik:RadTextBox>--%>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtServiceNumber" runat="server" Width="120px" CssClass="input" ReadOnly ="true" Visible ="false" ></telerik:RadTextBox>
                                <asp:LinkButton ID="cmdShowItem" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                            </span>
                            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtStatus" runat="server" Width="120px" CssClass="input" ReadOnly="True" Enabled="false"
                                MaxLength="100"></telerik:RadTextBox>
                        </td>
                        <td>
                           <asp:Label ID="lblmakerRef" runat ="server"  Text ="Maker Reference"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtMakerReference" runat="server" Width="120px" MaxLength ="50" CssClass="input" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtPartName" runat="server" Width="300px" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                       <td>
                            <telerik:RadLabel ID="lblReceiptstatus" runat="server" Text="Receipt status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlQuick ID="ucReciptstatus" AppendDataBoundItems="true" CssClass="input" Width="120px"
                                runat="server" />
                        </td>
                        <td>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtExtraNumber" runat="server" Width="120px" CssClass="input" Visible="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trComponent" runat ="server" >
                        <td>
                            <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <span id="spnPickComponent">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponent" runat="server" Width="90px" Enabled="false" CssClass="input"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentName" runat="server" Width="207px" Enabled="false" CssClass="input"></telerik:RadTextBox>
                                <asp:LinkButton ID="cmdShowComponent" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickComponent', 'codehelp1', '', 'Common/CommonPickListComponentPurchase.aspx', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentID" runat="server" Width="0px" />
                                <asp:ImageButton runat="server" ID="imgJobList" ImageUrl="<%$ PhoenixTheme:images/component-detail.png %>"
                                    ImageAlign="AbsMiddle" Text=".." ToolTip="Jobs" />
                            </span>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDrawingNo" runat="server" Text="Drawing No."></telerik:RadLabel>
                        
                        </td>
                        <td>
                             <telerik:RadTextBox RenderMode="Lightweight" ID="txtDrawingNo" runat="server" Width="120px" MaxLength ="50"  CssClass="input"></telerik:RadTextBox>
                            <asp:CheckBox ID="chkBudgetedPurchase" runat="server" Text=" Budgeted Purchase" Visible="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPosition" runat="server" Text="Position"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtPosition" runat="server" Width="120px" Enabled="false" CssClass="input"></telerik:RadTextBox>
                            <asp:CheckBox ID="chkIncludeOnForm" runat="server" Text="Include On Form" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Label ID="lblPrice" runat="server"> Price </asp:Label>
                        </td>
                        <td>
                            <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" Width="0px" CssClass="input"
                                runat="server" Visible="false" />
                            <eluc:Number ID="txtPrice" runat="server" ReadOnly="true" Type="Number" 
                                CssClass="input" Width="120px"/>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRequestedQty" runat="server" Text="Requested Qty"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtRequestedQty" runat="server" Width="120px" CssClass="input_mandatory" DecimalDigits="0" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOrderQty" runat="server" Text="Order Qty"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtOrderQty" runat="server" Width="120px" CssClass="input" DecimalDigits="0" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblUnit" runat="server" Text="Unit"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Unit ID="ucUnit" AppendDataBoundItems="true" CssClass="input_mandatory" runat="server" Width="120px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblReceivedQty" runat="server" Text="Received Qty"></telerik:RadLabel>
                        </td>
                        <td>
                            <%--<eluc:Decimal ID="txtRecivedQty" runat="server" Width="90px" Mask="99.99" CssClass="input" />--%>
                             <eluc:Decimal RenderMode="Lightweight" DecimalPlace="0" ID="txtRecivedSpareQty" runat="server" MaxLength="22" Width="120px" CssClass="input" 
                                 Visible="false" DecimalDigits="0" ></eluc:Decimal>
                                     
                             <eluc:Decimal runat="server" DecimalDigits="3" CssClass="input" ID="txtRecivedStoreQty" MaxLength="8" Width="120px" Visible="false"/>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCancelledQty" runat="server" Text="Cancelled Qty"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtCanceledQty" runat="server" Width="120px" CssClass="input" Type="Number" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLastSuppliedDate" runat="server" Text="Last Supplied Date"></telerik:RadLabel>
                            
                        </td>
                        <td >
                            <eluc:Date ID="txtLastSuppliedDate" runat="server" CssClass="readonlytextbox" Width="120px" />                            
                        </td>
                        <td colspan="1">
                            <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
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
                        
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ForeColor="Blue" ID="lblNoteOrderQtycannotbealteredaftertheapproval" runat="server" Text="Note: 'Order Qty' cannot be altered after the approval."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOwnerbudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <span id="spnPickListOwnerBudget">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOwnerBudgetCode" runat="server" Text="" MaxLength="20" CssClass="input"
                                    Width="246"></telerik:RadTextBox>
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
                    </tr>
                </table>
        </telerik:RadAjaxPanel>

    <telerik:RadSplitter runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
