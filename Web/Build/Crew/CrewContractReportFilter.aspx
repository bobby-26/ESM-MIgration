<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewContractReportFilter.aspx.cs" Inherits="Crew_CrewContractReportFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOff" Src="~/UserControls/UserControlSignOffReasonList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>       
    <style>
        #ucVessel_divVesselList {
            width: 240px;
        }
        #ucPrincipal_chkboxlist, #ucRank_divRankList {
            overflow-y: hidden !important;
        }
        
    </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand">
        </eluc:TabStrip>
        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
    
            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td style="padding-left:10px">
                            <telerik:radlabel ForeColor="Black" ID="lblFileNumber" runat="server" Text="File No"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFileNumber" runat="server" Width="240px" MaxLength="10"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:radlabel  ForeColor="Black" ID="lblYear" runat="server" Text="Year"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadListBox ID="ddlYear" SelectionMode="Multiple" AppendDataBoundItems="true"  Width="240px" Height="80px" runat="server"></telerik:RadListBox>
                        </td>
                    </tr>
                    <tr>
                       <td style="padding-left:10px">
                            <telerik:radlabel  ForeColor="Black" ID="lblPrincipal" runat="server" Text="Principal"></telerik:radlabel>
                        </td>
                        <td>
                            <div style="overflow: hidden;">
                                <eluc:Principal ID="ucPrincipal" runat="server" AppendDataBoundItems="true"  AddressType="128" Width="240px"/>
                            </div>
                        </td>
                        <td>
                            <telerik:radlabel  ForeColor="Black" ID="lblVessel" runat="server" Text="Vessel">
                            </telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" EntityType="VSL" AssignedVessels="true"/>
                        </td>
                    </tr>
                   
                    <tr>
                        <td style="padding-left:10px">
                            <telerik:radlabel  ForeColor="Black" ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true"  Width="240px" />
                        </td>
                         <td>
                            <telerik:radlabel  ForeColor="Black" ID="lblZone" runat="server" Text="Zone"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="240px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:10px">
                            <telerik:radlabel ForeColor="Black"  ID="lblPool" runat="server" Text="Pool"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="240px" />
                        </td>
                        <td>
                            <telerik:radlabel  ForeColor="Black" ID="lblSignOff" runat="server" Text="Sign Off Reason"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:SignOff ID="ucSignOff" runat="server" AppendDataBoundItems="true" Width="240px" />
                        </td>
                    </tr>
                   
                </table>
         </telerik:RadAjaxPanel>
    </form>
</body>
</html>