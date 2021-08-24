<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyScheduleLogFilter.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleLogFilter" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlVesselCertificateCategoryList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Filters</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSurveyScheduleLogFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>

            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="SurveyScheduleLogFilter" runat="server" OnTabStripCommand="SurveyScheduleLogFilter_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divControls">
                <table width="100%" cellspacing="15">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCategory" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Category ID="ucCategory" AppendDataBoundItems="true"  runat="server" Width="250px" height="100px"/>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="pnlperiod" runat="server" GroupingText="Done Date" Width="350px">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFromDate" Text="From" runat="server"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucFromDate" runat="server" />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblToDate" Text="To" runat="server"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucToDate" runat="server"  />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Certificate Issue Date" Width="350px">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblIssueFrom" Text="From" runat="server"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucIssueFrom" runat="server"  />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblIssueTo" Text="To" runat="server"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucIssueTo" runat="server"  />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
