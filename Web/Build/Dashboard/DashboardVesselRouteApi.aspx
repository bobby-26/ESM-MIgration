<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVesselRouteApi.aspx.cs" Inherits="Dashboard_DashboardVesselRouteApi" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drills</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       <%-- <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDrilllist.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>--%>
            </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1"  />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="tabmenu" runat="server" OnTabStripCommand="tabmenu_TabStripCommand"
                TabStrip="true" />
        <table>
            <tr>
                <td>
                    Vessel
                </td>
                <td>
                    <telerik:RadDropDownList ID="ddlvessel" runat="server" Width="250px"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    E-mail
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtemail" Width="250px" />
                </td>
            </tr>
        </table>
        <telerik:RadAjaxPanel runat="server" >


        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
