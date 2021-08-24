<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVoucherLineitemUpload.aspx.cs"
    Inherits="AccountsVoucherLineitemUpload" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voucher Upload</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="VoucherUpload" runat="server">
           <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--            <eluc:Title runat="server" ID="Attachment" Text="Voucher Upload" ShowMenu="false"></eluc:Title>--%>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuVoucherUpload" runat="server" OnTabStripCommand="MenuVoucherUpload_TabStripCommand">
            </eluc:TabStrip>
        <table id="tblFiles">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                </td>
                <td colspan="2">
                    <asp:FileUpload ID="txtFileUpload" runat="server" CssClass="input" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Visible="false">Status</telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="49"
                        CssClass="input_mandatory" Visible="false" ShortNameFilter="APP,NAP,CAP" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="lnkDownloadExcel" runat="server" Text="Download Template"
                        OnClick="lnkDownloadExcel_Click"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvVoucherUpload" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnItemDataBound="gvVoucherUpload_RowDataBound" AllowPaging="true" AllowCustomPaging="true"
            OnDeleteCommand="gvVoucherUpload_RowDeleting" AllowSorting="true" OnItemCommand="gvVoucherUpload_RowCommand" GroupingEnabled="false" EnableHeaderContextMenu="true" OnNeedDataSource="gvVoucherUpload_NeedDataSource">
              <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
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
                <telerik:GridTemplateColumn>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblDateHdr" runat="server" Text="Date"></telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucheruploadId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVOUCHERUPLOADID").ToString() %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE").ToString() %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblFileNameHdr" runat="server" Text="File Name"></telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Post" ImageUrl="<%$ PhoenixTheme:images/completed.png %>"
                            CommandName="POST" CommandArgument='<%# Container.DataItem %>' ID="cmdPost"
                            ToolTip="Post"></asp:ImageButton>
                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                            CommandName="DELETE" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                            ToolTip="Discard"></asp:ImageButton>
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
