<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewContractExtention.aspx.cs" Inherits="CrewContractExtention" ValidateRequest="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeniorityScale" Src="~/UserControls/UserControlSeniorityScale.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlContractCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CrewComponents" Src="~/UserControls/UserControlContractCrew.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Contract Extension</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .checkRtl {
            direction: rtl;
        }

        .fon {
            font-size: small !important;
        }
    </style>

</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmInActive" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmInActive" runat="server">
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
      
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%" CssClass="scrolpan">
            <eluc:TabStrip ID="MenuCrewContractSub" runat="server" OnTabStripCommand="CrewContract_TabStripCommand" Title="Contract Extension"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 17%;">
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 31%;">
                        <telerik:RadTextBox ID="txtFirstName" runat="server" CssClass="gridinput readonlytextbox"
                            ReadOnly="true" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 21%;">
                        <telerik:RadLabel ID="lblRankNationality" runat="server" Text="Rank / Nationality"></telerik:RadLabel>
                    </td>
                    <td style="width: 31%;">
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="input readonlytextbox" ReadOnly="true" Enabled="false" Width="120px"></telerik:RadTextBox>
                        /    
                        <telerik:RadTextBox ID="txtNationality" runat="server" CssClass="input readonlytextbox" ReadOnly="true" Enabled="false" Width="120px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtAddress" runat="server" CssClass="gridinput readonlytextbox"
                            ReadOnly="true" Enabled="false" Width="95%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblSeamanBookNo" runat="server" Text="Seaman Book No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanBook" runat="server" CssClass="input readonlytextbox" Width="240px" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblVesselJoining" runat="server" Text="Vessel Joining"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="input readonlytextbox" ReadOnly="true" Enabled="false" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblContractPeriod" runat="server" Text="Contract Period"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtContractPeriod" runat="server" CssClass="input_mandatory" IsInteger="true" />
                        +/-
                            <eluc:Number ID="txtPlusMinusPeriod" runat="server" CssClass="input_mandatory" MaxLength="3" />
                        (Months)
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContractPayCommencementDate" runat="server" Text="Contract/ Pay Commencement Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnconfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
