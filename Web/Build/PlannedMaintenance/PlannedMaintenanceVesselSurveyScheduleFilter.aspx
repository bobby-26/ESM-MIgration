<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyScheduleFilter.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
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
    <title>Filters</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvscriptsk">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmSurveyScheduleFilter" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSurveyScheduleFilter">
        <ContentTemplate>
            <div class="subHeader">
                <asp:Literal ID="lblSurveyScheduleFilter" runat="server" Text="Filters"></asp:Literal>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="SurveyScheduleFilter" runat="server" OnTabStripCommand="SurveyScheduleFilter_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divControls">
                <table width="100%" cellspacing="15">
                    <tr>
                        <td style="width: 70px">
                            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" CssClass="input_mandatory" runat="server" AppendDataBoundItems="true"
                                Width="250px" />
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:Label ID="lblDue" runat="server" Text="Status"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDue" runat="server" CssClass="input" Width="250px">
                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                <asp:ListItem Text="Due" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Overdue" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Due in 30 Days" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Due in 60 Days" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCategory" runat="server" Text="Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Category ID="ucCategory" AppendDataBoundItems="true" CssClass="input" runat="server"
                                Width="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px">
                            <asp:Literal ID="lblStatus" runat="server" Text="Show Planned"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkShowPlanned" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td style="width: 70px">
                            <asp:Literal ID="lblShowAll" runat="server" Text="Show All"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkShowAll" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnlperiod" runat="server" GroupingText="Due Date" Width="350px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblFromDate" Text="From" runat="server"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="lblToDate" Text="To" runat="server"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucToDate" runat="server" CssClass="input" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnlPlan" runat="server" GroupingText="Planned Date" Width="350px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblPlanFrom" Text="From" runat="server"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucPlanFrom" runat="server" CssClass="input" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="lblPlanTo" Text="To" runat="server"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucPlanTo" runat="server" CssClass="input" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
