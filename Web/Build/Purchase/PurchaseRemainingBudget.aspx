<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseRemainingBudget.aspx.cs"
    Inherits="PurchaseRemainingBudget" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Approve</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .table {
                border-collapse: collapse;
            }

                .table td, th {
                    border: 1px solid black;
                }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStockItemFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; border: none; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />

            <div style="font-weight: 600; font-size: 12px">
                <eluc:TabStrip ID="MenuFormFilter" runat="server" OnTabStripCommand="MenuFormFilter_TabStripCommand"></eluc:TabStrip>
            </div>
            <br clear="all" />
            <telerik:RadPanelBar RenderMode="Lightweight" runat="server" ID="RadPanelBar1" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Mail" Expanded="false">
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="lblBudgetGroup"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="80%" class="table" style="margin-left: 10px;">
                                <tr>
                                    <td style="width: 80%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblVarianceDesc" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 20%; text-align: right">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblVariance" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 80%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblBudgetDesc" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 20%; text-align: right">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblBudget" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 80%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblChaCommittedDesc" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 20%; text-align: right">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblChaCommitted" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 80%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblActualDesc" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 20%; text-align: right; color: red;">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblActual" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 80%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblCurrentDesc" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 20%; text-align: right; color: red;">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblCurrent" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 80%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblAppNotOrderDesc" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 20%; text-align: right; color: red;">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblAppNotOrder" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>
            <table width="90%" class="table">
                <tr>
                    <td style="width: 80%">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblMonthlyRemainigDesc" runat="server"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%; text-align: right;">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblMonthlyRemainig" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 80%">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblYTDRemainigDesc" runat="server"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%; text-align: right;">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblYTDRemainig" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 80%">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTechMonthlyRemainigDesc" runat="server"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%; text-align: right;">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTechMonthlyRemainig" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 80%">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTechYTDRemainigDesc" runat="server"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%; text-align: right;">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTechYTDRemainig" runat="server"></telerik:RadLabel>
                    </td>
                </tr>

            </table>
        </div>

    </form>
</body>
</html>
