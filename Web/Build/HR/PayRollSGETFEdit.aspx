<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGETFEdit.aspx.cs" Inherits="HR_PayRollSGETFEdit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Shared Group Help Fund  Edit</title>
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
                        Short code
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <telerik:RadTextBox ID="radtbshortcode" runat="server" Width="250px" CssClass="input_mandatory" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Name
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <telerik:RadTextBox ID="radtbname" runat="server" Width="250px" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Description
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <telerik:RadTextBox ID="radtbdescription" runat="server" Width="250px" TextMode="MultiLine" Rows="2"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                 <tr>
                    <td>
                        Race
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                       <eluc:Hard runat="server" ID="ddlhard" hardtypecode="269" CssClass="input_mandatory" />
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
