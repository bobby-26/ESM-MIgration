<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListUndesirableWorstCase.aspx.cs" Inherits="CommonPickListUndesirableWorstCase" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Undesirable Event - Worst Case</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand"></eluc:TabStrip>
            <br clear="all" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="search">
                <table width ="100%">
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblUndesiarableEvent" runat="server" Text="Undesirable Event"></telerik:RadLabel>
                        </td>
                        <td width="85%">
                            <eluc:Quick ID="ucevent" runat="server" AppendDataBoundItems="true" CssClass="input" QuickTypeCode="169" Width="180px"/>
                        </td>
                    </tr>
                </table>
            </div>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvUndesiarableEvent" runat="server" Height="80%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" EnableViewState="false" GridLines="None"  Font-Size="11px" OnItemCommand="gvUndesiarableEvent_ItemCommand" OnSortCommand="gvUndesiarableEvent_SortCommand"
                OnNeedDataSource="gvUndesiarableEvent_NeedDataSource" ShowFooter="False" ShowHeader="true" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORSTCASEUNDESIRABLEEVENTID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Undesirable Event">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblUndisirableId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDWORSTCASEUNDESIRABLEEVENTID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblUndesirable" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDEVENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Worst Case">
                            <HeaderStyle Width="22%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkWorstCase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASENAME") %>' CommandName="PICKLIST"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Severity">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblSeverity" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSEVERITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Score">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblScore" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
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
