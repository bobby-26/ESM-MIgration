<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOfficeIncidentNearMissFilter.aspx.cs"
    Inherits="Inspection_InspectionOfficeIncidentNearMissFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlIncidentNearMissCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubCategory" Src="~/UserControls/UserControlIncidentNearMissSubCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionScheduleFilter" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="Incident/Near Miss Filter" ShowMenu="false">
            </eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuIncidentFilter" runat="server" OnTabStripCommand="MenuIncidentFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlScheduleFilter">
        <ContentTemplate>
            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <div id="divvessel" runat="server">
                        <tr>
                            <td>
                                <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Fleet runat="server" ID="ucTechFleet" Width="240px" CssClass="input" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Owner runat="server" ID="ucAddrOwner" AddressType="128" Width="240px" AppendDataBoundItems="true"
                                    AutoPostBack="true" CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" Width="240px"
                                    VesselsOnly="true" CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ucVesselType" runat="server" Width="240px" AppendDataBoundItems="true"
                                    CssClass="input" HardTypeCode="81" />
                            </td>
                        </tr>
                    </div>
                    <tr>
                        <td>
                            <asp:Literal ID="lblIncidentClassification" Text="Incident Classification" runat="server"> </asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlIncidentNearmiss" runat="server" Width="240px" CssClass="input"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlIncidentNearmiss_Changed">
                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Incident" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Near Miss" Value="2"></asp:ListItem>
                                <%--<asp:ListItem Text="Serious Near Miss" Value="3"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblActivityRelevanttotheEvent" runat="server" Text="Activity relevant to the Event"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard runat="server" Width="240px" ID="ucActivity" AppendDataBoundItems="true"
                                CssClass="input" HardTypeCode="170" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblIncidentNearMissType" runat="server" Text="Incident / Near Miss Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Category ID="ucCategory" Width="240px" runat="server" AppendDataBoundItems="true"
                                CssClass="input" AutoPostBack="true" OnTextChangedEvent="ucCategory_Changed" />
                        </td>
                        <td>
                            <asp:Literal ID="lblIncidentNearMissSubtype" runat="server" Text="Incident / Near Miss Subtype"></asp:Literal>
                        </td>
                        <td>
                            <eluc:SubCategory ID="ucSubcategory" Width="240px" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefNo" runat="server" CssClass="input" MaxLength="20" Width="240px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblTitle" runat="server" Text="Title"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="input" MaxLength="20" Width="240px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="168"
                                Width="240px" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblConsequenceCategorization" runat="server" Text="Consequence Categorization"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard runat="server" ID="ucConsequenceCategory" CssClass="input" AppendDataBoundItems="true"
                                HardTypeCode="169" Width="240px" ShortNameFilter="A,B,C" />
                        </td>
                        <td>
                            <%--Potential Categorization--%>
                        </td>
                        <td>
                            <eluc:Hard ID="ucPotentialCategory" runat="server" Width="240px" AppendDataBoundItems="true"
                                CssClass="input" Visible="false" HardTypeCode="169" ShortNameFilter="A,B,C" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblIncidentDateFrom" runat="server" Text="Incident Date From"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucFromDate" CssClass="input" runat="server" DatePicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblIncidentDateTo" runat="server" Text="Incident Date To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucToDate" CssClass="input" runat="server" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblReportedDateFrom" runat="server" Text="Reported Date From"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucReportedDateFrom" CssClass="input" runat="server" DatePicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblReportedDateTo" runat="server" Text="Reported Date To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucReportedDateTo" CssClass="input" runat="server" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblShowOnlyOfficeIncidentNearMiss" runat="server" Text="Show Only Office Incident / Near Miss"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkOfficeAuditIncident" runat="server" AutoPostBack="true" OnCheckedChanged="chkOfficeAuditIncident_CheckedChanged" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCompany" runat="server" Text="Company"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Company ID="ucCompany" runat="server" Enabled="false" Width="240px" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblContractedRelatedIncidentYN" runat="server" Text="Contracted Related Incident Y/N"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkContractedRelatedIncidentYN" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
