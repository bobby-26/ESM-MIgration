<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersNoonReportRangeConfigVesselMap.aspx.cs" Inherits="RegistersNoonReportRangeConfigVesselMap" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRangeConfig" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>
        <table id="tblRangeConfig">
            <tr>
                <td>
                    <telerik:RadCheckBox ID="chkOption" runat="server" AutoPostBack="true" OnCheckedChanged="chkOption_OnCheckedChanged" Text="Select All" TextAlign="Right" />
                </td>
            </tr>
            <tr>
                <td>
                    <%--<div style="height: 98%; overflow: auto; border: 1px; color: Black; width: 98%;">--%>
                        <telerik:RadCheckBoxList ID="chkVesselList" Height="100%" Width="100%" runat="server"
                             Columns="6" Direction="Vertical" Layout="Flow" AutoPostBack="false">
                        </telerik:RadCheckBoxList>
                    <%--</div>--%>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
