<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionReInitializeCargoOperations.aspx.cs"
    Inherits="VesselPositionReInitializeCargoOperations" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargotype" Src="~/UserControls/UserControlCargoTypeMappedVesselType.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cargo Operations</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCargoOperations" runat="server">
    <telerik:RadScriptManager ID="ToolkitScriptManager1"
        runat="server">
    </telerik:RadScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:TabStrip ID="MenuCargoOperations" runat="server" Title="Cargo Operations" OnTabStripCommand="MenuCargoOperations_TabStripCommand"></eluc:TabStrip>

        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblCargo" runat="server" Text="Cargo"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Cargotype ID="ucCargo" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCargoOperation" runat="server" Text="Cargo Operation"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="ddlCargoOperation" AutoPostBack="true" CssClass="dropdown_mandatory" Enabled="false"
                        AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="Loading" Value="LOAD"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOilMajorCargo" runat="server" Text="Oil Major Cargo"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkOilMajor" runat="server" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td style="width: 15%;">
                    <telerik:RadLabel ID="lblOilmajor" runat="server" Text="Oil major"></telerik:RadLabel>
                </td>
                <td style="width: 25%;">
                    <telerik:RadComboBox runat="server" ID="ddlOilMajor" CssClass="input" AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="BP" Value="BP"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Exxon" Value="EXXON"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Shell" Value="SHELL"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Chevron" Value="CHEVRON"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Total" Value="TOTAL"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Others" Value="OTHERS"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblQuantity" runat="server" Text="Quantity"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="ucQuantity" runat="server" CssClass="input_mandatory" DecimalPlace="2" Width="135px" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblCargoloadedport" runat="server" Text="Cargo Loaded Port"></telerik:RadLabel></td>
                <td>
                    <eluc:Multiport ID="ucPortEdit" runat="server" CssClass="input" Width="300px" />
                </td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblLoadedDate" runat="server" Text="Cargo Loaded Date"></telerik:RadLabel></td>
                <td>
                    <eluc:Date runat="server" ID="txtLoadedDate" CssClass="input" DatePicker="true"/>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCommenced" runat="server" Text="Commenced" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtCommenced" CssClass="input_mandatory" DatePicker="true"  Visible="false"/> 
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCompleted" runat="server" Text="Completed" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtCompleted" CssClass="input_mandatory" DatePicker="true"  Visible="false"/>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
