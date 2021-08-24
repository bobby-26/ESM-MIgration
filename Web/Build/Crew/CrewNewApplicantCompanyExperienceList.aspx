<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantCompanyExperienceList.aspx.cs" Inherits="CrewNewApplicantCompanyExperienceList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>New Applicant Company Experience </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCompanyExperienceList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlCrewCompanyExperienceListEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuCrewCompanyExperienceList" runat="server" OnTabStripCommand="CrewCompanyExperienceList_TabStripCommand"></eluc:TabStrip>
            <table id="tblConfigureCrewCompanyExperienceList" width="100%">
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Rank runat="server" ID="ucRank" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                        <telerik:RadTextBox runat="server" ID="txtRankName" Width="60%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Vessel runat="server" ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Entitytype="VSL" ActiveVesselsOnly="true"/>
                        <telerik:RadTextBox runat="server" ID="txtVesselName" Width="60%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtFromDate" CssClass="input_mandatory" />
                    </td>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtToDate" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr id="trBTET" runat="server">
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblBToD" runat="server" Text="BToD"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtBToD" CssClass="input" />
                    </td>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblEToD" runat="server" Text="EToD"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtEToD" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSignOnReason" runat="server" Text="Reason"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:SignOnReason runat="server" ID="ucSignOnReason" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="SignOff Reason"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:SignOffReason runat="server" ID="ucSignOffReason" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr id="trSignonoffdates" runat="server">
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="SignOn Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="ucSignOnDate" CssClass="readonlytextbox" />
                    </td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSignOffDate" runat="server" Text="SignOff Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="ucSignOffDate" CssClass="readonlytextbox" />
                    </td>
                </tr>
                <tr id="trSignonoffby" runat="server">
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSignOnBy" runat="server" Text="SignOn By"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtSignOnBy" Width="60%" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSignOffBy" runat="server" Text="SignOff By"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtSignOffBy" Width="60%" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="trTravelDates" runat="server">
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblTravelToVessel" runat="server" Text="Travel To Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtTravelToVessel" CssClass="input" />
                    </td>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblOnLeave" runat="server" Text="On Leave"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtOnLeave" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblGap" runat="server" Text="Gap"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtGap" Width="60%" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel runat="server" ID="lblDuration" Text="Duration"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtDuration" Width="60%" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="trCOCEOC" runat="server">
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblOriginalEOC" runat="server" Text="Original EOC"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtOriginalEOC" CssClass="input" />
                    </td>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblOriginalCOC" runat="server" Text="Original COC"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtOriginalCOC" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSignOnPort" runat="server" Text="SignOn Port"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Port runat="server" ID="ucSignOnPort" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                    </td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSignOffPort" runat="server" Text="SignOff Port"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Port runat="server" ID="ucSignOffPort" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationalityofOfficers" runat="server" Text="Nationality of Officers"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNatlyOfficers" runat="server" AppendDataBoundItems="true"
                            CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNationalityofRatings" runat="server" Text="Nationality of Ratings"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNatlyRatings" runat="server" AppendDataBoundItems="true"
                            CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtRemarks" Width="60%" TextMode="MultiLine" Height="50px"
                            CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
