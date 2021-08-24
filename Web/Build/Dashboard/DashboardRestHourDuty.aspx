<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardRestHourDuty.aspx.cs" Inherits="Dashboard_DashboardRestHourDuty" %>

<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:TabStrip ID="MenuRestHour" runat="server" OnTabStripCommand="MenuRestHour_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table border="0" style="width: 100%" runat="server" id="tblActivityEdit">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblStartTime" runat="server" Text="Start Time"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="ddlStartTime" OnItemDataBound="ddlStartTime_ItemDataBound">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPersonel" runat="server" Text="Personnel"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlPersonel" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDEMPLOYEEID"
                        EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
