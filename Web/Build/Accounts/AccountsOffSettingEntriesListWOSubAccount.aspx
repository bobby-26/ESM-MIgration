<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOffSettingEntriesListWOSubAccount.aspx.cs"
    Inherits="AccountsOffSettingEntriesListWOSubAccount" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Off Setting List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">

        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 30);
            splitter.set_width("100%");
            var grid = $find("gvOffSettingList");
            var contentPane = splitter.getPaneById("listPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 120) + "px";
            //console.log(grid._gridDataDiv.style.height, contentPane._contentElement.offsetHeight);
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body>

    <form id="frmOffSettingList" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager2" Localization-OK="Yes" Localization-Cancel="No"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ifMoreInfo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvOffSettingList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvOffSettingList" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucConfirm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvOffSettingList" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuOffSettingList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuOffSettingList" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />

        <%--    <eluc:Title runat="server" ID="ttlOffSetting" Text="Off-SettingList" ShowMenu="false"></eluc:Title>--%>

        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="generalPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" style="height: 100%; width: 100%"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
            <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                <eluc:TabStrip ID="MenuOffSettingList" runat="server" OnTabStripCommand="MenuOffSettingList_TabStripCommand"></eluc:TabStrip>
                <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvCashOut" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvOffSettingList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvOffSettingList_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" Height="100%" OnSelectedIndexChanging="gvOffSettingList_SelectedIndexChanging"
                    OnItemCommand="gvOffSettingList_ItemCommand"
                    ShowFooter="false" ShowHeader="true" OnSortCommand="gvOffSettingList_SortCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDVOUCHERLINEITEMID">

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Voucher Number">
                                <HeaderStyle Width="108px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkVoucherNo" runat="server" CommandName="Edit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblVoucherIdNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVoucherLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Row Number">
                                <HeaderStyle Width="79px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRowNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMNO") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVoucherID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Account" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                                <HeaderStyle Width="140px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAccountId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reference Number">
                                <HeaderStyle Width="140px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency">
                                <HeaderStyle Width="85px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Prime Amount" AllowSorting="true" SortExpression="FLDAMOUNT">
                                <HeaderStyle Width="105px" HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSACTIONAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Allocated Voucher Amount">
                                <HeaderStyle Width="105px" HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPreviousAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSVOUCHERAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Balance Voucher Amount">
                                <HeaderStyle Width="105px" HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBalnceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Voucher Status">
                                <HeaderStyle Width="105px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSETVOUCHERSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency" Visible="false">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBaseCurrencyRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEEXCHANGERATE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblReportRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTEXCHANGERATE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSubAccountMapId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTMAPID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="User Name">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBaseRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBYUSERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Long Description">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLongDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION").ToString().Length>15 ? (DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") as string).Substring(0,15):DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION")%> '></telerik:RadLabel>
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
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </telerik:RadPane>
        </telerik:RadSplitter>

    </form>
</body>
</html>




