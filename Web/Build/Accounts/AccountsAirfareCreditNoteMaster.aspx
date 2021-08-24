<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfareCreditNoteMaster.aspx.cs" Inherits="AccountsAirfareCreditNoteMaster" %>

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
    <title>Credit Debit Note Master</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function PaneResized(sender, args) {
                var splitter = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                splitter.set_height(browserHeight - 40);
                splitter.set_width("100%");
                var grid = $find("gvCreditDebitNote");
                var contentPane = splitter.getPaneById("listPane");
                grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 120) + "px";
                //console.log(grid._gridDataDiv.style.height, contentPane._contentElement.offsetHeight);
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCreditDebitNoteMaster" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="frmTitle" Text="Airfare Credit Notes" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:TabStrip ID="MenuCreditDebitNoteTab" runat="server" OnTabStripCommand="MenuCreditDebitNoteTab_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                <telerik:RadPane ID="generalPane" runat="server" Scrolling="Y">
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 345px; width: 99.9%"></iframe>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
                <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized">

                    <eluc:TabStrip ID="MenuCreditDebitNote" runat="server" OnTabStripCommand="MenuCreditDebitNote_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCreditDebitNote" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                        Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" AllowSorting="true"
                        OnNeedDataSource="gvCreditDebitNote_NeedDataSource" OnItemCommand="gvCreditDebitNote_ItemCommand">
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" DataKeyNames="FLDAIRFARECREDITNOTEID"
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
                                <telerik:GridTemplateColumn HeaderText="Supplier Code" HeaderStyle-Width="15%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCreditDebitNoteId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARECREDITNOTEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSupplierCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lbltabflag" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTABFLAG") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVoucherId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Supplier Name" HeaderStyle-Width="25%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblSupplierName" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItem %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vendor Credit Note No" HeaderStyle-Width="15%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRefNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="7%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="20%">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Credit Note Register No" HeaderStyle-Width="20%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCNRegisterNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCNREGISTERNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="15%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
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
