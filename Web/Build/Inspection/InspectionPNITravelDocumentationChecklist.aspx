<%@ Page Language="C#" AutoEventWireup="True" CodeFile="InspectionPNITravelDocumentationChecklist.aspx.cs"
    Inherits="InspectionPNITravelDocumentationChecklist" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quick</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersQuick" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />        
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="92%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureQuick" width="100%">
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Department ID="ucDepartment" runat="server" VesselsOnly="true" Width="25%"
                            AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersQuick" runat="server" OnTabStripCommand="RegistersQuick_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvQuick" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvQuick_ItemCommand" OnNeedDataSource="gvQuick_NeedDataSource" Height="88%"
                OnItemDataBound="gvQuick_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
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
                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderText="Sort Order" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSortOrderHeader" runat="server">Sort Order</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSortOrderEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>' Width="90%"
                                    CssClass="gridinput_mandatory" MaxLength="10">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSortOrderAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="10" Width="90%"
                                    ToolTip="Enter Sort Order">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sort Order Name" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSortOrderNameHeader" runat="server">Sort Order Name</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSortOrderName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSortOrderNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDERNAME") %>' Width="90%"
                                    CssClass="gridinput" MaxLength="10">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSortOrderNameAdd" runat="server" CssClass="gridinput" MaxLength="10" Width="90%"
                                    ToolTip="Enter Sort Order">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Part" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPartHeader" runat="server">Part</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPart" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPART") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblPartId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPART") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblPartIdedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPART") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlPartEdit" runat="server" CssClass="gridinput_mandatory"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>                                        
                                        <telerik:RadComboBoxItem Text="Select" Value="Dummy" />
                                        <telerik:RadComboBoxItem Text="OFF-SIGNER" Value="1" />
                                        <telerik:RadComboBoxItem Text="ON-SIGNER" Value="2" />
                                        <telerik:RadComboBoxItem Text="PART C" Value="3" />
                                        <telerik:RadComboBoxItem Text="PART E" Value="5" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlPartAdd" runat="server" AutoPostBack="true" CssClass="gridinput_mandatory"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Select" Value="Dummy"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="OFF-SIGNER" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="ON-SIGNER" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="PART C" Value="3"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="PART E" Value="5"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Department" HeaderStyle-Width="170px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepartmentHeader" runat="server">Department</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDepartment" runat="server" CommandName="EDIT" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblDepartmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDepartmentIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTID") %>'></telerik:RadLabel>
                                <eluc:Department ID="ucDepartmentEdit" runat="server" CssClass="gridinput_mandatory" DepartmentList='<%#PhoenixRegistersDepartment.Listdepartment(1,null)%>'
                                    AppendDataBoundItems="true" SelectedDepartment='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Department ID="ucDepartmentAdd" runat="server" CssClass="gridinput_mandatory" 
                                    Width="90%" AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblItemHeader" runat="server">Item</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtItemNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' Width="95%"
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtItemNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width="95%"
                                    ToolTip="Enter Item Name">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActiveHeader" runat="server">Active</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE").ToString()=="1"?"YES":"NO" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="cbActiveEdit" runat="server" AutoPostBack="false" />
                                <telerik:RadLabel ID="lblActiveEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></telerik:RadLabel>                                
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="cbActiveAdd" runat="server" AutoPostBack="false" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FooterText="Short Code" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblshortcodehead" runat="server">Short Code</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblshortcodename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtshortcodenameedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'
                                    CssClass="gridinput" MaxLength="10">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtshortcodenameadd" runat="server" CssClass="gridinput" MaxLength="10" Width="90%"
                                    ToolTip="Enter short code">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                            <%--   <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
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
