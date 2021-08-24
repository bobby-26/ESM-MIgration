<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVoucherLineItemGST.aspx.cs" Inherits="Accounts_AccountsVoucherLineItemGST" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GST Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlStockItemEntry" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                Visible="false" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblVoucherRowNo" runat="server" Text="Voucher/Row No:"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherRow" runat="server" CssClass="input" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblCurrencyAmount" runat="server" Text="Currency/Amount:"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="input" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount:"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAmount" runat="server" CssClass="input" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="true" Width="90px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCompanyState" runat="server" Width="100px" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblbusinessplace" runat="server" Text="Place of Business"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtgstlienid" runat="server" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                        <%--<telerik:RadLabel ID="lblgstdetailsid" CssClass="hidden" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGSTDETAILID") %>'></telerik:RadLabel>--%>
                        <telerik:RadDropDownList runat="server" ID="ddlplaceofbusiness" CssClass="input_mandatory" Width="240px" OnSelectedIndexChanged="ddlplaceofbusiness_Changed" AutoPostBack="true">
                        </telerik:RadDropDownList>
                        <telerik:RadTextBox ID="lblgsttype" runat="server" CssClass="input" ReadOnly="true" Width="200px" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="lblsupplierplace" runat="server" CssClass="input" ReadOnly="true" Width="200px" Visible="false"></telerik:RadTextBox>
                    </td>
                    <%--  <td>
                        <telerik:RadTextBox ID="txtgstno" runat="server" CssClass="input" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>--%>
                    <td>
                        <telerik:RadLabel ID="lblvendorinvoiceno" runat="server" Text="Vendor invoice No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtvendorinvoiceno" runat="server" CssClass="input_mandatory" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblvendorinvoicedate" runat="server" Text="Vendor invoice Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtVoucherDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <br />
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbltaxamount" runat="server" Text="Taxable Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txttaxamount" Width="100px" CssClass="input" runat="server"
                            DecimalPlace="2" IsPositive="true" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <table>
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="GST"></telerik:RadLabel>
                        </b>
                    </td>

                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lbligstpercentage" runat="server" Text="IGST Percentage:"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <eluc:Number ID="txtigstpercentage" Width="100px" CssClass="input" runat="server" MaxLength="6"
                            DecimalPlace="2" IsPositive="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbligstamount" runat="server" Text="IGST Amount:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtigstamount" Width="100px" CssClass="input" runat="server"
                            DecimalPlace="2" IsPositive="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcgstpercentage" runat="server" Text="CGST Percentage:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtcgstpercentage" Width="100px" CssClass="input" runat="server" MaxLength="6"
                            DecimalPlace="2" IsPositive="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcgstamount" runat="server" Text="CGST Amount:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtcgstamount" Width="100px" CssClass="input" runat="server"
                            DecimalPlace="2" IsPositive="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblsgstpercentage" runat="server" Text="SGST Percentage:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtsgstpercentage" Width="100px" CssClass="input" runat="server" MaxLength="6"
                            DecimalPlace="2" IsPositive="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblsgstamount" runat="server" Text="SGST Amount:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtsgstamount" Width="100px" CssClass="input" runat="server"
                            DecimalPlace="2" IsPositive="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblugstpercentage" runat="server" Text="UGST Percentage:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtugstpercentage" Width="100px" CssClass="input" runat="server" MaxLength="6"
                            DecimalPlace="2" IsPositive="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblugstamount" runat="server" Text="UGST Amount:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtugstamount" Width="100px" CssClass="input" runat="server"
                            DecimalPlace="2" IsPositive="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblremark" runat="server" Text="Remark"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtremark" runat="server" TextMode="MultiLine" CssClass="input" Width="300px"></telerik:RadTextBox>
                    </td>

                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
