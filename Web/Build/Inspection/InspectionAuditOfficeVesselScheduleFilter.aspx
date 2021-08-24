<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditOfficeVesselScheduleFilter.aspx.cs" Inherits="InspectionAuditOfficeVesselScheduleFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
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
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection Schedule Filter</title>
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
    <form id="frmInspectionScheduleFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <div>
                            <strong>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                            </strong>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="fleetrow">
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
                            AutoPostBack="true" />
                        <eluc:addresstype runat="server" ID="ucCharterer" AddressType="123" Width="270px" AppendDataBoundItems="true"
                            AutoPostBack="true" Visible="false" />
                    </td>
                </tr>
                <tr runat="server" id="vesselrow">
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true"
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
                        <telerik:RadLabel ID="lblAuditInspectionCategory" runat="server" Text="Audit / Inspection Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            HardTypeCode="144" OnTextChangedEvent="Bind_UserControls" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlNCType" runat="server" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="NC" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Major NC" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Observation" Value="3"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Hi Risk Observation" Value="4"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAudit" runat="server" AutoPostBack="true" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"
                            OnTextChanged="ucInspection_Changed" Width="270px">
                        </telerik:RadComboBox>
                        <eluc:Inspection runat="server" ID="ucAudit" Visible="false" AppendDataBoundItems="true" />
                        <eluc:Hard ID="ucAuditType" runat="server" Visible="false" ShortNameFilter="AUD"
                            AutoPostBack="true" HardTypeCode="148" OnTextChangedEvent="Bind_UserControls" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblChaptera" runat="server" Text="Chapter"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:IChapter runat="server" ID="ucChapter" AppendDataBoundItems="true" Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCredited" runat="server" Text="Credited"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="ChkCredited" runat="server" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" Width="270px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPSCActionCode" runat="server" Text="PSC / VIR Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtKey" runat="server" Width="270px" onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPortFrom" runat="server" Text="Port From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucPort" runat="server" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPortTo" runat="server" Text="Port To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucPortTo" runat="server" Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastDoneDateFrom" runat="server" Text="Last Done From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDoneDateFrom" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDoneDateTo" runat="server" DatePicker="true" />
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
                        <telerik:RadComboBox ID="ddlInspectorName" runat="server" AutoPostBack="true" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"
                            Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="146"
                            ShortNameFilter="CMP,REV,CLD" Width="270px" />
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
                <span id="Office" runat="server">
                    <tr>
                        <td>
                            <div>
                                <strong>
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="Office Audit / Inspection"></telerik:RadLabel>
                                </strong>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOfficeAuditInspectionCategory" runat="server" Text="Audit / Inspection Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucOfficeAuditCategory" runat="server" AppendDataBoundItems="true"
                                AutoPostBack="true" HardTypeCode="144"
                                OnTextChangedEvent="Bind_OfficeUserControls" Width="270px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOfficeAuditInspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlOfficeAudit" runat="server" AutoPostBack="true" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"
                                OnTextChanged="ucOfficeInspection_Changed" Width="270px">
                            </telerik:RadComboBox>
                            <eluc:Inspection runat="server" ID="Inspection1" Visible="false" AppendDataBoundItems="true" />
                            <eluc:Hard ID="ucOfficeAuditType" runat="server" Visible="false" ShortNameFilter="AUD"
                                AutoPostBack="true" HardTypeCode="148"
                                OnTextChangedEvent="Bind_OfficeUserControls" />
                        </td>

                    </tr>
                    <tr>

                        <td>
                            <telerik:RadLabel ID="lblOfficeChapter" runat="server" Text="Chapter"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:IChapter runat="server" ID="ucOfficeChapter" AppendDataBoundItems="true" Width="270px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOfficeDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                        </td>

                        <td>
                            <telerik:RadComboBox ID="ddlOfficeNCType" runat="server" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="NC" Value="2"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Major NC" Value="1"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Observation" Value="3"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Hi Risk Observation" Value="4"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOfficeReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOfficeRefNo" runat="server" Width="270px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOfficePSCActionCode" runat="server" Text="PSC/ VIR Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOfficeKey" runat="server" Width="270px" onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
                        </td>

                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOfficeLastDoneDateFrom" runat="server" Text="Last Done From"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtOfficeDoneDateFrom" runat="server" DatePicker="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOfficeTo" runat="server" Text="To"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtOfficeDoneDateTo" runat="server" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOfficeExternalInspector" runat="server" Text="External Inspector"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOfficeExternalInspector" runat="server" Width="270px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOfficeExternalOrganization" runat="server" Text="External Organization"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOfficeExternalOrganization" runat="server" Width="270px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOfficeInternalInspector" runat="server" Text="Internal Inspector"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlOfficeInspectorName" runat="server" AutoPostBack="true"
                                Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOfficeStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucOfficeStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="146"
                                ShortNameFilter="CMP,REV,CLD" Width="270px" />
                        </td>
                    </tr>
                </span>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
