<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportsSubsidiaryLedgerVessel.aspx.cs"
    Inherits="AccountsReportsSubsidiaryLedgerVessel" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Subsidiary Ledger Vessel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="VesselReport" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="70%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" />
            <eluc:TabStrip ID="MenuVesselReport" runat="server" OnTabStripCommand="MenuVesselReport_TabStripCommand"></eluc:TabStrip>
            <table style="width: 60%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUserAccess" runat="server" Text="User Access Level" Width="100px"></telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadTextBox ID="txtUserAccess" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadTextBox ID="txtCompany" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server" Width="142px" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server" Width="142px" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtType" runat="server" CssClass="readonlytextbox" Text="Reporting Currency"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="4">
                        <font color="blue">Note: If User Access Level is Normal he can see only Normal Accounts.
                                    <br />
                                        If User Access Level is Restricted he can see both Normal and Restricted Accounts.
                                    <br />
                                        If User Access Level is Confidential he can see all Accounts. </font>
                    </td>
                </tr>
            </table>
            <br />
            <table style="width: 32%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" Width="150px" runat="server" Text="Vessel Account Code / Account Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAccountSearch" CssClass="input" MaxLength="10"
                            Width="170px">
                        </telerik:RadTextBox>&nbsp;
                                            <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                                                ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click" ToolTip="Search" />
                    </td>
                </tr>
            </table>
            <table style="width: 80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccount" runat="server" Text="Account"></telerik:RadLabel>
                    </td>
                    <td style="width: 5%"></td>
                    <td>
                        <div runat="server" id="Div1" class="input" style="overflow: auto; width: 50%; height: 223px">
                            <telerik:RadRadioButtonList ID="rblAccount" runat="server" Columns="1" DataBindings-DataTextField="FLDACCOANDEPT"
                                Direction="Horizontal" Layout="Flow" AutoPostBack="True" DataBindings-DataValueField="FLDACCOUNTID">
                            </telerik:RadRadioButtonList>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <table style="width: 62%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSubAccount" runat="server" Text="Sub Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFromAccount" runat="server" Text="From"></telerik:RadLabel>
                        <telerik:RadTextBox ID="txtFromAccount" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToAccount" runat="server" Text="To"></telerik:RadLabel>
                        <telerik:RadTextBox ID="txtToAccount" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherSelection" runat="server" Text="Voucher Selection"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblVoucherSelection" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="All" />
                                <telerik:ButtonListItem Value="1" Text="Billed" />
                                <telerik:ButtonListItem Value="2" Text="Unbilled" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
