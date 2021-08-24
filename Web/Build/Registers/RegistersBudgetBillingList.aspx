<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBudgetBillingList.aspx.cs" Inherits="RegistersBudgetBillingList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel List</title>
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
            var grid = $find("gvBudgetBillingList");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 130) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>

</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersBudgetBillingList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlBudgetBillingListEntry" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                    <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 320px; width: 100%"></iframe>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                    <eluc:TabStrip ID="MenuRegistersBudgetBillingList" runat="server" OnTabStripCommand="RegistersBudgetBillingList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetBillingList" runat="server" AutoGenerateColumns="False"
                        AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Font-Size="11px" OnSortCommand="gvBudgetBillingList_SortCommand" Width="100%" CellPadding="3"
                        OnItemCommand="gvBudgetBillingList_ItemCommand" OnItemDataBound="gvBudgetBillingList_ItemDataBound" OnNeedDataSource="gvBudgetBillingList_OnNeedDataSource"
                        OnRowDeleting="gvBudgetBillingList_RowDeleting" ShowFooter="false" OnSelectedIndexChanging="gvBudgetBillingList_SelectedIndexChanged"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDBUDGETBILLINGID">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Billing Item Description" HeaderStyle-Width="165px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkBudgetBillingIDHeader" runat="server" CommandName="Sort" CommandArgument="FLDBILLINGITEMDESCRIPTION">Billing Item Description&nbsp;</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblBudgetBillingId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETBILLINGID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkBillingDescriptionName" runat="server" CommandName="EDIT"
                                            CommandArgument='<%# Container.DataItem %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGITEMDESCRIPTION") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Frequency" HeaderStyle-Width="105px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblFrequencyHeader" runat="server">
                                            Frequency&nbsp;
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Billing Basis" HeaderStyle-Width="145px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblBillingBasisHeader" runat="server">
                                            Billing Basis&nbsp;
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblBillingBasis" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGBASIS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Billing Unit" HeaderStyle-Width="115px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblBillingUnitHeader" runat="server" CommandName="Sort" CommandArgument="FLDBILLINGUNIT">Billing Unit&nbsp;</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblBillingUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGUNIT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel Budget Code" HeaderStyle-Width="140px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblVesselBudgetCodeHeader" runat="server">
                                            Vessel Budget Code&nbsp;
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Credit Account" HeaderStyle-Width="265px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCreditAccountHeader" runat="server">
                                            Credit Account&nbsp;
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCreditAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITACCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>


                    <%--     <Triggers>
            <asp:PostBackTrigger ControlID="gvBudgetBillingList" />
        </Triggers>--%>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

