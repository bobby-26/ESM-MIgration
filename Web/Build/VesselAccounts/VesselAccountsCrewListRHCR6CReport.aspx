<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCrewListRHCR6CReport.aspx.cs" Inherits="VesselAccountsCrewListRHCR6CReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CR6C Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmcr6breport" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel1" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table runat="server" width="100%" id="tblReport">
                <tr>
                    <td width="5%">
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel : "></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" AutoPostBack="true" EntityType="VSL" ActiveVessels="true" Width="60%" />
                    </td>
                    <td width="13%">
                        <telerik:RadLabel ID="lblReportfortheMonthof" runat="server" Text="Report for the Month of :"></telerik:RadLabel>
                    </td>
                    <td width="32%">
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true" Filter="Contains" Width="50%" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                            <%--<Items>
                                <telerik:RadComboBoxItem Text="January" Value="1" />
                                <telerik:RadComboBoxItem Text="February" Value="2" />
                                <telerik:RadComboBoxItem Text="March" Value="3" />
                                <telerik:RadComboBoxItem Text="April" Value="4" />
                                <telerik:RadComboBoxItem Text="May" Value="5" />
                                <telerik:RadComboBoxItem Text="June" Value="6" />
                                <telerik:RadComboBoxItem Text="July" Value="7" />
                                <telerik:RadComboBoxItem Text="August" Value="8" />
                                <telerik:RadComboBoxItem Text="September" Value="9" />
                                <telerik:RadComboBoxItem Text="October" Value="10" />
                                <telerik:RadComboBoxItem Text="November" Value="11" />
                                <telerik:RadComboBoxItem Text="December" Value="12" />
                            </Items>--%>
                        </telerik:RadComboBox>
                    </td>
                    <%--<td width="5%">
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year :"></telerik:RadLabel>
                    </td>--%>
                    <%--<td width="30%">
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            Filter="Contains" OnDataBound="ddlYear_DataBound" Sort="Descending">
                        </telerik:RadComboBox>
                    </td>--%>
                </tr>
            </table>
            <iframe runat="server" id="ifMoreInfo" scrolling="yes" frameborder="0" style="min-height:96%; width: 100%"></iframe> 
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
