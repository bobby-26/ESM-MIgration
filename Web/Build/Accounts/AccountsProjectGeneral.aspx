<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectGeneral.aspx.cs"
    Inherits="AccountsProjectGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Order</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <form id="frmInvoiceDirctPurchaseOrder" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVoucher">
            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
            </telerik:RadWindowManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" Visible="false" />
            <eluc:TabStrip ID="Menu" runat="server" OnTabStripCommand="Menu_TabStripCommand" TabStrip="true" Visible="false"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDirectPO" runat="server" OnTabStripCommand="MenuDirectPO_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblprojectcode" runat="server" Text="Project Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtprojectcode" runat="server" ReadOnly="true" CssClass="readonlytextbox" Text=""></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblallowpoyn" runat="server" Text="Allow New PO"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkAllowPOyn" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblPorjectTitle" runat="server" Text="Project Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input_mandatory" Text=""></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblAllowAccountingyn" runat="server" Text="Allow Accounting Voucher"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkAccountingVoucher" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lbl" runat="server" Text="Project Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ddltype" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            QuickTypeCode="156" Width="30%" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblCreatedBy" runat="server" Text="Created By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" Text="" Width="90%"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListExpenseAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input" ReadOnly="false"
                                MaxLength="20" Width="20%">
                            </telerik:RadTextBox>&nbsp;&nbsp;
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input"
                                ReadOnly="false" MaxLength="50" Width="40%">
                            </telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowAccount" Style="cursor: pointer; vertical-align: top"
                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx?iframename=true',true); " />
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="hidden" MaxLength="20" Width="10px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountSource" CssClass="hidden" runat="server" Width="30%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountUsage" CssClass="hidden" runat="server"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="30%" CssClass="hidden"></telerik:RadTextBox>&nbsp;&nbsp;
                            <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="55%" CssClass="hidden"></telerik:RadTextBox>
                            <%--  <asp:ImageButton ID="imgShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListSubAccount.aspx',true); " />--%>
                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblSubAccount" runat="server" Text="Sub Account"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <div runat="server" id="dvaccount" class="input" style="overflow: auto; width: 100%; height: 100px">
                            <asp:CheckBoxList runat="server" ID="cblsubAccount" Height="100%" RepeatColumns="1"
                                RepeatDirection="Horizontal" AppendDataBoundItems="true"
                                RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text="Budget Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBudgetAmount" runat="server" Style="text-align: right;" CssClass="input"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>Parent Project Code</td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlProjectCode" runat="server" DataTextField="FLDPROJECTCODE" DataValueField="FLDID" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>

                    </td>

                </tr>
            </table>
            <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
