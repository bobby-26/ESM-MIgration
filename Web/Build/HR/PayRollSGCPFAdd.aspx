<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGCPFAdd.aspx.cs" Inherits="HR_PayRollSGCPFAdd" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Central Provident Fund rate add</title>
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
                        Mininimum age
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="radtbminimumage" MaxValue="100" MinValue="0" CssClass="input_mandatory" />
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        Maximum age
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="radtblmaximumage" MaxValue="100" MinValue="0" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                        Mininimum TW (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="radminimumtw"  MinValue="0"  CssClass="input_mandatory"/>
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        Maximum TW (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="radmaximumtw"  MinValue="0" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                       Employer OW contribution (%)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="rademployerow"  MinValue="0"  MaxValue="100"/>
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        Employer AW contribution (%)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="rademployeraw"  MinValue="0" MaxValue="100"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                       Employee OW contribution (%)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="rademployeeow"  MinValue="0"  MaxValue="100"/>
                    </td>
                   
                    <td>
                        &nbsp
                    </td>
                    <td>
                        Employee AW contribution (%)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="rademployeeaw"  MinValue="0" MaxValue="100"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                       Employer OW contribution limit (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="rademployerowlimit"  MinValue="0" />
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        Employee OW contribution limit (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="rademployeeowlimit"  MinValue="0" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        Employee contribution correction factor (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    
                    <td colspan="3">
                         <eluc:Decimal runat="server" ID="rademployeecorrectionfactor"  MinValue="0" />
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
