<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsERMVoucherFilter.aspx.cs"
    Inherits="AccountsERMVoucherFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>

       
                <div id="divFind">
                    <table width="75%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblEsmBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtEsmBudgetCode" MaxLength="200" 
                                    Width="150px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtOwnerBudgetCode" MaxLength="200" 
                                    Width="150px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVoucherDateFrom" runat="server" Text="Voucher Date From"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="txtVoucherDateFrom" runat="server"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVoucherDateTo" runat="server" Text="Voucher Date To"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="txtVoucherDateTo" runat="server"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<telerik:RadTextBox runat="server" ID="txtAccountCode" MaxLength="200" 
                                Width="150px"></telerik:RadTextBox>--%>
                                <span id="spnPickListCreditAccount">
                                    <telerik:RadTextBox ID="txtAccountCode" runat="server" 
                                        MaxLength="20" Width="150px"></telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="txtCreditAccountDescription" runat="server" CssClass="hidden" Enabled="false"
                                        MaxLength="50" Width="0px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCreditAccountId" runat="server" CssClass="hidden" MaxLength="20"
                                        Width="0px"></telerik:RadTextBox>
                                </span>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblDebitNoteReference" runat="server" Text="Debit Note Reference"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtDebitNoteReference" MaxLength="200" 
                                    Width="150px"></telerik:RadTextBox>
                            </td>
                        </tr>

                    </table>
                </div>
           
    </form>
</body>
</html>
