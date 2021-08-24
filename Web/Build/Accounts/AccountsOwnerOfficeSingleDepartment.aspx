<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOwnerOfficeSingleDepartment.aspx.cs" Inherits="AccountsOwnerOfficeSingleDepartment" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Single Department</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
          <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmmsg.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSingleDepartment" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
           <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="ucConfirmmsg" runat="server" OnClick="ucConfirmmsg_Click" CssClass="hidden" />


            <eluc:TabStrip ID="MenuSingleDepartment" runat="server" OnTabStripCommand="MenuSingleDepartment_TabStripCommand"></eluc:TabStrip>
            <table id="tbldiv" runat="server" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <eluc:TabStrip ID="MenuPost" runat="server" OnTabStripCommand="MenuPost_TabStripCommand"></eluc:TabStrip>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBankAccount" runat="server" Text="Bank Account"></telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadComboBox ID="ddlBankAccount" runat="server" CssClass="dropdown_mandatory" Width="95%">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSource" runat="server" Text="Source"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSource" runat="server" CssClass="input_mandatory">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Owner" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Office" Value="2"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text="Voucher Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherStatus" runat="server" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyAmount" runat="server" Text="Currency / Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" Enabled="false" Width="10%"></telerik:RadTextBox>/
                            <eluc:Number ID="ucAmount" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReference" runat="server" Text="Reference"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReference" runat="server" CssClass="input_mandatory" Width="360px" TextMode="MultiLine" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBankCharges" runat="server" Text="Bank Charges"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucBankCharges" runat="server" CssClass="readonlytextbox" Width="30%" />
                         <eluc:Number ID="ucbankchargeamtusd" runat="server" Visible="false" CssClass="readonlytextbox"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLongDescription" runat="server" Text="Long Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLongDescription" runat="server" CssClass="input_mandatory" Width="360px" TextMode="MultiLine" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuSingleDepartmentGrid" runat="server" OnTabStripCommand="MenuSingleDepartmentGrid_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFundAllocate" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" Height="50%" CellPadding="3" OnItemDataBound="gvFundAllocate_ItemDataBound" ShowFooter="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemCommand="gvFundAllocate_ItemCommand" OnRowCancelingEdit="gvFundAllocate_RowCancelingEdit" OnNeedDataSource="gvFundAllocate_NeedDataSource"
                OnRowUpdating="gvFundAllocate_RowUpdating" OnRowEditing="gvFundAllocate_RowEditing" ShowHeader="true"
                OnRowDeleting="gvFundAllocate_RowDeleting" EnableViewState="false" AllowSorting="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Reference No" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerOfficeAllocateId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNEROFFICEALLOCATEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="35%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="lblDescriptionEdit" runat="server" CssClass="input_mandatory" Width="360px"
                                    TextMode="MultiLine" Resize="Both" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Allocated Amount" HeaderStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllocatedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOCATIONAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAllocatedAmountEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOCATIONAMOUNT") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Allocated Amount" HeaderStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAllocatedAmountBankCurHeader" runat="server">Allocated Amount</telerik:RadLabel>
                                (<%=strCurrency%>)
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllocatedAmountBankCur" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOCATIONAMOUNTBANKCUR","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=strAmountTotal%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="Edit" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Reject" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Delete" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Currency Convert" ImageUrl="<%$ PhoenixTheme:images/add_task.png %>"
                                    CommandName="Convert" CommandArgument='<%# Container.DataItem %>' ID="cmdConvert"
                                    ToolTip="Allocate"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItem %>' ID="cmdEditCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="false" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
