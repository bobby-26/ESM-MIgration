<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateDetails.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCLQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Certificate Details</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvscriptsk">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="CertificatesMaping" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlFBQOptions">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblTitle" runat="server" Text="Details"></asp:Literal>
                    </div>
                </div>
                <div id="divControls">
                    <table width="100%" cellspacing="10">
                        <tr>
                            <td>
                                <asp:Literal ID="lblCertificate" runat="server" Text="Certificate"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCertificate" runat="server" Width="350px" CssClass="readonlytextbox"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblCertificateCode" runat="server" Text="Certificate Code"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCertificateCode" runat="server" Width="100px" CssClass="readonlytextbox"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCategory" runat="server" Text="Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCategory" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="350px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNumber" runat="server" Width="100px" CssClass="readonlytextbox"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblIssueAuthority" runat="server" Text="Issuing Authority"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIssueAuthority" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="350px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblIssueDate" runat="server" Text="Issue Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucIssueDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPlaceofIssue" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="350px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblExpiryDate" runat="server" Text="Expiry Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucExpiryDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblLastSurveyType" runat="server" Text="Last Survey Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastSurveyType" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="350px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblRemarksType" runat="server" Text="Full Term / Short term / Interim / Provisional"
                                    Width="100px"></asp:Label>
                            </td>
                            <td>
                                 <asp:TextBox ID="txtRemarksType" runat="server" Width="100px" CssClass="readonlytextbox"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSurveyorName" runat="server" Text="Name of Auditor / Surveyor"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSurveyorName" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="350px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblLastSurveyDate" runat="server" Text="Last Survey Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucLastSurveyDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSurveyPort" runat="server" Text="Port of Audit / Survey"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSurveyPort" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="350px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblUpdateAuditLog" runat="server" Text="Update Audit Log"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkAuditLog" runat="server" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRemaks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="350px" Height="30px"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblNotApplicable" runat="server" Text="Not applicable for this vessel"
                                    Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkNotApplicable" runat="server" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblReason" runat="server" Text="Reason why not applicable"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReason" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="350px" Height="30px"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblAttachYN" runat="server" Text="Is uploaded Attachment correct"
                                    Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkAttachYN" runat="server" Enabled="false"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
