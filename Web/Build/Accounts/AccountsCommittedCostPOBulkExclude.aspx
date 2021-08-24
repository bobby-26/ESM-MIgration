<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCommittedCostPOBulkExclude.aspx.cs" Inherits="Accounts_AccountsCommittedCostPOBulkExclude" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Committed Cost PO Bulk Exclude</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuAdminPage" runat="server" OnTabStripCommand="MenuAdminPage_OnTabStripCommand"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td>

                    <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblfromDate" runat="server" Text="Exclude with effect from"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucExcludeDate" runat="server" CssClass="input_mandatory" DatePicker="true" />

                </td>
            </tr>
        </table>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" Height="52%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnItemCommand="gvAttachment_ItemCommand" OnItemDataBound="gvAttachment_ItemDataBound" OnNeedDataSource="gvAttachment_NeedDataSource"
            ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                    <telerik:GridTemplateColumn HeaderText="File Name" AllowSorting="true">
                        <HeaderStyle Width="35%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblattacmentid" runat="server" CssClass="hidden" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDATTACHMENTID")) %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblfilename" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDFILENAME")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <%-- <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" visible="false" />
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Verify"
                                CommandName="VERIFY" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdverify"
                                ToolTip="Verify">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>--%>                            
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Update"
                                CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdupdate"
                                ToolTip="Update">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
                            </asp:LinkButton>
                            <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" visible="false" />
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmddelete"
                                ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
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
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblnotfound" runat="server" Text="Can Not Found"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <telerik:RadLabel ID="lblnotfouncount" runat="server"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblfailedpo" runat="server" Text="Excluding Po Failed"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <telerik:RadLabel ID="lblfailedcount" runat="server"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblpoclosed" runat="server" Text="Closed PO"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <telerik:RadLabel ID="lblclosedcount" runat="server"></telerik:RadLabel>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblponotfound" runat="server" Text="Can not be found"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblpofailed" runat="server" Text="Excluding PO failed"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblclosedpo" runat="server" Text="Closed PO"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvponotfound" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvponotfound_NeedDataSource"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                                <telerik:GridTemplateColumn HeaderText="PO Number">
                                    <HeaderStyle Width="35%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="ponumbernotfound" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPOCANNOTBEFOUNDLIST")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </td>
                <td>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvpofailed" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvpofailed_NeedDataSource"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                                <telerik:GridTemplateColumn HeaderText="PO Number">
                                    <HeaderStyle Width="35%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="failedponumber" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDEXCLUDINGPOFAILED")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </td>
                <td>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvpoclosed" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvpoclosed_NeedDataSource"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                                <telerik:GridTemplateColumn HeaderText="PO Number">
                                    <HeaderStyle Width="35%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="closedponumber" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCLOSEDPOLIST")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
        <asp:Button ID="confirm" runat="server" CssClass="hidden" Text="confirm" />

    </form>
</body>
</html>
