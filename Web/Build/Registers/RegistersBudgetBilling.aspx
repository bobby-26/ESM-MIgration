<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBudgetBilling.aspx.cs" Inherits="RegistersBudgetBilling" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="split" Src="~/usercontrols/usercontrolsplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegisterBudgetBilling" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlBudgetCodeEntry">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" Visible="false" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuBudgetBilling" runat="server" OnTabStripCommand="BudgetBilling_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 10%">
                        <asp:Literal ID="lblBillingItemDescription" runat="server" Text="Billing Item Description"></asp:Literal>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtBillingItemName" MaxLength="100" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                        <telerik:RadLabel runat="server" ID="lblBudgetbillingid" Visible="false"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <asp:Literal ID="lblFrequency" runat="server" Text="Frequency"></asp:Literal>
                    </td>
                    <td style="width: 20%">
                        <eluc:Hard ID="ucFrequency" runat="server" AppendDataBoundItems="true" OnTextChangedEvent="ucFrequency_Changed"
                            AutoPostBack="true" CssClass="dropdown_mandatory" HardTypeCode="188" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <asp:Literal ID="lblBillingBasis" runat="server" Text="Billing Basis"></asp:Literal>
                    </td>
                    <td style="width: 20%">
                        <eluc:Hard ID="ucBillingBasis" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Width="180px"
                            CssClass="input" HardTypeCode="189" OnTextChangedEvent="ucBillingBasis_Changed" />
                    </td>
                    <td style="width: 10%">
                        <asp:Literal ID="lblBillingUnit" runat="server" Text="Billing Unit"></asp:Literal>
                    </td>
                    <td style="width: 20%">
                        <eluc:Hard ID="ucBillingUnit" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            CssClass="dropdown_mandatory" HardTypeCode="187" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <asp:Literal ID="lblVesselBudgetCode" runat="server" Text="Vessel Budget Code"></asp:Literal>
                    </td>
                    <td style="width: 25%">
                        <span id="spnPickListBudgetCode">
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" CssClass="input" MaxLength="20" Enabled="false"
                                Width="50px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetCodeDescription" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="130px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowBudgetCode" runat="server" OnClientClick="return showPickList('spnPickListBudgetCode', 'codehelp1', '', '../Common/CommonPickListBudget.aspx',true); ">
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtBudgetCodeId" runat="server" CssClass="input" MaxLength="20"
                                Width="10px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td style="width: 10%">
                        <asp:Literal ID="lblCreditAccount" runat="server" Text="Credit Account"></asp:Literal>
                    </td>
                    <td style="width: 25%">
                        <span id="spnPickListCreditAccount">
                            <telerik:RadTextBox ID="txtCreditAccountCode" runat="server" CssClass="input" Enabled="false"
                                MaxLength="20" Width="50px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtCreditAccountDescription" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="130px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowAccount" runat="server" OnClientClick="return showPickList('spnPickListCreditAccount', 'codehelp1', '', '../Common/CommonPickListCompanyAccount.aspx',true); ">
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtCreditAccountId" runat="server" CssClass="input" MaxLength="20"
                                Width="10px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblrank" runat="server" Text="Ranks"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="chkRankList" runat="server" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                            DataTextField="FLDRANKNAME" DataValueField="FLDRANKID"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" Width="200px">
                        </telerik:RadComboBox>
                    </td>

                </tr>
            </table>
            <%--          </hr>
           </div>--%>
        </telerik:RadAjaxPanel>

        <%--            <telerik:RadSplitter runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
