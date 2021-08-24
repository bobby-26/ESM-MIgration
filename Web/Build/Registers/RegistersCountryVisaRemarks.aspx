<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCountryVisaRemarks.aspx.cs" Inherits="RegisterCountryVisaRemarks" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Remarks</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmProcedure" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:TabStrip ID="MenuComment" runat="server" Title="Procedure" OnTabStripCommand="MenuComment_TabStripCommand"></eluc:TabStrip>
            <telerik:RadEditor ID="txtProcedure" runat="server" Width="100%" EmptyMessage="" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            </telerik:RadEditor>
            <%--<ajaxToolkit:Editor ID="txtProcedure" runat="server" Height="90%" Width="100%" ActiveMode="Design" />   --%>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvRemarks" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvRemarks_ItemCommand" OnNeedDataSource="gvRemarks_NeedDataSource"
                EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSortCommand="gvRemarks_SortCommand">
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="200px" HeaderText="Posted By">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPostedBy" runat="server" Text="Posted By"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPostedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="300px" HeaderText="Comments">
                            <ItemStyle Wrap="true" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblComments" runat="server" Text="Comments"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCOmments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' CommandName="Select" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Posted Date"> 
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPostedDate" runat="server" Text="Posted Date"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPostedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true"  ScrollHeight=""/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
