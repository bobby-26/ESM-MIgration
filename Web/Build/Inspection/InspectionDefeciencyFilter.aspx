<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDefeciencyFilter.aspx.cs"
    Inherits="InspectionDefeciencyFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDefeciencyFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuDefeciencyFilter" runat="server" OnTabStripCommand="MenuDefeciencyFilter_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:fleet ID="ucFleet" runat="server" Width="240px" AppendDataBoundItems="true"></eluc:fleet>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType runat="server" ID="ucAddrOwner" AddressType="128" Width="240px"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucAddrOwner_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByOwner runat="server" ID="ucVessel" Width="240px" AppendDataBoundItems="true"
                            VesselsOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="240px"
                            HardTypeCode="81" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblINspectionType" runat="server" Text="Inspection Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucInspectionType" runat="server" Width="240px" AppendDataBoundItems="true"
                            HardTypeCode="148" AutoPostBack="true" OnTextChangedEvent="ucInspectionType_Changed" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInspectionCategory" runat="server" Text="Inspection Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucInspectionCategory" runat="server" Width="240px"
                            AppendDataBoundItems="true" HardTypeCode="144" AutoPostBack="true" OnTextChangedEvent="ucInspectionCategory_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInspection" runat="server" Text="Inspection"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Inspection ID="ucInspection" runat="server" Visible="false" 
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucInspection_Changed" />
                        <telerik:RadComboBox ID="ddlInspection" runat="server" Width="240px" 
                            AutoPostBack="true" OnTextChanged="ucInspection_Changed" Filter="Contains" >
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblChapter" runat="server" Text="Chapter"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:IChapter runat="server" ID="ucChapter" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlNCType" runat="server" Width="240px" Filter="Contains" >
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                <telerik:RadComboBoxItem Text="NC" Value="2" />
                                <telerik:RadComboBoxItem Text="Major NC" Value="1" />
                                <telerik:RadComboBoxItem Text="Observation" Value="3" />
                                <telerik:RadComboBoxItem Text="Hi Risk Observation" Value="4" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text="Deficiency Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucNonConformanceCategory" runat="server" AppendDataBoundItems="true"
                            Width="240px" QuickTypeCode="47" Visible="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSource" runat="server" Text="Source"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSource" runat="server" Width="240px" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0" Selected="True" />
                                <telerik:RadComboBoxItem Text="Audit/Inspection" Value="1" />
                                <telerik:RadComboBoxItem Text="Vetting" Value="2" />
                                <telerik:RadComboBoxItem Text="Open Reports" Value="3" />
                                <telerik:RadComboBoxItem Text="Direct" Value="4" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSourceReferenceNo" runat="server" Text="Source Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSourceRefNo" runat="server" Width="240px" MaxLength="50"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" Width="240px" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="146"
                            ShortNameFilter="OPN,CLD,CAD,REV" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowOnlyOfficeAuditDeficiencies" runat="server" Text="Show Only Office Audit Deficiencies"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOfficeAuditDeficiencies" runat="server" AutoPostBack="true"
                            OnCheckedChanged="chkOfficeAuditDeficiencies_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" Enabled="false" Width="240px" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lbLPSCActionCode" runat="server" Text="PSC code / VIR Condition"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtKey" runat="server" Width="240px" onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRCA" runat="server" Text="RCA"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="RCA" runat="server" class="input" style="width: 240px;">
                            <telerik:RadCheckBoxList ID="cblRCA" runat="server" Direction="Vertical" Columns="1">
                                <Items>
                                    <telerik:ButtonListItem Text="RCA Required" Value="1" />
                                    <telerik:ButtonListItem Text="RCA Completed" Value="2" />
                                    <telerik:ButtonListItem Text="RCA Pending" Value="3" />
                                </Items>
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
