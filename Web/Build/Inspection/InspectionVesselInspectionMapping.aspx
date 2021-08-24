<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVesselInspectionMapping.aspx.cs" Inherits="InspectionVesselInspectionMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vetting Configuration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTemplateMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title ID="ucTitle" runat="server" Text="Vetting Configuration" ShowMenu="true"  Visible="false"/>
            <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand"></eluc:TabStrip>
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory" Width="200px"
                            VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_changed" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Literal ID="lblInspection" runat="server" Text="Inspection"></asp:Literal>
                    </td>
                    <td>
                        <div id="divInspection" runat="server" class="input" style="overflow-y: auto; overflow-x: auto; width: 50%; height: 100%">
                            <telerik:RadCheckBoxList ID="cblInspection" runat="server" Columns="3"
                                RepeatColumns="3">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

