<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSupplierAccountMapping.aspx.cs"
    Inherits="Accounts_AccountsSupplierAccountMapping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplier A/c Mapping Account</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSupplierAccount.ClientID %>"));
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
    <form id="frmVesselAccounts" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVesselAccount">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--            <telerik:RadLabel ID="lblMenuVesselAccount" runat="server" Text="Supplier A/C Mapping"></telerik:RadLabel>--%>
            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
            <tr>
                <td width="15">
                    <telerik:RadLabel ID="lblAccountUserId" runat="server" Text="Account User Name"></telerik:RadLabel>
                </td>
                <td width="15%" colspan="2">
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtDesignation" runat="server" CssClass="input" ReadOnly="false"
                            Width="180px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="lblEmail" runat="server" CssClass="input" ReadOnly="false"
                            Width="1px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtUserCode" runat="server" CssClass="input" ReadOnly="false"
                            Width="1px"></telerik:RadTextBox>
                        <img id="ImgAccountsUserIdPickList" runat="server"
                            src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                        <telerik:RadTextBox ID="txtusercodes" runat="server" CssClass="input" ReadOnly="false"
                            MaxLength="20" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>
                <td width="15%">
                    <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                </td>
                <span id="spnPickListSupMaker">
                    <td width="15%" colspan="2">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                            Width="60px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                            Width="180px"></telerik:RadTextBox>
                        <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                            style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                </span>
                </td>                           
            </tr>
            <br />
            <br />
            <eluc:TabStrip ID="MenuVesselAccountlist" runat="server" OnTabStripCommand="MenuVesselAccountlist_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSupplierAccount" runat="server" AutoGenerateColumns="false" Font-Size="11px" OnItemDataBound="gvSupplierAccount_RowDataBound"
                Width="100%" CellPadding="3" OnItemCommand="gvSupplierAccount_RowCommand" OnDeleteCommand="gvSupplierAccount_delete" AllowPaging="true" AllowCustomPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvSupplierAccount_NeedDataSource">
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
                        <%--                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="280px">
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkIncludeyn" runat="server" AutoPostBack="true" Enabled="false"  />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supplier Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="600px">
<%--                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>--%>
                            <ItemTemplate >
                                <telerik:RadLabel ID="lblSupplierCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
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
