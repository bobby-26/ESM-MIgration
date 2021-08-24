<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetProposalAddEdit.aspx.cs"
    Inherits="OwnerBudgetProposalAddEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="../UserControls/UserControlAddressType.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager ID="ToolkitScriptManager1"
            runat="server">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlBudgetProposal" Height="100%">


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuAddEditProposal" runat="server" OnTabStripCommand="MenuAddEditProposal_TabStripCommand" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProposalTitle" runat="server" Text="Proposal Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtProposalTitle" Width="30%" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td align="justify">
                        <telerik:RadRadioButtonList ID="rblst" runat="server" RepeatDirection="Horizontal" Direction="Horizontal" AutoPostBack="true"
                            OnSelectedIndexChanged="rblst_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Text="New Vessel" Value="0" Selected="true" />
                                <telerik:ButtonListItem Text="Existing Vessel" Value="1" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProposalDate" runat="server" Text="Proposal Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucProposalDate" DatePicker="true" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProposalVessel" runat="server" Text="Proposal Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" Width="30%" runat="server" CssClass="input_mandatory" />
                        <eluc:Vessel ID="ucVesselName" Visible="false" runat="server" AppendDataBoundItems="true"
                            CssClass="input_mandatory" OnTextChangedEvent="ddlCrewWageChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAnalyzewith" runat="server" Text="Analyze with"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucAnalyzeWith" runat="server" AppendDataBoundItems="true" CssClass="input" OnTextChangedEvent="ucAnalyzeWith_TextChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOfficer" runat="server" Text="Officer"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType runat="server" ID="ucOfficerWageScale" CssClass="input" AppendDataBoundItems="true"
                            AddressType="134" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRatings" runat="server" Text="Ratings"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType runat="server" ID="ucRatingsWageScale" CssClass="input" AppendDataBoundItems="true"
                            AddressType="134" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetYear" runat="server" Text="Budget Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlYear" runat="server" CssClass="input">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
