<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentsDataDictionary.aspx.cs" Inherits="PhoenixDataDictionary" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvTableColumnSearch");
                grid._gridDataDiv.style.height = (browserHeight - 120) + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <br />
            <telerik:RadLabel ID="TableLbl" runat="server" Text="Table Name :"></telerik:RadLabel>
            <telerik:RadComboBox ID="TableDropDown" runat="server" Width="30%" AutoPostBack="true" OnSelectedIndexChanged="TableDropDown_SelectedIndexChanged" Filter="Contains"> 
            </telerik:RadComboBox>
            <br />
            <asp:Button ID="ColumnSearchButton" runat="server" Text="Search" OnClick="ColumnSearchButton_Click" CssClass="hidden" />
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTableColumnSearch" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false" Width="100%"
                OnNeedDataSource="gvTableColumnSearch_NeedDataSource" OnItemCommand="gvTableColumnSearch_ItemCommand" OnItemDataBound="gvTableColumnSearch_ItemDataBound"
                OnUpdateCommand="gvTableColumnSearch_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
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
                        <telerik:GridTemplateColumn HeaderText="Column Name">
                            <HeaderStyle HorizontalAlign="Left" Width="25%" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkColumnName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.COLUMNNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="FlblTableName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.TABLENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTableName" runat="server" Visible="false" Text='TABLENAME'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data Type">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="FlblDataType" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.DATATYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Is Nullable">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="FlblIsNullable" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.ISNULLABLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Column Description">
                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="FlblColumnDescription" runat="server" Visible="True" Width="250" Text='<%# DataBinder.Eval(Container,"DataItem.COLUMNDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="ColumnDescriptionTxtBoxEdit" runat="server" Width="250" Text='<%# DataBinder.Eval(Container,"DataItem.COLUMNDESCRIPTION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="250"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="cmdEdit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    ToolTip="Edit" CommandName="EDIT"></asp:ImageButton>
                                <asp:ImageButton runat="server" ID="spacer" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                <asp:ImageButton runat="server" ID="cmdDelete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" ID="cmdSave" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    ToolTip="Save" CommandName="Update"></asp:ImageButton>
                                <asp:ImageButton runat="server" ID="spacer1" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                <asp:ImageButton runat="server" ID="cmdCancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    ToolTip="Cancel" CommandName="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

