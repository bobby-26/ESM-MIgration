<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceBatchFilter.aspx.cs"
    Inherits="AccountsRemittanceBatchFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
   <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
<%--                <telerik:RadLabel runat="server" ID="lblCaption" Font-Bold="true" Text="Remittance Filter "></telerik:RadLabel>--%>
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
              <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

        <telerik:RadAjaxPanel runat="server" ID="pnlRemittence">
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBatchNumber" runat="server" Text="Batch Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtRemittenceBatchNumberSearch" MaxLength="200" 
                                    Width="150px"></telerik:RadTextBox>
                            </td>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlBankAccount ID="ddlBankAccount" 
                                    AppendDataBoundItems="true" runat="server"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPaymentFromDate" runat="server" Text="Payment From Date"></telerik:RadLabel>
                            </td>
                            <td>
<%--                                <telerik:RadTextBox ID="txtPaymentFromdateSearch" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox>--%>
                               <%-- <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtPaymentFromdateSearch" PopupPosition="TopLeft">

                                </ajaxToolkit:CalendarExtender>--%>
                                <eluc:Date ID="txtPaymentFromdateSearch" runat="server" DatePicker="true" />
                           </td>
                            <td>
                                <telerik:RadLabel ID="lblPaymentToDate" runat="server" Text="Payment To Date"></telerik:RadLabel>
                            </td>
                            <td>
<%--                                <telerik:RadTextBox ID="txtPaymentTodateSearch" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox>--%>
                              <%--  <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtPaymentTodateSearch" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>--%>
                                <eluc:Date ID="txtPaymentTodateSearch" runat="server" DatePicker="true" />

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ddlPaymentmode" runat="server"  HardTypeCode="132"
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 132) %>' AppendDataBoundItems="true" ShortNameFilter="CHQ,TT,ACH,MTT,MCH,ACHS,FT"
                                    Width="300px" />
                            </td>

                             <td>
                                <telerik:RadLabel ID="lblRemittancenumber" runat="server" Text="Remittance Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtRemittenceNumber" MaxLength="50" 
                                    Width="150px"></telerik:RadTextBox>
                            </td>
                            <%--  <td>
                            Remittance Status
                        </td>
                        <td>
                            <eluc:Hard ID="ucRemittanceStatus" AppendDataBoundItems="true" CssClass="input" runat="server" />
                        </td>--%>


                            
                        </tr>
                    </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
