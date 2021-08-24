<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListDocumentCategory.aspx.cs" Inherits="CommonPickListDocumentCategory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Document Category</title>
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
        <eluc:TabStrip ID="MenuCategory" Visible="false" runat="server" OnTabStripCommand="MenuCategory_TabStripCommand"></eluc:TabStrip>
        <br clear="all" />
        <telerik:RadAjaxPanel ID="pnlCategory" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtNameSearch" CssClass="input" runat="server" Text="" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlParentCategory" runat="server" CssClass="input" Width="300px" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlParentCategory_Changed"
                            DataTextField="FLDCATEGORYNAME" DataValueField="FLDCATEGORYID" AutoPostBack="true">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCategory" runat="server" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true"
                AutoGenerateColumns="False" CellPadding="3" Font-Size="11px" OnNeedDataSource="gvCategory_NeedDataSource" OnItemCommand="gvCategory_OnItemCommand"
                OnRowEditing="gvCategory_RowEditing" OnItemDataBound="gvCategory_ItemDataBound"  Height="100%" ShowFooter="true" ShowHeader="true" Width="100%"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDCATEGORYID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <itemstyle horizontalalign="Left" wrap="False" />
                            <headertemplate>
                                    <asp:LinkButton ID="lblCategoryNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDCATEGORYNUMBER"
                                        >Number&nbsp;</asp:LinkButton>                                    
                                </headertemplate>
                            <itemtemplate>
                                    <telerik:RadLabel runat="server" ID="lblCategoryId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel runat="server" ID="lblCategoryNumber" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDCATEGORYNUMBER") %>'></telerik:RadLabel>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <itemstyle horizontalalign="Left" wrap="False" />
                            <headertemplate>
                                    <telerik:RadLabel ID="lblCategoryNameHeader" runat="server">Category Name</telerik:RadLabel>
                                </headertemplate>
                            <itemtemplate>
                                    <asp:LinkButton ID="lnkCategoryName" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:LinkButton>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
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
