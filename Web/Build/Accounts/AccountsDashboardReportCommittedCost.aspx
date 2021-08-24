<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDashboardReportCommittedCost.aspx.cs" Inherits="Accounts_AccountsDashboardReportCommittedCost" %>

<!DOCTYPE html>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Committed Cost</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function resizeFrame() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resizeFrame()">
    <form id="frmReportCommittedCost" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" />
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="ucVessel" CssClass="readonlytextbox" AppendDataBoundItems="true" Width="70%"
                                OnSelectedIndexChanged="VesselAccount_Changed" AutoPostBack="true" Filter="Contains" EmptyMessage="Type to select">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="" Selected="True"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" DateTimeFormat="dd/MMM/yyyy" CssClass="readonlytextbox"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDspOwnerBudget" runat="server" Text="Display Owner Budget Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rblDspOwnerBC" runat="server" Filter="Contains" EmptyMessage="Type to select" Width="70%">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Owner budget code" Selected="True" Value="1"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Budget code" Value="2"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Budget code with owner code" Value="3"></telerik:RadComboBoxItem>
                                </Items>

                            </telerik:RadComboBox>
                        </td>

                        <td />
                        <td />
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCommittedCostVoucherNo" runat="server" Text="Committed Cost Voucher No:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVoucherNumber" runat="server" CssClass="input" Width="70%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                CommandName="ATTACHMENT" ID="cmdAtt" Visible="false"
                                ToolTip="Attachment"></asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Panel ID="pnlOrdereddate" runat="server" GroupingText="Ordered Date" Width="340px">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucOrderedFromDate" runat="server" DateTimeFormat="dd/MMM/yyyy" />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucOrderedToDate" runat="server" DateTimeFormat="dd/MMM/yyyy" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px; width: 100%;"></iframe>
            </div>
        </div>
    </form>
</body>
</html>

