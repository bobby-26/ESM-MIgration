<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionArrivalReportFilter.aspx.cs"
    Inherits="VesselPosition_VesselPositionArrivalReportFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Clinic" Src="~/UserControls/UserControlClinic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByUserType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Arrival Report Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlPlanReliever" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="ArrivalReportFilterMain" runat="server" OnTabStripCommand="ArrivalReportFilterMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel runat="server" ID="pnlPlanReliever">
            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" VesselsOnly="true" AutoPostBack="true" SyncActiveVesselsOnly="True"
                                OnTextChangedEvent="ddlVessel_Changed" AssignedVessels="true" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlFleet" runat="server" CssClass="input" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblmVoyage" runat="server" Text="Voyage"></telerik:RadLabel>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <eluc:Voyage ID="ucVoyage" runat="server" AppendDataBoundItems="true" CssClass="input"></eluc:Voyage>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input" AutoPostBack="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Period of Report" Width="50%">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblReportFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="txtReportFrom" runat="server" CssClass="input" DatePicker="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblReportToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="txtReportTo" runat="server" CssClass="input" DatePicker="true" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblArrivalPort" runat="server" Text="Port"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MultiPort ID="UcArrivalPort" runat="server" CssClass="readonlytextbox" Width="300px" />
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
