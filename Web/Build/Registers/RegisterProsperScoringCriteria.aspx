<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterProsperScoringCriteria.aspx.cs" Inherits="Registers_RegisterProsperScoringCriteria" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="num" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scoring Criteria</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmprosperscoringcriteria" runat="server">
        <div>
            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxPanel ID="panel1" runat="server">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

                    <div id="divPart" style="position: relative; z-index: 2">
                        <table id="tblprosperscore" width="100%">
                            <tr>
                                <td width="20%">
                                    <telerik:RadLabel ID="lblCategoryName" runat="server" Text="Category Name"></telerik:RadLabel>
                                </td>
                                <td width="30%">
                                    <telerik:RadTextBox ID="txtCategoryName" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="385px"></telerik:RadTextBox>
                                </td>
                                <td width="20%">
                                    <telerik:RadLabel ID="lblmeasureName" runat="server" Text="Measure Name"></telerik:RadLabel>
                                </td>
                                <td width="30%">
                                    <telerik:RadTextBox ID="txtMeasureName" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="385px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                        </table>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0">
                        <%-- <asp:GridView ID="gvprosperscoringcriteria" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowFooter="true"
                            ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnRowDataBound="gvprosperscoringcriteria_RowDataBound" OnRowEditing="gvprosperscoringcriteria_RowEditing"
                            OnRowCancelingEdit="gvprosperscoringcriteria_RowCancelingEdit" OnRowCommand="gvprosperscoringcriteria_RowCommand"
                            OnRowDeleting="gvprosperscoringcriteria_RowDeleting" OnRowUpdating="gvprosperscoringcriteria_RowUpdating"
                            OnRowCreated="gvprosperscoringcriteria_RowCreated"
                            DataKeyNames="FLDSCORINGCRITERIAID" OnSorting="gvprosperscoringcriteria_Sorting">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvprosperscoringcriteria" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvprosperscoringcriteria_NeedDataSource"
                            OnItemCommand="gvprosperscoringcriteria_ItemCommand"
                            OnItemDataBound="gvprosperscoringcriteria_ItemDataBound"
                            OnSortCommand="gvprosperscoringcriteria_SortCommand"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                            AutoGenerateColumns="false" ShowFooter="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDSCORINGCRITERIAID">
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
                                    <telerik:GridTemplateColumn HeaderText="Minimum Range">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                        <ItemTemplate>

                                            <telerik:RadLabel ID="lblcategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblmeasureid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASUREID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblmin" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIN") %>' CommandName="EDIT">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <telerik:RadLabel ID="lblscorecriteriaid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORINGCRITERIAID") %>'></telerik:RadLabel>

                                            <eluc:num ID="txtminedit" runat="server" MaxLength="6"
                                                CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIN") %>' />
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>

                                            <eluc:num ID="txtminadd" runat="server" MaxLength="6" CssClass="gridinput_mandatory" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Maximum Range">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                        <ItemTemplate>

                                            <telerik:RadLabel ID="lblMax" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAX") %>' CommandName="EDIT">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <eluc:num ID="txtmaxedit" runat="server" MaxLength="6"
                                                CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAX") %>' />
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>

                                            <eluc:num ID="txtmaxadd" runat="server" MaxLength="6" CssClass="gridinput_mandatory" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Score">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                        <ItemTemplate>

                                            <telerik:RadLabel ID="lblscore" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>' CommandName="EDIT">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <eluc:num ID="txtscoreedit" runat="server" MaxLength="6" IsInteger="true" IsPositive="false"
                                                CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>' />
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>

                                            <eluc:num ID="txtscoreadd" runat="server" MaxLength="6" IsInteger="true" IsPositive="false" CssClass="gridinput_mandatory" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
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
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" 
                                                CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                ToolTip="Save">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            
                                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                                CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" 
                                                CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                                ToolTip="Add New">
                                                <span class="icon"><i class="fa fa-plus-circle"></i></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="300px" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>

                </div>
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
