<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCrewBankAccountList.aspx.cs" Inherits="Accounts_AccountsCrewBankAccountList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Bank Account List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmCrewBankAccountList" runat="server" submitdisabledcontrols="true">
       <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <asp:UpdatePanel runat="server" ID="pnlCrewBankAccountListEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
       
               <eluc:TabStrip ID="MenuAccountsBankAccountList" runat="server" OnTabStripCommand="AccountsBankAccountList_TabStripCommand"></eluc:TabStrip>
                   
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblfileno" runat="server" Text="File No."></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtFileno" CssClass="readonlytextbox" Width="80%"
                                    MaxLength="100" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblname" runat="server" Text="Employee"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtEmployee" CssClass="readonlytextbox" Width="80%"
                                    MaxLength="100" ReadOnly="true"></telerik:RadTextBox>
                            </td>

                            <td>
                                <telerik:RadLabel ID="lblrank" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" Width="80%"
                                    MaxLength="100" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAccountType" runat="server" Text="Account Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtAccountType" CssClass="readonlytextbox" Width="80%"
                                    MaxLength="100" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text="Beneficiary"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtAccountName" CssClass="readonlytextbox" Width="80%"
                                    MaxLength="100" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td align="left">
                                <telerik:RadLabel ID="lblAccountNumber" runat="server" Text="Account No."></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtAccountNumber" CssClass="readonlytextbox" Width="80%"
                                    MaxLength="50" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td align="left">
                                <telerik:RadLabel ID="lblBankName" runat="server" Text="Bank Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtBankName" CssClass="readonlytextbox" Width="80%"
                                    MaxLength="100" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td align="left">
                                <telerik:RadLabel ID="lblSwiftCodeSeafarerBank" runat="server" Text="Swift Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtSeafarerBankSwiftCode" CssClass="readonlytextbox" Width="80%"
                                    MaxLength="100" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblIFSCCode" runat="server" Text="IFSC Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtBankIFSCCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBankCurrency" runat="server" Text="Bank Currency"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtBankCurrency" CssClass="readonlytextbox" Width="80%"
                                    MaxLength="50" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text="Verified By"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtEmployeeId" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtVerifiedBy" runat="server" CssClass="readonlytextbox" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text="Verified Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtVerifiedDate" runat="server" CssClass="input_mandatory" />
                            </td>

                        </tr>


                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblInActiveYN" runat="server" Text="InActive YN"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkInActiveYN" runat="server" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblInActiveRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtInActiveRemarks" TextMode="MultiLine" Width="270px" Height="75px"
                                    CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
             
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
