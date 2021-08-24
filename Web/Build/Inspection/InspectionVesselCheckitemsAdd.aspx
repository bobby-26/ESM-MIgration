<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVesselCheckitemsAdd.aspx.cs" Inherits="InspectionVesselCheckitemsAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Check Items</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="Menuvessel" runat="server" OnTabStripCommand="Menuvessel_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <table runat="server" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                         <eluc:VesselByOwner runat="server" ID="ucVessel" AppendDataBoundItems="true"
                                VesselsOnly="true" Width="270px" />
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
