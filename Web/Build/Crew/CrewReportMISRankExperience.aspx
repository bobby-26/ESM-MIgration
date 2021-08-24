<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMISRankExperience.aspx.cs"
    Inherits="Crew_CrewReportMISRankExperience" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MIS Rank Experience Stastics</title>
 <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
     <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                
                            <table width="100%">
                            <tr>
                                <td>
                                    <telerik:radlabel ID="lblManager" runat="server" Text="Manager"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Manager ID="ucManager" AddressType="126" runat="server" AppendDataBoundItems="true"  Width="180px"  />
                                </td>
                                <td rowspan="3" colspan="2">
                                    <asp:Panel ID="pnlExperience" GroupingText="Experience" runat="server"  Width="120px" >
                                        <telerik:RadRadioButtonList ID="rblSelection" runat="server" AutoPostBack="false">
                                            <Items>
                                            <telerik:ButtonListItem Value="1" text="Greater Than" />
                                            <telerik:ButtonListItem Value="2" text="Less Than"  />
                                                </Items>
                                        </telerik:RadRadioButtonList>
                                        <br />
                                        <telerik:RadTextBox ID="txtExperience" runat="server" Width="40px" CssClass="input_mandatory"    text="0"    ></telerik:RadTextBox>
                                        &nbsp Months</asp:Panel>
                                </td>
                                <td rowspan="3">
                                    <telerik:radlabel ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                                </td>
                                <td rowspan="3">
                                    <eluc:Rank ID="ucRank" AppendDataBoundItems="true" runat="server" Width="180px" />
                                </td>
                                <td rowspan="3">
                                    <telerik:radlabel ID="lblNationality" runat="server" Text="Nationality"></telerik:radlabel>
                                </td>
                                <td rowspan="3">
                                    <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true"  Width="180px" />
                                </td>
                                <td rowspan="3">
                                    <telerik:radlabel ID="lblPool" runat="server" Text="Pool"></telerik:radlabel>
                                </td>
                                <td rowspan="3">
                                     <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true"  Width="180px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:radlabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128"  Width="180px"     AppendDataBoundItems="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                     <telerik:radlabel ID="lblStatus" runat="server" Text="Status"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Hard ID="ddlSelect" runat="server" HardTypeCode="54" ShortNameFilter="ONB,ONL"
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" SortByShortName="true"  Width="180px"  />
                                </td>
                            </tr>                            
                        </table>
                   
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                
                        <table width="100%">
                        <tr>
                            <td align="left">
                                <telerik:radlabel ID="ltGrid" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkDetails" runat="server" Text="Click Here To See Seafarers Details"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                 
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>