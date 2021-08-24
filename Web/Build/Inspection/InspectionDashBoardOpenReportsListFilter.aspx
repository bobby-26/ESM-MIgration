<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashBoardOpenReportsListFilter.aspx.cs" Inherits="InspectionDashBoardOpenReportsListFilter" %>

<!DOCTYPE html >
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
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
<%@ Register TagPrefix="eluc" TagName="vesseltype" Src="~/UserControls/UserControlVesselType.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Open Reports / Crew Complaints Filter</title>
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
                    <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlFleet" runat="server" DataTextField="FLDFLEETDESCRIPTION" DataValueField="FLDFLEETID" AutoPostBack="true"
                        EmptyMessage="Type to select fleet" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                        OnItemChecked="ddlFleet_ItemChecked" Width="270px">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                </td>
                <td>
                    <eluc:Owner ID="ucOwner" runat="server" EmptyMessage="Type to select rank" Filter="Contains" Width="270px" AddressType='<%# ((int)PhoenixAddressType.PRINCIPAL).ToString() %>' />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="270px">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                </td>
                <td>
                    <eluc:vesseltype ID="ucVesselType" runat="server" EmptyMessage="Type to select rank" Filter="Contains" Width="270px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReviewCategory" runat="server" Text="Review Category"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucReviewCategory" Width="270px" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="89" />
                    <eluc:Quick ID="ucCrewReviewCategory" Width="270px" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="93" Visible="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblAssignedTo" runat="server" Text="Assigned to"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Department ID="ucDept" runat="server" AppendDataBoundItems="true" Width="270px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFrom" runat="server" Text="From Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucFrom" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblTo" runat="server" Text="To Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucTo" runat="server" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

