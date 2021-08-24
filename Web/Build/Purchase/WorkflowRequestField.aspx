<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowRequestField.aspx.cs" Inherits="WorkflowRequestField" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Process" Src="~/UserControls/UserControlWFProcess.ascx" %>

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
            <table id="tblSearch">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcess" runat="server" Text="Process"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Process ID="UcProcess" runat="server" AutoPostBack="true" AppendDataBoundItems="true" Width="120px" />
                    </td>

                </tr>
            </table>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuWFRequestField" runat="server" OnTabStripCommand="MenuWFRequestField_TabStripCommand" TabStrip="true" />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkflowRequestField" runat="server" CellSpacing="0" GridLines="None" Height="88%" EnableHeaderContextMenu="true"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" GroupingEnabled="false" EnableViewState="false" 
                OnNeedDataSource="gvWorkflowRequestField_NeedDataSource" OnItemDataBound="gvWorkflowRequestField_ItemDataBound" OnItemCommand="gvWorkflowRequestField_ItemCommand">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">

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

                        <telerik:GridTemplateColumn HeaderText="Process" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcessRequestFieldId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSREQUESTFIELDID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblProcessId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PROCESSNAME") %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Field Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Data Type" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDataType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATATYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Length" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLength" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLENGTH") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
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
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" EnableAllOptionInPagerComboBox="true" />
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

    </form>
</body>
</html>
