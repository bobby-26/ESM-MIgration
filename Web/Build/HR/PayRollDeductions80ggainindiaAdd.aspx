<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollDeductions80ggainindiaAdd.aspx.cs" Inherits="PayRoll_PayRollDeductions80ggainindia" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deduction 80GGA</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
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
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"  Enabled="false">
                    </telerik:RadComboBox>
                </td>

            </tr>
            <tr>
                <td>Name</td>
                <td>
                    <telerik:RadTextBox ID="txtnameofdone" runat="server" Width="180px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td>Address</td>
                <td>
                    <telerik:RadTextBox ID="txtaddress" runat="server" Width="180px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td>City</td>
                <td>
                    <telerik:RadTextBox ID="txtcity" runat="server" Width="180px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Country</td>
                <td>
                    <eluc:Country runat="server" ID="ddlcountry" AppendDataBoundItems="true"
                        AutoPostBack="true" Width="180px" OnTextChangedEvent="ddlCountry_Changed" />
                </td>
            </tr>
            <tr>
                <td>State</td>
                <td>
                    <eluc:State runat="server" ID="ucState" AppendDataBoundItems="true"
                        AutoPostBack="true" Width="180px" />
                </td>
            </tr>
            <tr>
                <td>Pin Code</td>
                <td>
                    <telerik:RadTextBox ID="txtpincode" runat="server" MaxLength="6" Width="180px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td>Pan Number</td>
                <td>
                    <telerik:RadTextBox ID="txtpannumber" runat="server" MaxLength="10" Width="180px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td>Eligible Amount</td>
                <td>
                    <eluc:Decimal ID="txteliamt" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            <tr>
                <td>Amount</td>
                <td>
                    <eluc:Decimal ID="txtamount" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            <tr>
                <td>Percentage</td>
                <td>
                    <eluc:Decimal ID="txtpercentage" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
