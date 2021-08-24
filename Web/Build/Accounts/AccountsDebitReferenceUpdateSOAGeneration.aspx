<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDebitReferenceUpdateSOAGeneration.aspx.cs" Inherits="AccountsDebitReferenceUpdateSOAGeneration" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PDFError" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Debit Note Reference</title>
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
            <eluc:PDFError ID="ucPDFError" runat="server" Text="" Visible="false"></eluc:PDFError>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


                <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />



                <eluc:TabStrip ID="MenuGenralSub" runat="server" OnTabStripCommand="MenuGenralSub_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>


                <%--                 <div class="subHeader">
                        <div class="divFloat" style="clear: right">
                            <eluc:TabStrip ID="MenuGenralMain" runat="server" OnTabStripCommand="MenuGenralMain_TabStripCommand"
                                TabStrip="true"></eluc:TabStrip>
                        </div>
                    </div>--%>

                <eluc:TabStrip ID="MenuDebitReference" runat="server" OnTabStripCommand="MenuDebitReference_TabStripCommand"></eluc:TabStrip>

                <br />
                <div id="divFind" style="position: relative;">
                    <table>
                        <tr>
                            <td>Account Code
                            </td>
                            <td>
                                <%--<telerik:RadTextBox ID="txtAccountCOde" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>--%>
                                <span id="spnPickListCreditAccount">
                                    <telerik:RadTextBox ID="txtAccountCOde" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="20" Width="160px">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="txtCreditAccountDescription" runat="server" CssClass="hidden" Enabled="false"
                                        MaxLength="50" Width="0px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCreditAccountId" runat="server" CssClass="hidden" MaxLength="20"
                                        Width="0px">
                                    </telerik:RadTextBox>
                                </span>
                            </td>
                            <td>SOA Reference
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlDebitReference" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                                    Enabled="false">
                                </telerik:RadComboBox>
                                <telerik:RadLabel ID="lblOwner" runat="server" Visible="false"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>From Date 
                            </td>
                            <td>
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>To Date 
                            </td>
                            <td>
                                <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 30px;">
                </div>
                <telerik:RadLabel ID="lblcaption" runat="server" Visible="false" Text="Voucher List" Font-Bold="true" Font-Size="Medium"></telerik:RadLabel>
                <div style="height: 10px;">
                </div>
                <div style="height: 20px;">
                    <telerik:RadLabel ID="lblAttcahMsg" runat="server" ForeColor="Red" Font-Bold="true"></telerik:RadLabel>
                </div>
                <div id="divCurrGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%-- <asp:GridView ID="gvCurrencyGrid" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" AllowSorting="true" DataKeyNames="FLDVOUCHERID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCurrencyGrid" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCurrencyGrid_NeedDataSource"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDVOUCHERID">
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
                                <telerik:GridTemplateColumn HeaderText="Voucher Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVoucherID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVoucherLineItemID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Voucher Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <asp:Literal ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE")) %>'></asp:Literal>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Row No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRowNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Currency">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCurrencycode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <asp:Literal ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:###,###,###,##0.00}") %>'></asp:Literal>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%--<asp:GridView ID="gvFormDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvFormDetails_ItemDataBound"
                        AllowSorting="true"
                        DataKeyNames="FLDVOUCHERID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvFormDetails" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvFormDetails_NeedDataSource"
                        OnItemDataBound="gvFormDetails_ItemDataBound1"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDVOUCHERID">
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
                                <telerik:GridTemplateColumn HeaderText="Voucher Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVoucherID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblBoucherLineItemID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Row No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRowNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="100px">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="VoucherAttachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" ID="cmdAttachment"
                                            ToolTip="VoucherAttachment"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="VoucherLineAttachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" ID="cmdNoAttachment"
                                            ToolTip="VoucherLineAttachment"></asp:ImageButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

                <div style="height: 20px;">
                    <telerik:RadLabel ID="lblPDFissue" Font-Size="Large" runat="server" ForeColor="Red" Visible="false"></telerik:RadLabel>
                </div>
                <div id="dvPDFissues" style="position: relative; z-index: 0; width: 100%;">
                    <%-- <asp:GridView ID="gvPDFissues" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"
                        AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPDFissues" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvFormDetails_NeedDataSource"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
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
                                <telerik:GridTemplateColumn HeaderText="Voucher Number-Row No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="File Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRowNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Error Message">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblmsg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMSG") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            Action
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="VoucherAttachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" ID="cmdAttachment"
                                            ToolTip="VoucherAttachment"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="VoucherLineAttachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" ID="cmdNoAttachment"
                                            ToolTip="VoucherLineAttachment"></asp:ImageButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

                <div style="height: 20px;">
                    <telerik:RadLabel ID="lblobcissue" Font-Size="Large" runat="server" ForeColor="Red" Visible="false"></telerik:RadLabel>
                </div>
                <div id="dvOBCissues" style="position: relative; z-index: 0; width: 100%;">

                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOBCissues" runat="server" Height="600px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOBCissues_NeedDataSource" OnItemDataBound="gvOBCissues_ItemDataBound"
                        OnItemCommand="gvOBCissues_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
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
                                <telerik:GridTemplateColumn HeaderText="Voucher Date" HeaderStyle-Width="6%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVoucherdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Voucher Number" HeaderStyle-Width="16%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLineItemByVoucherId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'></telerik:RadLabel>

                                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="ESM Budget Code" HeaderStyle-Width="7%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblesmowner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblesmownereditcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadLabel>
                                        <asp:TextBox ID="txtBudgetIdEdit" runat="server" Width="0px" AutoPostBack="true" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtaccountid" runat="server" Width="0px" CssClass="hidden" AutoPostBack="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtprincipal" runat="server" Width="0px" CssClass="hidden" AutoPostBack="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Owner Budget Code" HeaderStyle-Width="12%" Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblownerbudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Owner Budget Code" AllowSorting="true" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblownerbudgetid" runat="server" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblownerbudgetcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="spnPickListOwnerBudgetEdit">
                                            <asp:TextBox ID="txtOwnerBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>' Width="60px"
                                                MaxLength="20" CssClass="input"></asp:TextBox>
                                            <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="AbsMiddle" Text=".." />
                                            <asp:TextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"
                                                Enabled="False"></asp:TextBox>
                                            <asp:TextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="hidden"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEMAPID") %>'></asp:TextBox>
                                            <asp:TextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                        </span>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Voucher Reference" HeaderStyle-Width="15%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblrefNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="5%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblcurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="8%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Voucher Description" HeaderStyle-Width="15%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVoucherDesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVDESCRIPTION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Line Item Description" HeaderStyle-Width="10%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVoucherLIDesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVLIDESCRIPTION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Included in Owner SOA" HeaderStyle-Width="5%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblownersoa" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOTINCULDEINOWNERREPORT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Show Seperately in the Vessel Summery Balance" HeaderStyle-Width="8%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblsummbalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHOWINSUMMARYBALANCE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="UPDATEOBC" ID="cmdUpdate" ToolTip="Update">
                                <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <telerik:RadLabel ID="lblcount" runat="server"></telerik:RadLabel>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
