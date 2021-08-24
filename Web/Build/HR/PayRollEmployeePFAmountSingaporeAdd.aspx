<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollEmployeePFAmountSingaporeAdd.aspx.cs" Inherits="PayRoll_PayRollEmployeePFAmountSingapore" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PayRoll Employee PF Amount Singapore</title>
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
                <td>Employee</td>
                <td>
                    <telerik:RadComboBox DropDownPosition="Static" Style="width: 180px" ID="ddlEmployee" runat="server" EnableLoadOnDemand="True"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>PF Contribution</td>
                <td> <telerik:RadComboBox DropDownPosition="Static" Style="width: 180px" ID="ddlPfcontribution" runat="server" EnableLoadOnDemand="True"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox></td>    
            </tr>
            <tr>
                <td>Month</td>
                <td>
                    <telerik:RadTextBox ID="txtmonth" runat="server" Width="180px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Year</td>
                <td>
                    <eluc:Number ID="txtyear" runat="server" Width="180px"></eluc:Number>
                </td>
            </tr>
            <tr>
                <td>Date</td>
                <td>
                    <eluc:Date ID="ucdate" runat="server" Width="180px"></eluc:Date>
                </td>
            </tr>
            <tr>
                <td>OW Amount</td>
                <td>
                    <eluc:Decimal ID="ucowamt" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            <tr>
                <td>OW CPF Amount</td>
                <td>
                    <eluc:Decimal ID="ucowcptamt" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            <tr>
                <td>OW Employer Amount</td>
                <td>
                    <eluc:Decimal ID="ucowemployeramt" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            <tr>
                <td>OW Employee Amount</td>
                <td>
                    <eluc:Decimal ID="ucowemployeeamt" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            <tr>
                <td>AW Amount</td>
                <td>
                    <eluc:Decimal ID="ucawamt" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            <tr>
                <td>AW CPF Amount</td>
                <td>
                    <eluc:Decimal ID="ucawcpfamt" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            <tr>
                <td>AW Employer Amount</td>
                <td>
                    <eluc:Decimal ID="ucawemployeramt" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            <tr>
                <td>AW Employee Amount</td>
                <td>
                    <eluc:Decimal ID="ucawemployeeamt" runat="server" Width="180px"></eluc:Decimal>
                </td>
            </tr>
            </table>
    </form>
</body>
</html>
