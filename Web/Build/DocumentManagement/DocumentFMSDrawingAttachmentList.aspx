<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentFMSDrawingAttachmentList.aspx.cs"
    Inherits="DocumentFMSDrawingAttachmentList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload Attachments</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function fnConfirmTelerik(sender, msg) {
                var callBackFn = function (shouldSubmit) {
                    if (shouldSubmit) {
                        //sender.click();
                        //if (Telerik.Web.Browser.ff) {
                        //    sender.get_element().click();
                        //}
                        eval(sender.target.parentElement.parentElement.href);
                    }
                    else {
                        if (e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                var confirm;

                if (msg == null)
                    confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
                else
                    confirm = radconfirm(msg, callBackFn);

                return false;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuRegistersFMSFileNo" runat="server" OnTabStripCommand="RegistersFMSFileNo_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />


            <%--<eluc:TabStrip ID="MenuRegistersFMSFileNo" runat="server" OnTabStripCommand="RegistersFMSFileNo_TabStripCommand"></eluc:TabStrip>--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFMSFileNoattachment" Height="100%" runat="server"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                GridLines="None" OnNeedDataSource="gvFMSFileNoattachment_NeedDataSource" OnItemDataBound="gvFMSFileNoattachment_ItemDataBound"
                OnRowDeleting="gvFMSFileNoattachment_RowDeleting" OnItemCommand="gvFMSFileNoattachment_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                        ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <NoRecordsTemplate>
                        <table id="Table1" runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>                        
                        <telerik:GridTemplateColumn HeaderText="File Name">
                            <HeaderStyle Width="55%" />
                            <ItemTemplate>
                                <%-- <asp:HyperLink ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'>
                                </asp:HyperLink>--%>
                                <asp:HyperLink ID="lnkfile" Target="_blank" runat="server" ToolTip="Download File"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFILENAME") %>'>
                                </asp:HyperLink>
                                <telerik:RadLabel ID="lblattachmentcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>' Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Uploaded Date">
                            <HeaderStyle Width="35%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUploaddate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUPLOADEDDATE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="Action" AllowSorting="true"
                            SortExpression="">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" runat="server" ToolTip="Download File" Visible="false"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                    <span class="icon"><i class="fas fa-file-download"></i></span>
                                </asp:HyperLink>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
