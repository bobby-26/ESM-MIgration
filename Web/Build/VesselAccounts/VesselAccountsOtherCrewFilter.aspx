<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOtherCrewFilter.aspx.cs"
    Inherits="VesselAccountsOtherCrewFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:TabStrip ID="MenuSignOnOffeFilterMain" runat="server" OnTabStripCommand="MenuSignOnOffeFilterMain_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtName" MaxLength="200" Width="240px" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblACTypeforSeafarers" runat="server" Text="A/C Type for Seafarers"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlAccountType" runat="server" CssClass="input_mandatory" Width="240px">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="" />
                                <telerik:DropDownListItem Text="Staff Account" Value="1" />
                                <telerik:DropDownListItem Text="Owners Account" Value="-1" />
                                <telerik:DropDownListItem Text="Charterers Account" Value="-2" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOnDateBetween" runat="server" Text="Sign On Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOnStartDate" runat="server" />
                        <eluc:Date ID="txtSignOnEndDate" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="SignOffDateBetween" runat="server" Text="Sign Off Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOffStartDate" runat="server" />
                        <eluc:Date ID="txtSignOffEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOnPort" runat="server" Text="Sign On Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ddlSignOnPort" runat="server" CssClass="input" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffPort" runat="server" Text="Sign Off Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ddlSignoffport" runat="server" CssClass="input" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowSignedOff" runat="server" Text="Show Signed Off"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkSignedOff" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
