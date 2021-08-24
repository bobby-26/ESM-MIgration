<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCOCFilter.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyCOCFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Certificate" Src="~/UserControls/UserControlCertificate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlSurveyStatus.ascx" %>
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
    <form id="frmSurveyCOCFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="SurveyCOCFilter" runat="server" OnTabStripCommand="SurveyCOCFilter_TabStripCommand"></eluc:TabStrip>
        </div>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="divControls">
                <table width="100%" >
                    <tr runat="server" visible="false">
                        <td>
                            <telerik:RadLabel ID="lblSurveyType" runat="server" Text="Survey Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlSurveyType" runat="server"  Width="250px">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblCertificate" runat="server" Text="Certificate" ></telerik:RadLabel>
                        </td>
                        <td width="90%">
                            <telerik:RadComboBox ID="ucCertificate" runat="server" 
                               MarkFirstMatch="true" AppendDataBoundItems="true" Width="250px" EnableLoadOnDemand="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Status ID="ddlStatus" runat="server"  AppendDataBoundItems="true" Width="250px" />
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="pnlperiod" runat="server" GroupingText="Due Date" Width="350px">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFromDate" Text="From" runat="server"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucFromDate" runat="server"  />
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
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
