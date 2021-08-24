<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewRecommendedCourseFilter.aspx.cs" Inherits="Crew_CrewRecommendedCourseFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/UserControlTabsTelerik.ascx" TagPrefix="eluc" TagName="TabStrip" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="../UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Recommend Course Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="coursefilter" runat="server" OnTabStripCommand="coursefilter_TabStripCommand"></eluc:TabStrip>
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCourseCode" runat="server" Text="Course Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCourseCode" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtcourseName" runat="server" Width="300px" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ucVessel" runat="server" Visible="false" CssClass="input" AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAvailableFrom" runat="server" Text="Date Between"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                    <telerik:RadLabel ID="lblDash" runat="server" Text="-"></telerik:RadLabel>
                    <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
