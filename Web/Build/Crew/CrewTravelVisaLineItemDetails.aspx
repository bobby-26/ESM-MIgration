<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelVisaLineItemDetails.aspx.cs"
    Inherits="CrewTravelVisaLineItemDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seafarer Detail</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewTravelVisaList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuCrewVisa" runat="server" OnTabStripCommand="MenuCrewVisa_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNo" runat="server" Text="Passport No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassportNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucPaymentMode" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="50%"
                            HardTypeCode="185" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisaType" runat="server" Text=" Visa Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucVisaType" runat="server" AppendDataBoundItems="true" Width="50%"
                            HardTypeCode="107" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVisaReason" runat="server" Text="Visa Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucVisaReason" runat="server" CssClass="dropdown_mandatory" Width="50%"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisacost" runat="server" Text="Visa Cost"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmount" runat="server" CssClass="readonlytextbox" DecimalPlace="2" ReadOnly="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisaProcessType" runat="server" Text=" Visa Process Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVisaProcessType" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppliedOn" runat="server" Text="Applied on"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtAppliedon" runat="server" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExpected" runat="server" Text="Expected Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtExpectedCollon" runat="server" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDocReceivedOn" runat="server" Text="Doc. Received On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDocReceivedOn" runat="server" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisaStatus" runat="server" Text="Visa Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVisaStatus" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="50%"
                            DataValueField="FLDVISASTATIS" DataTextField="FLDVISASTATUSNAME" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Joined Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlvessel" runat="server" AppendDataBoundItems="true" AssignedVessels="true" Width="50%"
                            Entitytype="VSL" ActiveVesselsOnly="true" />
                    </td>
                </tr>              
            </table>
            <eluc:Status ID="ucStatus" runat="server" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
