<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsBankStatementExcelUploadFilter.aspx.cs"
    Inherits="AccountsBankStatementExcelUploadFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
<%--            <telerik:RadLabel runat="server" ID="lblCaption" Font-Bold="true" Text="Bank Statement excel upload Filter"></telerik:RadLabel>--%>
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
           <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

    <telerik:RadAjaxPanel runat="server" ID="pnlAddressEntry">
                <table id="tblbankstatement">
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblBankAccount" runat="server" Text="Bank Account"></telerik:RadLabel>
                        </td>
                        <td width="40%">
                            <telerik:RadTextBox ID="txtBankAccount" runat="server" MaxLength="100" CssClass="input"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAccountDescription" runat="server" Text="Account Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAccDesc" runat="server" MaxLength="100" CssClass="input" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Currency runat="server" ID="ucCurrency" Width="150px" ActiveCurrency="true"
                                AppendDataBoundItems="true" />
                        </td>
                        <td width="20%">
                            <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="ddlType"  Width="150px">
                                <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Transactions" Value="TRANSACTIONS"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Bank Charges" Value="BANK CHARGES"></telerik:RadComboBoxItem>
                                    </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard runat="server" ID="ucMonth" AppendDataBoundItems="true" Width="150px"
                                HardTypeCode="55" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblExcludePostedBankStatement" runat="server" Text="Exclude Posted Bank Statement"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkExclPostedBankStmt" runat="server" Checked="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblExcludeArchived" runat="server" Text="Exclude Archived Bank Statement"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkExcludeArchived" runat="server" Checked="true" />
                        </td>
                        <td colspan="2"></td>
                    </tr>
                </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
