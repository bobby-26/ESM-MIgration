<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsBankStatementReconExcelUploadLineitem.aspx.cs"
    Inherits="Accounts_AccountsBankStatementReconExcelUploadLineitem" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bank Statement Upload</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
          <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvAttachment.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmForwarderExcelUpload" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"  EnableAJAX="false">

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--        <eluc:Title runat="server" ShowMenu="false" ID="Attachment" Text="More Info"></eluc:Title>--%>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="Bankupload" runat="server"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuAttachment" runat="server" OnTabStripCommand="MenuAttachment_OnTabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" GroupingEnabled="false" EnableHeaderContextMenu="true" OnNeedDataSource="gvAttachment_NeedDataSource">
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
                    <%--   Tag ID	Currency	Bank Amount Net	Allocated in Leger	Remaining amount	Allocation Status	Action--%>
                    <%--                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                    <telerik:GridTemplateColumn HeaderText="Bank Tagging Id">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBankTagId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDTAGNO")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Bank Voucher Number">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBank" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDBANKVOUCHERNUMBER")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Ledger TT Ref">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBankAmount" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDTTREFERENCE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Ledger Long description">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAllocatedinLeger" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION" )) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Currency">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE" )) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblAmount" runat="server"></telerik:RadLabel>
                            <%=strTransactionAmountTotal%>
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="RIGHT"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemainingamount" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:n2}" )) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status">
                        <ItemStyle HorizontalAlign="LEFT"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAllocationStatus" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSTATUS")) %>'></telerik:RadLabel>
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
