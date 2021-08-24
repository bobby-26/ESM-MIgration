<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsErmDebitNoteReference.aspx.cs"
    Inherits="AccountsErmDebitNoteReference" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Statement Reference</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmErmDebitReference" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:TabStrip ID="MenuFormMain" runat="server" OnTabStripCommand="MenuFormMain_TabStripCommand"></eluc:TabStrip>

                <div id="divFind">
                    <table width="100%;">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDebitNoteReference" runat="server" Text="Debit Note Reference"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDebitRefernce" runat="server" CssClass="input"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStatusMain" runat="server" Text="Status"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadRadioButtonList ID="rblStatus" runat="server" AutoPostBack="true" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Value="Pending" Selected="True" Text="Pending" />
                                        <telerik:ButtonListItem Value="1st Level Checked" Text="1st Level Checked" />
                                        <telerik:ButtonListItem Value="2nd Level Checked" Text="2nd Level Checked" />
                                        <telerik:ButtonListItem Value="Published" Text="Published" />
                                        <telerik:ButtonListItem Value="Cancelled" Text="Cancelled" />
                                        <telerik:ButtonListItem Value="1" Text="All" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                    </table>
                </div>

                <eluc:TabStrip ID="MenuDebitReference" runat="server" OnTabStripCommand="MenuDebitReference_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="width: 100%;">
                    <%--<asp:GridView ID="gvDebitReference" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCreated="gvDebitReference_RowCreated" Width="100%" CellPadding="3" OnRowCommand="gvDebitReference_RowCommand"
                        OnRowDataBound="gvDebitReference_ItemDataBound" OnRowCancelingEdit="gvDebitReference_RowCancelingEdit"
                        OnSelectedIndexChanging="gvDebitReference_SelectedIndexChanging" OnRowDeleting="gvDebitReference_RowDeleting"
                        OnRowUpdating="gvDebitReference_RowUpdating" OnRowEditing="gvDebitReference_RowEditing"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnSorting="gvDebitReference_Sorting" DataKeyNames="FLDDEBINOTEREFERENCEID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvDebitReference" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="true" OnNeedDataSource="gvDebitReference_NeedDataSource"
                        OnItemDataBound="gvDebitReference_ItemDataBound1"
                        OnItemCommand="gvDebitReference_ItemCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false" ShowFooter="true">
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

                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblAccountCodeHeader" runat="server" Text="Account"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAccountCode" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex%>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE")  %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblAccountCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDebitReferenceId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblDebitReferenceId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <span id="spnPickListCreditAccount">
                                            <telerik:RadTextBox ID="txtAccountCodeEdit" runat="server" CssClass="input_mandatory" Enabled="false"
                                                MaxLength="20" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE")  %>'>
                                            </telerik:RadTextBox>
                                            <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="AbsMiddle" Text=".." CommandName="picklist" CommandArgument="<%# Container.DataSetIndex %>" />
                                            <telerik:RadTextBox ID="txtCreditAccountDescriptionEdit" runat="server" CssClass="hidden"
                                                Enabled="false" MaxLength="50" Width="0px">
                                            </telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtCreditAccountIdEdit" runat="server" CssClass="hidden" MaxLength="20"
                                                Width="0px">
                                            </telerik:RadTextBox>
                                        </span>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <%--<telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>--%>
                                        <span id="spnPickListCreditAccount">
                                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" Enabled="false"
                                                MaxLength="20" Width="60px">
                                            </telerik:RadTextBox>
                                            <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="AbsMiddle" Text=".." CommandName="picklist" CommandArgument="<%# Container.DataSetIndex %>" />
                                            <telerik:RadTextBox ID="txtCreditAccountDescription" runat="server" CssClass="hidden" Enabled="false"
                                                MaxLength="50" Width="0px">
                                            </telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtCreditAccountId" runat="server" CssClass="hidden" MaxLength="20"
                                                Width="0px">
                                            </telerik:RadTextBox>
                                        </span>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblAccountDescriptionHeader" runat="server" Text="Account Description"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAccountCodeDescriptin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODEDESCRIPTION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTypeHeader" runat="server" Text="Type"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="ddlTypeEdit" runat="server" CssClass="input_mandatory" Filter="Contains" EmptyMessage="Type to select">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                <telerik:RadComboBoxItem Value="Monthly Report" Text="Monthly Report" />
                                                <telerik:RadComboBoxItem Value="Non-Budgeted Report" Text="Non-Budgeted Report" />
                                                <telerik:RadComboBoxItem Value="Predelivery Report" Text="Predelivery Report" />
                                                <telerik:RadComboBoxItem Value="Dry Docking Report" Text="Dry Docking Report" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox ID="ddlType" runat="server" CssClass="input_mandatory" Filter="Contains" EmptyMessage="Type to select">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                <telerik:RadComboBoxItem Value="Monthly Report" Text="Monthly Report" />
                                                <telerik:RadComboBoxItem Value="Non-Budgeted Report" Text="Non-Budgeted Report" />
                                                <telerik:RadComboBoxItem Value="Predelivery Report" Text="Predelivery Report" />
                                                <telerik:RadComboBoxItem Value="Dry Docking Report" Text="Dry Docking Report" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblMonthHeader" runat="server" Text="Month"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <%--                                  <eluc:Country ID="ucCountryEdit" runat="server" CountryList='<%# PhoenixRegistersCountry.ListCountry(null)%>'
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" SelectedCountry='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>' />--%>

                                        <eluc:Month ID="ucMonthEdit" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" HardList='<%# PhoenixRegistersHard.ListHard(1, 55) %>'
                                            HardTypeCode="55" Width="65px" SortByShortName="true" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Month ID="ucMonth" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                            HardTypeCode="55" Width="65px" SortByShortName="true" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblYearHeader" runat="server" Text="Year"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Year ID="ucYearEdit" runat="server" CssClass="input_mandatory" QuickList='<%# PhoenixRegistersQuick.ListQuick(1, 55) %>' AppendDataBoundItems="true"
                                            QuickTypeCode="55" Width="65px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Year ID="ucYear" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            QuickTypeCode="55" Width="65px" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDebitNoteRefHeader" runat="server" Text="Debit Note Reference"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDebitNoteRef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtDebitReferenceEdit" runat="server" CssClass="input_mandatory"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'>
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtDebitNoteRef" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblStatusHeader" runat="server" Text="Status"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox ID="ddlStatusAdd" runat="server" CssClass="dropdown_mandatory" Filter="Contains" EmptyMessage="Type to select"
                                            Enabled="false">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="Pending" Selected="True" Text="Pending" />
                                                <telerik:RadComboBoxItem Value="1st Level Checked" Text="1st Level Checked" />
                                                <telerik:RadComboBoxItem Value="2nd Level Checked" Text="2nd Level Checked" />
                                                <telerik:RadComboBoxItem Value="Published" Text="Published" />
                                                <telerik:RadComboBoxItem Value="Cancelled" Text="Cancelled" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblURLHeader" runat="server" Text="URL"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblURL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDURL").ToString().Substring(0, 30)+ "..." : DataBinder.Eval(Container, "DataItem.FLDURL").ToString() %>'></telerik:RadLabel>
                                        <eluc:ToolTip ID="ucToolTiplblURL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtURLEdit" runat="server" TextMode="MultiLine" Rows="1"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'>
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtURL" runat="server" TextMode="MultiLine" Rows="1"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>


                                <telerik:GridTemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            Action
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="1st Check" ImageUrl="<%$ PhoenixTheme:images/select.png %>"
                                            CommandName="1STCHECK" CommandArgument='<%# Container.DataSetIndex %>' ID="img1stCheck"
                                            ToolTip="1st Level Checked"></asp:ImageButton>
                                        <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="2nd Check" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                            CommandName="2NDCHECK" CommandArgument='<%# Container.DataSetIndex %>' ID="img2ndCheck"
                                            ToolTip="2nd Level Checked"></asp:ImageButton>
                                        <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Published" ImageUrl="<%$ PhoenixTheme:images/visa-issued.png %>"
                                            CommandName="PUBLISHED" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPublished"
                                            ToolTip="Publish" Visible="false"></asp:ImageButton>

                                        <asp:ImageButton runat="server" AlternateText="UnPublished" ImageUrl="<%$ PhoenixTheme:images/ticket_cancel.png %>"
                                            CommandName="UNPUBLISHED" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdUnPublished"
                                            ToolTip="UnPublish" Visible="false"></asp:ImageButton>

                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Save" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
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
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
