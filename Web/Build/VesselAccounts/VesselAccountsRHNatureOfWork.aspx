<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHNatureOfWork.aspx.cs"
    Inherits="VesselAccountsRHNatureOfWork" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .RadRadioButtonList.rbHorizontalList .RadRadioButton {
                min-width: 100px;
                text-align: left;
            }
        </style>
        <style>
            .RadCheckBoxList span.rbText.rbToggleCheckbox {
                text-align: left;
            }
        </style>
        <%--<style type="text/css">
            .RadCheckBox {
                width: 99% !important;
            }

            .rbText {
                text-align: left;
                width: 89% !important;
            }

            .rbVerticalList {
                width: 32% !important;
            }
        </style>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRestHourWorkCalenderRemarks" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:TabStrip ID="MenuRHGeneral" runat="server" OnTabStripCommand="RHGeneral_TabStripCommand" Title="Nature of Work"></eluc:TabStrip>
        <br />
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmpName" runat="server" CssClass="input" Enabled="false" Width="240px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input"
                        Enabled="false" Width="240px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtDate" runat="server" CssClass="input" Enabled="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblHour" runat="server" Text="Hour"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtHour" runat="server" CssClass="input txtNumber" MaxLength="2"
                        Enabled="false" Width="80px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReportingDay" runat="server" Text="Reporting Day"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtReportingday" runat="server" CssClass="input txtNumber" Enabled="false" Width="80px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadRadioButtonList ID="rbtnadvanceretard" runat="server" Enabled="false" Direction="Horizontal">
                        <Items>
                            <telerik:ButtonListItem Text="IDL W-E" Value="1" />
                            <telerik:ButtonListItem Text="IDL E-W" Value="2" />
                            <telerik:ButtonListItem Text="None" Value="0" />
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td colspan="2">
                    <telerik:RadRadioButtonList ID="rbtnworkplace" runat="server" Enabled="false" Direction="Horizontal">
                        <Items>
                            <telerik:ButtonListItem Text="Work at Port" Value="1" />
                            <telerik:ButtonListItem Text="Work at Sea" Value="2" />
                            <telerik:ButtonListItem Text="Work at Sea/Port" Value="3" />
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadRadioButtonList ID="rbnhourchange" runat="server" Enabled="false" Direction="Horizontal">
                        <Items>
                            <telerik:ButtonListItem Text="Advance" Value="1" />
                            <telerik:ButtonListItem Text="Retard" Value="2" />
                            <telerik:ButtonListItem Text="Reset" Value="0" Selected="True" />
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td colspan="2">
                    <telerik:RadRadioButtonList ID="rbnhourvalue" runat="server" Enabled="false" Direction="Horizontal">
                        <Items>
                            <telerik:ButtonListItem Text="0.5 Hour" Value="1" />
                            <telerik:ButtonListItem Text="1.0 Hour" Value="2" />
                            <telerik:ButtonListItem Text="2.0 Hour" Value="3" />
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
            </tr>
        </table>
        <table width="95%" cellpadding="1" cellspacing="1">
            <tr valign="top">
                <th>
                    <telerik:RadLabel ID="lblNatureofWork" runat="server" Text="Nature of Work"></telerik:RadLabel>
                </th>
            </tr>
            <tr valign="top">
                <td>
                    <telerik:RadCheckBoxList runat="server" ID="chknatureofwork" Columns="6" AutoPostBack="false">
                    </telerik:RadCheckBoxList>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
