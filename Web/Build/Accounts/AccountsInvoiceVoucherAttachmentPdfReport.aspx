<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceVoucherAttachmentPdfReport.aspx.cs" Inherits="AccountsInvoiceVoucherAttachmentPdfReport" %>

<!DOCTYPE html >

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Billed Invoices Pdf</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuBtn" runat="server" OnTabStripCommand="MenuBtn_OnTabStripCommand"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlMonth" runat="server" CssClass="input">
                            <Items>
                                <telerik:DropDownListItem Text="January" Value="1"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="February" Value="2"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="March" Value="3"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="April" Value="4"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="May" Value="5"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="June" Value="6"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="July" Value="7"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="August" Value="8"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="September" Value="9"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="October" Value="10"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="November" Value="11"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="December" Value="12"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlYear" runat="server" CssClass="input">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReport" runat="server" Text="Report"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlReportType" runat="server" CssClass="input">
                            <Items>
                                <telerik:DropDownListItem Text="Invoice Report" Value="Invoice"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Vessel Acknowledgement Report" Value="MST Acknowledgement"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Invoice and Vessel Acknowledgement Report" Value="Invoice and MST Acknowledgement"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td id="spnPickListCreditAccount">
                        <telerik:RadTextBox ID="txtCreditAccountCode" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="20" Width="20%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCreditAccountDescription" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="50%">
                        </telerik:RadTextBox>
                        <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtCreditAccountId" runat="server" CssClass="input_mandatory" MaxLength="20"
                            Width="10px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%">
                <tr>
                    <td style="width: 100%; vertical-align: top; border: 1px solid #CCC;">
                        <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 443px; width: 99.5%" frameborder="0"></iframe>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
