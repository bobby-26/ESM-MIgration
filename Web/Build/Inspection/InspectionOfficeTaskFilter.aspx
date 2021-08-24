<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOfficeTaskFilter.aspx.cs"
    Inherits="InspectionOfficeTaskFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="addresstype" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Office Task Filter</title>
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
        <eluc:Title runat="server" ID="Title1" Text="Office Corrective Task Filter" ShowMenu="true" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
        <div id="divFind">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Width="270px" AppendDataBoundItems="true"
                            AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Owner runat="server" ID="ucAddrOwner" AddressType="128" Width="270px" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ucAddrOwner_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByOwner ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                           VesselsOnly="true" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblvesseltype" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true"
                            HardTypeCode="81" AutoPostBack="true" Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInspectionType" runat="server" Text="Inspection Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucInspectionType" runat="server" AppendDataBoundItems="true"
                            HardTypeCode="148" AutoPostBack="true" OnTextChangedEvent="ucInspectionType_Changed" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInspection" runat="server" Text="Inspection"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Inspection ID="ucInspection" runat="server" Visible="false" 
                            AppendDataBoundItems="true" AutoPostBack="true" />
                        <telerik:RadComboBox ID="ddlInspection" runat="server" AutoPostBack="true"
                            OnTextChanged="ucInspection_Changed" Width="270px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlNCType" runat="server" Width="270px"
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
                        <eluc:IChapter runat="server" ID="ucChapter" AppendDataBoundItems="true" Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text="Deficiency Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucNonConformanceCategory" runat="server" AppendDataBoundItems="true"
                            Width="270px" QuickTypeCode="47" Visible="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbLVIRItem" runat="server" Text="VIR Item"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtItem" runat="server"  Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcarsourcetype" runat="server" Text="Source Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlcarsourcetype" runat="server" Width="270px"
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
                    <td>
                        <telerik:RadLabel ID="lblSourceReferenceNo" runat="server" Text="Source Reference number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcarsourcerefno" runat="server" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcartargetdatefrom" runat="server" Text="Target From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="uccartargetdatefrom" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcartargetdateto" runat="server" Text="Target To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="uccartargetdateto" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcarcompletiondatefrom" runat="server" Text="Completion From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="uccarcompletiondatefrom" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcarcompletiontdatefrom" runat="server" Text="Completion To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="uccarcompletiondateto" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true"
                            HardTypeCode="146" ShortNameFilter="OPN,CMP,CLD" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExcludeVIR" runat="server" Text="Exclude VIR"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkExcludeVIR" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVerificationLevel" runat="server" Text="Verification Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucVerficationLevel" runat="server" AppendDataBoundItems="true"
                            Width="270px" HardList='<%# PhoenixRegistersHard.ListHard(1, 195) %>' HardTypeCode="195" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblShowOnlyRescheduledTasks" runat="server" Text="Show only rescheduled Tasks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkShowRescheduledTasks" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblofficetask" runat="server" Text="Show Only Office Audit Task"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOfficeAudit" runat="server" AutoPostBack="true"
                            OnCheckedChanged="chkOfficeAudit_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCARCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ddlCompany" runat="server" Enabled="false" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            AppendDataBoundItems="true" Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPendingRescheduleTask" runat="server" Text="Pending Reschedule Task"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPendingRescheduleTask" runat="server" />
                    </td>
                </tr>
<%--              <tr>
                    <td colspan="4">
                            <h3><telerik:RadLabel ID="lblPreventive" runat="server" Text="Preventive Tasks"></telerik:RadLabel></h3>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" AutoPostBack="true"
                            Width="270px" OnSelectedIndexChanged="ddlCategory_Changed" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Sub Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubcategory" runat="server" Width="270px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSourceType" runat="server" Text="Source Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSourceType" runat="server" Width="270px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="NC" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="OBS" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="INC" Value="3"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="MDG" Value="4"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="TSK" Value="5"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSourceReferenecNo" runat="server" Text="Source Reference number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSourceRefNo" runat="server" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeparment" runat="server" Text="Assigned Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Department ID="ucDepartment" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                           OnTextChanged="selection_Changed" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAcceptedBy" runat="server" Text="Accepted By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAcceptedBy" runat="server" Width="270px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWONoFrom" runat="server" Text="WO No From"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWONoFrom" runat="server" Width="270px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWONOTo" runat="server" Text="WO No To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWONoTo" runat="server" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTargetDateFrom" runat="server" Text="Target From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFrom" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTargetDateTo" runat="server" Text="Target To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtTo" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompletionDateFrom" runat="server" Text="Completion From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDoneDateFrom" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompletionDateTo" runat="server" Text="Completion To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDoneDateTo" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowOnlyOfficeAuditTask" runat="server" Text="Show Only Office Audit Task"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOfficeAuditDeficiencies" runat="server" AutoPostBack="true"
                            OnCheckedChanged="chkOfficeAuditDeficiencies_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" Enabled="false" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            AppendDataBoundItems="true" Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTaskStatus" runat="server" Text="Task Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAcceptance" runat="server" AppendDataBoundItems="true" Width="270px"
                            AutoPostBack="true" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Open" Value="0" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Accepted" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Completed" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Closed" Value="4"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>--%>
            </table>
        </div>
    </form>
</body>
</html>
