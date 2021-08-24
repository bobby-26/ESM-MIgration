<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCBAVesselMappingAdd.aspx.cs" Inherits="Registers_RegistersCBAVesselMappingAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselList" Src="../UserControls/UserControlVesselList.ascx" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CBA Vessel Mapping</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuCBAVesselMapping" runat="server" OnTabStripCommand="MenuCBAVesselMapping_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUnion" runat="server" Text="Union"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ddlUnion" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            AutoPostBack="true" AddressType="134" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" Text="Vessel" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlVesselList ID="ucVessel" runat="server" AppendDataBoundItems="true" Entitytype="VSL"
                            AssignedVessels="true" VesselsOnly="true" Width="240px" Height="200px" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
