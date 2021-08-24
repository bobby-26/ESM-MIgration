<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollTaxSingaporeAdd.aspx.cs" Inherits="PayRoll_PayRollTaxSingapore" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payroll configuration Add</title>
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
        <br />
        <table>
            <tbody>
             <tr>
                <td>Name</td>
                 <td>&nbsp</td>
                <td colspan="4">
                    <telerik:RadTextBox runat="server" ID="radtbdescription"  Width="100%" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
              <td colspan="6">&nbsp</td>  
            </tr>
            <tr>
                <td>From </td>
                <td>&nbsp</td>
                <td>
                    <eluc:Date ID="ucfromdate" runat="server" Width="180px" CssClass="input_mandatory" />
                </td>
                 <td>&nbsp</td>
                <td>To </td>
                <td>
                    <eluc:Date ID="uctodate" runat="server" Width="180px" />
                </td>
            </tr>
            <tr>
              <td colspan="6">&nbsp</td>  
            </tr>
            <tr>
                <td>Currency</td>
                <td>&nbsp</td>
                <td colspan="4">
                    <eluc:UserControlCurrency ID="radddcurrency" runat="server" Width="180px" CssClass="input_mandatory"></eluc:UserControlCurrency></td>
            </tr>
                <tr>
              <td colspan="6">&nbsp</td>  
            </tr>
                 <tr>
                <td>Revision notes</td>
                <td>&nbsp</td>
                <td colspan="4">
                    <telerik:RadTextBox runat="server" ID="radtbchanges" TextMode="MultiLine" Rows="4" width="100%"  MaxLength="499"/>

                </td>
            </tr>
                </tbody>
        </table>

    </form>
</body>
</html>
