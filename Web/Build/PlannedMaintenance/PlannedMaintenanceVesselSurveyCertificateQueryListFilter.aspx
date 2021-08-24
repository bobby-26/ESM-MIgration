<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateQueryListFilter.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateQueryListFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Certificate" Src="~/UserControls/UserControlCertificate.ascx" %>
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
    <form id="frmSurveyCertificateFilter" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSurveyCertificateFilter">
        <ContentTemplate>
            <div class="subHeader">
                <asp:Literal ID="lblSurveyCertificateFilter" runat="server" Text="Filters"></asp:Literal>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="SurveyCertificateFilter" runat="server" OnTabStripCommand="SurveyCertificateFilter_TabStripCommand">
                </eluc:TabStrip>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            </div>
            <table width="100%" cellspacing="15">
                <tr>
                    <td width="100px">
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" CssClass="input_mandatory" runat="server" AppendDataBoundItems="true"
                            Width="230px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblSurveyType" runat="server" Text="Survey Type"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSurveyType" runat="server" CssClass="input" Width="230px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblSurveyNumber" runat="server" Text="Survey Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSurveyNumber" CssClass="input" Width="230px"></asp:TextBox>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td width="150px">
                        <asp:Literal ID="lblEndorsementYN" runat="server" Text="Show Endorsements"></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEndorse" runat="server" />
                    </td>
                </tr>
                <tr >
                    <td width="150px">
                        <asp:Literal ID="lblVesselNotapplicable" runat="server" Text="Show not Applicable"></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkShowNotApplicable" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblIssueDate" runat="server" Text="Issue Date"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="lblFromDate" Text="From" runat="server"></asp:Literal>
                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input" />
                        <asp:Literal ID="lblToDate" Text="To" runat="server"></asp:Literal>
                        <eluc:Date ID="ucToDate" runat="server" CssClass="input" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
