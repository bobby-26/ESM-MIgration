<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollIncomesourcesindiaAdd.aspx.cs" Inherits="PayRollIncomesourcesindia" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Income Source</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
   
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <table>
            <tr>
                <td>Payroll Tax</td>
                <td>
                    <telerik:RadComboBox DropDownPosition="Static" Style="width: 180px" ID="ddltax" runat="server" EnableLoadOnDemand="True"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>

            </tr>
            <tr>
                <td>Employer</td>
                <td>
                    <telerik:RadComboBox DropDownPosition="Static" Style="width: 180px" ID="ddlemployer" runat="server" EnableLoadOnDemand="True"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>

            </tr>
            <tr>
                <td>Employee</td>
                <td>
                    <telerik:RadComboBox DropDownPosition="Static" Style="width: 180px" ID="ddlemploye" runat="server" EnableLoadOnDemand="True"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" Enabled="false">
                    </telerik:RadComboBox>
                </td>

            </tr>

            <tr>
                <td>Income Head</td>
                <td>
                    <telerik:RadTextBox ID="txtincomehead" runat="server" Width="180px"></telerik:RadTextBox></td>

            </tr>
            <tr>
                <td>Amount</td>
                <td>
                    <eluc:Decimal ID="txtamount" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
