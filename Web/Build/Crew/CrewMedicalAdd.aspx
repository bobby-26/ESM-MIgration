<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewMedicalAdd.aspx.cs" Inherits="CrewMedicalAdd" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Medical Documents</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewMedical" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuCrewMedical" runat="server" OnTabStripCommand="CrewMedical_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMedical" runat="server" Text="Medical Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard runat="server" ID="ucMedical" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                        HardTypeCode="95" ShortNameFilter="P&I,UKP,FLM,PMU" AutoPostBack="true" OnTextChangedEvent="DisableFlag"
                        Width="154px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblIssueDate" runat="server" Text="Issued"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtIssueDate" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPlaceofissue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" CssClass="input_mandatory" MaxLength="200"
                        Width="150px" ToolTip="Enter Place of issue">
                    </telerik:RadTextBox>
                </td>

                <td>
                    <telerik:RadLabel ID="lblExpiryDate" runat="server" Text="Expiry"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtExpiryDate" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Flag runat="server" ID="ucFlag" Enabled="false" CssClass="input" MedicalRequiredYN="1"
                        AppendDataBoundItems="true" Width="154" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard runat="server" ID="ddlStatus" CssClass="input" AppendDataBoundItems="true"
                        HardTypeCode="105" AutoPostBack="true" OnTextChangedEvent="Status_OnTextChangedEvent"
                        Width="130px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblWeight" runat="server" Text="Weight"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtWeight" runat="server" CssClass="input_mandatory txtNumber" MaxLength="6"
                        Width="150px" AutoPostBack="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text="Verified">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucVerifiedDate" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVerifiedByHeader" runat="server" Text="Verified By">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtVerifiedBy" Width="150px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblVerificationMethod" runat="server" Text="Verification Method">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucVerificationMethod" runat="server" CssClass="input" AppendDataBoundItems="true"
                        Width="130px" QuickTypeCode="131"></eluc:Quick>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadLabel ID="lblNote" runat="server" CssClass="guideline_text">Note: Below fields are mandatory if the status is Temp unfit or Unfit</telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRemarks" runat="server">Remarks</telerik:RadLabel>
                </td>
                <td colspan="2">
                    <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" MaxLength="200" TextMode="MultiLine"
                        Width="360px" Height="40px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMedicalTest" runat="server">Unfit Medical Test</telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadCheckBoxList ID="cblMedicalTest" runat="server" Height="100%" Columns="4"></telerik:RadCheckBoxList>
                    <%--<asp:CheckBoxList ID="cblMedicalTest" runat="server" Height="100%" RepeatColumns="4">
                                </asp:CheckBoxList>--%>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
