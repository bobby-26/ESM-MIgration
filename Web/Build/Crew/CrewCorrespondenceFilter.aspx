<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCorrespondenceFilter.aspx.cs"
    Inherits="CrewCorrespondenceFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:TabStrip ID="MenuCorrespondenceFilterMain" runat="server" OnTabStripCommand="CorrespondenceFilterMain_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtSubject" runat="server" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCorrespondenceType" runat="server" Text="Correspondence Type"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Quick ID="ddlCorrespondenceType" runat="server" QuickTypeCode="11" Width="45%"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>



    </form>
</body>
</html>
