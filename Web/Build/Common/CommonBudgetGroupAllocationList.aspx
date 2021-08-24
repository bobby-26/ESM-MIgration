<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetGroupAllocationList.aspx.cs" Inherits="CommonBudgetGroupAllocationList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="mask" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Budget Group Allocation List</title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBudgetGroupAllocationList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuBudgetGroupAllocationList" runat="server" OnTabStripCommand="BudgetGroupAllocationList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="5">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="false" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Financial Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetGroup" runat="server" Text="Budget Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucBudgetGroup" HardTypeCode="30" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            HardList='<%# PhoenixRegistersBudgetGroup.ListBudgetGroup(83) %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApportionmentMethod" runat="server" Text="Apportionment Method"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucApportionmentMethod" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            HardTypeCode="106" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text="Budget Amount"></telerik:RadLabel>
                    </td>
                    <td>
<%--                        <eluc:mask runat="server" ID="txtBudgetAmount" style="text-align: right;" Width="80px" MaskText="##,###,###.##" CssClass="input_mandatory"></eluc:mask>--%>
                    <eluc:Decimal ID = "txtBudgetAmount" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAllowance" runat="server" Text="Allowance" Width="200px"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="ucAllowance" CssClass="input" IsPositive="false" DecimalPlace="3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccess" runat="server" Text="Access" ></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucAccess" HardTypeCode="70" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <font color="blue">
                            <asp:Literal ID="lblNoteBudgetamountshouldbeenteredastheamountforoneyear12months" runat="server" Text="Note: Budget amount should be entered as the amount for one year (12 months)"></asp:Literal></font>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
