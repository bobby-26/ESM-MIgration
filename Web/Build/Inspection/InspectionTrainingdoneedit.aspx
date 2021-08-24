<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionTrainingdoneedit.aspx.cs" Inherits="Inspection_InspectionTrainingdoneedit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:TabStrip ID="Tabstriptrainingschedule" runat="server" OnTabStripCommand="Tabstriptrainingschedule_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px; margin-top: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Due Date">
                        </telerik:RadLabel>
                    </td>
                    <td>&nbsp &nbsp
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="radduedate" CssClass="input_mandatory"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Last Done Date">
                        </telerik:RadLabel>
                    </td>
                    <td>&nbsp &nbsp
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="radlastdonedate"  CssClass="input_mandatory"/>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
