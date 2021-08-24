<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportAvailabilityReport.aspx.cs"
    Inherits="Crew_CrewReportAvailabilityReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Availability Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

        <script type="text/javascript">
        function resizeFrame() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
        }
        </script>

    </telerik:RadCodeBlock>
</head>
<body onload="resizeFrame()" onresize="resizeFrame()">
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrew">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Crew Availability Report"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                            <asp:Literal ID="lblRank" Text="Rank" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Nationality ID="ucNationality" AppendDataBoundItems="true" runat="server" CssClass="input" />
                                </td>
                                <td>
                            <asp:Literal ID="lblZone" Text="Zone" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Zone ID="ucZone" AppendDataBoundItems="true" runat="server" CssClass="input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                            <asp:Literal ID="lblPool" Text="Pool" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Pool ID="ucPool" AppendDataBoundItems="true" runat="server" CssClass="input" />
                                </td>
                                <td>
                            <asp:Literal ID="lblName" Text="Name" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" ToolTip="Enter the Name" CssClass="input"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:Panel ID="pnlPeriod" runat="server" GroupingText="Period">
                                    <table>
                                    <tr>
                                    <td>
                                        <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal> </td>
                                        <td>
                                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" /></td>
                                        <td><asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal></td>
                                        <td>
                                        <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" /></td>
                                        </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                           </tr>
                           <tr>
                                <td>
                            <asp:Literal ID="lblPrincipal" Text="Principal" runat="server"></asp:Literal>
                                </td>
                                <td>
                                <eluc:Principal ID="ucPrinicpal" runat="server" AddressType="128" AppendDataBoundItems="true" CssClass="input" />
                                </td>
                                <td colspan="2">
                                <asp:Panel ID="pnlInclude" runat="server" GroupingText="Include">
                                <asp:CheckBox ID="chkNewApp" runat="server" Text="New Applicant" />
                                <asp:CheckBox ID="chkInactive" runat="server" Text="Inactive/NTBR/MGR.NTBR" />
                                </asp:Panel>
                                </td>
                                <td colspan="2">
                                    <asp:Panel ID="pnlFormat" runat="server" GroupingText="Formats">
                                        <asp:RadioButtonList ID="rblFormats" OnSelectedIndexChanged="rblFormats_SelectedIndexChanged"
                                            RepeatDirection="Horizontal" runat="server" AutoPostBack="true">
                                            <asp:ListItem>Format1</asp:ListItem>
                                            <asp:ListItem>Format2</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <iframe runat="server" id="ifMoreInfo" style="min-height: 620px; width: 100%; border: 0"
                    scrolling="yes"></iframe>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
