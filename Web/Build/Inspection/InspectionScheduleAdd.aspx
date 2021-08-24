<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionScheduleAdd.aspx.cs" Inherits="Inspection_InspectionScheduleAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITopic" Src="~/UserControls/UserControlInspectionTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Checklist" Src="~/UserControls/UserControlInspectionChecklist.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersInspectionIncident" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionIncidentEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="ucTitle" Text="Add" ShowMenu="false"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuInspectionSchedule" runat="server" OnTabStripCommand="MenuInspectionSchedule_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureInspectionIncident" width="100%">
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference Number"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:TextBox runat="server" ID="txtRefNo" CssClass="readonlytextbox" MaxLength="200"
                                    Enabled="false"></asp:TextBox>
                            </td>
                            <td width="20%">
                               <asp:Literal ID="lbLLastDoneDate" runat="server" Text="Last Done Date"></asp:Literal>
                            </td>
                            <td width="30%">
                                <%--<asp:TextBox ID="txtLastDoneDate" runat="server" CssClass="input_mandatory" MaxLength="200"
                                    Width="128px"></asp:TextBox>--%>
                                <%--<eluc:Date ID="txtLastDoneDate" runat="server" CssClass="input_mandatory" />--%>
                                <eluc:Date ID="txtLastDoneDate" runat="server" Width="90px" CssClass="input" DatePicker="true" />
                                <%--<asp:TextBox ID="txtLastDoneDate" runat="server" Width="90px" CssClass="input_mandatory"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ceRecivedDate" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtLastDoneDate" PopupPosition="TopRight">
                                </ajaxToolkit:CalendarExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <eluc:Hard runat="server" ID="ucInspectionType" CssClass="dropdown_mandatory" Visible="false"
                                    AppendDataBoundItems="true" HardTypeCode="148" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Hard ID="ucInspectionCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    CssClass="dropdown_mandatory" HardTypeCode="144" OnTextChangedEvent="InspectionType_Changed" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblLastPortOfVetting" runat="server" Text="Last Port of Vetting"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Port ID="ucLastPort" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    Width="280px" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                               <asp:Literal ID="lblVetting" runat="Server" Text="Vetting"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:DropDownList ID="ddlInspectionShortCodeList" runat="server" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblDueDate" runat="server" Text="Due Date"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Hard ID="ucDoneType" runat="server" CssClass="input_mandatory" HardTypeCode="5"
                                    Visible="false" />
                                <eluc:Date ID="txtDueDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    CssClass="dropdown_mandatory" VesselsOnly="true" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblwindowPeriod" runat="server" Text="Window Period ( in days )"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Number ID="txtWindowperiod" runat="server" CssClass="input" MaxLength="3" IsPositive="true"
                                    Width="45px"></eluc:Number>
                                <eluc:Hard ID="Hard1" Visible="false" runat="server" AppendDataBoundItems="false"
                                    CssClass="input" HardTypeCode="7" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" Rows="2" TextMode="MultiLine"
                                    Width="240px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr style="height: -15px" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblPlannedDate" runat="server" Text="Planned Date"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Date ID="txtDateRangeFrom" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblPortofAuditInspection" runat="server" Text="Port of Audit / Inspection"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Port ID="ucPort" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    Width="70%" />
                                &nbsp;<asp:CheckBox ID="chkatsea" runat="server" AutoPostBack="true" OnCheckedChanged="chkatsea_CheckedChanged"
                                    Text="At Sea" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                &nbsp;
                            </td>
                            <td width="30%">
                                &nbsp;
                            </td>
                            <td width="20%">
                                &nbsp;
                            </td>
                            <td width="30%">
                                <asp:RadioButtonList ID="rblLocation" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Text="At Berth" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="At Anchorage" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td width="10%">
                                <asp:Literal ID="lblFromPort" runat="server" Text="From Port"></asp:Literal>
                            </td>
                            <td width="25%">
                                <eluc:Port ID="ucFromPort" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Width="280px" Enabled="false" />
                            </td>
                            <td width="10%">
                                <asp:LIteral ID="lblToPort" runat="server" Text="To Port"></asp:LIteral>
                            </td>
                            <td width="10%">
                                <eluc:Port ID="ucToPort" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Width="280px" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                M<asp:Literal ID="lblETA" runat="server" Text="ETA"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Date ID="txtETA" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblETD" runat="server" Text="ETD"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Date ID="txtETD" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" colspan="4">
                                <asp:Panel ID="Internal" runat="server" GroupingText="Internal Auditor / Inspector"
                                    Width="100%">
                                    <table>
                                        <tr>
                                            <td width="19%">
                                               <asp:Literal ID="lblName" runat="server" Text=" Name &amp; Designation&nbsp;"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                <asp:DropDownList ID="ddlInspectorName" runat="server" CssClass="input" AutoPostBack="true"
                                                    Width="280px" />
                                            </td>
                                            <td width="20%">
                                                <asp:Literal ID="lblOrganization" runat="server" Text="Organization"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                &nbsp;&nbsp;
                                                <asp:TextBox ID="txtOrganization" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                     Width="90px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" colspan="4">
                                <asp:Panel ID="External" runat="server" GroupingText="External Auditor / Inspector"
                                    Width="100%">
                                    <table>
                                        <tr>
                                            <td width="19%">
                                                <asp:Literal ID="lblNameDesignation" runat="server" Text="Name &amp; Designation"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                <asp:DropDownList ID="ddlExternalInspectorName" runat="server" CssClass="input" AutoPostBack="true"
                                                    OnTextChanged="ExtrenalInspector" Width="280px" />
                                            </td>
                                            <td width="19%">
                                               <asp:Literal ID="lblOrganisation" runat="server" Text="Organization"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:TextBox ID="txtExternalOrganisationName" runat="server" CssClass="readonlytextbox"
                                                    ReadOnly="true" Width="90px"></asp:TextBox>
                                                <asp:Label ID="lblExternalOrganisationId" runat="server" Visible="false" />
                                                <asp:DropDownList ID="ddlExternalOrganisation" runat="server" CssClass="input" AutoPostBack="true"
                                                    Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="19%">
                                               <asp:Literal ID="lblInternalAuditor" runat="server" Text="Internal Auditor"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                <asp:DropDownList ID="ddlAuditorName" runat="server" CssClass="input" AutoPostBack="true"
                                                    Width="280px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
