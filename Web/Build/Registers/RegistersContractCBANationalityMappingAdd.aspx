<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersContractCBANationalityMappingAdd.aspx.cs" Inherits="Registers_RegistersContractCBANationalityMappingAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationalityList" Src="../UserControls/UserControlNationalityList.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CBA Nationality Mapping</title>
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
        <eluc:TabStrip ID="MenuCBANationalityMapping" runat="server" OnTabStripCommand="MenuCBANationalityMapping_TabStripCommand"></eluc:TabStrip>
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
                        <telerik:RadLabel ID="lblNationality" Text="Nationality" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlNationalityList ID="lstNationality" runat="server" AppendDataBoundItems="true" Width="240px" Height="200px"/>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
