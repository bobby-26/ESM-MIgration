<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDMRProductType.aspx.cs" Inherits="RegistersDMRProductType" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Product Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    </head>
<body>
    <form id="frmLocation" runat="server">
     <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                      
                    <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="MenuLocation_TabStripCommand">
                    </eluc:TabStrip>
                    
                    <telerik:RadGrid ID="gvProductType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvProductType_ItemCommand" OnItemDataBound="gvProductType_ItemDataBound"
                        AllowSorting="true" OnSorting="gvProductType_Sorting" ShowFooter="true" AllowPaging="true" AllowCustomPaging="true"
                        ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvProductType_NeedDataSource " RenderMode="Lightweight"
                        GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true">

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
                          
                            <telerik:GridTemplateColumn HeaderText="Short code" FooterText="New Short"  AllowSorting="true" SortExpression="FLDSHORTNAME">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblShortName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' ></telerik:RadLabel>
                                </ItemTemplate>
                                
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtShortNameEdit" runat="server" CssClass="gridinput_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' Width=50% visible = "false"  />
                                          <telerik:RadLabel ID="lblShortName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' ></telerik:RadLabel>
                                </EditItemTemplate>
                                
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtShortNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="10" Width=50% />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Description" FooterText="New Dmr"   AllowSorting="true" SortExpression="FLDPRODUCTTYPENAME">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <%--<HeaderTemplate>
                                    <asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDPRODUCTTYPENAME"
                                        ForeColor="White">Description</asp:LinkButton>
                                    <img id="FLDLOCATIONNAME" alt="" runat="server" visible="false" />
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblProducttypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPEID") %>'></telerik:RadLabel>
                                   <telerik:RadLabel ID="lblLocationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPENAME") %>' ></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblProducttypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPEID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtProductTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPENAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" Width=90% ></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtProductTypeAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width=90% ></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                         <%--   <asp:TemplateField FooterText="Sort Order">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSortOrder" runat="server" Text="Sort Order"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtSortOrderEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'
                                        CssClass="input_mandatory" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="txtSortOrderAdd" runat="server" CssClass="input_mandatory"/>
                                </FooterTemplate>
                            </asp:TemplateField>--%>
                            
                            <telerik:GridTemplateColumn HeaderText="Action" >
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