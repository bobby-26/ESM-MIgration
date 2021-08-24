<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewRetentionReportFilter.aspx.cs" Inherits="Crew_CrewRetentionReportFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       
    </telerik:RadCodeBlock>
      <style>
         #ucRank_divRankList {
            overflow-y: hidden !important;
        }
          #ucVessel_divVesselList {
            width: 240px;
        }
          #ucPrincipal_chkboxlist {
              overflow-y: hidden !important;
          }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
       
         <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand">
        </eluc:TabStrip>
        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
    
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <br />
                <table cellpadding="2" cellspacing="2" >
                    <tr>
                        <td style="padding-left:10px;padding-right:10px">
                            <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                        <br /> <br /> 
                            <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                        </td>
                        <td style="padding-left:10px;padding-right:10px">
                            <eluc:Date ID="txtFrom" runat="server" Width="270px" />
                          <br /> <br />
                            <eluc:Date ID="txtTo" runat="server"  Width="270px" />
                        </td>
                        <td style="padding-left:10px;padding-right:10px">
                            <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                        </td>
                        <td style="padding-left:10px;padding-right:10px;">
                            <telerik:RadListBox ID="ddlYear" runat="server" Height="85px" ></telerik:RadListBox>
                        </td>
                        </tr>
                    <tr>
                       <td style="padding-left:10px;padding-right:10px">
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td style="padding-left:10px;padding-right:10px">
                             <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true"  Width="270px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:10px;padding-right:10px">
                            <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                        </td>
                        <td style="padding-left:10px;padding-right:10px">
                            <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" Width="270px" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:10px;padding-right:10px">
                            <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                        </td>
                        <td style="padding-left:10px;padding-right:10px">
                            <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="270px"  />
                        </td>
                    </tr>
                  
                </table>
          
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>