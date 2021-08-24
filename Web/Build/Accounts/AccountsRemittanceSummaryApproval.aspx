<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceSummaryApproval.aspx.cs"
    Inherits="AccountsRemittanceSummaryApproval" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvRemittance.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>

        <%--   <style>
        .bold {
            font-weight: bold;
        }
    </style>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="Form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <telerik:RadAjaxPanel runat="server" ID="pnlStockItemEntry">

            <%--                        <eluc:Title runat="server" ID="Title1" Text="MD Approval "></eluc:Title>--%>
            <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRemittance" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnRowCreated="gvRemittance_OnRowCreated" OnNeedDataSource="gvRemittance_NeedDataSource"
                OnItemCommand="gvRemittance_RowCommand" OnItemDataBound="gvRemittance_ItemDataBound" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                        <telerik:GridTemplateColumn HeaderText="Bank Account">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNTNUMBER") %>
                                <telerik:RadLabel ID="lblBankInstructionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEINSTRUCTIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblBankAccountCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Company">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYSHORTCODE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supplier">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTCURRENCYCODE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDTOTALPAYABLEAMOUNT","{0:n2}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="USD Equivalent">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDUSDEQUVALENT","{0:n2}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center" HeaderText="Action">
                            <HeaderStyle />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">Action </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemTemplate>
                                <asp:ImageButton ID="cmdReject" runat="server" AlternateText="Order" CommandArgument="<%# Container.DataItem %>"
                                    CommandName="REJECT" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Reject" />
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
