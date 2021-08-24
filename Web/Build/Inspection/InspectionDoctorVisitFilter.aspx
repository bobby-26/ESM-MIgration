<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDoctorVisitFilter.aspx.cs" Inherits="InspectionDoctorVisitFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Clinic" Src="~/UserControls/UserControlClinic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Filter</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


            <eluc:TabStrip ID="PlanRelieverFilterMain" runat="server" OnTabStripCommand="PlanRelieverFilterMain_TabStripCommand"></eluc:TabStrip>


            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtName" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" VesselsOnly="true" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ddlRank" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Principal ID="ucPrincipal" AddressType="128" AppendDataBoundItems="true" runat="server"
                                CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:VesselType ID="ucVesselType" AppendDataBoundItems="true" runat="server" Width="200px" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Period of Illness" Width="80%">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="txtIllnessFrom" runat="server" CssClass="input" DatePicker="true" />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="txtIllnessTo" runat="server" CssClass="input" DatePicker="true" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td colspan="2">
                            <asp:Panel ID="Panel2" runat="server" GroupingText="Period of Closing" Width="80%">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblPeriodofClosingFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="txtClosingFrom" runat="server" CssClass="input" DatePicker="true" />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblPeriodofClosingToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="txtClosingTo" runat="server" CssClass="input" DatePicker="true" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlStatus" runat="server" EmptyMessage="Select the Status" Filter="Contains" MarkFirstMatch="true" CssClass="input">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                                    <telerik:RadComboBoxItem Text="Open" Value="0" />
                                    <telerik:RadComboBoxItem Text="Closed" Value="1" />
                                </Items>

                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </div>

        </div>
    </form>
</body>
</html>
