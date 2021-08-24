<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListMachineryRA.aspx.cs" Inherits="CommonPickListMachineryRA" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Hazards</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersPortAgent" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuPortAgent" runat="server" OnTabStripCommand="MenuPortAgent_TabStripCommand"></eluc:TabStrip>
            <table id="tblConfigurePortAgent" width="100%">
                <tr>
                    <td runat="server" visible="false">
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td runat="server" visible="false">
                        <telerik:RadRadioButtonList ID="rblType" runat="server" CssClass="input" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Text="Process" Value="1"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Generic" Value="2"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Machinery" Value="3"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Navigation" Value="4"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblKeyword" runat="server" Text="Keyword"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtActivity" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td runat="server">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlCategory" runat="server" CssClass="input" AutoPostBack="true" DefaultMessage="--Select--">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvRA" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRA" runat="server" Height="80%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" Font-Size="11px" OnItemCommand="gvRA_ItemCommand" OnSortCommand="gvRA_SortCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvRA_NeedDataSource" OnItemDataBound="gvRA_ItemDataBound" ShowFooter="False" ShowHeader="true" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKASSESSMENTID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Category" AllowSorting="true" SortExpression="FLDCATEGORY">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCategory" runat="server" CommandName="PICKLIST" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity" AllowSorting="true" SortExpression="FLDACTIVITY">
                            <ItemTemplate>
                                <asp:Label ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDACTIVITY").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDACTIVITY").ToString() %> '></asp:Label>
                                <eluc:ToolTip ID="ucToolTipActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process Name" UniqueName="Process">
                            <ItemTemplate>
                                <asp:Label ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME")  %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reference No" AllowSorting="true" SortExpression="FLDREFNO">
                            <ItemTemplate>
                                <asp:Label ID="lblRAId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID")  %>'></asp:Label>
                                <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></asp:Label>
                                <asp:Label ID="lblRefNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO")  %>'></asp:Label>
                                <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="PICKLIST" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Risks/Aspects" AllowSorting="true" SortExpression="FLDRISKASSESSMENT" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENT")  %>'></asp:Label>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
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
