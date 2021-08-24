<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollTaxRateIndiaAdd.aspx.cs" Inherits="PayRoll_PayRollTaxRateIndia" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PayRoll Tax Slab</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        /*table {
            width: 30%;
        }*/
    </style>
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
                <td>Taxable Year</td>
                <td>
                    <telerik:RadComboBox DropDownPosition="Static" Style="width: 180px" ID="ddlPayroll" runat="server" EnableLoadOnDemand="True"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td> 
            </tr>
            <tr>   
                <td>Slab Minimum</td>
                <td> 
                    <eluc:Number ID="txtSlabMinimum" runat="server" Width="180px" />
                </td>
            </tr>
            <tr>   
                <td>Slab Maximum</td>
                <td>
                    <eluc:Number ID="txtSlabMaximum" runat="server" Width="180px" />
                </td>
            </tr>
            <tr>
                <td>Tax Percent</td>
                <td>
                    <eluc:Number ID="txtPercent" runat="server" Width="180px" />
                </td>
            </tr>
            <tr>
                <td>Previous Tax Slab Amount if any</td>
                <td>
                    <eluc:Number ID="txtPreviousTaxSlab" runat="server" Width="180px" />
                </td>
            </tr>
      <%--      <tr>
                <td>Slab Tax Amount</td>
                <td>
                    <eluc:Number ID="txtTaxAmount" runat="server" Width="180px" />
                </td>
            </tr>
            <tr>
                <td>Gross Tax Payable</td>
                <td>
                    <eluc:Number ID="txtGrossTaxPayable" runat="server" Width="180px" />
                </td>
            </tr>--%>
        </table>
    </form>
</body>
</html>
