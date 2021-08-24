<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionNoonReportListFilter.aspx.cs"
    Inherits="VesselPosition_VesselPositionNoonReportListFilter" %>

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
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Noon Report Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlPlanReliever" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="NoonReportFilterMain" runat="server" OnTabStripCommand="NoonReportFilterMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel runat="server" ID="pnlPlanReliever">
            <table cellpadding="2" cellspacing="2" width="100%">

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" VesselsOnly="true" SyncActiveVesselsOnly="True" AssignedVessels="true" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlVessel_Changed" />
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
                    <td colspan="2">
                        <asp:Panel ID="Panel3" runat="server" GroupingText="Period of Report" Width="50%">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblETAFromDate" runat="server" Text="ETA From Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtETAFrom" runat="server" CssClass="input" DatePicker="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblETAToDate" runat="server" Text="ETA To Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtETATo" runat="server" CssClass="input" DatePicker="true" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="Panel2" runat="server" GroupingText="Port" Width="50%">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPortFrom" runat="server" Text="At Port"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:MultiPort ID="UcPortfrom" runat="server" CssClass="readonlytextbox" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPortTo" runat="server" Text="Next Port"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:MultiPort ID="UcPortTo" runat="server" CssClass="readonlytextbox" Width="300px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
