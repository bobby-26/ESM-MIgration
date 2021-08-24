<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAcademic.aspx.cs" Inherits="CrewAcademic" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Qualificaiton" Src="~/UserControls/UserControlQualification.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Academic Details</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewAcademic" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />

        <eluc:TabStrip ID="MenuCrewAcademic" runat="server" OnTabStripCommand="CrewAcademic_TabStripCommand"></eluc:TabStrip>

        <table cellpadding="1" cellspacing="1" width="90%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblQualification" runat="server" Text="Qualification"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Qualificaiton ID="ddlCertificate" runat="server" CssClass="input_mandatory"
                        AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInstitution" runat="server" Text="Institution"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtInstitution" runat="server" CssClass="input_mandatory" MaxLength="200"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Country runat="server" ID="ucCountry" AutoPostBack="true" AppendDataBoundItems="true"
                        CssClass="input" OnTextChangedEvent="ucCountry_TextChanged" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblState" runat="server" Text="State"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:State ID="ucState" CssClass="input" runat="server" AppendDataBoundItems="true"
                        AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPlaceofInstitution" runat="server" Text="Place of Institution"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:City ID="ddlPlace" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" Width="130px" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" Width="130px" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPassDate" runat="server" Text="Pass Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtPassDate" runat="server" Width="130px" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPercentage" runat="server" Text="Percentage"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtPercentage" runat="server" CssClass="input txtNumber" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblGrade" runat="server" Text="Grade"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtGrade" runat="server" CssClass="input" MaxLength="1"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
