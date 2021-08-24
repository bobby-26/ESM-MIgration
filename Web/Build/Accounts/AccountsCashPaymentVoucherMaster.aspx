<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCashPaymentVoucherMaster.aspx.cs"
    Inherits="AccountsCashPaymentVoucherMaster" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("gvFormDetails");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 95) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlOrderForm">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="height: 100%; width: 99%"></iframe>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                    <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvFormDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px" EnableHeaderContextMenu="true" GroupingEnabled="false"
                        Height="45%" Width="100%" CellPadding="3" OnItemCommand="gvFormDetails_RowCommand" OnItemCreated="gvFormDetails_ItemDataBound" AllowPaging="true" AllowCustomPaging="true"
                        OnDeleteCommand="gvFormDetails_RowDeleting" OnNeedDataSource="gvFormDetails_NeedDataSource" OnPageIndexChanged="gvFormDetails_PageIndexChanged"
                        AllowSorting="true" EnableViewState="false" OnPreRender="gvFormDetails_PreRender"
                        OnSortCommand="gvFormDetails_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDVOUCHERID,FLDVOUCHERNUMBER">
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
                                <telerik:GridTemplateColumn HeaderText="Voucher Number" AllowSorting="true" SortExpression="FLDVOUCHERNUMBER">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkVoucherId" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER")  %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Voucher Date" AllowSorting="true" SortExpression="FLDVOUCHERDATE">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reference No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sub Voucher Type">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBVOUCHERTYPE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Voucher Year">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblBackToBack" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERYEAR") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Voucher Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPayRollHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERSTATUS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Locked YN">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLocked" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLOCKEDYN").ToString().Equals("1"))?"YES":"NO"  %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItem %>' ID="cmdAttachment"
                                            ToolTip="Attachment"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataItem %>' ID="cmdNoAttachment"
                                            ToolTip="No Attachment"></asp:ImageButton>
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
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
