<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceOutgoingInvoice.aspx.cs"
    Inherits="AccountsInvoiceOutgoingInvoice" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlEarMarkCompany" Src="~/UserControls/UserControlEarMarkCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Company</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuCompanyList" runat="server" OnTabStripCommand="CompanyList_TabStripCommand"></eluc:TabStrip>
        <table>
            <tr>
                <td width="10%">
                    <telerik:RadLabel ID="lblEarMarkedCompany" runat="server" Text="Ear Marked Company"></telerik:RadLabel>
                </td>
                <td width="20%">
                    <eluc:UserControlEarMarkCompany ID="ddlEarmarkedCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                        OnTextChangedEvent="PopulateInvoice" AutoPostBack="true" CssClass="input" runat="server"
                        AppendDataBoundItems="true" />
                    &nbsp;
                </td>
                <td width="10%">
                    <telerik:RadLabel ID="lblDateofDispatch" runat="server" Text="Date of Dispatch"></telerik:RadLabel>
                </td>
                <td width="20%">
                    <eluc:UserControlDate ID="txtDateofDispatch" runat="server" CssClass="input_mandatory"
                        Width="120" />
                    &nbsp; &nbsp;
                </td>
            </tr>
            <tr>
                <td width="10%">
                    <telerik:RadLabel ID="lblAirWayBillNumber" runat="server" Text="AirWay Bill Number"></telerik:RadLabel>
                </td>
                <td width="20%">
                    <telerik:RadTextBox ID="txtBillNumber" runat="server" CssClass="input_mandatory" MaxLength="100"
                        Width="120"></telerik:RadTextBox>
                </td>
                <td width="10%">
                    <telerik:RadLabel ID="lblPersonPrepared" runat="server" Text='Person prepared'></telerik:RadLabel>
                </td>
                <td width="20%">
                    <telerik:RadTextBox ID="txtPersonPrepared" runat="server" CssClass="readonlytextbox" MaxLength="100"
                        Width="120"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInvoiceList" runat="server" Text="Invoice List"></telerik:RadLabel>
                </td>
                <td>
                    <asp:CheckBoxList ID="CblInvoiceList" runat="server" BorderWidth="2" Width="240">
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
