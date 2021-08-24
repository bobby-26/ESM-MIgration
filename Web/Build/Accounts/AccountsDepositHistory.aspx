<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDepositHistory.aspx.cs" Inherits="Accounts_AccountsDepositHistory" %>

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
    <title>Accounts Deposit History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" />
        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" Title="Deposit History" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <br />
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblHistoryType" runat="server" Text="History Type"></telerik:RadLabel>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblHistoryType" runat="server" AppendDataBoundItems="false"
                        RepeatDirection="Horizontal" AutoPostBack="true" RepeatLayout="Table" OnSelectedIndexChanged="ReBindData"
                        CssClass="readonlytextbox" Enabled="true">
                        <asp:ListItem Value="DEPOSIT STATUS" Selected="True">Status</asp:ListItem>
                        <asp:ListItem Value="DEPOSIT CURRENCY">Currency</asp:ListItem>
                        <asp:ListItem Value="DEPOSIT AMOUNT">Amount</asp:ListItem>
                        <asp:ListItem Value="DEPOSIT SUPPLIER">Supplier</asp:ListItem>
                        <asp:ListItem Value="DEPOSIT VOUCHER NUMBER">Deposit Voucher No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <br />
        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvDepositHistory" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid ID="gvDepositHistory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvDepositHistory_NeedDataSource">
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDDEPOSITID">
                <HeaderStyle Width="102px" HorizontalAlign="Center" />
                <Columns>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblHistoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHISTORYID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblDepositCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPOSITID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date/Time of Change">
                        <HeaderStyle Width="154px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDateHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUPDATEDATE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type of Change">
                        <HeaderStyle Width="134px" HorizontalAlign="Left" />
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
                    <telerik:GridTemplateColumn HeaderText="User Name">
                        <HeaderStyle Width="175px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Field">
                        <HeaderStyle Width="175px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblField" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELD")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Old Value">
                        <HeaderStyle Width="175px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOldValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSVALUE") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="New Value">
                        <HeaderStyle Width="175px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNewValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVALUE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Procedure Used">
                        <HeaderStyle Width="175px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblProcedureUsed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" EnablePostBackOnRowClick="false" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                <%--<ClientEvents OnGridCreated="GridCreated" />--%>
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
