<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVStockCheckAdd.aspx.cs" Inherits="VesselPositionEUMRVStockCheckAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Tank Sounding Log</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlNoonReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVoyageList">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuStockCheck" runat="server" OnTabStripCommand="MenuStockCheck_TabStripCommand"></eluc:TabStrip>

            <table width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReportDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtReportDate" runat="server" DatePicker="true" CssClass="input_mandatory" />
                        <telerik:RadTimePicker ID="txtReportTime" runat="server" Width="80px" CssClass="input_mandatory" 
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLocation" Text="Location" runat="server"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td colspan="3">
                        <telerik:RadComboBox ID="ddlLocation" runat="server" CssClass="input_mandatory">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="ATSEA" Text="At Sea"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="ATANCHOR" Text="At Anchor"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="INPORT" Text="In Port"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        &nbsp;&nbsp;
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOccasion" runat="server" Text="Occasion"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtOccasion" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDraftf" runat="server" Text="Draft F"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtDraftf" DecimalPlace="2" MaxLength="5" CssClass="input" />
                        &nbsp;<telerik:RadLabel ID="lblDraftfUnit" runat="server" Text="m"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDrafta" runat="server" Text="Draft A"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtDrafta" DecimalPlace="2" MaxLength="5" CssClass="input" />
                        &nbsp;<telerik:RadLabel ID="lblDraatfUnit" runat="server" Text="m"></telerik:RadLabel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblList" runat="server" Text="List"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtList" DecimalPlace="1" MaxLength="4" CssClass="input" />
                        &nbsp;<b><telerik:RadLabel ID="lblListunit" runat="server" Text="˚"></telerik:RadLabel></b>&nbsp;&nbsp;
                            <telerik:RadComboBox ID="ddlPS" runat="server" CssClass="input">
                                <Items>
                                    <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="P" Text="P"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="S" Text="S"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
