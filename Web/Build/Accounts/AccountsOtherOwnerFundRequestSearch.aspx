<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOtherOwnerFundRequestSearch.aspx.cs" Inherits="AccountsOtherOwnerFundRequestSearch" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Debit/Credit Note" Visible="false"></asp:Label>
            <eluc:TabStrip ID="MenuFilterMain" runat="server" OnTabStripCommand="MenuFilterMain_TabStripCommand" Title="Debit/Credit Note"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1" style="z-index: 2;">
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <telerik:RadTextBox ID="txtSubject" runat="server"  TextMode="MultiLine" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td width="20%">
                        <telerik:RadLabel ID="lblOpenCloseHeader" runat="server" Text="Open/Close"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <telerik:RadComboBox ID="ddlOpenClose" runat="server" >
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="Open" Text="Open"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="Close" Text="Close"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server"  DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server"  DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNo" runat="server"  Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBillingCompany" runat="server" Text="Billing Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            AutoPostBack="true"  runat="server" AppendDataBoundItems="true"
                            OnTextChangedEvent="ddlBillToCompany_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlType" runat="server"  DataSource='<%# PhoenixRegistersQuick.ListQuick(1, 140)%>'
                            DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" OnDataBound="ddlType_DataBound">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBank" runat="server" Text="Bank receiving funds"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBank" runat="server"  DataTextField="FLDBANKACCOUNTNUMBER"
                            DataValueField="FLDSUBACCOUNTID" OnDataBound="ddlBank_DataBound">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromAmount" runat="server" Text="From Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtFromAmount" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToAmount" runat="server" Text="To Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtToAmount" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                             runat="server" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Received Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="Div2" runat="server" class="input" style="overflow: auto; width: 60%; height: 80px">
                            <asp:CheckBoxList ID="chkStatus" runat="server" Height="100%" OnSelectedIndexChanged="chkStatus_Changed"
                                RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
        </form>
</body>
</html>
