<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentList.aspx.cs" Inherits="PlannedMaintenanceComponentList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPlannedMaintenanceComponent" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

        <eluc:TabStrip ID="MenuRegistersComponent" runat="server" OnTabStripCommand="MenuRegistersComponent_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadGrid RenderMode="Lightweight" ID="gvComponent" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvComponent_NeedDataSource" Width="100%" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvComponent_ItemDataBound" OnItemCommand="gvComponent_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCOMPONENTID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" Visible="false" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDMISCELLANEOUS1">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMISCELLANEOUS1") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Class" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTCLASS">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentClassName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.COMPONENTCLASSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobFrequencyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastDoneDateName" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Next Due Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNextDueDateName" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBNEXTDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="150px" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
