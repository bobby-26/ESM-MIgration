<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashBoardAuditRecordListFilterExtn.aspx.cs" Inherits="InspectionDashBoardAuditRecordListFilterExtn" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
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
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Audit & Insppection Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Title runat="server" ID="Title1" Text="Audit / Inspection Filter" ShowMenu="true" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="MenuScheduleFilter" runat="server" OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td>
                    <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlFleet" runat="server" DataTextField="FLDFLEETDESCRIPTION" DataValueField="FLDFLEETID" AutoPostBack="true"
                        EmptyMessage="Type to select fleet" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                        OnItemChecked="ddlFleet_ItemChecked" Width="273px">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                </td>
                <td>
                    <eluc:Owner ID="ucOwner" runat="server" EmptyMessage="Type to select rank" Filter="Contains" Width="273px" AddressType='<%# ((int)PhoenixAddressType.PRINCIPAL).ToString() %>' />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"
                        EmptyMessage="Type to select fleet" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="273px">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                </td>
                <td>
                    <eluc:vesseltype ID="ucVesselType" runat="server" EmptyMessage="Type to select rank" Filter="Contains" Width="273px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblstatus" runat="server" Text="Status"></telerik:RadLabel>
                    <%--<telerik:RadLabel ID="lblAuditInspectionCategory" runat="server" Text="Audit / Inspection Category"></telerik:RadLabel>--%>
                </td>
                <td>
                    <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" ShortNameFilter="CMP,REV,CLD" HardTypeCode="146" Width="270px" />
                   <%-- <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                        HardTypeCode="144" OnTextChangedEvent="Bind_UserControls" Width="273px" />--%>
                </td>
                <td>
                    <telerik:RadLabel ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlAudit" runat="server" AutoPostBack="true" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"
                        Width="273px">
                    </telerik:RadComboBox>
                    <eluc:Inspection runat="server" ID="ucAudit" Visible="false" AppendDataBoundItems="true" />
                    <eluc:Hard ID="ucAuditType" runat="server" Visible="false" ShortNameFilter="AUD"
                        AutoPostBack="true" HardTypeCode="148" OnTextChangedEvent="Bind_UserControls" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPortFrom" runat="server" Text="Port From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiPort ID="ucPort" runat="server" Width="273px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPortTo" runat="server" Text="Port To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiPort ID="ucPortTo" runat="server" Width="273px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFrom" runat="server" Text="Completed Between"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucFrom" runat="server" />
                    -
                    <eluc:Date ID="ucTo" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRefNo" runat="server" Width="273px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
