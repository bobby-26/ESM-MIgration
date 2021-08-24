<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOpeningSummaryBalance.aspx.cs"
    Inherits="AccountsOpeningSummaryBalance" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Opening Summary</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvOpeningSummary.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOpeningsummary" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />



            <%--  <eluc:TabStrip ID="MenuOpeningSummary" runat="server" OnTabStripCommand="MenuOpeningSummary_TabStripCommand"></eluc:TabStrip>--%>

            <eluc:TabStrip ID="MenuOpeningSummaryGrid" runat="server" OnTabStripCommand="MenuOpeningSummaryGrid_TabStripCommand"></eluc:TabStrip>


            <%--  <asp:GridView ID="gvOpeningSummary" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowCreated="gvOpeningSummary_RowCreated" Width="100%" CellPadding="3" OnRowCommand="gvOpeningSummary_RowCommand"
                    OnRowDataBound="gvOpeningSummary_ItemDataBound" OnRowCancelingEdit="gvOpeningSummary_RowCancelingEdit"
                    OnSelectedIndexChanging="gvOpeningSummary_SelectedIndexChanging" OnRowDeleting="gvOpeningSummary_RowDeleting"
                    OnRowUpdating="gvOpeningSummary_RowUpdating" OnRowEditing="gvOpeningSummary_RowEditing"
                    ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true" DataKeyNames="FLDOPENINGSUMMARYID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOpeningSummary" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOpeningSummary_NeedDataSource"
                OnItemCommand="gvOpeningSummary_ItemCommand"
                OnItemDataBound="gvOpeningSummary_ItemDataBound1"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDOPENINGSUMMARYID" ShowFooter="true">
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
                        <telerik:GridTemplateColumn HeaderText="Account" HeaderStyle-Width="100px" AllowSorting="true" SortExpression="FLDACCOUNTCODE">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOpeningSummaryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGSUMMARYID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkAccountCode" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex%>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE")  %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOpeningSummaryIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGSUMMARYID") %>'></telerik:RadLabel>
                                <span id="spnPickListAccountEdit">
                                    <telerik:RadTextBox ID="txtAccountCodeEdit" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="20" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE")  %>' Width="100%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccountEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="picklist" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtAccountDescriptionEdit" runat="server" CssClass="hidden" Enabled="false"
                                        MaxLength="50" Width="0px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountIdEdit" runat="server" CssClass="hidden" MaxLength="20" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID")  %>'
                                        Width="0px">
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListAccount">
                                    <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="20" Width="60px">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="picklist" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="hidden" Enabled="false"
                                        MaxLength="50" Width="0px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="hidden" MaxLength="20" Width="0px"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Description" HeaderStyle-Width="200px" AllowSorting="true" SortExpression="FLDDESCRIPTION">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCodeDescriptin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlTypeEdit" runat="server" CssClass="dropdown_mandatory" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="--Select--" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="Monthly Report" Text="Monthly Report" />
                                        <telerik:RadComboBoxItem Value="Non-Budgeted Report" Text="Non-Budgeted Report" />
                                        <telerik:RadComboBoxItem Value="Predelivery Report" Text="Predelivery Report" />
                                        <telerik:RadComboBoxItem Value="Dry Docking Report" Text="Dry Docking Report" />
                                    </Items>

                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlTypeAdd" runat="server" CssClass="dropdown_mandatory" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="--Select--" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="Monthly Report" Text="Monthly Report" />
                                        <telerik:RadComboBoxItem Value="Non-Budgeted Report" Text="Non-Budgeted Report" />
                                        <telerik:RadComboBoxItem Value="Predelivery Report" Text="Predelivery Report" />
                                        <telerik:RadComboBoxItem Value="Dry Docking Report" Text="Dry Docking Report" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Month" HeaderStyle-Width="80px">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Month ID="ucMonthEdit" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="55" SortByShortName="true" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Month ID="ucMonth" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="55" SortByShortName="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Year" HeaderStyle-Width="80px" AllowSorting="true" SortExpression="FLDQUICKNAME">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Year ID="ucYearEdit" runat="server" CssClass="input_mandatory" QuickList='<%# PhoenixRegistersQuick.ListQuick(1, 55) %>'
                                    AppendDataBoundItems="true" QuickTypeCode="55" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Year ID="ucYear" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="55" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Opening Amount" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOpeningAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtOpeningAmountEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGAMOUNT") %>' Width="100%"></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtOpeningAmountAdd" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="120px"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Delete" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>



        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
