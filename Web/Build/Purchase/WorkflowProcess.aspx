<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowProcess.aspx.cs" Inherits="WorkflowProcess" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

            <table id="tblSearch">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUniqueName" runat="server" Text="Unique Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUniqueName" runat="server"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server"></telerik:RadTextBox>
                    </td>

                </tr>
            </table>

            <eluc:TabStrip ID="MenuWorkProcess" runat="server" OnTabStripCommand="MenuWorkProcess_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvProcess" runat="server" CellSpacing="0" GridLines="None" Height="88%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableHeaderContextMenu="true" EnableViewState="false"
                GroupingEnabled="false" OnNeedDataSource="gvProcess_NeedDataSource" OnItemCommand="gvProcess_ItemCommand">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDPROCESSID">

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

                        <telerik:GridTemplateColumn HeaderText="Unique Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNIQUENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblId" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPROCESSID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Administrator" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdministrator" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADMINISTRATOR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Procedure Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcedureName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />

                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>

            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>

        <div>
        </div>
    </form>
</body>
</html>
