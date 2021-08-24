<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceHistory.aspx.cs" Inherits="AccountsRemittanceHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="frmTitle" Text="Remittance History" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand" 
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHistoryType" runat="server" Text="History Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblHistoryType" runat="server" AppendDataBoundItems="false"
                            RepeatDirection="Horizontal" AutoPostBack="true" RepeatLayout="Table" OnSelectedIndexChanged="ReBindData"
                            CssClass="readonlytextbox" Enabled="true">
                            <asp:ListItem Value="REMITTANCE STATUS" Selected="True">Remittance Status</asp:ListItem>
                            <asp:ListItem Value="PAYMENT VOUCHER">Payment Voucher</asp:ListItem>
                            <asp:ListItem Value="PAYMENT MODE">Payment Mode</asp:ListItem>
                            <asp:ListItem Value="ACCOUNT CODE">Account Code</asp:ListItem>
                            <asp:ListItem Value="BANK CHARGE BASIS">Bank Charge Basis</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvInvoiceHistory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false"
                AllowSorting="true" EnableViewState="false" OnNeedDataSource="gvInvoiceHistory_NeedDataSource">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" DataKeyNames="FLDREMITTANCEID"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHistoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHISTORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblInvoiceCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date/Time of Change" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUPDATEDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type of Change" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeofChange" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User Short Code" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User Name" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Field" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblField" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELD")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Old Value">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOldValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSVALUE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Value" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVALUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Procedure Used" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcedureUsed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
