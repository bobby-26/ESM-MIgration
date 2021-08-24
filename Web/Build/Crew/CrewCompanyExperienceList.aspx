<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCompanyExperienceList.aspx.cs"
    Inherits="CrewCompanyExperienceList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Company Experience</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCompanyExperienceList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuCrewCompanyExperienceList" runat="server" OnTabStripCommand="CrewCompanyExperienceList_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="divFind" style="position: relative; z-index: 2">
                <table id="tblConfigureCrewCompanyExperienceList" width="100%">
                    <tr>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Rank runat="server" ID="ucRank" CssClass="dropdown_mandatory" Width="50%" AppendDataBoundItems="true" />
                            <telerik:RadTextBox ID="txtRankName" runat="server" Width="50%" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Vessel runat="server" ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true" Width="50%"
                                CssClass="dropdown_mandatory" Entitytype="VSL" ActiveVesselsOnly="true" />
                            <telerik:RadLabel ID="txtVesselName" runat="server" Text="Vessel" Width="50%" Enabled="false"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="txtFromDate" Width="50%"  CssClass="input_mandatory" />
                        </td>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="txtToDate" Width="50%"  CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr id="trBTET" runat="server">
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblBToD" runat="server" Text="BToD"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="txtBToD" Width="50%"/>
                        </td>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblEToD" runat="server"  Text="EToD"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="txtEToD" Width="50%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblSignOnReason" runat="server" Text="SignOn Reason"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:SignOnReason runat="server" ID="ucSignOnReason" Width="50%" AppendDataBoundItems="true" />
                        </td>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="SignOff Reason"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:SignOffReason runat="server" ID="ucSignOffReason" AppendDataBoundItems="true" Width="50%" />
                            <asp:CheckBox ID="chkPromtedyn" runat="server" AutoPostBack="true" OnCheckedChanged="DisableSignoffReason"
                                Text="Promoted Onboard Y/N" />
                            <telerik:RadTextBox ID="txtPromotedReason" runat="server" Visible="false" Width="50%" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trSignonoffdates" runat="server">
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="SignOn Date"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="ucSignOnDate" Width="50%" Enabled="false"/>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblSignOffDate" runat="server" Text="SignOff Date"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="ucSignOffDate" Width="50%" Enabled="false"/>
                        </td>
                    </tr>
                    <tr id="trSignonoffby" runat="server">
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblSignOnBy" runat="server" Text="SignOn By"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtSignOnBy" runat="server" Width="50%" Enabled="false"></telerik:RadTextBox>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblSignOffBy" runat="server" Text="SignOff By"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtSignOffBy" runat="server" Width="50%" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trTravelDates" runat="server">
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblTravelToVessel" runat="server" Text="Travel To Vessel"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="txtTravelToVessel" Width="50%" />
                        </td>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblOnLeave" runat="server" Text="On Leave"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="txtOnLeave" Width="50%" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblGap" runat="server" Text="Gap"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtGap" runat="server" Width="50%"></telerik:RadTextBox>
                        </td>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblDuration" runat="server" Text="Duration"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtDuration" runat="server" Width="50%" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr id="trCOCEOC" runat="server">
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblOriginalEOC" runat="server" Text="Original EOC"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="txtOriginalEOC" Width="50%" />
                        </td>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblOriginalCOC" runat="server" Text="Original COC"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Date runat="server" ID="txtOriginalCOC" Width="50%"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblSignOnPort" runat="server" Text="SignOn Port"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Port runat="server" ID="ucSignOnPort" CssClass="dropdown_mandatory" Width="50%" AppendDataBoundItems="true" />
                        </td>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblSignOffPort" runat="server" Text="SignOff Port"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <eluc:Port runat="server" ID="ucSignOffPort" CssClass="dropdown_mandatory" Width="50%" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNationalityofOfficers" runat="server" Text="Nationality of Officers"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Nationality ID="ucNatlyOfficers" runat="server" AppendDataBoundItems="true" Width="50%"
                               />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblNationalityofRatings" runat="server" Text="Nationality of Ratings"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Nationality ID="ucNatlyRatings" runat="server" AppendDataBoundItems="true" Width="50%"
                                />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblReliverName" runat="server" Text="Reliever"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtRelieverName" runat="server" Width="50%" Enabled="false"></telerik:RadTextBox>
                        </td>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblRelieverRank" runat="server" Text="Reliever Rank"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtRelieverRank" runat="server" Width="50%" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblSignonRemarks" runat="server" Text="Sign On Remarks"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtSignOnRemarks" runat="server" Width="50%" TextMode="MultiLine"
                                Height="50px">
                            </telerik:RadTextBox>
                        </td>
                        <td align="left" style="width: 10%">
                            <telerik:RadLabel ID="lblSignoffRemarks" runat="server" Text="Sign Off Remarks"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtSignOffRemarks" runat="server" Width="50%" TextMode="MultiLine"
                                Height="50px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
