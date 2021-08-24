<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInactiveTransferToOfficeFilter.aspx.cs"
    Inherits="CrewInactiveTransferToOfficeFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Src="../UserControls/UserControlRankList.ascx" TagName="UserControlRankList" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlVesselTypeList.ascx" TagName="UserControlVesselTypeList" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlDate.ascx" TagName="UserControlDate" TagPrefix="eluc" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transfer to Office Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuActivityFilterMain" runat="server" OnTabStripCommand="NewApplicantFilterMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <table cellpadding="2" cellspacing="10" >
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblAppliedBetween" runat="server" Text="Applied Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtAppliedStartDate" runat="server" />

                        <eluc:UserControlDate ID="txtAppliedEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRankList ID="lstRank" runat="server" AppendDataBoundItems="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <eluc:UserControlVesselTypeList ID="ddlVesselType" runat="server" AppendDataBoundItems="true"  />
                        <br />                        
                        <telerik:RadCheckBox ID="chkIncludepastexp" runat="server" Text="Include past experience"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
