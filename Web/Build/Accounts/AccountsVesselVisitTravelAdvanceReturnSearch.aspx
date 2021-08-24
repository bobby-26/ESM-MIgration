<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitTravelAdvanceReturnSearch.aspx.cs"
    Inherits="AccountsVesselVisitTravelAdvanceReturnSearch" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Travel Advance / Return" Visible="false"></asp:Label>
            <eluc:TabStrip ID="MenuFilterMain" runat="server" OnTabStripCommand="MenuFilterMain_TabStripCommand" Title="Travel Advance / Return"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1" style="z-index: 2;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNameSearch" runat="server" MaxLength="100" CssClass="input"
                            Width="60%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbladvanceNumber" runat="server" Text="Travel Advance Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAdvanceNumber" runat="server" MaxLength="100" CssClass="input"
                            Width="60%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisitId" runat="server" Text="Vessel Visit Id"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVisitId" runat="server" MaxLength="100" CssClass="input"
                            Width="60%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblpurpose" runat="server" Text="Purpose of Visit"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPurpose" runat="server" MaxLength="100" CssClass="input"
                            Width="60%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="dvVessel" runat="server" class="input" style="overflow: auto; width: 60%; height: 80px">
                            <asp:CheckBoxList ID="chkVesselList" runat="server" Height="100%" OnSelectedIndexChanged="chkVesselList_Changed"
                                RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="Div1" runat="server" class="input" style="overflow: auto; width: 60%; height: 80px">
                            <asp:CheckBoxList ID="chkCurrencyList" runat="server" Height="100%" OnSelectedIndexChanged="chkCurrencyList_Changed"
                                RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblclaimStatus" runat="server" Text="Claim Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="Div2" runat="server" class="input" style="overflow: auto; width: 60%; height: 80px">
                            <asp:CheckBoxList ID="chkClaimStatus" runat="server" Height="100%" OnSelectedIndexChanged="chkClaimStatus_Changed"
                                RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAdvanceStatus" runat="server" Text="Advance Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucAdvanceStatus" runat="server" QuickTypeCode="133" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
