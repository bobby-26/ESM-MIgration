<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSubCategoryTaskList.aspx.cs" Inherits="CrewOffshore_CrewOffshoreSubCategoryTaskList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvOffshoreSubCategoryTask.ClientID %>"));
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
    <form id="frmOffshoreCategory" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

            <table id="tblcompetenceconduct" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCompetenceCategory" runat="server" CssClass="input" Width="200px"
                            EmptyMessage="Type to select" MarkFirstMatch="true"
                            AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCompetenceCategory_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Sub Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddsubcategory" runat="server" CssClass="input" Width="200px"
                            OnSelectedIndexChanged="ddsubcategory_SelectedIndexChanged"
                            AppendDataBoundItems="true" AutoPostBack="true"
                            EmptyMessage="Type to select" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblappliesrank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlrank" runat="server" Width="200px" EmptyMessage="Type to select" MarkFirstMatch="true"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbllabel" runat="server" Text="Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddllevel" runat="server" Width="200px"
                            EmptyMessage="Type to select level" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="1" Value="1" />
                                <telerik:RadComboBoxItem Text="2" Value="2" />
                                <telerik:RadComboBoxItem Text="3" Value="3" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuOffshoreSubCategory" runat="server" OnTabStripCommand="MenuOffshoreSubCategory_TabStripCommand"></eluc:TabStrip>

            <%-- <asp:GridView ID="gvOffshoreSubCategory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvOffshoreSubCategory_RowCommand"
                            OnRowEditing="gvOffshoreSubCategory_RowEditing" OnRowCancelingEdit="gvOffshoreSubCategory_RowCancelingEdit"
                            OnRowUpdating="gvOffshoreSubCategory_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDSUBCATEGORYID"
                            OnRowDataBound="gvOffshoreSubCategory_RowDataBound" OnRowDeleting="gvOffshoreSubCategory_OnRowDeleting">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshoreSubCategoryTask" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOffshoreSubCategoryTask_NeedDataSource"
                OnItemDataBound="gvOffshoreSubCategoryTask_ItemDataBound"
                OnItemCommand="gvOffshoreSubCategoryTask_ItemCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false" ShowFooter="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDTASKID">
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
                        <%-- <telerik:GridTemplateColumn HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCategoryHeader" runat="server">Category</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                           
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblsubCategoryHeader" runat="server">Sub Category</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FKDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lbldiscripHeader" runat="server">Task</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCategoryHeader" runat="server">Description</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldiscrip" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblranksHeader" runat="server">Rank</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblranksNAME" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' runat="server"></telerik:RadLabel>


                            </ItemTemplate>


                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblassesHeader" runat="server">Assessor</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblassesHeader1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSESSORNAME") %>'></telerik:RadLabel>


                            </ItemTemplate>


                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lbllevelHeader" runat="server">Level</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbllevel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>'></telerik:RadLabel>
                            </ItemTemplate>


                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.ItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.ItemIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />

                            </ItemTemplate>


                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>



    </form>
</body>
</html>
