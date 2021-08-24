<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelReportFilter.aspx.cs" Inherits="Crew_CrewTravelReportFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Designation" Src="~/UserControls/UserControlDesignation.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
      
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
          
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
       
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblFileNumber" runat="server" Text="File No"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadTextBox ID="txtFileNumber" runat="server"  Width="240px"></telerik:RadTextBox>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel   ForeColor="Black"    ID="lblRequisition" runat="server" Text="Requisition No"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadTextBox ID="txtRequisition" runat="server"  Width="240px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblPassportno" runat="server" Text="Passport No"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadTextBox ID="txtPassportno" runat="server" Width="240px"></telerik:RadTextBox>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblTicketno" runat="server" Text="Ticket No"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadTextBox ID="txtTicketno" runat="server" Width="240px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblOfficeCrew" runat="server" Text="Office / Crew"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadListBox ID="ddlOfficeCrew" SelectionMode="Multiple" AppendDataBoundItems="true" Width="240px" Height="80px" runat="server"></telerik:RadListBox>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="240px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadListBox ID="ddlYear" SelectionMode="Multiple" AppendDataBoundItems="true" Width="240px" Height="80px" runat="server"></telerik:RadListBox>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblQuarter" runat="server" Text="Quarter"></telerik:RadLabel>
                            </td>
                           <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadListBox ID="ddlQuarter" SelectionMode="Multiple" AppendDataBoundItems="true" Width="240px" Height="80px" runat="server"></telerik:RadListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                            </td>
                           <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadListBox ID="ddlMonth" SelectionMode="Multiple" AppendDataBoundItems="true" Width="240px" Height="80px" runat="server"></telerik:RadListBox>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                            </td>
                           <td style="padding-left:10px;padding-right:10px">
                                <eluc:City ID="ddlOrigin" runat="server" AppendDataBoundItems="true" Width="240px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <eluc:City ID="ddlDestination" runat="server" AppendDataBoundItems="true" Width="240px" />
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblTravelreason" runat="server" Text="Travel Reason"></telerik:RadLabel>
                            </td>
                          <td style="padding-left:10px;padding-right:10px">

                                <eluc:TravelReason ID="ddlTravelreason" runat="server" AppendDataBoundItems="true" Width="240px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="240px" />
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblDesignation" runat="server" Text="Designation"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px">
                                <eluc:Designation ID="ddlDesignation" runat="server" AppendDataBoundItems="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:RadLabel    ForeColor="Black"   ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td style="padding-left:10px;padding-right:10px"> 
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" EntityType="VSL" AssignedVessels="true" Height="80px"  Width="240px" />
                            </td>
                        </tr>
                    </table>
             </telerik:RadAjaxPanel>
    </form>
</body>
</html>