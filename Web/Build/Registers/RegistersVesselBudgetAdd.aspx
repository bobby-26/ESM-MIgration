<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselBudgetAdd.aspx.cs"
    Inherits="RegistersVesselBudgetAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Budget</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersBudget" DecoratedControls="All" />
    <form id="frmRegistersBudget" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuDetails" runat="server" OnTabStripCommand="MenuDetails_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblrank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRankAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="240px" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblbudget" runat="server" Text="Budgeted Wage"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucWage" runat="server" CssClass="input_mandatory" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblpreferdnatnl" runat="server" Text="Preferred Nationality"></telerik:RadLabel>

                    </td>
                    <td>
                        <div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden; height: 120px; width: 240px;">
                            <telerik:RadListBox RenderMode="Lightweight" ID="cblNationality" CheckBoxes="true" runat="server" Width="240px">
                            </telerik:RadListBox>
                        </div>

                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
