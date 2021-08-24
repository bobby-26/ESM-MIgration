<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditPurchaseAdvancePayment.aspx.cs"
    Inherits="AccountsCreditPurchaseAdvancePayment" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Projectcode" Src="~/UserControls/UserControlProjectCode.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Credit Purchase Advance Payment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCreditPurchaseAdvancePayment" runat="server" submitdisabledcontrols="true">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <%-- <eluc:Title runat="server" ID="ucTitle" Text="Committ PO" ShowMenu="false"></eluc:Title>--%>
            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand" TabStrip="true"></eluc:TabStrip>


            <eluc:TabStrip ID="MenuGeneralSub" runat="server" OnTabStripCommand="MenuGeneralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <eluc:TabStrip ID="MenuSave" runat="server" OnTabStripCommand="MenuSave_TabStripCommand"></eluc:TabStrip>

              <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" CssClass="hidden" />
       
            <table id="CommittPO" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMainBudget">
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input_mandatory"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input_mandatory"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListOwnerBudget">
                            <telerik:RadTextBox ID="txtOwnerBudgetCode" runat="server" Text="" Enabled="false" MaxLength="20"
                                CssClass="input" Width="240">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOwnerBudgetName" runat="server" Width="0px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtOwnerBudgetId" runat="server" Width="0px" CssClass="input" Text=""></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOwnerBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBilltoCompany" runat="server" Text="Bill to Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBudgetDate" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblAmountInUSD" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblAmountInUSDBudgetCommit" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblRefNo" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblAmount" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblAmountBudgetCommit" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblAddressCode" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblIsSupplierConfirmed" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblOrderDate" runat="server" Visible="false"></telerik:RadLabel>
                        <eluc:Company ID="ddlCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME"
                            DataValueField="FLDACCOUNTID" OnSelectedIndexChanged="ddlAccountDetails_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadDropDownList>
                        <%--OnSelectedIndexChanged="ddlAccountDetails_SelectedIndexChanged" AutoPostBack="true"--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProjectCode" runat="server" Text="Project Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Projectcode ID="ucProjectcode" runat="server" Width="190px" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                </tr>

            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
