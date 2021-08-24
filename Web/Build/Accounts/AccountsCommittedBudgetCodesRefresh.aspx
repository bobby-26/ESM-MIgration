<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCommittedBudgetCodesRefresh.aspx.cs" Inherits="AccountsCommittedBudgetCodesRefresh" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiOwnerBudgetCode" Src="~/UserControls/UserControlMultiColumnOwnerBudgetCodeT.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Projectcode" Src="~/UserControls/UserControlProjectCode.ascx" %>

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
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
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
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:BudgetCode ID="ucBudgetCode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            OnTextChangedEvent="ucBudgetCode_Changed" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiOwnerBudgetCode ID="ucOwnerBudgetCode" runat="server" CssClass="dropdown_mandatory" Enabled="true" />
                    </td>

                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProjectCode" runat="server" Text="Project Code"></telerik:RadLabel>
                    </td>
                    <td>
                   
                         <eluc:Projectcode ID="ucProjectcode" runat="server" AppendDataBoundItems="true" AutoPostBack="true" />
                    
                    </td>


                </tr>
            </table>
        </div>

    </form>
</body>
</html>
