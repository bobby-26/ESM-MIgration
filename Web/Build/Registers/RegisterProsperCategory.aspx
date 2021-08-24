<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterProsperCategory.aspx.cs" Inherits="Registers_RegisterProsperCategory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="num" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Category</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmprospercategory" runat="server">
        <div>
            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager ID="skin1" runat="server"></telerik:RadSkinManager>
            <telerik:RadAjaxPanel ID="panel1" runat="server">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />


                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblprospercategory" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCode" runat="server" Text="Category Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtcategorycode" runat="server" MaxLength="6" CssClass="input"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblName" runat="server" Text="Category Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtname" runat="server" MaxLength="200"
                                    CssClass="input" Width="240px">
                                </telerik:RadTextBox>
                            </td>

                        </tr>
                    </table>
                </div>

                <eluc:TabStrip ID="MenuRegistersProsper" runat="server" OnTabStripCommand="RegistersProsper_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 0">
                    <%-- <asp:GridView ID="gvprospercategory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3"
                            OnRowDataBound="gvprospercategory_RowDataBound" OnRowEditing="gvprospercategory_RowEditing"
                            OnRowCancelingEdit="gvprospercategory_RowCancelingEdit" OnRowCommand="gvprospercategory_RowCommand"
                            OnRowDeleting="gvprospercategory_RowDeleting" OnRowUpdating="gvprospercategory_RowUpdating"
                            OnRowCreated="gvprospercategory_RowCreated"
                            ShowFooter="false" DataKeyNames="FLDCATEGORYID" EnableViewState="false" AllowSorting="true" OnSorting="gvprospercategory_Sorting">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvprospercategory" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvprospercategory_NeedDataSource"
                        OnItemCommand="gvprospercategory_ItemCommand"
                        OnItemDataBound="gvprospercategory_ItemDataBound"
                        OnSortCommand="gvprospercategory_SortCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDCATEGORYID">
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
                                <telerik:GridTemplateColumn HeaderStyle-Width="20%" HeaderText="Code">

                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkcatogoryCode" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYCODE") %>' CommandName="EDIT"></asp:LinkButton>

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="txtCategoryCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYCODE") %>'
                                            MaxLength="10">
                                        </telerik:RadLabel>

                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <telerik:RadTextBox ID="txtCategoryCodeAdd" runat="server" CssClass="gridinput_mandatory"
                                            MaxLength="10">
                                        </telerik:RadTextBox>

                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="60%" HeaderText="Name">

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkCategoryName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>' CommandName="EDIT">
                                        </telerik:RadLabel>

                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <telerik:RadTextBox ID="txtCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'
                                            CssClass="gridinput_mandatory" MaxLength="200" Width="98%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>

                                        <telerik:RadTextBox ID="txtCategoryNameAdd" runat="server" CssClass="gridinput_mandatory" Width="98%"
                                            MaxLength="200">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="10%" HeaderText="Max Score">
                                    <ItemStyle HorizontalAlign="Right" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkscore" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXSCORE") %>'>
                                        </telerik:RadLabel>

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:num ID="txtmaxscore" runat="server" MaxLength="6"
                                            CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXSCORE") %>' />


                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:num ID="txtmaxscoreAdd" runat="server" MaxLength="6" CssClass="gridinput_mandatory" />


                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="10%" HeaderText="No Inspection</br> Score">
                                    <ItemStyle HorizontalAlign="Right" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkINSscore" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOINSPECTIONSCORE") %>'>
                                        </telerik:RadLabel>

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:num ID="txtnoinsscore" runat="server" MaxLength="6"
                                            CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOINSPECTIONSCORE") %>' />


                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:num ID="txtnoinsscoreAdd" runat="server" MaxLength="6" CssClass="gridinput_mandatory" />


                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="10%" HeaderText="Sort Order">
                                    <ItemStyle HorizontalAlign="Right" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkorder" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDER") %>'>
                                        </telerik:RadLabel>

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:num ID="txtsortorderedit" runat="server" MaxLength="6"
                                            CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDER") %>' />


                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:num ID="txtsortorderadd" runat="server" MaxLength="6" CssClass="gridinput_mandatory" />


                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="10%" HeaderText="Action">

                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>

                                        </asp:LinkButton>
                                      
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete">
                                             <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Map Category" 
                                            CommandName="MAPPING" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMap"
                                            ToolTip="Map Category">
                                            <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>

                                        </asp:LinkButton>


                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
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


            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
