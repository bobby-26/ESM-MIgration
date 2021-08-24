<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAdminRiskAssessment.aspx.cs" Inherits="InspectionAdminRiskAssessment" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRiskAssessment" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="Title1" Text="RA Date Change" ShowMenu="true" Visible="false"></eluc:Title>
        <%--<asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:TabStrip ID="MenuRiskAssessmentGeneral" runat="server" TabStrip="true" OnTabStripCommand="RiskAssessmentGeneral_TabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuRiskAssessment" runat="server" OnTabStripCommand="MenuRiskAssessment_TabStripCommand"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:VesselByCompany ID="ucVessel" runat="server" AppendDataBoundItems="true" Width="204px" OnTextChangedEvent="ucVessel_Changed" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRiskAssessment" runat="server" Text="Risk Assessment"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnRA">
                        <telerik:RadTextBox ID="txtRANumber" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="80px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtRA" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                            Width="120px">
                        </telerik:RadTextBox>
                        <asp:LinkButton ID="imgShowRA" runat="server"
                            ImageAlign="AbsMiddle" Text="..">
                        <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtRAId" runat="server" CssClass="input" MaxLength="20" Width="0px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtRaType" runat="server" CssClass="input" MaxLength="2" Width="0px"></telerik:RadTextBox>
                    </span>
                    <asp:LinkButton ID="imgSearch" runat="server" OnClick="imgSearch_Click" ToolTip="Search">
                        <span class="icon"><i class="fas fa-search"></i></span>
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblIntendedWorkDate" runat="server" Text="Intended Work Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucIntendedWorkDate" runat="server" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPreparedDate" runat="server" Text="Prepared Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblApprovedDate" Text="Approved Date" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucApprovedDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblIssuedDate" runat="server" Text="Issued Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucIssuedDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
