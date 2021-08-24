<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOfficeDashboardShipBoardTasksFilterExtn.aspx.cs" Inherits="InspectionOfficeDashboardShipBoardTasksFilterExtn" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vesseltype" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="addresstype" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task Filter</title>
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
        <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
        <div id="divFind">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFleet" runat="server" DataTextField="FLDFLEETDESCRIPTION" DataValueField="FLDFLEETID" AutoPostBack="true"
                            EmptyMessage="Type to select fleet" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            OnItemChecked="ddlFleet_ItemChecked" Width="274px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Owner ID="ucOwner" runat="server" EmptyMessage="Type to select rank" Filter="Contains" Width="274px" AddressType='<%# ((int)PhoenixAddressType.PRINCIPAL).ToString() %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="274px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                    </td>
                    <td>
                        <eluc:vesseltype ID="ucVesselType" runat="server" EmptyMessage="Type to select rank" Filter="Contains" Width="274px" />
                    </td>
                </tr>
                <tr id="rowinspection" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblInspectionType" runat="server" Text="Inspection Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucInspectionType" runat="server" Width="274px" AppendDataBoundItems="true"
                            HardTypeCode="148" AutoPostBack="true" OnTextChangedEvent="ucInspectionType_Changed" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInspection" runat="server" Text="Inspection"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Inspection ID="ucInspection" runat="server" Visible="false"
                            AppendDataBoundItems="true" AutoPostBack="true" />
                        <telerik:RadComboBox ID="ddlInspection" runat="server" Width="274px" AutoPostBack="true"
                            OnTextChanged="ucInspection_Changed"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlNCType" runat="server" Width="274px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="NC" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Major NC" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Observation" Value="3"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Hi Risk Observation" Value="4"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblChapter" runat="server" Text="Chapter"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlChapter" runat="server" Width="274px" AutoPostBack="true" DataTextField="FLDCHAPTERNAME" DataValueField="FLDCHAPTERID"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text="Deficiency Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucNonConformanceCategory" runat="server" AppendDataBoundItems="true"
                            Width="274px" QuickTypeCode="47" Visible="true" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblSourceType" runat="server" Text="Source Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSourceType" runat="server" Width="274px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="NC" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="OBS" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="INC" Value="3"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="OFFICE TASK" Value="4"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="PREV TASK" Value="5"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="MDG" Value="6"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="Target From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFrom" runat="server" /> - <eluc:Date ID="ucTo" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSourceReferenceNo" runat="server" Text="Source Reference number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSourceRefNo" Width="274px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

