<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitTravelAdvanceReturn.aspx.cs"
    Inherits="AccountsVesselVisitTravelAdvanceReturn" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel Advance Return</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmITVisit" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">

            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="MenuFormGeneral">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="MenuFormGeneral" />
                            <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="Title1" Text="Travel Advance Return" ShowMenu="false" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuTravelAdvance" runat="server" OnTabStripCommand="MenuTravelAdvance_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuTravelAdvanceSub" runat="server" OnTabStripCommand="MenuTravelAdvanceSub_TabStripCommand"></eluc:TabStrip>
            <table id="Table2" width="100%" style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbltest" runat="server" Font-Bold="true"
                            Text="* For the returning of Travel Advances in Currencies not given earlier, please use the ADD button to insert a 0.00 Travel Advance">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTravelAdvanceNo" runat="server" Text="Travel Advance Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTravelAdvanceNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="320px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPurpose" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="320px" Resize="Both"
                            Height="60px" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployee" runat="server" Text="Sub account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployee" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="320px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPort" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="320px"></telerik:RadTextBox>
                    </td>

                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Advance Approved Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="320px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentAmount" runat="server" Text="Payment Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtPaymentAmount" runat="server" CssClass="input" Width="320px" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApprovedAmount" runat="server" Text="Approved Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtApprovedAmount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="320px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentDate" runat="server" Text="Payment Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtPaymentDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequestDate" runat="server" Text="Request Approved Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRequestDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReturnAmount" runat="server" Text="Return Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtReturnAmount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="320px" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLiabilityCompany" runat="server" Text="Posting Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlLiabilitycompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            Readonly="true" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReturnDate" runat="server" Text="Return Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtReturnDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                </tr>


                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Width="320px" Resize="Both"
                            Height="60px" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBalance" runat="server" Text="Balance"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBalance" runat="server" CssClass="readonlytextbox" Style="text-align: right;" ReadOnly="true" Width="320px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="320px"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblSalAdjVoucherNumber" runat="server" Text="Salary Adjustment Voucher"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSalAdjVoucherNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="320px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
