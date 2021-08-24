<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentApproveCrewEdit.aspx.cs"
    Inherits="AccountsAllotmentApproveCrewEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Allotment Approve</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .success {
                margin-bottom: 15px;
                padding: 4px 12px;
                background-color: #ddffdd;
                border-left: 6px solid #4CAF50;
            }

            .warning {
                margin-bottom: 15px;
                padding: 4px 12px;
                background-color: #ffffcc;
                border-left: 6px solid #ffeb3b;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewBankAccountList" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewBankAccountList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <eluc:TabStrip ID="MenuChecking" runat="server" OnTabStripCommand="MenuChecking_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellspacing="1" cellpadding="3">
                <tr>
                    <td style="width: 13%">
                        <telerik:RadLabel ID="lblbEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadTextBox ID="txtName" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>

                    <td style="width: 13%">
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%">
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadTextBox ID="txtFileNo" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAllotmentType" runat="server" Text="Allotment Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblAllotment" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAmount" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox txtNumber" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblBankVerification" runat="server" Font-Bold="true" Text="Bank Details"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBeneficiary" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBeneficiaryBank" runat="server" Text="Bank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBeneficiaryBank" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBeneficiaryAccountNo" runat="server" Text="Account No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountNo" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>

                    <td>
                        <telerik:RadLabel ID="lblBankAddress" runat="server" Text="Bank Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBankAddress" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIFSCCode" runat="server" Text="IFSC Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIFSCCode" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblswiftcode" runat="server" Text="Swift Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtswiftcode" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="200px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBOW" runat="server" Text="Balance of Wages"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBOW" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox txtNumber" Width="100px"></telerik:RadTextBox>
                        <telerik:RadLabel ID="Label1" runat="server" Font-Bold="true" Text="USD as of"></telerik:RadLabel>
                                <telerik:RadLabel ID="txtBowDate" runat="server" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPBRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="2" Columns="20" CssClass="input_mandatory" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblApprovedOn" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="txtApprovedBy" runat="server" Text=""></telerik:RadLabel></b>
                    </td>
                </tr>

            </table>
            <eluc:Status ID="ucStatus" runat="server" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
