<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditDebitHistory.aspx.cs" Inherits="AccountsCreditDebitHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Accounts CreditDebit History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlOrderForm">

            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Visible="false" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblHistoryType" runat="server" Text="History Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblHistoryType" runat="server" AppendDataBoundItems="false"
                                RepeatDirection="Horizontal" AutoPostBack="true" RepeatLayout="Table" OnSelectedIndexChanged="ReBindData"
                                CssClass="readonlytextbox" Enabled="true" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Value="CREDIT NOTE STATUS" Selected="True" Text="Status" />
                                    <telerik:ButtonListItem Value="CREDIT NOTE SUPPLIER" Text="Supplier" />
                                    <telerik:ButtonListItem Value="VENDOR CREDIT NOTE NUMBER" Text="Vendor Credit Note" />
                                    <telerik:ButtonListItem Value="CREDIT NOTE VOUCHER NUMBER" Text="Credit Note Voucher No" />
                                    <telerik:ButtonListItem Value="PAYMENT VOUCHER NUMBER NUMBER" Text="Payment Voucher No" />
                                    <telerik:ButtonListItem Value="CREDIT NOTE CURRENCY" Text="Currency" />
                                    <telerik:ButtonListItem Value="CREDIT NOTE AMOUNT" Text="Amount" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                </table>

                <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvInvoiceHistory" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    ShowFooter="true" ShowHeader="true" EnableViewState="true" OnSortCommand="gvInvoiceHistory_SortCommand"
                    OnNeedDataSource="gvInvoiceHistory_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDCREDITDEBITNOTEID">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" Visible="false">
                                <HeaderStyle Width="8%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblHistoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHISTORYID") %>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblInvoiceCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEBITNOTEID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date/Time of Change" AllowSorting="true">
                                <HeaderStyle Width="8%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDateHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUPDATEDATE") %>'></telerik:RadLabel>

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Type of Change" AllowSorting="true">
                                <HeaderStyle Width="8%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTypeofChange" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="User Short Code" Visible="false" AllowSorting="true">
                                <HeaderStyle Width="8%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblUserShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="User Name" AllowSorting="true">
                                <HeaderStyle Width="8%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Field" AllowSorting="true">
                                <HeaderStyle Width="8%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblField" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELD") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Old Value" AllowSorting="true">
                                <HeaderStyle Width="8%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOldValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSVALUE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="New Value" AllowSorting="true">
                                <HeaderStyle Width="8%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNewValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVALUE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Procedure Used" AllowSorting="true">
                                <HeaderStyle Width="8%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblProcedureUsed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
