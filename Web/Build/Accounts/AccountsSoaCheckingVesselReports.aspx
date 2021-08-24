<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaCheckingVesselReports.aspx.cs" Inherits="AccountsSoaCheckingVesselReports" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOwnersAccounts" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuGenralSub" runat="server" OnTabStripCommand="MenuGenralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuReports" runat="server" OnTabStripCommand="MenuReports_TabStripCommand"></eluc:TabStrip>

            <div>
                <table cellpadding="2" cellspacing="1" style="width: 100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlType" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlType_OnSelectedIndexChanged" Filter="Contains" EmptyMessage="Type to select">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Vessel Variance Report" Value="1" />
                                    <telerik:RadComboBoxItem Text="Vessel Summary Balance" Value="2" />
                                    <telerik:RadComboBoxItem Text="Vessel Trial Balance(Accumulated)" Value="3" />
                                    <telerik:RadComboBoxItem Text="Vessel Trial Balance(YTD) " Value="7" />
                                    <telerik:RadComboBoxItem Text="Vessel Trial Balance(YTD) Owner" Value="8" />
                                    <telerik:RadComboBoxItem Text="Vessel Variance" Value="4" />
                                    <telerik:RadComboBoxItem Text="Statement of Accounts(Summary)" Value="5" />
                                    <telerik:RadComboBoxItem Text="Statement of Accounts(Summary) Subaccount" Value="6" />
                                    <telerik:RadComboBoxItem Text="Statement of Accounts(Summary) Without BudgetGroup" Value="9" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSubType" runat="server" Text="Sub Type"></asp:Literal>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlSubType" runat="server" CssClass="readonlytextbox" Enabled="false" 
                                Filter="Contains" EmptyMessage="Type to select">
                                <Items>
                                    <telerik:RadComboBoxItem Value="0" Text="--Select--" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAsOnDate" runat="server" Text="As On Date" Visible="false"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucAsOnDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblYearMonth" runat="server" Text="Year / Month" Visible="false"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick runat="server" ID="ucYear" QuickTypeCode="55" CssClass="readonlytextbox"
                                Enabled="false" AppendDataBoundItems="true" Visible="false" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="readonlytextbox" Enabled="false" Visible="false"
                            DataSource='<%#PhoenixRegistersHard.ListHardOrderByHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 55) %>'
                            DataTextField="FLDHARDNAME" DataValueField="FLDSHORTNAME" AppendDataBoundItems="true" Filter="Contains" EmptyMessage="Type to select">
                        </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </div>
            <%--<div>
            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px;
                width: 100%;"></iframe>
        </div>--%>
        </div>
    </form>
</body>
</html>
