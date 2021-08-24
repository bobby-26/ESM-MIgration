<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListFMS.aspx.cs" Inherits="Common_CommonPickListFMS" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Management System</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:TabStrip ID="MenuCategory" runat="server" OnTabStripCommand="MenuCategory_TabStripCommand"></eluc:TabStrip>

        <br clear="all" />
        <telerik:RadAjaxPanel ID="pnlCategory" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table cellpadding="1" cellspacing="1" style="width: 100%;">
                    <tr>
                        <td colspan="6">
                        </td>
                    </tr>                 
                    <tr>
                        <td>
                            <asp:Literal ID="lblFormName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadTextBox ID="txtFormName" runat="server" CssClass="input">
                            </telerik:RadTextBox>
                        </td>
                         <td>
                            <asp:Literal ID="lblcontent" runat="server" Text="Content"></asp:Literal>
                        </td>                        
                        <td>
                            <telerik:RadTextBox ID="txtcontent" runat="server" CssClass="input">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCategory" runat="server" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true"
                AutoGenerateColumns="False" CellPadding="3"
                Font-Size="11px" OnNeedDataSource="gvCategory_NeedDataSource" OnItemCommand="gvCategory_OnItemCommand"
                OnRowEditing="gvCategory_RowEditing" OnItemDataBound="gvCategory_ItemDataBound" Height="100%" ShowFooter="true" ShowHeader="true" Width="100%"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDREPORTID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Name">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkReportName" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtReportNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>'
                                        CssClass="input"></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%-- <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>--%>
                            <%--  <HeaderTemplate>
                        <telerik:RadLabel ID="lblheaderFormName" runat="server">Name</telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID") %>'
                            Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                            Visible="false"></telerik:RadLabel>
                        <asp:LinkButton ID="lnkReportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadTextBox ID="txtReportNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>'
                            CssClass="input"></telerik:RadTextBox>
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblStatusname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--    <asp:TemplateField>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblheaderstatus" runat="server" Text="Status"></telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblStatusname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                            <telerik:GridTemplateColumn HeaderText="Created On">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--   <asp:TemplateField ItemStyle-Width="150px">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lblhdrCreated" runat="server" CommandName="Sort" CommandArgument="FLDCREATEDDATE">Created On</asp:LinkButton>
                        <img id="FLDCREATEDDATE" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                            <telerik:GridTemplateColumn Visible="false" HeaderText="Action" AllowSorting="true" SortExpression="">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                        ToolTip="Delete">
                                      <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--   <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action </telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                    <ItemTemplate>--%>
                            <%--                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />--%>
                            <%--  <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" 
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder" >
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
<%--            <triggers>
                <asp:PostBackTrigger ControlID="gvCategory" />
            </triggers>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>