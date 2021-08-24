<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsBankStatementExcelUpload.aspx.cs"
    Inherits="AccountsBankStatementExcelUpload" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bank Statement Upload</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmForwarderExcelUpload" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenutravelInvoice" runat="server" OnTabStripCommand="MenutravelInvoice_OnTabStripCommand"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td style="width: 20%">
                    <telerik:RadLabel ID="lblBankAccount" runat="server" Text="Bank Account"></telerik:RadLabel>
                </td>
                <td style="width: 80%">
                    <eluc:UserControlBankAccount ID="ddlBankAccount"
                        AppendDataBoundItems="true" OnTextChangedEvent="ddlBankAccount_SelectedIndexChanged"
                        AutoPostBack="true" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblBankAccountDesc" runat="server" Text="Bank Account Desc"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtDesc" Width="240px" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtCurrency" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="ddlType" Width="240px" Filter="Contains" EmptyMessage="type to select">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Transactions" Value="TRANSACTIONS"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Bank Charges" Value="BANK CHARGES"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload" runat="server" Width="240px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" Width="240px" />
                </td>
            </tr>
        </table>
        <%--        <hr />--%>
        <%--        <br />--%>
        <eluc:TabStrip ID="MenuAttachment" runat="server" OnTabStripCommand="MenuAttachment_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnItemDataBound="gvAttachment_RowDataBound" OnItemCommand="gvAttachment_RowCommand" OnNeedDataSource="gvAttachment_NeedDataSource"
            AllowSorting="true">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
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
                    <telerik:GridTemplateColumn HeaderText="Bank Account">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT").ToString() %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblexceluploadId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDUPLOADID").ToString() %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVoucherRefUpdatedYN" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDISVOUCHERREFUPDATEDYN").ToString() %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Bank Account Desc">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAccountDesc" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Currency">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Month">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMonthName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDMONTHNAME")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="From Date">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%#Bind("FLDFROMDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="To date">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblToDate" runat="server" Text='<%# Bind("FLDTODATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Post" ImageUrl="<%$ PhoenixTheme:images/completed.png %>"
                                CommandName="APPROVE" CommandArgument='<%# Container.DataItem %>' ID="cmdPost"
                                ToolTip="Post"></asp:ImageButton>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                CommandName="VIEW" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                ToolTip="View Excel"></asp:ImageButton>
                            <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            <asp:ImageButton ID="cmdArchive" runat="server" AlternateText="Archive" CommandArgument="<%# Container.DataItem %>"
                                CommandName="ARCHIVE" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                ToolTip="Archive" />
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

    </form>
</body>
</html>
