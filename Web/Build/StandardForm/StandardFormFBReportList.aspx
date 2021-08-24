<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormFBReportList.aspx.cs"
    Inherits="StandardFormFBReportList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function PaneResized() {
                //            var sender = $find('RadSplitter1');
                //            var browserHeight = $telerik.$(window).height();
                //            sender.set_height(browserHeight - 40);
                //            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 72);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="tvwCategory">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="tblConfigureAirlines"
        DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="97%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"/>
        <eluc:TabStrip ID="MenuStoreItemInOutTransaction" runat="server" OnTabStripCommand="MenuGridStoreItemInOutTransaction_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%"
            Height="97%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="200px" Height="95%">
                <eluc:TreeView ID="tvwCategory" runat="server" OnNodeClickEvent="ucTree_SelectNodeEvent" />
                <asp:Label runat="server" ID="lblSelectedNode"></asp:Label>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Height="95%">
                <table cellpadding="1" cellspacing="1" style="width: 100%;">
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFormName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td>
                        <td>
                            <telerik:RadTextBox ID="txtFormName" runat="server" CssClass="input">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <eluc:TabStrip ID="MenuGridItem" runat="server" TabStrip="false" OnTabStripCommand="MenuGridItem_TabStripCommand">
                </eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" Height="85%" ID="gvFormList" runat="server"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                    GridLines="None" OnItemCommand="gvFormList_ItemCommand" OnItemDataBound="gvFormList_ItemDataBound"
                    OnUpdateCommand="gvFormList_UpdateCommand" ShowFooter="True" EnableViewState="false"
                    OnNeedDataSource="gvFormList_NeedDataSource" OnDeleteCommand="OnDeleteCommand"
                    OnSortCommand="gvFormList_SortCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                        AllowNaturalSort="false" AutoGenerateColumns="false">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                            ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                        <%--    <asp:GridView ID="gvFormList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvFormList_RowCommand" OnRowDataBound="gvFormList_ItemDataBound"
                    OnRowCancelingEdit="gvFormList_RowCancelingEdit" OnRowDeleting="gvFormList_RowDeleting"
                    OnSorting="gvFormList_Sorting" OnRowEditing="gvFormList_RowEditing" AllowSorting="true"
                    ShowFooter="true" ShowHeader="true" EnableViewState="false" OnRowUpdating="gvFormList_RowUpdating"
                    OnRowCreated="gvFormList_RowCreated">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                        <Columns>
                            <%--     <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Name">
                                <ItemTemplate>
                                    <%-- <asp:Label ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID") %>'
                                    Visible="false"></asp:Label>--%>
                                    <asp:Label ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkReportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblheaderFormName" runat="server">Name</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>--%>
                            <%-- <asp:Label ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID") %>'
                                    Visible="false"></asp:Label>--%>
                            <%--  <asp:Label ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                    Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkReportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></asp:LinkButton>
                            </ItemTemplate>--%>
                            <%--<EditItemTemplate>
                                <asp:TextBox ID="txtReportNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>'
                                    CssClass="input"></asp:TextBox>
                            </EditItemTemplate>--%>
                            <%--</asp:TemplateField>--%>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <ItemTemplate>
                                    <%--   <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>--%>
                                    <asp:Label ID="lblStatusname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--     <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblheaderstatus" runat="server" Text="Status"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>--%>
                            <%--   <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>--%>
                            <%--     <asp:Label ID="lblStatusname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                            <%-- <asp:TemplateField ItemStyle-Width="150px">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblhdrCreated" runat="server" CommandName="Sort" CommandArgument="FLDCREATEDDATE">Created On</asp:LinkButton>
                                <img id="FLDCREATEDDATE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                              <asp:Label ID="lblCreatedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                            <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                        ToolTip="Delete">
                                      <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--  <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">Action </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>--%>
                            <%--                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />--%>
                            <%-- <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>--%>
                            <%--                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdUpdate"
                                        ToolTip="Update"></asp:ImageButton>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>--%>
                            <%-- </asp:TemplateField>--%>
                            <%--<table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                                <tr>
                                    <td nowrap align="center">
                                        <asp:Label ID="lblPagenumber" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblPages" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblRecords" runat="server">
                                        </asp:Label>&nbsp;&nbsp;
                                    </td>
                                    <td nowrap align="left" width="50px">
                                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                    </td>
                                    <td width="20px">&nbsp;
                                    </td>
                                    <td nowrap align="right" width="50px">
                                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                    </td>
                                    <td nowrap align="center">
                                        <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                                        </asp:TextBox>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                            Width="40px"></asp:Button>
                                    </td>
                                </tr>
                            </table>--%>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                            AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                        AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
