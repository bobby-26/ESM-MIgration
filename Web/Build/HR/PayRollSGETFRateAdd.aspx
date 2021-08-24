<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGETFRateAdd.aspx.cs" Inherits="HR_PayRollSGETFRateAdd" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SHG fund contribution add</title>
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
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

    <br />
        <table>
            <tbody>
                <tr>
                    <td>
                        SHG Fund Name
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" ID="radlblfundname" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Minimum Total Wages (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="radtbminimumtw" MinValue="0" CssClass="input_mandatory"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                        Maximum Total Wages (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="radtbmaximumtw" MinValue="0" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr> <tr>
                    <td>
                       Monthly Contribution (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="radtbcontribution" MinValue="0" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
