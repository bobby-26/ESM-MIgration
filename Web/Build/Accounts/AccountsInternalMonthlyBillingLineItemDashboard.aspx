<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInternalMonthlyBillingLineItemDashboard.aspx.cs" Inherits="Accounts_AccountsInternalMonthlyBillingLineItemDashboard" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Standard Bill</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <%--   <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>--%>
        <script type="text/javascript">
            function PostRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmPost.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCommonBudgetGroupAllocation" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" CssClass="hidden" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuBudgetTab" runat="server" TabStrip="true" OnTabStripCommand="BudgetTab_TabStripCommand" Visible="false"></eluc:TabStrip>
            <eluc:Status runat="server" ID="ucStatus" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPortageBillstatus" runat="server" Text="Portage Bill status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPortagebillStatus" runat="server" CssClass="input" ReadOnly="true"
                            Width="90px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStartDate" runat="server" Text="Start Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPortagebillStartDate" runat="server" CssClass="input" ReadOnly="true"
                            Width="90px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEndDate" runat="server" Text="End Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPortagebillEndDate" runat="server" CssClass="input" ReadOnly="true"
                            Width="90px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCrew" runat="server" OnTabStripCommand="MenuCrew_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMonthlyBillingCrew" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvMonthlyBillingCrew_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="true" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvMonthlyBillingCrew_SelectedIndexChanging"
                OnItemDataBound="gvMonthlyBillingCrew_ItemDataBound" OnItemCommand="gvMonthlyBillingCrew_ItemCommand"
                ShowFooter="false" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPORTAGEBILLID">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No.">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSerialNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMonthlyBillingEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHLYBILLINGEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPostedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTEDYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign On Date">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>


                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off Date">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
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
            <eluc:TabStrip ID="MenuBillingItem" runat="server" OnTabStripCommand="MenuBillingItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMonthlyBillingItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvMonthlyBillingItem_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvMonthlyBillingItem_SelectedIndexChanging"
                OnItemDataBound="gvMonthlyBillingItem_ItemDataBound" OnItemCommand="gvMonthlyBillingItem_ItemCommand"
                ShowFooter="false" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPORTAGEBILLID">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Billing Item" AllowSorting="true" SortExpression="FLDBILLINGITEMDESCRIPTION">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                            <%-- <HeaderTemplate>
                                    <asp:LinkButton ID="lnkBillingItemHeader" runat="server" CommandName="Sort" CommandArgument="FLDBILLINGITEMDESCRIPTION"
                                        ForeColor="White">Billing Item &nbsp;</asp:LinkButton>
                                    <img id="FLDBILLINGITEMDESCRIPTION" runat="server" visible="false" />
                                </HeaderTemplate>--%>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBillingItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGITEMDESCRIPTION") %>'
                                    CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex%>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblMonthlyBillingItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHLYBILLINGITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetBillingId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETBILLINGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPortageBillId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGEBILLID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsPosted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPOSTED") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPostAllVisibleYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTALLVISIBLEYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselBudgetAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETALLOCATIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Billing Unit">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBillingUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGUNITNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBillingUnit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGUNIT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rate">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBillingItemRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGITEMRATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Apportionment(%)">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBillingItemApportionment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPORTIONMENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtBillingItemApportionment" runat="server" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPORTIONMENT") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:ImageButton runat="server" AlternateText="Post All" ImageUrl="<%$ PhoenixTheme:images/pr.png %>"
                                    CommandName="POSTALL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPostAll"
                                    ToolTip="Post All" Visible="true"></asp:ImageButton>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Post" ImageUrl="<%$ PhoenixTheme:images/pr.png %>"
                                    CommandName="POST" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPost"
                                    ToolTip="Post"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Save" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
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
            <asp:Button ID="ucConfirmPost" runat="server" OnClick="btnConfirmPost_Click" CssClass="hidden" />
            <%-- <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
            <eluc:Confirm ID="ucConfirmPost" runat="server" OnConfirmMesage="btnConfirmPost_Click"
                OKText="Yes" CancelText="No" />
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
                CancelText="No" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
