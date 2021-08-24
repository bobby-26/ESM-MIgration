<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsERMVoucherExcelUpload.aspx.cs" Inherits="AccountsERMVoucherExcelUpload" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ERM Voucher Excel Upload</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmForwarderExcelUpload" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenutravelInvoice" runat="server" OnTabStripCommand="MenutravelInvoice_OnTabStripCommand"></eluc:TabStrip>

        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload" runat="server" CssClass="input" />
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <%-- <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvAttachment_RowDataBound"
            AllowSorting="true">

            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <RowStyle Height="10px" />--%>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvAttachment_NeedDataSource"
            OnItemCommand="gvAttachment_ItemCommand"
            OnItemDataBound="gvAttachment_ItemDataBound"
            GroupingEnabled="false" EnableHeaderContextMenu="true"

            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                  
                    <telerik:GridTemplateColumn HeaderText="Account Code">
                        <itemstyle horizontalalign="Left"></itemstyle>
                       
                        <itemtemplate>
                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE").ToString() %>'> </telerik:RadLabel>
                        <telerik:RadLabel ID="lblexceluploadId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDEXCELUPLOADID").ToString() %>'></telerik:RadLabel>
                    </itemtemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Account Description">
                        <itemstyle horizontalalign="Left"></itemstyle>
                        <itemtemplate>
                        <telerik:RadLabel ID="lblAccountDesc" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACCOUNTDESCRIPTION")) %>'></telerik:RadLabel>
                    </itemtemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Voucher Earliest Date">
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <itemtemplate>
                        <telerik:RadLabel ID="lblEarliestdate" runat="server" Text='<%#Bind("FLDVOUCHERDATEEARLIEST","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                    </itemtemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Voucher Latest date">
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <itemtemplate>
                        <telerik:RadLabel ID="lblLatestDate" runat="server" Text='<%# Bind("FLDVOUCHERDATELATEST","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                    </itemtemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn>
                        <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                        <headertemplate>
                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                    </headertemplate>
                        <itemstyle wrap="False" horizontalalign="Center"></itemstyle>
                        <itemtemplate>
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                            CommandName="VIEW" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                            ToolTip="View Excel"></asp:ImageButton>
                        <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton ID="cmdLineItems" runat="server" AlternateText="Voucher Items"
                            CommandArgument="<%# Container.DataSetIndex %>" CommandName="LINEITEMS"
                            ImageUrl="<%$ PhoenixTheme:images/po-staging.png %>" ToolTip="Voucher Items" />
                    </itemtemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

        </div>
    </form>
</body>
</html>
