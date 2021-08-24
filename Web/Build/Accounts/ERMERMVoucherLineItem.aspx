<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ERMERMVoucherLineItem.aspx.cs"
    Inherits="ERMERMVoucherLineItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" scrolling="yes" style="min-height: 500px; width: 100%;">
        <telerik:RadScriptManager runat="server" ID="ToolkitScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVoucher" Height="100%" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuVoucherLineItem" OnTabStripCommand="Voucher_TabStripCommand" runat="server"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblERMVoucherNumber" runat="server" Text="ERM Voucher Number"></telerik:RadLabel>
                    </td>
                    <td width="15%">
                        <telerik:RadTextBox ID="txtERMVoucherNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td width="15%">
                        <telerik:RadTextBox ID="txtVoucherNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblERMcur" runat="server" Text="ERM Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtERMcurrency" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblESMCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCurrency" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblERMPrime" runat="server" Text="ERM PRIME"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtERMPrime" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtPrimeAmoutEdit" runat="server" CssClass="readonlytextbox txtNumber"
                                Width="150px"></eluc:Number>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBase" runat="server" Text="Base"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtBase" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblERMBase" runat="server" Text="ERM Base"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtERMBase" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblERMReport" runat="server" Text="ERM Report"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtERMReport" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblERMexchange" runat="server" Text="ERM Exchange"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtERMexchange" runat="server" CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblBaseexchange" runat="server" Text="Base Exchange"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtBaseexchange" runat="server" CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblERMexchangereport" runat="server" Text="ERM Exchange Report"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtERMexchangereport" CssClass="readonlytextbox" runat="server"
                                Width="150px">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblReportexchange" runat="server" Text="Exchange Report "></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtReportexchange" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCheque" runat="server" Text="ERM Cheque"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCheque" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblChequeno" runat="server" Text="Cheque No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtChequeno" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <%-- <tr></tr>--%>
                    <td>
                        <telerik:RadLabel ID="lblzid" runat="server" Text="Zid"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtzid" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="50px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRowNumber" runat="server" Text="Row Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRowNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="50px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccID" runat="server" Text="Account ID"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAccountCocde" runat="server" Text="Account Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountCocde" runat="server" CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccSource" runat="server" Text="Account Source"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountSource" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAccUsage" runat="server" Text="Account Usage"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountUsage" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSubAcc" runat="server" Text="Sub Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubAccount" runat="server" CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblpayact" runat="server" Text="ERM Pay Act"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtpayact" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget ID"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBudget" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBaseBalance" runat="server" Text="Base Ballance"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBaseBalance" runat="server" CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportBalance" runat="server" Text="Report Ballance "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReportBalance" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblERMTypeTransaction" runat="server" Text="ERM Type Transaction "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMTypeTransaction" CssClass="readonlytextbox" runat="server"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblERMManager" runat="server" Text="ERM Manager"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMManager" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblERMReference" runat="server" Text="ERM Reference"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMReference" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblERMYear" runat="server" Text="ERM Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMYear" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblERMStatus" runat="server" Text="ERM Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMStatus" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblERMInvoiceNo" runat="server" Text="ERM Invoice No "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMInvoiceNo" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblERMDate" runat="server" Text="ERM Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMDate" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblERMDateDue" runat="server" Text="ERM Date Due "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMDateDue" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblERMPer" runat="server" Text="ERM Per"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMPer" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblERMDiv" runat="server" Text="ERM DIV"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMDiv" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProject" runat="server" Text="ERM Project"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtProject" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblERMSec" runat="server" Text="ERM Sec"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMSec" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblERMlongdescription" runat="server" Text="ERM Longdescription"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtERMlongdescription" runat="server"
                            Width="200px" TextMode="MultiLine" Height="50px" CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLongDescription" runat="server" Text="Long Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLongDescription" Width="200px" TextMode="MultiLine"
                            Height="50px" CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" CssClass="input" runat="server" TextMode="MultiLine"
                            Height="50px" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCreatedDate" runat="server" Text="Created Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCreatedDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDate" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcreatedby" runat="server" Text="Created By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCreatedby" runat="server" CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblupdatedby" runat="server" Text="Updated By "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedBy" CssClass="readonlytextbox" runat="server" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false"></telerik:RadLabel>
                <telerik:RadLabel ID="lblaccountusage" runat="server" Visible="false"></telerik:RadLabel>
                <telerik:RadLabel ID="lblbudgetid" runat="server" Visible="false"></telerik:RadLabel>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
