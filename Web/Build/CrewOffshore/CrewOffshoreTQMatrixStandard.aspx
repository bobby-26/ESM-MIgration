<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTQMatrixStandard.aspx.cs"
    Inherits="CrewOffshoreTQMatrixStandard" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="eluc" TagName="CharterAddress" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Src="../UserControls/UserControlTitle.ascx" TagName="Title" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlTabsTelerik.ascx" TagName="TabStrip" TagPrefix="eluc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Training and Qualification Matrix Standard</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersTrainingMatrix" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false" />



            <eluc:TabStrip ID="TabMenu" OnTabStripCommand="TabMenu_TabStripCommand" runat="server" />

            <%-- <asp:GridView ID="gvTMatrix" runat="server" Font-Size="11px" Width="100%" OnRowCommand="gvTMatrix_RowCommand"
                OnRowDataBound="gvTMatrix_RowDataBound" OnRowCancelingEdit="gvTMatrix_RowCancelEdit"
                OnRowEditing="gvTMatrix_RowEditing" OnRowUpdating="gvTMatrix_RowUpdating" OnRowCreated="gvTMatrix_RowCreated"
                OnRowDeleting="gvTMatrix_RowDeleting" OnSorting="gvTMatrix_Sorting" AllowSorting="true"
                ShowFooter="true" EnableViewState="false" AutoGenerateColumns="false" DataKeyNames="FLDTRAININGMATRIXSTANDARDID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTMatrix" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvTMatrix_NeedDataSource"
                OnItemCommand="gvTMatrix_ItemCommand"
                OnItemDataBound="gvTMatrix_ItemDataBound"
                OnSortCommand="gvTMatrix_SortCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true"

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
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTMSTDId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGMATRIXSTANDARDID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTMSTDIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGMATRIXSTANDARDID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblStdName" runat="server" Text="Standard Name" CommandName="Sort"
                                    CommandArgument="FLDSTDNAME" ForeColor="white"></asp:LinkButton>
                                <img id="FLDSTDNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStdName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTANDARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStdNameEdit" runat="server" Width="260px" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTANDARDNAME") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtStdnameAdd" runat="server" Width="260px" CssClass="input_mandatory"></asp:TextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Training and Qualification Matrix Standard">
                            <HeaderStyle Width="200px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCharter" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARTERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:CharterAddress ID="ddlCharterEdit" runat="server" AppendDataBoundItems="true"
                                    CssClass="input_mandatory" Width="260px" AddressType='<%# ((int)PhoenixAddressType.CHARTERER).ToString() %>'
                                    AddressList='<%#PhoenixRegistersAddress.ListAddress(((int)PhoenixAddressType.CHARTERER).ToString() ) %>' />
                                <telerik:RadLabel ID="lblCharterEdit" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCHARTERID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:CharterAddress ID="ddlCharterAdd" runat="server" AppendDataBoundItems="true"
                                    CssClass="input_mandatory" Width="260px" AddressType='<%# ((int)PhoenixAddressType.CHARTERER).ToString() %>'
                                    AddressList='<%#PhoenixRegistersAddress.ListAddress(((int)PhoenixAddressType.CHARTERER).ToString() ) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <FooterStyle Wrap="false" HorizontalAlign="Center" />

                          
                            <ItemTemplate>
                                <asp:LinkButton ID="cmdedit" runat="server" AlternateText="Edit" 
                                    Visible="false" CommandName="EDIT" CommandArgument='<%Container.DataSetIndex %>' ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" 
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" 
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
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



        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
