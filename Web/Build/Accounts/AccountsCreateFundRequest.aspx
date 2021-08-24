<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreateFundRequest.aspx.cs" Inherits="AccountsCreateFundRequest" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cash Out Paid Date</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCashOutDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
<%--                <eluc:Title runat="server" ID="Title1" Text="Create Fund Request" ShowMenu="false"></eluc:Title>--%>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:TabStrip ID="MenuSave" runat="server" OnTabStripCommand="MenuSave_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Addressee"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListSupplier">
                            <telerik:RadTextBox ID="txtSupplierCode" runat="server" Width="60px" ></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSupplierName" runat="server" BorderWidth="1px" Width="275px"
                                CssClass="readonlytextbox"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnPickSupplier" runat="server"
                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                Enabled="false" />
                            <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1" ></telerik:RadTextBox>
                        </span>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblDebitnoteNoReferenceno" runat="server" Text="Debit note No/Reference no"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDebitnoteNoReferenceno" runat="server" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblbankdetail" runat="server" Text="Banking Details </br> (Bank Receiving Funds)"></telerik:RadLabel>
                    </td>

                    <td>
                        <%-- <span id="spnPickListBank">
                            <telerik:RadTextBox ID="txtAccountNo" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="60px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBankName" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="180px"></telerik:RadTextBox>
                            <img id="imgBankPicklist" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtBankID" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>--%>
                        <telerik:RadComboBox ID="ddlBank" runat="server"  Width="240px">
                        </telerik:RadComboBox>
                    </td>
                    <td>

                        <telerik:RadLabel ID="lbldate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucDate" runat="server"  ReadOnly="false" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>

                        <telerik:RadLabel ID="lblcurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcurrencyid" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadTextBox runat="server" ID="txtcurrency"  Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>

                    <td rowspan="3">
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td rowspan="3">
                        <telerik:RadTextBox runat="server" ID="txtDescription" TextMode="MultiLine" Width="270px" Height="75px"
                            ></telerik:RadTextBox>
                    </td>
                    <td>

                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblbilltocompany" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSubject" Width="270px" Height="75px"
                             TextMode="MultiLine"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
    </form>
</body>
</html>
