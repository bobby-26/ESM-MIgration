<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOfficeRATaskFilter.aspx.cs" Inherits="Inspection_InspectionOfficeRATaskFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vesseltype" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="addresstype" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RA Task Filter</title>
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
        <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
        <div id="divFind">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByOwner ID="ucVessel" runat="server" AppendDataBoundItems="true" Width="240px" AutoPostBack="true"
                            VesselsOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSourceReferenceNo" runat="server" Text="RA Ref.No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSourceRefNo" Width="240px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTargetDateFrom" runat="server" Text="Target Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFrom" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTargetDateTo" runat="server" Text="Target Date To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtTo" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompletionDateFrom" runat="server" Text="Completion Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDoneDateFrom" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompletionDateTo" runat="server" Text="Completion Date To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDoneDateTo" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" Width="240px" Visible="false"
                            HardTypeCode="146" ShortNameFilter="OPN,EXR,PSA,CMP,CLD" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
