<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDMRDeckCargoItem.aspx.cs" Inherits="RegistersDMRDeckCargoItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMRRov Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersDMRRovType" autocomplete="off" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">             
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
                    <table id="tblDeckCargoItem"    >
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDeckCargoItem" runat="server" Text="Description"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDeckCargoItem" runat="server" MaxLength="200" 
                                    CssClass="input" Width="240px" ></telerik:RadTextBox>
                            </td>
                    
                        </tr>
                    </table>
                 <eluc:TabStrip ID="MenuRegistersDeckCargoItem" runat="server" OnTabStripCommand="RegistersDeckCargoItem_TabStripCommand">
                    </eluc:TabStrip>
                    <telerik:RadGrid ID="gvDeckCargoItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvDeckCargoItem_ItemCommand" OnItemDataBound="gvDeckCargoItem_ItemDataBound"
                        
                        ShowFooter="true" ShowHeader="true" OnSorting="gvDeckCargoItem_Sorting"  AllowSorting="true" AllowPaging="true" AllowCustomPaging="true" 
                        
                         OnNeedDataSource="gvDeckCargoItem_NeedDataSource"
                        RenderMode="Lightweight" GridLines="None" EnableViewState="false"  GroupingEnabled="false" EnableHeaderContextMenu="true">
                        <%--OnSelectedIndexChanging="gvDeckCargoItem_SelectedIndexChanging" --%>

                         <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                         <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                             HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                             AutoGenerateColumns="false" TableLayout="Fixed">
                             <NoRecordsTemplate>
                                 <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                     Font-Bold="true">
                                 </telerik:RadLabel>
                             </NoRecordsTemplate>
                             <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn FooterText="DeckCargoItem" HeaderText="Short Code"  AllowSorting="true" SortExpression="FLDDECKCARGOITEMCODE">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDeckCargoItemID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDECKCARGOITEMID") %>' ></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkDeckCargoItemCode" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDECKCARGOITEMCODE") %>'></asp:LinkButton>
                                </ItemTemplate>

                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblDeckCargoItemIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDECKCARGOITEMID") %>' ></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtDeckCargoItemIDEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDECKCARGOITEMCODE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"  ></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtDeckCargoItemCodeAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="4" ToolTip="Enter Deck Cargo Item Code"  ></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Description" AllowSorting="true" SortExpression="FLDDECKCARGOITEMNAME"  >
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbDeckCargoItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDECKCARGOITEMNAME") %>'  ></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtDeckCargoItemNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDECKCARGOITEMNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"  ></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtDeckCargoItemNameAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Unit"  >
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbDeckCargoUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDECKCARGOITEMUNITNAME") %>'  ></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                 <eluc:Unit ID="ucUnitEdit" runat="server" AppendDataBoundItems="true" UnitList='<%# PhoenixRegistersUnit.ListUnit() %>'
                                        SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>' CssClass="gridinput_mandatory" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                 <eluc:Unit ID="ucUnitAdd" runat="server" AppendDataBoundItems="true" UnitList='<%#PhoenixRegistersUnit.ListUnit() %>' CssClass="gridinput_mandatory" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                         
                            <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                ToolTip="Edit">
              <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                ToolTip="Delete">
              <span class="icon"><i class="fa fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave"
                                ToolTip="Save">
        <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel"
                                ToolTip="Cancel">
                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Add New" CommandName="Add"
                                ID="cmdAdd">
                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                        </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                    ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>