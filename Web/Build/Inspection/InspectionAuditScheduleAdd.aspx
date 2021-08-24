<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditScheduleAdd.aspx.cs" Inherits="InspectionAuditScheduleAdd" %>

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
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">    
    <style type="text/css">
        .style1
        {
            width: 20%;
        }
        .style2
        {
            width: 21%;
        }
        .style3
        {
            width: 22%;
        }
        </style>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        
       <%-- <script type="text/javascript">
            function DisableDiv(divid) {
                var nodes = document.getElementById(divid).getElementsByTagName('*');                
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = true;
                }
            }
            function EnableDiv(divid) {
                var nodes = document.getElementById(divid).getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].disabled = false;
                }
            }
        </script>--%>
   </telerik:RadCodeBlock>
</head>
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
                    <eluc:Title runat="server" ID="ucTitle" Text="Add" ShowMenu="false"></eluc:Title>
                     <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
                </div>                
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuInspectionScheduleGeneral" runat="server"
                        OnTabStripCommand="MenuInspectionScheduleGeneral_TabStripCommand"></eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureInspectionIncident" width="100%">
                        <tr>
                            <td class="style1" width="20%">
                                <asp:Literal ID="lblSerialNo" runat="server" Text="Serial Number"></asp:Literal>
                            </td>
                            <td class="style2" width="30%">
                                <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="readonlytextbox" MaxLength="200"
                                    Width="90px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td class="style1" width="20%">
                               <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference Number"></asp:Literal>
                            </td>
                            <td class="style2" width="30%">
                                <%--<asp:CheckBox ID="chkIsLastDoneDateYN" runat="server" Text="First Audit" AutoPostBack="true" OnCheckedChanged="chkIsLastDoneDateYN_CheckedChanged" /> --%>
                                <asp:TextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" MaxLength="200"
                                    Width="90px" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" 
                                    AutoPostBack="true" CssClass="input_mandatory" OnTextChangedEvent="ucVessel_changed" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblLastPortofAudit" runat="server" Text="Last Port of Audit / Inspection"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Port ID="ucLastPort" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    Width="280px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style3" width="20%">
                                <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                            </td>
                            <td class="style3" width="30%">
                                <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    CssClass="dropdown_mandatory" HardTypeCode="144" OnTextChangedEvent="InspectionType_Changed" />
                                <eluc:Hard ID="ucAuditType" runat="server" AutoPostBack="true" CssClass="input" HardTypeCode="148"
                                    OnTextChangedEvent="InspectionType_Changed" ShortNameFilter="AUD" Visible="false" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblLastDoneDate" runat="server" Text="Last Done Date"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Date ID="txtLastDoneDate" runat="server" CssClass="input" DatePicker="true"
                                    Width="90px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style3" width="20%">
                                <asp:Literal ID="lblExternalAuditType" runat="server" Text="External Audit Type"></asp:Literal>
                            </td>
                            <td class="style3" width="30%">
                                <eluc:Hard ID="ucExternalAuditType" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    CssClass="input" HardList="<%# PhoenixRegistersHard.ListHard(1,190) %>" HardTypeCode="190"
                                    OnTextChangedEvent="ExternalAuditType_Changed" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblDueDate" runat="server" Text="Due Date"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Date ID="txtDueDate" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                <asp:Literal ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="lblInspectionId" runat="server" Visible="false" />
                                <eluc:Inspection ID="ucAudit" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    CssClass="dropdown_mandatory" Visible="false" Width="200px" />
                                <asp:DropDownList ID="ddlAuditShortCodeList" runat="server" AutoPostBack="true" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
                            </td>
                            <td width="20%">
                               <asp:Literal ID="lblWindowPeriod" runat="server" Text="Window Period ( in days )"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Number ID="txtWindowperiod" runat="server" CssClass="input" IsPositive="true"
                                    MaxLength="3" Width="90px" />
                                <eluc:Hard ID="ucwindowperiodtype" runat="server" AppendDataBoundItems="false" CssClass="dropdown_mandatory"
                                    HardTypeCode="7" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:literal ID="lblRemarks" runat="server" Text="Remarks"></asp:literal>
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" Rows="2" TextMode="MultiLine"
                                    Width="280px"></asp:TextBox>
                            </td>
                            <td width="20%">
                                
                            </td>
                            <td width="30%">
                                <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    ShortNameFilter="ASG" CssClass="dropdown_mandatory" HardTypeCode="146" SelectedHard="716"
                                    Visible="false" />
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
                                <asp:Literal ID="lblPortOfAuditInspection" runat="server" Text="Port of Audit / Inspection "></asp:Literal>
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
                                    Width="280px" Enabled="false"  />
                            </td>
                            <td width="10%">
                                <asp:Literal ID="lblToPort" runat="server" Text="To Port"></asp:Literal>
                            </td>
                            <td width="10%">
                                <eluc:Port ID="ucToPort" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Width="280px" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblETA" runat="server" Text="ETA"></asp:Literal>
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
                                <asp:Panel ID="Internal" runat="server" GroupingText="Internal Auditor / Inspector" Enabled="false"
                                    Width="100%" style="table-layout:fixed; display:block;">
                                    <table style="table-layout:fixed; display:block;">
                                        <tr>
                                            <td width="20%">
                                               <asp:Label ID="lblNameDesignationHeader" runat="server"> Name &amp; Designation</asp:Label>
                                            </td>
                                            <td width="30%">
                                                <asp:DropDownList ID="ddlInspectorName" runat="server" CssClass="input" AutoPostBack="true"
                                                    Width="280px" />
                                            </td>
                                            <td width="20%">
                                                &<asp:Label ID="lblOrganizationHeader" runat="server">nbsp; Organization</asp:Label>
                                            </td>
                                           <td width="30%">
                                                &nbsp;<asp:TextBox ID="txtOrganization" runat="server" CssClass="readonlytextbox" ReadOnly="true"
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
                                <asp:Panel ID="External" runat="server" GroupingText="External Auditor / Inspector" Enabled="false"
                                    Width="100%">
                                    <table style="table-layout:fixed; display:block;">
                                         <tr>
                                            <td width="19%">
                                                <asp:Literal ID="lblNameDesignation" runat="server" Text="Name &amp; Designation"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                <%--<asp:DropDownList ID="ddlExternalInspectorName" runat="server" CssClass="input" AutoPostBack="true" 
                                                  OnTextChanged="ExtrenalInspector"  Width="280px"/>--%><asp:TextBox ID="txtExternalInspectorName" runat="server" CssClass="input" Width="160px"></asp:TextBox>
                                                <asp:TextBox ID="txtExternalInspectorDesignation" runat="server" CssClass="input" Width="105px"></asp:TextBox>
                                            </td>
                                            <td width="20%">
                                                <asp:Literal ID="lblOrganization" runat="server" Text="&nbsp; Organization"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                &nbsp;<asp:TextBox ID="txtExternalOrganisationName" runat="server" CssClass="input" Width="90px"></asp:TextBox>
                                                <asp:Label ID="lblExternalOrganisationId" runat="server" Visible="false" />
                                                <asp:DropDownList ID="ddlExternalOrganisation" runat="server" CssClass="input" AutoPostBack="true" 
                                                Visible="false"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <asp:Literal ID="lblInternalAuditor" runat="server" Text="Internal Auditor"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                <asp:DropDownList ID="ddlAuditorName" runat="server" CssClass="input" AutoPostBack="true" Width="280px" />
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
