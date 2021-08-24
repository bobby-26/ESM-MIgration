<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardShipMeanTime.aspx.cs" Inherits="Dashboard_DashboardShipMeanTime" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="../UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />        
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuSMT" runat="server" TabStrip="true" OnTabStripCommand="MenuSMT_TabStripCommand"></eluc:TabStrip>
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblShipMeanTime" runat="server" Text="Ship Mean Time"></telerik:RadLabel>
                </td>
                <td>
                    <b>
                        <telerik:RadLabel ID="ltrlUTC" runat="server" Text="UTC"></telerik:RadLabel>
                    </b>
                    <telerik:RadComboBox runat="server" ID="txtShipMeanTimeSymbol" CssClass="dropdown_mandatory" Width="50px">
                        <Items>
                            <telerik:RadComboBoxItem Text="+" Value="+"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="-" Value="-"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                    <eluc:Decimal runat="server" ID="txtShipMeanTime" CssClass="input_mandatory" DecimalDigits="1" Mask="99.9" Width="50px"></eluc:Decimal>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
