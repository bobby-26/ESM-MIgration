<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentListFilter.aspx.cs" Inherits="Accounts_AccountsAllotmentListFilter" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EntryType" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlPool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Allotment List Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function OnClientSelectedIndexChanged(sender, args) {
                args.get_item().set_checked(args.get_item().get_selected());
            }
        </script>

        <script type="text/javascript">
            function OnClientItemChecked(sender, args) {
                args.get_item().set_selected(args.get_item().get_checked());
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:TabStrip ID="MenuActivityFilterMain" runat="server" OnTabStripCommand="AccountsAllotmentList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td colspan="4">
                       <%-- <telerik:RadButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />--%>
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                            Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Month ID="ddlMonth" runat="server" Width="180px" AppendDataBoundItems="true"></eluc:Month>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>

                    <td>

                        <eluc:Year ID="ddlyear" runat="server" Width="180px" OrderByAsc="false" AppendDataBoundItems="true"></eluc:Year>
                        <%--  <eluc:Year runat="server" YearStartFrom="2005" NoofYearFromCurrent="0" ID="radcbyear" OrderByAsc="false"  Width="180px" AppendDataBoundItems="true" />--%>

                        <%--<telerik:RadComboBox ID="ddlYear" runat="server" MarkFirstMatch="true" Width="180px" AllowCustomText="true"></telerik:RadComboBox>--%>
                    </td>


                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblManningOffice" runat="server" Text="Manning Office"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ucZone" runat="server" CssClass="input" Width="180px" AppendDataBoundItems="true"></eluc:Zone>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblPaymentCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="180px"
                            runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" CssClass="input" AppendDataBoundItems="true" Width="180px" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemittingAgent" runat="server" Text="Remitting Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address runat="server" ID="ucOwner" AddressType="128" AppendDataBoundItems="true" AutoPostBack="true" Width="180px" />

                    </td>

                    <td>
                        <telerik:RadLabel ID="lblBankCountry" runat="server" Text="Bank Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ddlCountry" runat="server" CssClass="input" Width="180px" AppendDataBoundItems="true"></eluc:Country>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAllotmentType" runat="server" Text="Allotment Type"></telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Hard ID="ddlAllotmentType" runat="server" AppendDataBoundItems="true"
                            HardTypeCode="239" Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucRequestStatus" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            HardTypeCode="238" ShortNameFilter="PVC,CBA,CND,PDA,PRA,APD,PVG" Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwnerPool" runat="server" Text="Owner Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlPool ID="ucPool" runat="server" CssClass="input" AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>

            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

