<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPurchaseFormAddress.aspx.cs" Inherits="InspectionPurchaseFormAddress" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table align="left" width="100%">
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtName" CssClass="input" Width="90%" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblAddress" runat="server" Text="Address 1:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress1" runat="server" Width="90%" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblAddress2" runat="server" Text="Address 2:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress2" runat="server" Width="90%" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblAddress3" runat="server" Text="Address 3:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress3" runat="server" Width="90%" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblAddress4" runat="server" Text="Address 4:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress4" runat="server" CssClass="input" Width="90%" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" CssClass="input" Enabled="false" Width="180px" />
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblState" runat="server" Text="State:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:State ID="ddlState" CssClass="input" runat="server" AppendDataBoundItems="true" Width="180px" />

                    </td>
                </tr>
                <tr valign="top">

                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country runat="server" ID="ucCountry" AutoPostBack="true" AppendDataBoundItems="true" Enabled="false"
                            CssClass="input" Width="180px" />
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblPostalCode" runat="server" Text="Postal Code:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPostalCode" runat="server" Width="180px" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">

                    <td width="10%">
                        <telerik:RadLabel ID="lblPhone1" runat="server" Text="Phone 1:"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <eluc:PhoneNumber ID="txtPhone1" Width="15%" runat="server" ReadOnly="true" CssClass="input"
                            IsMobileNumber="true" />
                    </td>
                </tr>
                <tr valign="top">
                    <td width="10%">
                        <telerik:RadLabel ID="lblPhone2" runat="server" Text="Phone 2:"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <eluc:PhoneNumber ID="txtPhone2" Width="9%" runat="server" ReadOnly="true" CssClass="input" />
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblFax1" runat="server" Text="Fax 1:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFax1" runat="server" Width="15%" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblFax2" runat="server" Text="Fax 2:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFax2" runat="server" Width="15%" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">

                    <td>
                        <telerik:RadLabel ID="lblEmail1" runat="server" Text="Email 1:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmail1" runat="server" Width="80%" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblEmail2" runat="server" Text="Email 2:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmail2" runat="server" Width="80%" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblWebSiter" runat="server" Text="Web Site:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtURL" runat="server" Width="80%" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
