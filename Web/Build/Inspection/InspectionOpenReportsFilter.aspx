<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOpenReportsFilter.aspx.cs"
    Inherits="InspectionOpenReportsFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="addresstype" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlInspectionDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Open Reports Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOpenReportsFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Title runat="server" ID="Title1" Text="Open Reports Filter" ShowMenu="true" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="MenuOpenReportsFilter" runat="server" OnTabStripCommand="MenuOpenReportsFilter_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
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
                        AutoPostBack="true" OnTextChangedEvent="ucAddrOwner_Changed" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:VesselByOwner ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                        VesselsOnly="true" Width="270px" AssignedVessels="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="270px"  HardTypeCode="81" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReviewCategory" runat="server" Text="Review Category"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucReviewCategory" Width="270px" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="89"/>
                </td>
                <td>
                    <telerik:RadLabel ID="lblAssignedTo" runat="server" Text="Assigned to"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucReviewSubcategory" runat="server" Width="270px" AppendDataBoundItems="true"
                        QuickTypeCode="90" Visible="false" />
                    <eluc:Department ID="ucDept" runat="server" AppendDataBoundItems="true" Width="270px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDateFrom" runat="server" Text="Date From "></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucFromDate" runat="server" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDateTo" runat="server" Text="Date To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucToDate" runat="server" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" Visible="false" runat="server">
                        <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Unclassified" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Classified" Value="8S"></asp:ListItem>
                        <asp:ListItem Text="Incident Raised" Value="1"></asp:ListItem>
                        <asp:ListItem Text="NC Raised" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Observation Raised" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Near Miss Raised" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Crew Complaint Raised" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Closed" Value="6"></asp:ListItem>
                        <asp:ListItem Text="Cancelled" Value="7"></asp:ListItem>
                    </asp:DropDownList>
                    <eluc:Hard ID="ucORStatus" runat="server" Width="270px" AppendDataBoundItems="true" HardTypeCode="243" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Company ID="ucCompany" runat="server" Width="270px" AppendDataBoundItems="true" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
