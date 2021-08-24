<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCompanyFinancialStatement.aspx.cs"
    Inherits="Accounts_AccountsCompanyFinancialStatement" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="TrailBalanceMonthly" Src="~/UserControls/UserControlFYTrailBalanceMonthly.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Finanacial Year Statement</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
           <%: Scripts.Render("~/bundles/js") %>
           <%: Styles.Render("~/bundles/css") %>
    
        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>
   
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
           <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1" ></telerik:RadWindowManager>
       <%-- <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>--%>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <asp:Button ID="ucConfirm" runat="server" Text="confirmation message" OnClick="OnAction_Click" cssclass="hidden"/>
         
     <%--<eluc:Confirm ID="ucConfirmMessage" runat="server" OnConfirmMesage="OnAction_Click"
            OKText="Yes" CancelText="No" />--%>
   
        
            <eluc:TabStrip ID="MenuFinancialYearStatement" runat="server" OnTabStripCommand="MenuFinancialYearStatement_TabStripCommand"></eluc:TabStrip>
       
        <table>
            <tr>
            </tr>
            <tr>
                <td>Company
                </td>
                <td>

                     <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlCompany"  AutoPostBack="true" EnableLoadOnDemand="true">
                     
                    </telerik:RadComboBox>
                    <%-- <telerik:RadDropDownList ID="ddlCompany" runat="server" class="dropdown_mandatory">
                    </telerik:RadDropDownList>--%>
                 
                </td>
                <td colspan="2"></td>
                <td>Finanacial Year
                </td>
                <td>
                    <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory"
                        AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucFinancialYear_OnTextChangedEvent" />
                </td>
            </tr>
            <tr>
                <td>Report
                </td>
                <td>

                      <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlReport"  AutoPostBack="true" EnableLoadOnDemand="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="Trial Balance" Value="T" />
                            <telerik:RadComboBoxItem Text="Profit/Loss" Value="P" />
                            <telerik:RadComboBoxItem Text="Balance Sheet" Value="B" />
                        </Items>
                    </telerik:RadComboBox>

                 <%--   <select id="ddlReport" runat="server" class="input" style="width: 110px;">
                        <option value="T">Trial Balance</option>
                        <option value="P">Profit/Loss</option>
                        <option value="B">Balance Sheet</option>
                    </select>--%>
                </td>
                <td colspan="2"></td>
                <td>Period
                </td>
                <td>
                    <%-- <asp:DropDownList ID="ddlMonth" runat="server" CssClass="dropdown_mandatory">
                    <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                    <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                    <asp:ListItem Value="5" Text="May"></asp:ListItem>
                    <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                    <asp:ListItem Value="7" Text="Jul"></asp:ListItem>
                    <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                    <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                    <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                    <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                    <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                </asp:DropDownList>--%>

                  <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlMonth"  AutoPostBack="true" EnableLoadOnDemand="true">
                    </telerik:RadComboBox>

<%--                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="input_mandatory" Width="120px">
                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                    </asp:DropDownList>--%>
                </td>
            </tr>
            <tr>
                <td>Format
                </td>
                <td>
                     <%-- <telerik:RadDropDownList ID="ddlFormat" runat="server" CssClass="input_mandatory" Width="120px">
                         <Items>
                        <telerik:DropDownListItem Value="0" Text="Year To Date"></telerik:DropDownListItem>
                         <telerik:DropDownListItem Value="1" Text="Monthly Break up"></telerik:DropDownListItem>
                         </Items>
                    </telerik:RadDropDownList>--%>

                <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlFormat"  AutoPostBack="true" EnableLoadOnDemand="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="Year To Date" Value="Y" />
                            <telerik:RadComboBoxItem Text="Monthly Break up" Value="M" />
                        </Items>
                    </telerik:RadComboBox>

              <%--    <select id="ddlFormat" runat="server" class="input">
                        <option value="Y">Year To Date</option>
                        <option value="M">Monthly Break up</option>
                    </select>--%>
                </td>
                <td colspan="2"></td>
                <td>Currency
                </td>
                <td>

                     <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlCurrency"  AutoPostBack="true" EnableLoadOnDemand="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="Base Currency" Value="B" />
                            <telerik:RadComboBoxItem Text="Report Currency" Value="R"/>
                        </Items>
                    </telerik:RadComboBox>

                  <%--  <select id="ddlCurrency" runat="server" class="input" style="width: 120px;">
                        <option value="B">Base Currency</option>
                        <option value="R">Report Currency</option>
                    </select>--%>
                </td>
            </tr>
            <tr>
                <td>Report is Accurate as On
                </td>
                <td>
                    <telerik:RadLabel ID="lblAccuratedate" runat="server">
                    </telerik:RadLabel>
                </td>
                <td colspan="4">
                    <telerik:RadLabel ID="lblCurrencycode" runat="server" Visible="false"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton ID="imgExcel" runat="server" ImageUrl="~/css/Theme1/images/xls.png"
                        OnClick="imgExcel_OnClientClick" Width="20px" />
                    <asp:ImageButton ID="imgExcel2nd" runat="server" ImageUrl="~/css/Theme1/images/xls.png"
                        OnClick="imgExcel2nd_OnClientClick" Width="20px" />
                    <asp:ImageButton ID="imgExcel3rd" runat="server" ImageUrl="~/css/Theme1/images/xls.png"
                        OnClick="imgExcel3rd_OnClientClick" Width="20px" />
                    <asp:ImageButton ID="imgExcel4th" runat="server" ImageUrl="~/css/Theme1/images/xls.png"
                        OnClick="imgExcel4th_OnClientClick" Width="20px" />
                </td>
                <td colspan="5"></td>
            </tr>
        </table>
        <asp:Repeater ID="rptCustomers" runat="server" OnItemDataBound="OnItemDataBound">
            <HeaderTemplate>
                <table class="Grid" cellspacing="0" rules="all" border="1">
                    <tr>
                        <th scope="col">&nbsp;
                        </th>
                        <th scope="col" align="center" style="width: 150px">Account
                        </th>
                        <th scope="col" align="center" style="width: 150px">Opening Balance
                        </th>
                        <th scope="col" align="center" style="width: 150px">YTD (<%#lblCurrencycode.Text %>)
                        </th>
                        <th scope="col" align="center" style="width: 150px">Closing Balance
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="background-color: Olive;">
                    <td>
                        <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />

                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                            <asp:Repeater ID="rptOrders" runat="server" OnItemDataBound="rptOrders_OnItemDataBound">
                                <HeaderTemplate>
                                    <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                        <tr>
                                            <th scope="col">&nbsp;
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Account
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Opening Balance
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">YTD (<%#lblCurrencycode.Text %>)
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Closing Balance
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="background-color: Aqua;">
                                        <td>
                                            <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                            <asp:Panel ID="pnlOrders1" runat="server" Style="display: none">
                                                <asp:Repeater ID="rptOrders1" runat="server" OnItemDataBound="rptOrders1_OnItemDataBound">
                                                    <HeaderTemplate>
                                                        <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                            <tr>
                                                                <th scope="col">&nbsp;
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Account
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Opening Balance
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">YTD (<%#lblCurrencycode.Text %>)
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Closing Balance
                                                                </th>
                                                            </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr style="background-color: Green;">
                                                            <td>
                                                                <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                                                <asp:Panel ID="pnlTBlevel4" runat="server" Style="display: none">
                                                                    <asp:Repeater ID="rptTBlevel4" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                                                <tr>
                                                                                    <th scope="col" align="center" style="width: 150px">Account
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">Opening Balance
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">YTD (<%#lblCurrencycode.Text %>)
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">Closing Balance
                                                                                    </th>
                                                                                </tr>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label1" runat="server" Text='<%# Eval("FLDOPENINGBALANCE","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDBALANCE","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label3" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </asp:Panel>
                                                                <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label1" runat="server" Text='<%# Eval("FLDOPENINGBALANCE","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDBALANCE","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label3" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label2" runat="server" Text='<%# Eval("FLDOPENINGBALANCE","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDBALANCE","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label4" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                        <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContactName" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDOPENINGBALANCE","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="lblCity" runat="server" Text='<%# Eval("FLDBALANCE","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label5" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptPnLBalancesheet" runat="server" OnItemDataBound="rptPnLBalancesheet_OnItemDataBound">
            <HeaderTemplate>
                <table class="Grid" cellspacing="0" rules="all" border="1">
                    <tr>
                        <th scope="col">&nbsp;
                        </th>
                        <th scope="col" align="center" style="width: 150px">Account
                        </th>
                        <th scope="col" align="center" style="width: 150px">YTD (<%#lblCurrencycode.Text %>)
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="background-color: Olive;">
                    <td>
                        <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                            <asp:Repeater ID="rptPnLBalancesheet2nd" runat="server" OnItemDataBound="rptPnLBalancesheet2nd_OnItemDataBound">
                                <HeaderTemplate>
                                    <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                        <tr>
                                            <th scope="col">&nbsp;
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Account
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">YTD (<%#lblCurrencycode.Text %>)
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="background-color: Aqua;">
                                        <td>
                                            <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                            <asp:Panel ID="pnlOrders1" runat="server" Style="display: none">
                                                <asp:Repeater ID="rptPnLBalancesheet3rd" runat="server" OnItemDataBound="rptPnLBalancesheet3rd_OnItemDataBound">
                                                    <HeaderTemplate>
                                                        <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                            <tr>
                                                                <th scope="col">&nbsp;
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Account
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">YTD (<%#lblCurrencycode.Text %>)
                                                                </th>
                                                            </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr style="background-color: Green;">
                                                            <td>
                                                                <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                                                <asp:Panel ID="pnlOrders4th" runat="server" Style="display: none">
                                                                    <asp:Repeater ID="rptPnLBalancesheet4th" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                                                <tr>
                                                                                    <th scope="col" align="center" style="width: 150px">Account
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">YTD (<%#lblCurrencycode.Text %>)
                                                                                    </th>
                                                                                </tr>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDBALANCE","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </asp:Panel>
                                                                <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDBALANCE","{0:#,##0.00}") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDBALANCE","{0:#,##0.00}") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                        <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContactName" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="lblCity" runat="server" Text='<%# Eval("FLDBALANCE","{0:#,##0.00}") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptTrailBalance" runat="server" OnItemDataBound="rptTrailBalance_OnItemDataBound">
            <HeaderTemplate>
                <table class="Grid" cellspacing="0" rules="all" border="1">
                    <tr>
                        <th scope="col">&nbsp;
                        </th>
                        <th scope="col" align="center" style="width: 150px">Account
                        </th>
                        <th scope="col" align="center" style="width: 150px">Opening Balance
                        </th>
                        <th scope="col" align="center" style="width: 150px">April
                        </th>
                        <th scope="col" align="center" style="width: 150px">May
                        </th>
                        <th scope="col" align="center" style="width: 150px">June
                        </th>
                        <th scope="col" align="center" style="width: 150px">July
                        </th>
                        <th scope="col" align="center" style="width: 150px">August
                        </th>
                        <th scope="col" align="center" style="width: 150px">September
                        </th>
                        <th scope="col" align="center" style="width: 150px">October
                        </th>
                        <th scope="col" align="center" style="width: 150px">November
                        </th>
                        <th scope="col" align="center" style="width: 150px">December
                        </th>
                        <th scope="col" align="center" style="width: 150px">January
                        </th>
                        <th scope="col" align="center" style="width: 150px">February
                        </th>
                        <th scope="col" align="center" style="width: 150px">March
                        </th>
                        <th scope="col" align="center" style="width: 150px">Closing Balance
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="background-color: Olive;">
                    <td>
                        <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                        <asp:Panel ID="pnlLevel2" runat="server" Style="display: none">
                            <asp:Repeater ID="rptTrailBalance2nd" runat="server" OnItemDataBound="rptTrailBalance2nd_OnItemDataBound">
                                <HeaderTemplate>
                                    <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                        <tr>
                                            <th scope="col">&nbsp;
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Account
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Opening Balance
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">April
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">May
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">June
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">July
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">August
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">September
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">October
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">November
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">December
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">January
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">February
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">March
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Closing Balance
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="background-color: Aqua;">
                                        <td>
                                            <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                            <asp:Panel ID="pnlLevel3" runat="server" Style="display: none">
                                                <asp:Repeater ID="rptTrailBalance3rd" runat="server" OnItemDataBound="rptTrailBalance3rd_OnItemDataBound">
                                                    <HeaderTemplate>
                                                        <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                            <tr>
                                                                <th scope="col">&nbsp;
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Account
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Opening Balance
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">April
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">May
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">June
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">July
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">August
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">September
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">October
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">November
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">December
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">January
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">February
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">March
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Closing Balance
                                                                </th>
                                                            </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr style="background-color: Green;">
                                                            <td>
                                                                <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                                                <asp:Panel ID="pnlLevel3" runat="server" Style="display: none">
                                                                    <asp:Repeater ID="rptTrailBalance4th" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                                                <tr>
                                                                                    <th scope="col" align="center" style="width: 150px">Account
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">Opening Balance
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">April
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">May
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">June
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">July
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">August
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">September
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">October
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">November
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">December
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">January
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">February
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">March
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">Closing Balance
                                                                                    </th>
                                                                                </tr>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label1" runat="server" Text='<%# Eval("FLDOPENINGBALANCE","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label9" runat="server" Text='<%# Eval("FLDMAY","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label10" runat="server" Text='<%# Eval("FLDJUNE","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label11" runat="server" Text='<%# Eval("FLDJULY","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDJANUARY","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label7" runat="server" Text='<%# Eval("FLDMARCH","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label3" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </asp:Panel>
                                                                <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label1" runat="server" Text='<%# Eval("FLDOPENINGBALANCE","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label9" runat="server" Text='<%# Eval("FLDMAY","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label10" runat="server" Text='<%# Eval("FLDJUNE","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label11" runat="server" Text='<%# Eval("FLDJULY","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDJANUARY","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label7" runat="server" Text='<%# Eval("FLDMARCH","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label3" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label2" runat="server" Text='<%# Eval("FLDOPENINGBALANCE","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label9" runat="server" Text='<%# Eval("FLDMAY","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label10" runat="server" Text='<%# Eval("FLDJUNE","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label11" runat="server" Text='<%# Eval("FLDJULY","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDJANUARY","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label7" runat="server" Text='<%# Eval("FLDMARCH","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label4" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                        <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContactName" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDOPENINGBALANCE","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label9" runat="server" Text='<%# Eval("FLDMAY","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label10" runat="server" Text='<%# Eval("FLDJUNE","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label11" runat="server" Text='<%# Eval("FLDJULY","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label17" runat="server" Text='<%# Eval("FLDJANUARY","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label7" runat="server" Text='<%# Eval("FLDMARCH","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label5" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptMnthPnLBS" runat="server" OnItemDataBound="rptMnthPnLBS_OnItemDataBound">
            <HeaderTemplate>
                <table class="Grid" cellspacing="0" rules="all" border="1">
                    <tr>
                        <th scope="col">&nbsp;
                        </th>
                        <th scope="col" align="center" style="width: 150px">Account
                        </th>
                        <th scope="col" align="center" style="width: 150px">April
                        </th>
                        <th scope="col" align="center" style="width: 150px">May
                        </th>
                        <th scope="col" align="center" style="width: 150px">June
                        </th>
                        <th scope="col" align="center" style="width: 150px">July
                        </th>
                        <th scope="col" align="center" style="width: 150px">August
                        </th>
                        <th scope="col" align="center" style="width: 150px">September
                        </th>
                        <th scope="col" align="center" style="width: 150px">October
                        </th>
                        <th scope="col" align="center" style="width: 150px">November
                        </th>
                        <th scope="col" align="center" style="width: 150px">December
                        </th>
                        <th scope="col" align="center" style="width: 150px">January
                        </th>
                        <th scope="col" align="center" style="width: 150px">February
                        </th>
                        <th scope="col" align="center" style="width: 150px">March
                        </th>
                        <th scope="col" align="center" style="width: 150px" runat="server" id="thClosing1PLnBS">Closing Balance
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="background-color: Olive;">
                    <td>
                        <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                        <asp:Panel ID="pnlLevel2" runat="server" Style="display: none">
                            <asp:Repeater ID="rptMnthPnLBS2nd" runat="server" OnItemDataBound="rptMnthPnLBS2nd_OnItemDataBound">
                                <HeaderTemplate>
                                    <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                        <tr>
                                            <th scope="col">&nbsp;
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">Account
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">April
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">May
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">June
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">July
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">August
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">September
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">October
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">November
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">December
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">January
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">February
                                            </th>
                                            <th scope="col" align="center" style="width: 150px">March
                                            </th>
                                            <th scope="col" align="center" style="width: 150px" runat="server" id="thClosing2PLnBS">Closing Balance
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="background-color: Aqua;">
                                        <td>
                                            <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                            <asp:Panel ID="pnlLevel3" runat="server" Style="display: none">
                                                <asp:Repeater ID="rptMnthPnLBS3rd" runat="server" OnItemDataBound="rptMnthPnLBS3rd_OnItemDataBound">
                                                    <HeaderTemplate>
                                                        <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                            <tr>
                                                                <th scope="col">&nbsp;
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">Account
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">April
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">May
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">June
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">July
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">August
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">September
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">October
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">November
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">December
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">January
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">February
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px">March
                                                                </th>
                                                                <th scope="col" align="center" style="width: 150px" runat="server" id="thClosing3PLnBS">Closing Balance
                                                                </th>
                                                            </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr style="background-color: Green;">
                                                            <td>
                                                                <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                                                <asp:Panel ID="pnlLevel3" runat="server" Style="display: none">
                                                                    <asp:Repeater ID="rptMnthPnLBS4th" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                                                <tr>
                                                                                    <th scope="col" align="center" style="width: 150px">Account
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">April
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">May
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">June
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">July
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">August
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">September
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">October
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">November
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">December
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">January
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">February
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px">March
                                                                                    </th>
                                                                                    <th scope="col" align="center" style="width: 150px" runat="server" id="thClosing4PLnBS">Closing Balance
                                                                                    </th>
                                                                                </tr>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label9" runat="server" Text='<%# Eval("FLDMAY","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label10" runat="server" Text='<%# Eval("FLDJUNE","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label11" runat="server" Text='<%# Eval("FLDJULY","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDJANUARY","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <telerik:RadLabel ID="Label7" runat="server" Text='<%# Eval("FLDMARCH","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                                <td align="right" id="tdClosing4PLnBS">
                                                                                    <telerik:RadLabel ID="lblClosing4PLnBS" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </asp:Panel>
                                                                <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label9" runat="server" Text='<%# Eval("FLDMAY","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label10" runat="server" Text='<%# Eval("FLDJUNE","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label11" runat="server" Text='<%# Eval("FLDJULY","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDJANUARY","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <telerik:RadLabel ID="Label7" runat="server" Text='<%# Eval("FLDMARCH","{0:#,##0.00}") %>' />
                                                            </td>
                                                            <td align="right" id="tdClosing3PLnBS">
                                                                <telerik:RadLabel ID="lblClosing3PLnBS" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label9" runat="server" Text='<%# Eval("FLDMAY","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label10" runat="server" Text='<%# Eval("FLDJUNE","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label11" runat="server" Text='<%# Eval("FLDJULY","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# Eval("FLDJANUARY","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right">
                                            <telerik:RadLabel ID="Label7" runat="server" Text='<%# Eval("FLDMARCH","{0:#,##0.00}") %>' />
                                        </td>
                                        <td align="right" id="tdClosing2PLnBS" runat="server">
                                            <telerik:RadLabel ID="lblClosing2PLnBS" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                        <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContactName" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label9" runat="server" Text='<%# Eval("FLDMAY","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label10" runat="server" Text='<%# Eval("FLDJUNE","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label11" runat="server" Text='<%# Eval("FLDJULY","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label17" runat="server" Text='<%# Eval("FLDJANUARY","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right">
                        <telerik:RadLabel ID="Label7" runat="server" Text='<%# Eval("FLDMARCH","{0:#,##0.00}") %>' />
                    </td>
                    <td align="right" id ="tdClosing1PLnBS" runat="server">
                        <telerik:RadLabel ID="lblClosing1PLnBS" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE","{0:#,##0.00}") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>


           <telerik:RadGrid RenderMode="Lightweight" ID="gvExcludedVouchers" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" 
                    ShowFooter="false" ShowHeader="true" >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >
                    <Columns>
                <telerik:GridTemplateColumn HeaderText="Voucher Date">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Number">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                  
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Status">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                  
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Created Date/By">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDET") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Update Date/By">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                  
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTUPDATEDET") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
          </MasterTableView>
               </telerik:RadGrid>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery.min.js"></script>        
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

      <%--  <asp:GridView ID="gvExcludedVouchers" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="true" ShowHeader="true"
            EnableViewState="false" AllowSorting="true">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <Columns>
                <asp:TemplateField HeaderText="Voucher Date">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblHdrVoucherDate" runat="server" Text="Voucher Date"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voucher Date">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblHdrVoucherNumber" runat="server" Text="Voucher Number"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voucher Status">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblHdrVoucherStatus" runat="server" Text="Voucher Status"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voucher Created Date/By">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblHdrVoucherStatus" runat="server" Text="Voucher Created Date/By"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDET") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voucher Update Date/By">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Literal ID="lblHdrVoucherStatus" runat="server" Text="Voucher Update Date/By"></asp:Literal>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTUPDATEDET") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery.min.js"></script>        
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
</html>--%>
