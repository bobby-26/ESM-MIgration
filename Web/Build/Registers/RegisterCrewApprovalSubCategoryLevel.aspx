<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewApprovalSubCategoryLevel.aspx.cs" Inherits="RegisterCrewApprovalSubCategoryLevel" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSubCategoryLevel.ClientID %>"));
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
    <form id="form1" runat="server">
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">

            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <table>
                <tr>
                    <td>Category</td>
                    <td>
                        <telerik:RadComboBox ID="ddlcategory" OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged" AutoPostBack="true" runat="server"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>Sub Category</td>
                    <td>
                        <telerik:RadComboBox ID="ddlsubcategory" OnSelectedIndexChanged="ddlsubcategory_SelectedIndexChanged" AutoPostBack="true" runat="server"></telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="gvSubCategoryLevelTab" Visible="false" runat="server" OnTabStripCommand="gvSubCategoryLevelTab_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSubCategoryLevel" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" ShowFooter="True" EnableViewState="true" OnSortCommand="gvSubCategoryLevel_SortCommand"
                OnNeedDataSource="gvSubCategoryLevel_NeedDataSource" OnItemCommand="gvSubCategoryLevel_ItemCommand" OnItemDataBound="gvSubCategoryLevel_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
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
                        <telerik:GridTemplateColumn HeaderText="Sub Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <HeaderStyle Width="35%" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsubcatname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSUBCATEGORYNAME") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblsubcatlevelid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYLEVELID") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>

                                <telerik:RadLabel ID="lblsubcatnameedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSUBCATEGORYNAME") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Level Name" AllowSorting="true" SortExpression="FLDLEVELNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="55%" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLevelName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblsubCategoryLevelId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYLEVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtLevelNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>'
                                    CssClass="gridinput_mandatory" Width="100%">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblsubCategoryLevelIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYLEVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtLevelnameAdd" runat="server" Text="" Width="100%" CssClass="gridinput_mandatory">
                                </telerik:RadTextBox>
                            </FooterTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approver" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="55%" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCongigName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGURATIONNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCongigid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROLECONFIGURATIONID") %>'></telerik:RadLabel>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCongigidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROLECONFIGURATIONID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlConfignameedit" runat="server"></telerik:RadComboBox>

                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlConfignameadd" runat="server"></telerik:RadComboBox>
                            </FooterTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sequence" AllowSorting="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsquence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCENUMBER") %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="More Info" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="55%" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/billingparties.png %>"
                                    CommandName="MOREINFO" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMoreInfo"
                                    ToolTip="User List"></asp:ImageButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="SAVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/add.png %>"
                                    CommandName="ADD" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria" PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
