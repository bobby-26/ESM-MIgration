<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyAuditPlanner.aspx.cs"
    Inherits="PlannedMaintenanceVesselSurveyAuditPlanner" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmReviewPlanner" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBudgetGroup">
        <ContentTemplate>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="div1" style="vertical-align: top">
                        <eluc:Title ID="ucTitle" runat="server" Text="Add Manual Audit/Inspection" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPlanAudit" runat="server" OnTabStripCommand="MenuPlanAudit_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="SubHeader" style="right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory"
                                VesselsOnly="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblAuditInspectionCategory" runat="server" Text="Audit / Inspection Category"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input" HardTypeCode="144" OnTextChangedEvent="Bind_UserControls" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAudit" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlAudit_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
