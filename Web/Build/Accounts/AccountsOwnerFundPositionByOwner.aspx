<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOwnerFundPositionByOwner.aspx.cs" Inherits="Accounts_AccountsOwnerFundPositionByOwner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Finanacial Year Statement</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>    
    <%--<style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .Grid, .ChildGrid
        {
            border: 1px solid #ccc;
        }
        .Grid td, .Grid th
        {
            border: 1px solid #ccc;
        }
        .Grid th
        {
            background-color: #B8DBFD;
            color: #333;
            font-weight: bold;
        }
        .ChildGrid td, .ChildGrid th
        {
            border: 1px solid #ccc;
        }
        .ChildGrid th
        {
            background-color: #ccc;
            color: #333;
            font-weight: bold;
        }
    </style>--%>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:Confirm ID="ucConfirmMessage" runat="server" OKText="Yes" CancelText="No" />
        <%--   <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <div class="divFloatLeft">
                <eluc:Title runat="server" ID="Title3" Text="Owner Fund Position" ShowMenu="true">
                </eluc:Title>
            </div>
            <div class="subHeader">
                <div class="divFloat">
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MenuMainFilter" runat="server" OnTabStripCommand="MenuMainFilter_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
            </div>
        </div>
        <div class="subHeader">
            <div class="divFloat" style="clear: right">
                <eluc:TabStrip ID="MenuFinancialYearStatement" runat="server" OnTabStripCommand="MenuFinancialYearStatement_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
    </div>--%>


        <eluc:TabStrip ID="MenuMainFilter" runat="server" OnTabStripCommand="MenuMainFilter_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>


        <eluc:TabStrip ID="MenuFinancialYearStatement" runat="server" OnTabStripCommand="MenuFinancialYearStatement_TabStripCommand" TabStrip="true"></eluc:TabStrip>

        <br />
        <br />
        <table cellpadding="1" cellspacing="1" width="30%">
            <tr>
                <td style="width: 10%">
                    <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                </td>
                <td style="width: 80%">
                    <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128" SelectedAddress="3811" Width="200"
                        AppendDataBoundItems="false" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%">
                    <asp:Literal ID="lblFromDate" runat="server" Text="As On Date"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <eluc:Date ID="txtFromDate" runat="server"  CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Repeater ID="rptCustomers" runat="server" OnItemDataBound="OnItemDataBound">
            <HeaderTemplate>
                <table class="Grid" cellspacing="0" rules="all" border="1">
                    <tr>
                        <th scope="col">&nbsp;
                        </th>
                        <th scope="col" align="center" style="width: 150px">Account Code Description
                        </th>
                        <th scope="col" align="center" style="width: 150px">Ledger Balance
                        </th>
                        <th scope="col" align="center" style="width: 150px">Excluded Vouchers
                        </th>
                        <th scope="col" align="center" style="width: 150px">Contractual Wages
                        </th>
                        <th scope="col" align="center" style="width: 150px">Monthly Billing
                        </th>
                        <th scope="col" align="center" style="width: 150px">Cash To Master
                        </th>
                        <th scope="col" align="center" style="width: 150px">Advance Payment
                        </th>
                        <th scope="col" align="center" style="width: 150px">Committed Cost
                        </th>
                        <th scope="col" align="center" style="width: 150px">Total Balance
                        </th>
                        <th scope="col" align="center" style="width: 150px">Fund Request
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                            <asp:Repeater ID="rptOrders" runat="server" OnItemDataBound="rptOrders_OnItemDataBound">
                                <HeaderTemplate>
                                    <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                        <tr>
                                            <th scope="col">&nbsp;
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Opex/Non-Opex
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Ledger Balance
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Excluded Vouchers
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Contractual Wages
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Monthly Billing
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Cash To Master
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Advance Payment
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Committed Cost
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Total Balance
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Fund Request
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                            <asp:Panel ID="pnlOrders1" runat="server" Style="display: none">
                                                <asp:Repeater ID="rptOrders1" runat="server">
                                                    <HeaderTemplate>
                                                        <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                            <tr>
                                                                <th scope="col" align="center" style="width: 150px">Budget Group
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Ledger Balance
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Excluded Vouchers
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Contractual Wages
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Monthly Billing
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Cash To Master
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Advance Payment
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Committed Cost
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Total Balance
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Fund Request
                                                                </th>
                                                            </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("FLDLEDGERBALANCEAMOUNT") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("FLDEXCLUDEDVOUCHERSAMOUNT") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("FLDCONTRACTUALWAGESAMOUNT") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("FLDMONTHLYBILLINGAMOUNT") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("FLDCASHTOMASTERAMOUNT") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="Label7" runat="server" Text='<%# Eval("FLDADVANCEPAYMENTAMOUNT") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="Label8" runat="server" Text='<%# Eval("FLDCOMMITTEDCOSTAMOUNT") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="Label9" runat="server" Text='<%# Eval("FLDTOTALBALANCEAMOUNT") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="Label10" runat="server" Text='<%# Eval("FLDFUNDREQUESTAMOUNT") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hfPrincipalId" runat="server" Value='<%# Eval("FLDOPEX") %>' />
                                            <asp:HiddenField ID="accountid" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("FLDLEDGERBALANCEAMOUNT") %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("FLDEXCLUDEDVOUCHERSAMOUNT") %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("FLDCONTRACTUALWAGESAMOUNT") %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("FLDMONTHLYBILLINGAMOUNT") %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("FLDCASHTOMASTERAMOUNT") %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("FLDADVANCEPAYMENTAMOUNT") %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label8" runat="server" Text='<%# Eval("FLDCOMMITTEDCOSTAMOUNT") %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("FLDTOTALBALANCEAMOUNT") %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label10" runat="server" Text='<%# Eval("FLDFUNDREQUESTAMOUNT") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                        <asp:HiddenField ID="accountid" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("FLDNAME") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("FLDLEDGERBALANCEAMOUNT") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("FLDEXCLUDEDVOUCHERSAMOUNT") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("FLDCONTRACTUALWAGESAMOUNT") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("FLDMONTHLYBILLINGAMOUNT") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("FLDCASHTOMASTERAMOUNT") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("FLDADVANCEPAYMENTAMOUNT") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("FLDCOMMITTEDCOSTAMOUNT") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label9" runat="server" Text='<%# Eval("FLDTOTALBALANCEAMOUNT") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("FLDFUNDREQUESTAMOUNT") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
        <script type="text/javascript">
            $("body").on("click", "[src*=collapsed]", function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "../css/Theme2/images/expanded.gif");
            });
            $("body").on("click", "[src*=expanded]", function () {
                $(this).attr("src", "../css/Theme2/images/collapsed.gif");
                $(this).closest("tr").next().remove();
            });
        </script>
    </form>
</body>
</html>
