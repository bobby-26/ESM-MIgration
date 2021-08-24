<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditDetailEdit.aspx.cs" Inherits="InspectionAuditDetailEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuRegistersInspection" runat="server" OnTabStripCommand="RegistersInspection_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellspacing="3">
                <tr>
                    <td valign="top" width="15%">
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td valign="top" width="35%">
                        <eluc:Company ID="ucCompany" runat="server" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' Enabled="false" Width="240px"
                            CssClass="input_mandatory" AppendDataBoundItems="true" />
                    </td>
                    <td valign="top" width="15%">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td valign="top" width="35%">
                        <eluc:Hard ID="ucInspectionCategoryAdd" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Width="240px"
                            CssClass="dropdown_mandatory" HardList="<%# PhoenixRegistersHard.ListHard(1,144) %>" HardTypeCode="144" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortcode" runat="server" Text="Short Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtShortcode" runat="server" CssClass="input_mandatory" Text="" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input_mandatory" Text="" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveyn" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkActiveYN" runat="server"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFrequency" runat="server" Text="Frequency (in Months)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtFrequency" runat="server" MaxLength="3" CssClass="input_mandatory" IsPositive="true" IsInteger="true"></eluc:Number>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAddtoSchedule" runat="server" Text="Add to Schedule"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAddToScheduleYN" runat="server"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOfficeAudit" runat="server" Text="Office Audit"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOfficeAuditYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkOfficeAuditYN_CheckedChanged"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWindowBefore" runat="server" Text="Window Before"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucWindowBefore" runat="server" MaxLength="1" IsPositive="true" IsInteger="true"></eluc:Number>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWindowAfter" runat="server" Text="Window After"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucWindowAfter" runat="server" MaxLength="1" IsPositive="true" IsInteger="true"></eluc:Number>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInspectionType" runat="server" MaxLength="20" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblQuestionType" runat="server" Text="Question Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlQuestionTypeAdd" runat="server" Width="240px" Filter="Contains"
                            CssClass="input_mandatory" EnableDirectionDetection="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr >
                     <td valign="top">
                        <telerik:RadLabel ID="lbllettercode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td valign="top" >
                        <telerik:RadTextBox ID="txtlettercode" runat="server" MaxLength="10" Width="120px" CssClass="input_mandatory">
                        </telerik:RadTextBox>
                    </td>

                    <td valign="top">
                        <telerik:RadLabel ID="lblAssessment" runat="server" Text="Assessment Standards"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadTextBox ID="txtAssessmentStd" runat="server" Text="" Width="240px" Height ="80px"
                          TextMode="MultiLine" Resize="Both" >
                        </telerik:RadTextBox>
                    </td>

                   

                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
