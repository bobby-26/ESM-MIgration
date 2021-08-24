<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListVesselAccount.aspx.cs"
    Inherits="CommonPickListVesselAccount" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Account List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuAccount" runat="server" OnTabStripCommand="MenuAccount_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table>
                <tr>
                    <td width="5%">
                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                    </td>
                    <td width="5%">
                        <telerik:RadTextBox ID="txtAccountCodeSearch" CssClass="input" runat="server" Text="" MaxLength="100"
                            Width="150px"></telerik:RadTextBox>
                    </td>
                    <td width="5%">
                        <telerik:RadLabel ID="lblAccountDescription" runat="server" Text="Account Description"></telerik:RadLabel>
                    </td>
                    <td width="5%">
                        <telerik:RadTextBox ID="txtAccountDescSearch" CssClass="input" runat="server" Text="" MaxLength="100"
                            Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvAccount" runat="server" Height="85%" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvAccount_NeedDataSource" OnItemCommand="gvAccount_ItemCommand" OnRowDataBound="gvAccount_ItemDataBound"
                ShowFooter="false" ShowHeader="true" Width="100%" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDACCOUNTID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Account Code" AllowSorting="true" SortExpression="FLDACCOUNTCODE">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAccountId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'
                                    CommandName="PICKADDRESS"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Description" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAccountDescription" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Type" AllowSorting="true" SortExpression="FLDACCOUNTTYPE">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" />
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
