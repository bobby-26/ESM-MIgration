<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsJournalVoucherVoucherLineItemDetails.aspx.cs"
    Inherits="AccountsJournalVoucherVoucherLineItemDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("gvLineItem");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 140) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
    <script type="text/javascript">
            function confirmdelete(args) {
                if (args) {
                    __doPostBack("<%=confirmdelete.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlStockItemEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="height: 100%; width: 99%;"></iframe>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                    <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvLineItem_RowCommand" OnItemDataBound="gvLineItem_RowDataBound"
                        OnDeleteCommand="gvLineItem_RowDeleting" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvLineItem_NeedDataSource"
                        OnRowCreated="gvLineItem_RowCreated" ShowHeader="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                        EnableViewState="false" AllowSorting="true" ShowFooter="true" OnPageIndexChanged="gvLineItem_PageIndexChanged">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDVOUCHERLINEITEMID">
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
                                <telerik:GridTemplateColumn HeaderText="Row Number" HeaderStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkVoucherLineItemNo" runat="server" CommandArgument="<%# (Container.DataItem)%>"
                                            CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMNO") %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblVoucherLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Account Code" HeaderStyle-Width="8%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblallocation" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOCATIONYN") %>'></telerik:RadLabel>
                                   </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Account Description" HeaderStyle-Width="15%">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sub Account Code" HeaderStyle-Width="8%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Owners Budget Code " HeaderStyle-Width="8%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOwnersBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERACCOUNT")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Transaction Currency" HeaderStyle-Width="8%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="10%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <FooterStyle Wrap="False" HorizontalAlign="Right" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblPrimeAmount" runat="server"></telerik:RadLabel>
                                        <%=strTransactionAmountTotal%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTranAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSACTIONAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b>
                                            <telerik:RadLabel ID="lblDebitsTotal" runat="server" Text="Debits Total"></telerik:RadLabel>
                                        </b>
                                        <br />
                                        <b>
                                            <telerik:RadLabel ID="lblCreditsTotal" runat="server" Text="Credits Total :"></telerik:RadLabel>
                                        </b>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="10%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <FooterStyle Wrap="False" HorizontalAlign="Right" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblBaseAmount" runat="server"></telerik:RadLabel>
                                        <%=strBaseAmountTotal%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblBaseAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b>
                                            <%=strBaseDebitTotal%>
                                        </b>
                                        <br />
                                        <b>
                                            <%=strBaseCrebitTotal%>
                                        </b>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="10%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <FooterStyle Wrap="False" HorizontalAlign="Right" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblReportAmount" runat="server"></telerik:RadLabel>
                                        <%=strReportAmountTotal%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReportAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b>
                                            <%=strReportDebitTotal%>
                                        </b>
                                        <br />
                                        <b>
                                            <%=strReportCrebitTotal%>
                                        </b>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <telerik:RadLabel ID="lblIsPeriodLocked" runat="server" Text="Period Locked" Visible="false" />
                                        <telerik:RadLabel ID="lblAccountActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUTACTIVEYN") %>'
                                            Visible="false" />
                                        <%--<asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# (Container.DataItemIndex)%>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />--%>
                                        <asp:ImageButton runat="server" AlternateText="Allocation" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                            CommandName="ALLOCATION" CommandArgument='<%# Container.DataItem %>' ID="hlnkSplit"
                                            ToolTip="Allocation"></asp:ImageButton>
                                         <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="GSTDetails" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                            CommandName="GST" CommandArgument='<%# Container.DataItem %>' ID="gstdetails"
                                            ToolTip="GST Details"></asp:ImageButton>
                                        <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                        <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItem %>' ID="cmdAttachment"
                                            ToolTip="Attachment"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataItem %>' ID="cmdNoAttachment"
                                            ToolTip="No Attachment"></asp:ImageButton>
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
                     <asp:Button ID="confirmdelete" runat="server" CssClass="hidden" Text="confirmdelete" OnClick="confirmdelete_Click" />
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
