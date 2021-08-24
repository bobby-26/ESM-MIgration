<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCashRequestSupplier.aspx.cs"
    Inherits="VesselAccountsCashRequestSupplier" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlVesselMappingCurrency.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="MultiVesselSupplier" Src="~/UserControls/UserControlMultiColumnVesselSupplier.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cash Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:TabStrip ID="MenuCTM" runat="server" OnTabStripCommand="MenuCTM_TabStripCommand"></eluc:TabStrip>

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>

                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%;">
                        <eluc:Date ID="txtDate" runat="server" CssClass="input" />
                    </td>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td style="width: 45%;">
                        <eluc:MultiPort ID="ucPort" runat="server" CssClass="input_mandatory" Width="330px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtETA" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPortAgent" runat="server" Text="Port Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiVesselSupplier ID="ucVesselSupplier" runat="server" CssClass="input_mandatory" Width="330px" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblETD" runat="server" Text="ETD"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtETD" runat="server" CssClass="input_mandatory" />
                    </td>
                    <%--   <td>
                                        <telerik:RadLabel ID="lblcurrency" runat="server" Text="Requested Currency"></telerik:RadLabel>
                                    </td>
                                    <td>
                                     <eluc:Currency ID="ddlCurrency" runat="server" CssClass="input_mandatory"
                                            AutoPostBack="true" Width="135px"
                                            OnTextChangedEvent="ddlCurrency_SelectedIndexChanged" />
                                    </td>--%>
                </tr>

            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
