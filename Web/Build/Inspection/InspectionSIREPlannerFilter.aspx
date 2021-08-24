<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSIREPlannerFilter.aspx.cs" Inherits="InspectionSIREPlannerFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CDI/SIRE Schedule Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Title runat="server" ID="Title1" Text="CDI / SIRE Schedule Filter" Visible="false" ShowMenu="true"></eluc:Title>
        <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Fleet runat="server" ID="ucTechFleet" Width="270px" AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Owner runat="server" ID="ucAddrOwner" AddressType="128" Width="270px" AppendDataBoundItems="true"
                        AutoPostBack="true" OnTextChangedEvent="ucAddrOwner_Changed" />
                    <eluc:AddressType runat="server" ID="ucCharterer" AddressType="123" Width="270px" AppendDataBoundItems="true"
                        AutoPostBack="true" Visible="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:VesselByOwner runat="server" ID="ucVessel" AppendDataBoundItems="true"
                        VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width="270px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" HardTypeCode="81" Width="270px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVetting" runat="server" Text="Vetting"></telerik:RadLabel>
                </td>
                <td>
                    <%--<eluc:inspection runat="server" id="ucVetting" appenddatabounditems="true" cssclass="input"
                                width="155px" AutoPostBack="true" OnTextChangedEvent="ucVetting_Changed" />--%>
                    <telerik:RadComboBox ID="ucVetting" runat="server" Width="270px" AutoPostBack="true"
                        OnTextChangedEvent="ucVetting_Changed" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                    <eluc:Hard ID="ucAuditType" runat="server" Visible="false" ShortNameFilter="INS"
                        AutoPostBack="true" HardTypeCode="148" OnTextChangedEvent="Bind_UserControls" Width="270px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblBasis" runat="server" Text="Basis"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlBasis" runat="server" AutoPostBack="true" Width="270px"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiPort ID="ucPort" runat="server"
                        Width="270px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblstatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlPlanned" runat="server" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="All" Value="2"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Planned" Value="1"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Not Planned" Value="0"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInspector" runat="server" Text="Inspector"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtInspector" runat="server" Width="270px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlCompany" runat="server" AutoPostBack="true"
                        Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDueFrom" runat="server" Text="Due From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFrom" runat="server" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDueTo" runat="server" Text="Due To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtTo" runat="server" DatePicker="true" />
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadLabel ID="lblPlannedFrom" runat="server" Text="Planned From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDoneFrom" runat="server" DatePicker="true" Visible="false" />
                    <eluc:Date ID="ucPlannedFrom" runat="server" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPlannedTo" runat="server" Text="Planned To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDoneTo" runat="server" DatePicker="true" Visible="false" />
                    <eluc:Date ID="ucPlannedTo" runat="server" DatePicker="true" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
