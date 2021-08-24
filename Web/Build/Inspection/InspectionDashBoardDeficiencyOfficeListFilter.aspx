<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashBoardDeficiencyOfficeListFilter.aspx.cs" Inherits="InspectionDashBoardDeficiencyOfficeListFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
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
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deficiency Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuDefeciencyFilter" runat="server" OnTabStripCommand="MenuDefeciencyFilter_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" OnTextChangedEvent="ucCompany_TextChangedEvent" AutoPostBack="true"
                            CssClass="input" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInspectionCategory" runat="server" Text="Inspection Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucInspectionCategory" runat="server" Width="240px"
                            AppendDataBoundItems="true" HardTypeCode="144" AutoPostBack="true" OnTextChangedEvent="ucInspectionCategory_TextChangedEvent" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInspection" runat="server" Text="Inspection"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlInspection" runat="server" Width="240px"
                            AutoPostBack="true" OnTextChanged="ucInspection_Changed" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblChapter" runat="server" Text="Chapter"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:IChapter runat="server" ID="ucChapter" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlNCType" runat="server" Width="240px" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                <telerik:RadComboBoxItem Text="NC" Value="2" />
                                <telerik:RadComboBoxItem Text="Major NC" Value="1" />
                                <telerik:RadComboBoxItem Text="Observation" Value="3" />
                                <telerik:RadComboBoxItem Text="Hi Risk Observation" Value="4" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text="Deficiency Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucNonConformanceCategory" runat="server" AppendDataBoundItems="true"
                            Width="240px" QuickTypeCode="47" Visible="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSource" runat="server" Text="Source"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSource" runat="server" Width="240px" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0" Selected="True" />
                                <telerik:RadComboBoxItem Text="Audit/Inspection" Value="1" />
                                <telerik:RadComboBoxItem Text="Vetting" Value="2" />
                                <telerik:RadComboBoxItem Text="Open Reports" Value="3" />
                                <telerik:RadComboBoxItem Text="Direct" Value="4" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSourceReferenceNo" runat="server" Text="Source Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSourceRefNo" runat="server" Width="240px" MaxLength="50"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" Width="240px" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td colspan="21"></td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

