<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGSDLAdd.aspx.cs" Inherits="HR_PayRollSGSDLAdd" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Skills Development Levy  add</title>
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
                    <td >
                        Employer Contribution towards SDL (% of TW)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td >
                        <eluc:Decimal runat="server" ID="rademployercontribution" MaxValue="1" MinValue="0"  CssClass="input_mandatory"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                      Mininimum employer contribution towards SDL (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="radminimum"  MinValue="0" CssClass="input_mandatory"/>
                    </td>
                     </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                <tr>
                    
                    <td>
                        Maximum employer contribution towards SDL (SGD)
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="radmaximum"  MinValue="0" CssClass="input_mandatory"/>
                    </td>
                </tr>
                 
            </tbody>
        </table>
    </form>
</body>
</html>
