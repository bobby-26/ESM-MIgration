<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyScheduleComplete.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleComplete" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiInspector" Src="~/UserControls/UserControlMultiColumnInspector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Survey</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvscripts">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmSurveyScheduleComplte" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSurveyScheduleComplte">
        <ContentTemplate>
            <div class="subHeader">
                <asp:Literal ID="lblSurveyScheduleComplte" runat="server" Text="Report Survey"></asp:Literal>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="SurveyScheduleComplte" runat="server" OnTabStripCommand="SurveyScheduleComplte_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divControls">
                <table width="100%" cellspacing="15">
                    <tr>
                        <td>
                            <asp:Literal ID="lblSurveyNumber" runat="server" Text="Survey Number"></asp:Literal>
                            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyNumber" runat="server" Width="150px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSurvey" runat="server" Text="Survey"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurvey" runat="server" Width="250px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSurveyType" runat="server" Text="Survey Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyType" runat="server" Width="150px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSurveyorNameDesignation" runat="server" Text="Surveyor Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyorName" runat="server" CssClass="input" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" Width="150px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCompany" runat="server" Text="Company"></asp:Literal>
                        </td>
                        <td>
                            <eluc:AddrType ID="ddlCompany" runat="server" AddressType="136" AddressList='<%# PhoenixRegistersAddress.ListAddress("136") %>'
                                AppendDataBoundItems="true" CssClass="input" Width="255px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSurveyorDesignation" runat="server" Text="Surveyor Designation"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyorDesignation" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblAttendingSupt" runat="server" Text="Attending Supt"></asp:Literal>
                        </td>
                        <td>
                            <eluc:MultiInspector ID="ucSurveyor" runat="server" Width="250px" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblComplte" runat="server" Text="Survey Done Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucComplteDate" runat="server" CssClass="input_mandatory" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
