<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGFWLAdd.aspx.cs" Inherits="HR_PayRollSGFWLAdd" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Foreign Workers Levy  add</title>
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
                        Employee work category
                    </td>
                    <td>'
                        &nbsp
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="radddworkcat" Hardtypecode ="270"  CssClass="input_mandatory"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                    Employee skill level
                        </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="radddskilllevel" Hardtypecode ="271" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                    FWL Tier
                        </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="radtbtier" CssClass="input_mandatory"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                    Minimum total workforce (%) 
                        </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server" CssClass="input_mandatory"  ID="radtbmintw" Width="130px" MinValue="0" MaxValue="100"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                    Maximum total workforce (%) 
                        </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server"   ID="radtbmaxtw" Width="130px" MinValue="0" MaxValue="100"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                    Levy rate Daily (SGD)
                        </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server"   ID="radtblevydaily" Width="130px" MinValue="0"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                    Levy rate Monthly (SGD)
                        </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <eluc:Decimal runat="server"   ID="radtblevymonthly" Width="130px" MinValue="0"/>
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
