<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountPortageBillComponentTrackInsert.aspx.cs"
    Inherits="VesselAccountPortageBillComponentTrackInsert" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew BOW</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand"
            Title="Component Tracking Add" TabStrip="false"></eluc:TabStrip>
        <table width="100%" cellpadding="1" cellspacing="1" runat="server" id="tblSefarer">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSeafarer" runat="server" Text="Seafarer File Number">
                    </telerik:RadLabel>
                </td>
                <td colspan="5">
                    <telerik:RadTextBox runat="server" ID="txtFileNo" CssClass="input_mandatory">
                    </telerik:RadTextBox>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="ImgBtnValidFileno_Click"
                        ToolTip="Verify Entered File Number"><span class="icon"><i class="fas fa-search"></i></span></asp:LinkButton>
                    <font color="blue">Note: Please verify the entered file number by clicking search icon
                        next to the File number textbox</font>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtFirstName" ReadOnly="true" CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtMiddleName" ReadOnly="true" CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtLastName" ReadOnly="true" CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                    </telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox runat="server" ID="txtRank" ReadOnly="true" CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLVessel" runat="server" Text="Last Vessel">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblLastVessel" runat="server" Text="" Font-Bold="true">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPvessel" runat="server" Text="Present Vessel">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPresentVessel" runat="server" Text="" Font-Bold="true">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblNVessel" runat="server" Text="Next Vessel">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblNextVessel" runat="server" Text="" Font-Bold="true">
                    </telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel">
                    </telerik:RadLabel>
                </td>
                <td style="width: 30%;">
                    <eluc:Vessel ID="ddlVesselChargeable" runat="server" AppendDataBoundItems="true"
                        CssClass="input_mandatory" VesselsOnly="true" Width="50%" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblcurrency" runat="server" Text="Currency">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Currency ID="ddlCurrency" runat="server" AppendDataBoundItems="true" AutoPostBack="false"
                        CssClass="input_mandatory" Width="85%" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' />
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    <telerik:RadLabel ID="lblcomponent" runat="server" Text="Component Group">
                    </telerik:RadLabel>
                </td>
                <td style="width: 30%;">
                    <telerik:RadComboBox ID="ddlcomponentgroup" runat="server" DataTextField="FLDNAME"
                        CssClass="input_mandatory" DataValueField="FLDCOMPONENTTRACKID" AutoPostBack="true"
                        Width="50%" OnSelectedIndexChanged="ddlComponentType_SelectedIndexChanged" EmptyMessage="Type to select"
                        Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSubType" runat="server" Text="Component">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlcomponent" runat="server" DataTextField="FLDCOMPONENTNAME"
                        CssClass="input_mandatory" DataValueField="FLDCOMPONENTID" Width="85%" Filter="Contains"
                        EmptyMessage="Type to select" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromdate" runat="server" Text="Date Between">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                    <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtAmount" runat="server" Width="85%" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVoucherNo" runat="server" Text="Voucher No.">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtVoucherNo" runat="server" Text="" MaxLength="50" Width="50%">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRemarksEdit" runat="server" Text="" TextMode="MultiLine"
                        MaxLength="200" Width="85%">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
