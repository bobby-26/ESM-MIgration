<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDeficiencyXLReportRequest.aspx.cs" Inherits="InspectionDeficiencyXLReportRequest" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
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
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
</telerik:RadCodeBlock></head>
<body>
    <form id="frmDefeciencyFilter" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:title runat="server" id="Title1" text="Deficiencies" showmenu="true">
            </eluc:title>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:tabstrip id="MenuDefeciencyFilter" runat="server" ontabstripcommand="MenuDefeciencyFilter_TabStripCommand">
        </eluc:tabstrip>
    </div>
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlScheduleFilter">
        <ContentTemplate>
            <div id="divFind">
               <eluc:Status ID="Status" runat="server" Text="" />
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                        </td>
                        <td>
                            <eluc:fleet id="ucFleet" runat="server" cssclass="input" appenddatabounditems="true"></eluc:fleet>
                        </td>
                        <td>
                           <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                        </td>
                        <td>
                            <eluc:addresstype runat="server" id="ucAddrOwner" addresstype="128" width="80%" appenddatabounditems="true"
                                autopostback="true" cssclass="input" />
                        </td>                        
                    </tr>
                    <tr>                        
                        <td>
                           <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselByCompany runat="server" id="ucVessel" appenddatabounditems="true" vesselsonly="true" 
                                cssclass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input" HardTypeCode="81"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:literal ID="lblINspectionType" runat="server" Text="Inspection Type"></asp:literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucInspectionType" runat="server" CssClass="input" AppendDataBoundItems="true" HardTypeCode="148" AutoPostBack="true" OnTextChangedEvent="ucInspectionType_Changed" />
                        </td>
                        <td>
                            <asp:Literal ID="lblInspectionCategory" runat="server" Text="Inspection Category"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucInspectionCategory" runat="server" CssClass="input" AppendDataBoundItems="true" HardTypeCode="144" AutoPostBack="true" OnTextChangedEvent="ucInspectionCategory_Changed" />
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblInspection" runat="server" Text="Inspection"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Inspection ID="ucInspection" runat="server" Visible="false" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucInspection_Changed" />
                            <asp:DropDownList ID="ddlInspection" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="ucInspection_Changed"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblChapter" runat="server" Text="Chapter"></asp:Literal>
                        </td>
                        <td>
                            <eluc:IChapter runat="server" ID="ucChapter" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>                    
                    <tr>                        
                        <td>
                            <asp:Literal ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlNCType" runat="server" CssClass="input">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="NC" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Major NC" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Observation" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Hi Risk Observation" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblDeficiencyCategory" runat="server" Text="Deficiency Category"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick id="ucNonConformanceCategory" runat="server" appenddatabounditems="true"
                                width="250px" cssclass="input" quicktypecode="47" visible="true" />
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                            <asp:Literal ID="lblSource" runat="server" Text="Source"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSource" runat="server" CssClass="input">
                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Audit/Inspection" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Vetting" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Open Reports" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Direct" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblSourceReferenceNo" runat="server" Text="Source Reference Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSourceRefNo" runat="server" CssClass="input" MaxLength="50"></asp:TextBox>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefNo" runat="server" CssClass="input" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:hard id="ucStatus" runat="server" appenddatabounditems="true" hardtypecode="146"
                                shortnamefilter="OPN,CLD,CAD,REV" cssclass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:date id="ucFromDate" cssclass="input" width="80px" runat="server" datepicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:date id="ucToDate" cssclass="input" width="80px" runat="server" datepicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                                <asp:Literal ID="lblShowOnlyOfficeAuditDeficiencies" runat="server" Text="Show Only Office Audit Deficiencies"></asp:Literal>
                        </td>
                         <td>
                                <asp:CheckBox ID="chkOfficeAuditDeficiencies" runat="server" AutoPostBack="true" OnCheckedChanged="chkOfficeAuditDeficiencies_CheckedChanged" />                                
                         </td>
                         <td>
                            <asp:Literal ID="lblCompany" runat="server" Text="Company"></asp:Literal>
                         </td>
                         <td>
                            <eluc:Company ID="ucCompany" runat="server" Enabled="false" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                CssClass="input" AppendDataBoundItems="true" />
                         </td>
                         
                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:Literal ID="lbLPSCActionCode" runat="server" Text="PSC Action code / VIR Condition"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKey" runat="server" CssClass="input" Width="60px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                        </td> 
                        <td>
                            <asp:Literal ID="lblRCA" runat="server" Text="RCA"></asp:Literal>
                        </td>
                        <td>
                            <div id="RCA" runat="server" class="input" style="width: 155px;">
                                <asp:CheckBoxList ID="cblRCA" runat="server" RepeatDirection="Vertical" RepeatColumns="1">
                                    <asp:ListItem Text="RCA Required" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="RCA Completed" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="RCA Pending" Value="3"></asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
