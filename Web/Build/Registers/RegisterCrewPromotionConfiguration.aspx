<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewPromotionConfiguration.aspx.cs" Inherits="RegisterCrewPromotionConfiguration" %>

<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Promotion</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPromotion.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }

            </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%" />
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table id="tabel1" rules="server" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRankTitle" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" Width="200px" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="CrewPromotionMenu" runat="server" OnTabStripCommand="CrewPromotionMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPromotion" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvPromotion_NeedDataSource"
                OnItemCommand="gvPromotion_ItemCommand" OnItemDataBound="gvPromotion_ItemDataBound" OnSortCommand="gvPromotion_SortCommand" OnDeleteCommand="gvPromotion_DeleteCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="30px"></ItemStyle>
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNo" runat="server" Width="30px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank From">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="400px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankFrom" Width="400px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKFROMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank To">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="400px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankTo" Width="400px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKTONAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="NAVIGATEEDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy"
                                    CommandName="COPY" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCopy"
                                    ToolTip="Copy">
                                        <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                    </Columns>
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
