<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitTravelClaimSearch.aspx.cs" Inherits="AccountsVesselVisitTravelClaimSearch" %>

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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Travel Claim Posting" Visible="false"></asp:Label>
            <eluc:TabStrip ID="MenuFilterMain" runat="server" OnTabStripCommand="MenuFilterMain_TabStripCommand" Title="Travel Claim Posting"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1" style="z-index: 2;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNameSearch" runat="server" MaxLength="100" CssClass="input"
                            Width="230px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFormNumber" runat="server" Text="Form Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFormNumber" runat="server" Width="230px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="Visit From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtFromDate" runat="server"/>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="Visit To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtToDate" runat="server" />
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
                        <telerik:RadLabel ID="lblclaimStatus" runat="server" Text="Claim Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="Div2" runat="server" class="input" style="overflow: auto; width: 60%; height: 80px">
                            <asp:CheckBoxList ID="chkClaimStatus" runat="server" Height="100%" OnSelectedIndexChanged="chkClaimStatus_Changed"
                                RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisitType" runat="server" Text="Visit Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="Div3" runat="server" class="input" style="overflow: auto; width: 60%; height: 80px">
                            <asp:CheckBoxList ID="chkVisitType" runat="server" Height="100%" OnSelectedIndexChanged="chkVisitType_Changed"
                                RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="1" Text="IT Visit"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Vessel Visit"></asp:ListItem>
                                <%--  <asp:ListItem Value="3" Text="Riding Superintendent  Visit-India"></asp:ListItem>--%>
                                <asp:ListItem Value="4" Text="Local claim/Business travel"></asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblExpensetype" runat="server" Text="Expense type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlExpensetype" runat="server" Width="230px">
                            <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Local Claim" Value="1"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Business travel" Value="2"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
