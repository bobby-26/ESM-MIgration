<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReviewOfficePlannerFilter.aspx.cs"
    Inherits="InspectionReviewOfficePlannerFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection Office Schedule Filter</title>
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
        <eluc:Title runat="server" ID="Title1" Text="Office Audit / Inspection Filter" ShowMenu="true" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
        <div id="divFind">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAuditInspectionCategory" runat="server" Text="Audit / Inspection Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            HardTypeCode="144" OnTextChangedEvent="Bind_UserControls" Width="280px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompanySelect" runat="server" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            AppendDataBoundItems="true" Width="280px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAudit" runat="server" Width="280px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                        <eluc:Inspection runat="server" ID="ucAudit" Visible="false" AppendDataBoundItems="true"
                          Width="155px" />
                        <eluc:Hard ID="ucAuditType" runat="server" Visible="false" ShortNameFilter="AUD"
                            AutoPostBack="true" HardTypeCode="148" OnTextChangedEvent="Bind_UserControls" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPlanned" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" Width="280px" >
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
                        <telerik:RadLabel ID="lblExternalInspector" runat="server" Text="External Inspector"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtExternalInspector" runat="server" Width="280px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExternalOrganization" runat="server" Text="External Organization"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtExternalOrganization" runat="server" Width="280px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInternalInspector" runat="server" Text="Internal Inspector"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlInspectorName" runat="server" AutoPostBack="true"
                            Width="280px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
