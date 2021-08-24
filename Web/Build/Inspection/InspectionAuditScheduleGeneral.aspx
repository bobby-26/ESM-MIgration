<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditScheduleGeneral.aspx.cs" Inherits="InspectionAuditScheduleGeneral" %>

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
<title></title>    

   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

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
                    <eluc:Title runat="server" ID="ucTitle" Text="Schedule" ShowMenu="true"></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:tabstrip id="MenuScheduleGeneral" runat="server" tabstrip="true" ontabstripcommand="MenuScheduleGeneral_TabStripCommand">
                    </eluc:tabstrip>
                </div>                
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:tabstrip id="MenuInspectionScheduleGeneral" runat="server" ontabstripcommand="MenuInspectionScheduleGeneral_TabStripCommand"></eluc:tabstrip>
                    </span>
                </div>
                <table id="tblGuidance" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblNote" runat="server" Text="*Note: When the Category is changed from 'Internal' to 'External' or Vice versa after Planning, Auditor details should be reentered 
                                     and Status will be changed from 'Planned' to 'Scheduled'." ForeColor="Blue" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureInspectionIncident" width="100%">
                        <tr>
                            <td class="style1" width="20%">
                               <asp:Literal ID="lblSerialNo" runat="server" Text="Serial Number"></asp:Literal>
                            </td>
                            <td class="style2" width="30%">
                                <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="readonlytextbox" 
                                    MaxLength="200" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td class="style1" width="20%">
                               <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference Number"></asp:Literal>
                            </td>
                            <td class="style2" width="30%">                               
                                <%--<asp:CheckBox ID="chkIsLastDoneDateYN" runat="server" Text="First Audit" AutoPostBack="true" OnCheckedChanged="chkIsLastDoneDateYN_CheckedChanged" /> --%>
                                <asp:TextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" 
                                    MaxLength="200" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>                            
                            <td width="20%">
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" AssignedVessels="true"   
                                    AutoPostBack="true" CssClass="input_mandatory" OnTextChangedEvent="ucVessel_changed" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblLastDoneDate" runat="server" Text="Last Done Date"></asp:Literal></td>
                            <td width="30%">
                                <eluc:Date ID="txtLastDoneDate" runat="server" CssClass="input" 
                                    DatePicker="true" Width="90px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style3" width="20%">
                                <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                            </td>
                            <td class="style3" width="30%">
                                <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" 
                                    AutoPostBack="true" CssClass="dropdown_mandatory" HardTypeCode="144" 
                                    OnTextChangedEvent="InspectionType_Changed" />
                                <eluc:Hard ID="ucAuditType" runat="server" AutoPostBack="true" 
                                    CssClass="dropdown_mandatory" HardTypeCode="148" 
                                    OnTextChangedEvent="InspectionType_Changed" ShortNameFilter="AUD" 
                                    Visible="false" />
                            </td>
                            <td width="20%">
                                <asp:literal ID="lblFrequency" runat="server" Text="Frequency"></asp:literal>
                            </td>
                            <td width="30%">
                                
                                <eluc:Number ID="ucFrequencyValue" runat="server" CssClass="input_mandatory" 
                                    IsPositive="true" MaxLength="3" Width="45px" />
                                <eluc:Hard ID="ucFrequency" runat="server" AppendDataBoundItems="false" 
                                    CssClass="dropdown_mandatory" HardTypeCode="7" />
                                
                            </td>                            
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                <asp:Literal ID="lblExternalAuditType" runat="server" Text="External Audit Type"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <eluc:Hard ID="ucExternalAuditType" runat="server" AutoPostBack="true" CssClass="input"
                                    AppendDataBoundItems="true" HardList="<%# PhoenixRegistersHard.ListHard(1,190) %>"
                                    HardTypeCode="190" OnTextChangedEvent="ExternalAuditType_Changed" />
                            </td>                           
                            <td width="20%">
                                <asp:Literal ID="lblPlanningMethod" runat="server" Text="Planning Method"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Hard ID="ucDoneType" runat="server" CssClass="dropdown_mandatory" HardTypeCode="5"  Enabled="false"/>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                               <asp:Literal ID="lblAuditInspection" runat="server" Text=" Audit / Inspection"></asp:Literal></td>
                            <td width="30%">
                                <eluc:Inspection ID="ucAudit" runat="server" AppendDataBoundItems="true" Width="200px" Visible="false"
                                    AutoPostBack="true" CssClass="dropdown_mandatory"/>
                                <asp:DropDownList ID="ddlAuditShortCodeList" runat="server" AutoPostBack="true" CssClass="dropdown_mandatory" OnTextChanged="ddlAuditShortCodeList_TextChanged">
                                </asp:DropDownList>
                            </td>
                            <td width="20%">
                               <asp:Literal ID="lblDueDate" runat="server" Text="Due Date"></asp:Literal>
                            </td>
                            <td width="30%">
                            
                                <eluc:Date ID="txtDueDate" runat="server" CssClass="input" DatePicker="true"/>
                            </td>
                        </tr>                        
                        <tr>
                            <td width="20%">
                                &nbsp;</td>
                            <td width="30%">
                                <asp:Panel ID="Panel1" CssClass="input" runat="server" Width="150">
                                    <asp:RadioButton ID="rdoAutomatic" runat="server" Text="Automatic" GroupName="Type"
                                        OnCheckedChanged="rdoAutomatic_CheckedChanged" AutoPostBack="true" />
                                    <asp:RadioButton ID="rdoMannual" runat="server" Text="Manual" GroupName="Type" OnCheckedChanged="rdoAutomatic_CheckedChanged"
                                        AutoPostBack="true" />
                                </asp:Panel></td>  
                                
                                <td width="20%">
                                <asp:Literal ID="lblwindowPeriod" runat="server" Text="Window Period ( in days )"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Number ID="txtWindowperiod" runat="server" CssClass="input_mandatory" MaxLength="3"
                                    IsPositive="true" Width="45px"></eluc:Number>
                                <eluc:Hard ID="ucwindowperiodtype" Visible="false" runat="server" AppendDataBoundItems="false" CssClass="dropdown_mandatory"
                                    HardTypeCode="7" />
                            </td>                       
                        </tr>                        
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input"
                                    Rows="2" TextMode="MultiLine" Width="240px"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblLastPortofAudit" runat="server" Text="Last Port of Audit / Inspection"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Port runat="server" ID="ucLastPort" CssClass="input" AppendDataBoundItems="true" Width="250px" />                                  
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblInterimAudit" runat="server" Text="Interim Audit"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:CheckBox ID="chkInterimAudit" runat="server" AutoPostBack="true" OnCheckedChanged="chkInterimAudit_CheckedChanged" />
                            </td>
                            <td width="20%">
                               <asp:Literal ID="lblScheduleISPSAuditAlso" runat="server" Text="Schedule ISPS Audit also"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:CheckBox ID="chkISPSAudit" runat="server" />
                            </td>
                        </tr>                                           
                    </table>
                    <table id="tblInternalAudit" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlInternalAudit" runat="server" GroupingText="Internal Audit Schedule">
                                    <table id="tblInternalAudit" runat="server" width="100%">
                                        <tr>
                                            <td width="20%">
                                                <asp:Literal ID="lblRefNo" runat="server" Text="Ref. No."></asp:Literal>
                                            </td>
                                            <td width="30%">                                                
                                                <asp:TextBox runat="server" ID="txtInternalAuditRefNo" CssClass="readonlytextbox"
                                                    MaxLength="200" ReadOnly="true"></asp:TextBox>
                                            </td>
                                            <td width="20%">                                             
                                                <asp:Literal ID="lblDueDate1" runat="server" Text="Due Date"></asp:Literal>
                                            </td>
                                            <td width="30%">                                                
                                                <eluc:Date ID="txtInternalAuditDueDate" runat="server" CssClass="input"
                                                    Enabled="false" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td width="20%">
                                                <asp:Literal ID="lblAudit" runat="server" Text="Audit"></asp:Literal>
                                            </td>
                                            <td width="30%">                                                
                                                <eluc:Inspection ID="ucInternalAudit" runat="server" AppendDataBoundItems="true"
                                                    AutoPostBack="false" CssClass="dropdown_mandatory" Width="200px" Visible="false" />
                                                <asp:DropDownList ID="ddlInternalAudit" runat="server" AutoPostBack="false" CssClass="dropdown_mandatory">
                                                </asp:DropDownList>
                                            </td>                                            
                                            <td width="20%">
                                                <asp:Panel ID="pnlInternalAutomatic" CssClass="input" runat="server" Width="150" Enabled="false" >
                                                    <asp:RadioButton ID="rdoInternalAutomatic" runat="server" Text="Automatic" Checked="false" GroupName="InternalType" />
                                                    <asp:RadioButton ID="rdoInternalManual" runat="server" Text="Manual" Checked="true" GroupName="InternalType" />
                                                </asp:Panel>
                                            </td>
                                            <td width="30%">                                                
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>     
                    </table>
                    <table id="tblExternalAudit" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlExternalAudit" runat="server" GroupingText="Initial Audit Schedule">
                                    <table id="tblExternalAudit" runat="server" width="100%">
                                        <tr>
                                            <td width="20%">
                                                <asp:Literal ID="lblRefNumber" runat="server" Text="Ref. No."></asp:Literal>
                                            </td>
                                            <td width="30%">                                                
                                                <asp:TextBox runat="server" ID="txtExternalAuditRefNo" CssClass="readonlytextbox"
                                                    MaxLength="200" ReadOnly="true"></asp:TextBox>
                                            </td>
                                            <td width="20%">
                                                <asp:Literal ID="lblFrequency1" runat="server" Text="Frequency"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                <eluc:Number ID="ucExternalFrequencyValue" runat="server" CssClass="input_mandatory" MaxLength="3"
                                                    IsPositive="true" Width="45px"></eluc:Number>
                                                <eluc:Hard ID="ucExternalFrequency" runat="server" AppendDataBoundItems="false" CssClass="dropdown_mandatory"
                                                    HardTypeCode="7" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <asp:Literal ID="lblAudit1" runat="server" Text="Audit"></asp:Literal>
                                                
                                            </td>
                                            <td width="30%">                                                
                                                <eluc:Inspection ID="ucExternalAudit" runat="server" AppendDataBoundItems="true"
                                                    AutoPostBack="false" CssClass="dropdown_mandatory" Width="200px" Visible="false" />
                                                <asp:DropDownList ID="ddlExternalAudit" runat="server" AutoPostBack="false" CssClass="dropdown_mandatory">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="20%">
                                                <asp:Literal ID="lblPlanningMethod1" runat="server" Text="Planning Method"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                <eluc:Hard ID="ucExternalDoneType" runat="server" CssClass="dropdown_mandatory" HardTypeCode="5" Enabled="false" />
                                            </td>
                                       </tr>
                                        <tr>
                                            <td width="20%"> 
                                                <asp:Literal ID="lblDueDate2" runat="server" Text="Due Date"></asp:Literal>
                                            </td>
                                            <td width="30%">                                                
                                                <eluc:Date ID="txtExternalAuditDueDate" runat="server" CssClass="input"
                                                    Enabled="false" />
                                            </td>
                                            <td width="20%">
                                                <asp:Literal ID="lblWindowPeriodinDays" runat="server" Text="Window Period ( in days )"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                <eluc:Number ID="txtExternalWindowperiod" runat="server" CssClass="input_mandatory" MaxLength="3"
                                                    IsPositive="true" Width="45px"></eluc:Number>
                                                <eluc:Hard ID="ucExternalwindowperiodtype" runat="server" AppendDataBoundItems="false"
                                                    CssClass="dropdown_mandatory" HardTypeCode="7" Visible="false" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td colspan="2" width="20%">                                               
                                            </td>
                                            <td colspan="2" width="30%">                                                
                                                <asp:Panel ID="pnlExternalAutomatic" CssClass="input" runat="server" Width="150" Enabled="false">
                                                    <asp:RadioButton ID="rdoExternalAutomatic" runat="server" Text="Automatic" Checked="true"
                                                        GroupName="ExternalType" />
                                                    <asp:RadioButton ID="rdoExternalManual" runat="server" Text="Manual" Checked="false" GroupName="ExternalType" />
                                                </asp:Panel>
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
