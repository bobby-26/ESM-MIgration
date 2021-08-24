<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ERMERMVoucherLineItemDetails.aspx.cs" Inherits="ERMERMVoucherLineItemDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="ToolkitScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlStockItemEntry" Height="100%" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <iframe runat="server" id="ifMoreInfo" scrolling="Yes" style="min-height: 325px; width: 100%"></iframe>
            <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadCodeBlock runat="server" ID="RadCodeBlock3">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnItemCommand="gvLineItem_ItemCommand" OnItemDataBound="gvLineItem_ItemDataBound"
                    AllowPaging="true" AllowCustomPaging="true" Height="33%" AllowSorting="true" EnableViewState="false" ShowFooter="true"
                    OnNeedDataSource="gvLineItem_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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
                            <telerik:GridTemplateColumn HeaderText="Row Number" AllowSorting="true">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkVoucherLineItemNo" runat="server" CommandArgument="<%# (Container.DataSetIndex)%>"
                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMNO") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblVoucherLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'
                                        Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Account Code" AllowSorting="true">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Account Description" AllowSorting="true">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sub Account Code" AllowSorting="true">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXSUB") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Transaction Currency" AllowSorting="true">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="" AllowSorting="true">
                                 <HeaderStyle  HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblhead1" runat="server" Text="Prime Amount"></telerik:RadLabel><br />
                                    <telerik:RadLabel ID="strTran1" runat="server"  Text='<%# strTransactionAmountTotal%>'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTranAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXPRIME","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                               
                                <FooterTemplate>
                                    <b>
                                        <asp:Literal ID="lblDebitsTotal" runat="server" Text="Debits Total"></asp:Literal>
                                    </b>
                                    <br />
                                    <b>
                                        <asp:Literal ID="lblCreditsTotal" runat="server" Text="Credits Total"></asp:Literal>
                                    </b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="" AllowSorting="true">
                                <HeaderStyle  HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblhead2" runat="server" Text="Base Amount"></telerik:RadLabel><br />
                                    <telerik:RadLabel ID="strTran2" runat="server" Text='<%# strBaseAmountTotal%>'></telerik:RadLabel>

                                    <%--<%=strBaseAmountTotal%>--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBaseAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDERMBASE","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                               
                                <FooterTemplate>
                                    <b>
                                    <telerik:RadLabel ID="strTran3" runat="server" Text='<%# strBaseDebitTotal%>'></telerik:RadLabel>

                                        <%--<%=strBaseDebitTotal%>--%>
                                    </b>
                                    <br />
                                    <b>
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text='<%# strBaseCrebitTotal%>'></telerik:RadLabel>

                                        <%--<%=strBaseCrebitTotal%>--%>
                                    </b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="" AllowSorting="true">
                                <HeaderStyle  HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblhead3" runat="server" Text="Report Amount"></telerik:RadLabel><br />
                                    <telerik:RadLabel ID="strTran4" runat="server" Text='<%# strReportAmountTotal%>'></telerik:RadLabel>

                                    <%--<%=strReportAmountTotal%>--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReportAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDERMREPORT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                               
                                <FooterTemplate>
                                    <b>
                                    <telerik:RadLabel ID="strTran5" runat="server" Text='<%# strReportDebitTotal%>'></telerik:RadLabel>

                                        <%--<%=strReportDebitTotal%>--%>
                                    </b>
                                    <br />
                                    <b>
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text='<%# strReportCrebitTotal%>'></telerik:RadLabel>

                                        <%--<%=strReportCrebitTotal%>--%>
                                    </b>
                                </FooterTemplate>
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
            </telerik:RadCodeBlock>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
