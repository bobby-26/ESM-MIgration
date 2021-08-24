<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersBatch.aspx.cs" Inherits="RegistersBatch"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Batch</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersBatch" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Training Batch"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="80%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureBatch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBatchType" runat="server" Text="Batch Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlType" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                            <Items>                                
                                <telerik:RadComboBoxItem Value="1" Text="PreSea"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="Dummy" Text="PostSea"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBatch" runat="server" Text="Batch"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBatch" runat="server" CssClass="input" MaxLength="50"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersBatch" runat="server" OnTabStripCommand="RegistersBatch_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBatch" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvBatch_ItemCommand" OnNeedDataSource="gvBatch_NeedDataSource" Height="99%"
                OnItemDataBound="gvBatch_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="false"
                 OnSortCommand="gvBatch_SortCommand">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <%-- <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="300px" HeaderText="Institution">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkInstitutionHeader" runat="server">Institution&nbsp;</asp:LinkButton>
                                <%-- <img id="FLDINSTITUTIONNAME" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn  HeaderStyle-Width="300px" HeaderText="Course">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkCourseHeader" runat="server">Course&nbsp;</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBatchId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCourseName" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn  HeaderStyle-Width="130px" HeaderText="Batch">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkBatchHeader" runat="server">Batch&nbsp;</asp:LinkButton>
                                <%--    <img id="FLDBATCH" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBatch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn  HeaderStyle-Width="100px" HeaderText="From Date">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn  HeaderStyle-Width="100px"  HeaderText="To Date">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblToDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn  HeaderStyle-Width="50px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>                              
                                    <telerik:RadLabel ID="lblActionHeader" runat="server">
                                        Action
                                    </telerik:RadLabel>                               
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <%-- <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
