<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportsMonthlyConsolidatedWCT.aspx.cs" Inherits="AccountsReportsMonthlyConsolidatedWCT" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>TDS Summary</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLedgerGeneral" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" RenderMode="Lightweight" EnableShadow="true"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" />
            <eluc:TabStrip ID="Menuledger" runat="server" OnTabStripCommand="Menuledger_TabStripCommand"></eluc:TabStrip>
            <telerik:RadLabel ID="lblYearMonth" runat="server" Text="Year / Month"></telerik:RadLabel>
            <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory"
                AppendDataBoundItems="true" />
            <telerik:RadDropDownList ID="ddlMonth" runat="server" CssClass="dropdown_mandatory">
                <Items>
                    <telerik:DropDownListItem Value="" Text="--Select--"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="1" Text="Jan"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="2" Text="Feb"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="3" Text="Mar"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="4" Text="Apr"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="5" Text="May"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="6" Text="Jun"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="7" Text="Jul"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="8" Text="Aug"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="9" Text="Sep"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="10" Text="Oct"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="11" Text="Nov"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Value="12" Text="Dec"></telerik:DropDownListItem>
                </Items>
            </telerik:RadDropDownList>
            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 524px; width: 99.5%;"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
