<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersContractCBASubCompList.aspx.cs"
    Inherits="RegistersContractCBASubCompList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Budget" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sub Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCity" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuContract" runat="server" OnTabStripCommand="Contract_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUnion" runat="server" Text="Union"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtUnion" runat="server" CssClass="readonlytextbox" Enabled="false" ReadOnly="true"
                            Width="400px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHeaderMainCompCode" runat="server" Text="Component Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMainCompCode" runat="server" CssClass="readonlytextbox" Width="200px" Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHeaderMainCompName" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMainCompName" runat="server" CssClass="readonlytextbox" Enabled="false" Width="200px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtShortCode" runat="server" CssClass="input_mandatory" MaxLength="5"
                            ToolTip="Enter Code" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="100"
                            ToolTip="Enter Name" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ddlCurrency" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblESMCompanyAccruing" runat="server" Text="Company Accruing"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ddlCompany" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplierPayable" runat="server" Text="Supplier Payable"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ddlSupplier" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AddressType="531" Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplierPayableBasis" runat="server" Text="Supplier Payable Basis"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlSupplierBasis" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            HardTypeCode="112" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCalculationUnitBasis" runat="server" Text="Calculation Unit Basis"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlCalUnitBasis" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            HardTypeCode="113" Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCalculationTimeBasis" runat="server" Text="Calculation Time Basis"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlCalTimeBasis" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            HardTypeCode="114" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Posting Budget Code"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <eluc:Budget ID="ddlBudget" runat="server" AppendDataBoundItems="true" CssClass="input"
                            Width="200px" />
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblChargingBudgetCode" runat="server" Text="Charging Budget Code"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <eluc:Budget ID="ddlChargingBudget" runat="server" AppendDataBoundItems="true" CssClass="input"
                            Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSortOrder" runat="server" Text="Sort Order"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucSortOrder" runat="server" CssClass="input txtNumber" Width="60px" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
