<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCElementsAdd.aspx.cs" Inherits="Dashboard_DashboardBSCElementsAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Strategy Elements</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvPIlist" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <div style="margin-left: 0px">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:TabStrip ID="Tabstripaddmenu" runat="server" OnTabStripCommand="Tabstripaddmenu_TabStripCommand"
                    TabStrip="true" />
                <br />
                <table style="margin-left: 20px">
                    <tr>
                        <th>
                            <telerik:RadLabel runat="server" Text="Perspective" />
                        </th>
                        <td>
                            &nbsp &nbsp
                        </td>
                        <th>
                            <telerik:RadComboBox runat="server" AllowCustomText="true" Enabled="false" Width="290px" ID="radcbperspective" />
                        </th>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <telerik:RadLabel runat="server" Text="Theme" />
                        </th>
                        <td>
                            &nbsp &nbsp
                        </td>
                        <th>
                            <telerik:RadComboBox runat="server" AllowCustomText="true" Enabled="false" Width="290px" id="radcbtheme" />
                        </th>
                    </tr>
                     <tr>
                        <td colspan="3">
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <telerik:RadLabel runat="server" Text="KPI" />
                        </th>
                        <td>
                            &nbsp &nbsp
                        </td>
                        <td>
                             <telerik:RadComboBox runat="server" AllowCustomText="true" EmptyMessage="Type to Select KPI " Width="290px" id="radcobkpi" CheckBoxes="true"/>
                          
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
