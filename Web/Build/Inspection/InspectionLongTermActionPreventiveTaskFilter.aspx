<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionLongTermActionPreventiveTaskFilter.aspx.cs" Inherits="Inspection_InspectionLongTermActionPreventiveTaskFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
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
    <title>Office Preventive Task Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionScheduleFilter" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="Office Preventive Task Filter" ShowMenu="false">
            </eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlScheduleFilter">
        <ContentTemplate>
            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                     <tr>
                        <td>
                            <asp:Literal ID="lblfleet" runat="server" Text="Fleet"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Fleet runat="server" ID="ucTechFleet" Width="270px" CssClass="input" AppendDataBoundItems="true"
                                AutoPostBack="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Owner runat="server" ID="ucAddrOwner" AddressType="128" Width="270px" AppendDataBoundItems="true"
                                AutoPostBack="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel "></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselByCompany ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input" VesselsOnly="true" Width="270px" />
                        </td>
                        <td>
                            <asp:Literal ID="lblvesseltype" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input"
                                HardTypeCode="81" AutoPostBack="true" Width="270px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input" AutoPostBack="true"
                                Width="270px" OnSelectedIndexChanged="ddlCategory_Changed">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblSubCategory" runat="server" Text="Sub Category"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubcategory" runat="server" CssClass="input" Width="270px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSourceType" runat="server" Text="Source Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSourceType" runat="server" CssClass="input" Width="270px">
                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="NC" Value="1"></asp:ListItem>
                                <asp:ListItem Text="OBS" Value="2"></asp:ListItem>
                                <asp:ListItem Text="INC" Value="3"></asp:ListItem>
                                <asp:ListItem Text="MDG" Value="4"></asp:ListItem>
                                <asp:ListItem Text="TSK" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblSourceReferenecNo" runat="server" Text="Source Reference number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSourceRefNo" runat="server" CssClass="input" Width="270px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDeparment" runat="server" Text="Assigned Department"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Department ID="ucDepartment" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input" OnTextChanged="selection_Changed" Width="270px" />
                        </td>
                        <td>
                            <asp:Literal ID="lblAcceptedBy" runat="server" Text="Accepted By"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAcceptedBy" runat="server" CssClass="input" Width="270px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblWONoFrom" runat="server" Text="WO No From"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWONoFrom" runat="server" CssClass="input" Width="270px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblWONOTo" runat="server" Text="WO No To"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWONoTo" runat="server" CssClass="input" Width="270px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTargetDateFrom" runat="server" Text="Target From"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtFrom" CssClass="input" runat="server" DatePicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblTargetDateTo" runat="server" Text="Target To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtTo" CssClass="input" runat="server" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCompletionDateFrom" runat="server" Text="Completion From"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDoneDateFrom" CssClass="input" runat="server" DatePicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCompletionDateTo" runat="server" Text="Completion To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDoneDateTo" CssClass="input" runat="server" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblShowOnlyOfficeAuditTask" runat="server" Text="Show Only Office Audit Task"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkOfficeAuditDeficiencies" runat="server" AutoPostBack="true"
                                OnCheckedChanged="chkOfficeAuditDeficiencies_CheckedChanged" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCompany" runat="server" Text="Company"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Company ID="ucCompany" runat="server" Enabled="false" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                CssClass="input" AppendDataBoundItems="true" Width="270px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTaskStatus" runat="server" Text="Task Status"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAcceptance" runat="server" AppendDataBoundItems="true" CssClass="input" Width="270px"
                                AutoPostBack="true">
                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                <asp:ListItem Text="Open" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Accepted" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Completed" Value="2"></asp:ListItem>                                
                                <asp:ListItem Text="Closed" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
