<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsUpdateVoucherListUsingTemplate.aspx.cs" Inherits="Accounts_AccountsUpdateVoucherListUsingTemplate" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Voucher List - Template</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <br />
        <br />
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" CssClass="input" AutoPostBack="true"
                        AppendDataBoundItems="true" Width="240px" OnTextChangedEvent="ucPrincipal_TextChangedEvent" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselaccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList ID="ddlaccount" runat="server" AutoPostBack="true" Width="250px" OnSelectedIndexChanged="ddlaccount_SelectedIndexChanged"></telerik:RadDropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblfromdate" runat="server" Text="From Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />

                </td>

                <td>
                    <telerik:RadLabel ID="lbltodate" runat="server" Text="To Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    <telerik:RadTextBox ID="txtfromdate" runat="server" CssClass="hidden"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtplus62days" runat="server" CssClass="hidden"></telerik:RadTextBox>
                </td>

            </tr>
        </table>
        <br />
        <eluc:TabStrip ID="MenuExcelUpload" runat="server" OnTabStripCommand="MenuExcelUpload_TabStripCommand"></eluc:TabStrip>
        <asp:Button ID="confirm" runat="server" CssClass="hidden" Text="confirm" OnClick="confirm_Click" />
    </form>
</body>
</html>

