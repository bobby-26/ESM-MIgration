<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostEvaluationChatQuoteDetails.aspx.cs"
    Inherits="CrewCostEvaluationChatQuoteDetails" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Cost Evaluation Quote</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCostEvaluationQuoteItems" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" EnableViewState="false" Height="95%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvLineItem_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvLineItem_ItemDataBound"
                OnItemCommand="gvLineItem_ItemCommand" ShowFooter="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Section Type" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTELINEITEMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblSectionTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Section" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" Wrap="False" />
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotal" runat="server" Font-Bold="true" Text="Total Amount"></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="AMOUNT" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" Wrap="False" />
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotalA" runat="server" Font-Bold="true" Text=""></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
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
