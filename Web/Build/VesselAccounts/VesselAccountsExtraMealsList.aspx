<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsExtraMealsList.aspx.cs"
    Inherits="VesselAccountsExtraMealsList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Extra Meals Add</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuExtraMeals" runat="server" OnTabStripCommand="MenuExtraMeals_TabStripCommand"></eluc:TabStrip>

            <table width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccounttype" runat="server" Text="Account type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlAccountType" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Type" Filter="Contains" MarkFirstMatch="true" OnTextChanged="ddlAccountType_OnTextChanged" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="Owners" Value="-1" />
                                <telerik:RadComboBoxItem Text="Charterer" Value="-2" />
                                <telerik:RadComboBoxItem Text="Staff" Value="-3" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="ucExMealsFromDate" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="ucExMealsToDate" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumberOfMandays" runat="server" Text="Number Of Mandays"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="ucMandays" IsInteger="true" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRate" runat="server" Text="Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtRate" CssClass="input_mandatory" runat="server" Width="100px"
                            MaxLength="7" DecimalPlace="2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblServedTo" runat="server" Text="Served To"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtServedTo" runat="server" MaxLength="500" TextMode="MultiLine" Width="800px" Height="200" CssClass="input_mandatory"
                            Font-Bold="true" Rows="12" Columns="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActualVictuallingRate" runat="server" Text="Actual Victualling Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkVictualRate" runat="server" OnCheckedChanged="chkVictualRate_OnCheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
