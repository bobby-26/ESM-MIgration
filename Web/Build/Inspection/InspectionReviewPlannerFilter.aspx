<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReviewPlannerFilter.aspx.cs"
    Inherits="InspectionReviewPlannerFilter" %>

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
    <title>Inspection Schedule Filter</title>
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
        <eluc:Title runat="server" ID="Title1" Text="Audit / Inspection Filter" ShowMenu="false" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
        <div id="divFind">
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
                        <eluc:AddressType runat="server" ID="ucCharterer" AddressType="123" Width="270px"
                            AppendDataBoundItems="true" AutoPostBack="true" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByOwner runat="server" ID="ucVessel" AppendDataBoundItems="true"
                            VesselsOnly="true" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true"
                            HardTypeCode="81" Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAuditInspectionCategory" runat="server" Text=" Audit / Inspection Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            HardTypeCode="144" OnTextChangedEvent="Bind_UserControls" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAudit" runat="server" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                        <eluc:Inspection runat="server" ID="ucAudit" Visible="false" AppendDataBoundItems="true"
                            CssClass="input" Width="270px" />
                        <eluc:Hard ID="ucAuditType" runat="server" Visible="false" ShortNameFilter="AUD"
                            AutoPostBack="true" HardTypeCode="148" OnTextChangedEvent="Bind_UserControls"
                            Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromPort" runat="server" Text="From Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucFromPort" runat="server" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToPort" runat="server" Text="To Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucToPort" runat="server" Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExternalInspector" runat="server" Text="External Inspector"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtExternalInspector" runat="server" Width="270px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExternalOrganization" runat="server" Text="External Organization"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtExternalOrganization" runat="server" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInternalInspector" runat="server" Text="Internal Inspector"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlInspectorName" runat="server" AutoPostBack="true"
                            Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
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
                        <eluc:Date ID="txtPlannedFrom" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlannedTo" runat="server" Text="Planned To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPlannedTo" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAtSea" runat="server" Text="At Sea"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAtSea" runat="server" />
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
