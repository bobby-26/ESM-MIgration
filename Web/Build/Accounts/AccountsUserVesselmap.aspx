<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsUserVesselmap.aspx.cs"
    Inherits="Accounts_Accountsuseraccountmap" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvUserVesselmap.ClientID %>"));
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlUserAccessEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--            <eluc:Title runat="server" ID="ucTitle" Text="User Vessel Mapping" ShowMenu="false" />--%>
            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuUserAccessList" runat="server" OnTabStripCommand="UserAccessList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <br />
            <tr>
                <td width="15">
                    <telerik:RadLabel ID="lblAccountUserId" runat="server" Text="Account User Name"></telerik:RadLabel>
                </td>
                <td width="15%" colspan="2">
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtDesignation" runat="server" CssClass="input" ReadOnly="false"
                            Width="200px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="lblEmail" runat="server" CssClass="input" ReadOnly="false"
                            Width="1px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtUserCode" runat="server" CssClass="input" ReadOnly="false"
                            Width="1px">
                        </telerik:RadTextBox>
                        <img id="ImgAccountsUserIdPickList" runat="server" style="cursor: pointer; vertical-align: top"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtusercodes" runat="server" CssClass="input" ReadOnly="false"
                            MaxLength="20" Width="10px">
                        </telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <br />
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvUserVesselmap" runat="server" AutoGenerateColumns="FALSE" Font-Size="11px"
                Width="100%" CellPadding="3" Style="margin-bottom: 0px" EnableViewState="false" OnDeleteCommand="gvuservesselmap_deleting" GroupingEnabled="false"
                OnItemDataBound="gvuserVesselmap_RowDataBound" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvUserVesselmap_NeedDataSource" EnableHeaderContextMenu="true"
                OnItemCommand="gvuservesselmap_Rowcommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Status" ItemStyle-HorizontalAlign="Left">
<%--                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountUserId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE")%>'></telerik:RadLabel>
                                <telerik:RadCheckBox ID="chkIncludeyn" runat="server" Enabled="false" AutoPostBack="true"   />
                                <telerik:RadLabel ID="lblUserVesselMapId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERVESSELMAPID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Vessel Name " ItemStyle-HorizontalAlign="Left">
<%--                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblvesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign ="center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
<%--                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>--%>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="add" ImageUrl="<%$ PhoenixTheme:images/select.png %>"
                                    CommandName="Add" CommandArgument='<%# Container.DataItem %>' ID="imgAdd"
                                    ToolTip="Add"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Del" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Delete" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
