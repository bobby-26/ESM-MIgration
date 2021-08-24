<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReportRHNonCompliance.aspx.cs" Inherits="VesselAccountsReportRHNonCompliance" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RHCrew" Src="~/UserControls/UserControlRestHourEmployee.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rest Hours NonCompliance</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmNonCompliance" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="93%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuReportNonCompliance" runat="server" OnTabStripCommand="MenuReportNonCompliance_TabStripCommand"
                 TabStrip="true" ></eluc:TabStrip>
            <table width="60%">
                <tr>
                    <%--<td>
                            Employee Name
                        </td>
                        <td>
                            <eluc:RHCrew ID="ddlEmployee" runat="server" CssClass="input" AppendDataBoundItems="true"  DefaultText="All Employees"/>
                        </td>--%>
                    <td>
                        <telerik:RadLabel ID="lblReportfortheMonthof" runat="server" Text="Report for the Month of :"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true" Filter="Contains">
                            <Items>
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
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year :"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input_mandatory" AutoPostBack="true" 
                            Filter="Contains" OnDataBound="ddlYear_DataBound" Sort="Descending">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 95%; width: 100%" frameborder="0"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
