<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsBankStatementReconExcelUploadAllocation.aspx.cs"
    Inherits="Accounts_AccountsBankStatementReconExcelUploadAllocation" %>

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
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmForwarderExcelUpload" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--            <eluc:Title runat="server" ID="Attachment" Text="Allocation/Bank Tag Report"></eluc:Title>--%>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="Bankupload" TabStrip="true" runat="server" OnTabStripCommand="Bankupload_OnTabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuAttachment" runat="server" OnTabStripCommand="MenuAttachment_OnTabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" Height="87%" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true" GroupingEnabled="false" EnableHeaderContextMenu="true" OnItemCommand="gvAttachment_ItemCommand"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnItemDataBound="gvAttachment_ItemDataBound" OnNeedDataSource="gvAttachment_NeedDataSource">
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
                    <telerik:GridTemplateColumn HeaderText="Bank Tagging Id">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBankTagId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDBANKTAGNO")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Currency">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Bank Amount Net">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="RIGHT"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBankAmount" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDBANKSTATEMENTAMOUNT", "{0:n2}")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Allocated in Ledger">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="RIGHT"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAllocatedinLeger" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDALLOCATEDAMOUNT", "{0:n2}")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remaining amount">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="RIGHT"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemainingamount" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDREMAININGAMOUNT", "{0:n2}")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Allocation Status">
                        <ItemStyle HorizontalAlign="LEFT"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAllocationStatus" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDALLOCATIONSTATUS")) %>'></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtuploadid" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDUPLOADID") %>'
                                Visible="false">
                            </telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="More Info" ImageUrl="<%$ PhoenixTheme:images/te_notes.png %>"
                                CommandName="INFO" CommandArgument="<%# Container.DataItem %>" ID="cmdMoreInfo"
                                ToolTip="More Info"></asp:ImageButton>
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

    </form>
</body>
</html>
