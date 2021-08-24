<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReviewPlannerManual.aspx.cs" Inherits="InspectionReviewPlannerManual" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Manual Audit/Inspection</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmReviewPlanner" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title ID="ucTitle" runat="server" Text="Add Manual Audit/Inspection" ShowMenu="false" Visible="false" />
        <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>
        <br />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory" VesselsOnly="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblAuditInspectionCategory" runat="server" Text="Audit / Inspection Category"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true"
                        AutoPostBack="true" HardTypeCode="144"
                        OnTextChangedEvent="Bind_UserControls" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlAudit" runat="server" CssClass="input_mandatory" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlAudit_SelectedIndexChanged">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblInspectingCompany" runat="server" Text="Inspecting Company"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlCompany" runat="server" Enabled="false" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text="Last Done Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtLastDoneDate" runat="server" DatePicker="true" AutoPostBack="true" OnTextChangedEvent="txtLastDoneDate_Changed" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDueDate" runat="server" Text="Due Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtDueDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
