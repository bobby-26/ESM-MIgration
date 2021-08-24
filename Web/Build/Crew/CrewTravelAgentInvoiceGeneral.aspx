<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelAgentInvoiceGeneral.aspx.cs"
    Inherits="CrewTravelAgentInvoiceGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ticket Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewChangeTravel" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager ID="RadWindowManager" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tbl1" cellpadding="1" cellspacing="1" width="90%">
                <tr>
                    <td>
                        <asp:Panel ID="pnlinvoice" runat="server" GroupingText="From Invoice">
                            <table id="Table1" cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblInvoiceNo" runat="server" Text="Invoice No."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtInvoiceNo" runat="server" CssClass="readonlytextbox" Width="80%"
                                            ReadOnly="true">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request No."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvRequestno" runat="server" CssClass="readonlytextbox" Width="80%"
                                            ReadOnly="true">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPassengerName" runat="server" Text=" Passenger Name"></telerik:RadLabel>

                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvpassname" runat="server" CssClass="readonlytextbox" Width="80%"
                                            ReadOnly="true">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTicketNo" runat="server" Text="Ticket No."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvticketno" runat="server" CssClass="readonlytextbox" Width="80%"
                                            ReadOnly="true">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblNo" runat="server" Text="PNR No."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvPNR" runat="server" CssClass="readonlytextbox" Width="80%"
                                            ReadOnly="true">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblAirlinecode" runat="server" Text="Airline Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvairlinecode" runat="server" CssClass="readonlytextbox" Width="80%"
                                            ReadOnly="true">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDepDate" runat="server" Text="Departure Date"></telerik:RadLabel>

                                    </td>
                                    <td>
                                        <eluc:Date ID="txtinvdepdate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvvessel" runat="server" CssClass="readonlytextbox" Width="80%"
                                            ReadOnly="true">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblBasic" runat="server" Text="Basic"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvbasic" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTotalTax" runat="server" Text="Total Tax(Tax + STX Collected)"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvtax" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDiscount" runat="server" Text="Discount"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvdiscount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCancellation" runat="server" Text="Cancellation"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvcancell" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTotal" runat="server" Text="Total"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvtotal" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCreditNote" runat="server" Text="Credit Note"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtCreditNote" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblAgentRemarks" runat="server" Text="Agent Remarks"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtAgentRemarks" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"
                                            Style="vertical-align: top; cursor: pointer">
                                        </telerik:RadTextBox>
                                        <eluc:ToolTip ID="ucToolTip" runat="server" Width="500" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="From Ticket Issued">
                            <table id="Table2" cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lbl1" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbl2" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblRequestNumber" runat="server" Text="Request No."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtRequestNo" runat="server" CssClass="readonlytextbox" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPasengerName" runat="server" Text="Passenger Name"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtpassengername" runat="server" CssClass="readonlytextbox" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTicketNumber" runat="server" Text="Ticket No."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtticket" runat="server" CssClass="readonlytextbox" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPNRNo" runat="server" Text="PNR No."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtpnr" runat="server" CssClass="readonlytextbox" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblAirlineCode1" runat="server" Text="Airline Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtairlinecode" runat="server" CssClass="readonlytextbox" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDepDate1" runat="server" Text="Departure Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtdepdate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblVessel1" runat="server" Text="Vessel"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtvessel" runat="server" CssClass="readonlytextbox" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblBasic1" runat="server" Text="Basic"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtbasic" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTotalTax1" runat="server" Text="Total Tax(Tax + STX Collected)"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txttax" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDiscount1" runat="server" Text="Discount"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtdiscount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCancelledReissued" runat="server" Text="Cancelled/Reissued"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtTktStatus" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTotal1" runat="server" Text="Total"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txttotal" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lbl3" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbl4" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lbl5" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbl6" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
