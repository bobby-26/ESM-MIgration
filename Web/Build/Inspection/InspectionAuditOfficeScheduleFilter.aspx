<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditOfficeScheduleFilter.aspx.cs"
    Inherits="InspectionAuditOfficeScheduleFilter" %>

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
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Audit/Inspection Log Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="DivHeader" runat="server">
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
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Title runat="server" ID="Title1" Text="Office Audit / Inspection Filter" Visible="false" ShowMenu="true"></eluc:Title>
        <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAuditInspectionCategory" runat="server" Text="Audit / Inspection Category"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                        HardTypeCode="144" OnTextChangedEvent="Bind_UserControls" Width="210px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlAudit" runat="server" AutoPostBack="true"
                        OnTextChanged="ucInspection_Changed" Width="210px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                    <eluc:Inspection runat="server" ID="ucAudit" Visible="false" AppendDataBoundItems="true" />
                    <eluc:Hard ID="ucAuditType" runat="server" Visible="false" ShortNameFilter="AUD"
                        AutoPostBack="true" HardTypeCode="148" OnTextChangedEvent="Bind_UserControls" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChapter" runat="server" Text="Chapter"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:IChapter runat="server" ID="ucChapter" AppendDataBoundItems="true"
                        Width="210px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlNCType" runat="server" Width="210px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
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
                    <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRefNo" runat="server" Width="210px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPSCActionCode" runat="server" Text="PSC / VIR Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtKey" runat="server" Width="210px" onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
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
                    <telerik:RadTextBox ID="txtExternalInspector" runat="server" Width="210px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblExternalOrganization" runat="server" Text="External Organization"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtExternalOrganization" runat="server" Width="210px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInternalInspector" runat="server" Text="Internal Inspector"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlInspectorName" runat="server" AutoPostBack="true"
                        Width="210px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="146"
                        ShortNameFilter="CMP,REV,CLD" Width="210px" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
