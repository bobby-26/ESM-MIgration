<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsBankStatementReconBankAndLedgerdifflist.aspx.cs"
    Inherits="Accounts_AccountsBankStatementReconBankAndLedgerdifflist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bank Statement Upload</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <style type="text/css">
        .style2
        {
            width: 216px;
        }
    </style>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmForwarderExcelUpload" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="Attachment" ShowMenu="false" Text="Bank and Ledger Differences"></eluc:Title>
            <eluc:Status runat="server" ID="ucStatus" />
        <%--  <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="Bankupload" runat="server"></eluc:TabStrip>
        </div>--%>
        <table id="tbldiv" runat="server" cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                            <eluc:TabStrip ID="MenutravelInvoice" runat="server" OnTabStripCommand="MenutravelInvoice_OnTabStripCommand">
                            </eluc:TabStrip>
                </td>
            </tr>
        </table>
        <%--  <div class="navSelect" style="top: 50px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenutravelInvoice" runat="server" >
            </eluc:TabStrip>
        </div>--%>
        <table>
            <tr>
                <td>
                    Amount from
                </td>
                <td class="style2">
                    <telerik:RadTextBox ID="txtAmountfrom" runat="server" CssClass="txtNumber" DecimalPlace="2"
                        IsPositive="true" Width="120px"></telerik:RadTextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                    Date from
                </td>
                <td class="style2">
                    <eluc:Date ID="txtDateFrom" runat="server"  DatePicker="true" />
                </td>
                <td>
                    TT ref
                </td>
                <td>
                    <telerik:RadTextBox ID="txtTTref" runat="server" CssClass="input" Width="120px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Amount to
                </td>
                <td class="style2">
                    <telerik:RadTextBox ID="txtAmountTo" runat="server" CssClass="txtNumber" DecimalPlace="2"
                        IsPositive="true" Width="120px"></telerik:RadTextBox>
                </td>
                <td>
                    Date to
                </td>
                <td class="style2">
                    <eluc:Date ID="txtDateTo" runat="server"  DatePicker="true" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <br />
        <br />
            <eluc:TabStrip ID="MenuAttachment" runat="server" OnTabStripCommand="MenuAttachment_TabStripCommand">
            </eluc:TabStrip>
        <b>Bank Statement </b>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvBankStatement" runat="server" AutoGenerateColumns="False" Font-Size="11px" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="true"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSortCommand="gvBankStatement_Sorting" AllowCustomPaging="true"
            DataKeyNames="FLDLINEITEMID" OnItemDataBound="gvBankStatement_RowDataBound" OnItemCommand="gvBankStatement_RowCommand" OnNeedDataSource="gvBankStatement_NeedDataSource">
           <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
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
                <%--   Tag ID	Currency	Bank Amount Net	Allocated in Leger	Remaining amount	Allocation Status	Action--%>
<%--                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                <telerik:GridTemplateColumn>
                    <%--  <HeaderTemplate>
                        <asp:CheckBox ID="chkAllRemittance" runat="server" Text="Check All" AutoPostBack="true"  OnPreRender="CheckAll" />
                    </HeaderTemplate>--%>
                    <ItemTemplate>
                        <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="BankStatementSaveCheckedValues" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Ledger TT Ref">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblBankAmount" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDTTREFERENCE")) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Amount" AllowSorting="true" SortExpression="FLDAMOUNT">
                   <%-- <HeaderTemplate>
                        <asp:LinkButton ID="lblAmount" runat="server" CommandName="Sort" CommandArgument="FLDAMOUNT" Text="Amount"></asp:LinkButton>
                    </HeaderTemplate>--%>
                    <ItemStyle HorizontalAlign="RIGHT"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblRemainingamount" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:n2}" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Value Date" AllowSorting="true" SortExpression="FLDVALUEDATE">
               <%--     <HeaderTemplate>
                        <asp:LinkButton ID="lblValueDate" runat="server" CommandName="Sort" CommandArgument="FLDVALUEDATE" Text="Value Date"></asp:LinkButton>
                    </HeaderTemplate>--%>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblAllocatedinLeger" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVALUEDATE","{0:dd/MMM/yyyy}" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Customer Ref.">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDBANKVOUCHERNUMBER" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Narrative">
                    <ItemStyle HorizontalAlign="LEFT"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblAllocationStatus" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDNARRATIVE")) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Post" ImageUrl="<%$ PhoenixTheme:images/new.png %>"
                            CommandName="NEW" CommandArgument='<%# Container.DataItem %>' ID="cmdNew"
                            ToolTip="New Voucher"></asp:ImageButton>
                        <%-- <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                            CommandName="VIEW" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                            ToolTip="View Excel"></asp:ImageButton>
                        <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton ID="cmdArchive" runat="server" AlternateText="Archive" CommandArgument="<%# Container.DataItemIndex %>"
                            CommandName="ARCHIVE" ImageUrl="<%$ PhoenixTheme:images/archive.png %>" ToolTip="Archive" />
                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                            CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete" />--%>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
                  
                    </MasterTableView>
            
        </telerik:RadGrid>
        <br />
        <br />
        <br />
        <%--  <table width="100%" border="0" class="datagrid_pagestyle">
            <tr>
                <td nowrap="nowrap" align="center">
                    <telerik:RadLabel ID="lblPagenumber" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="lblPages" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="lblRecords" runat="server">
                    </telerik:RadLabel>&nbsp;&nbsp;
                </td>
                <td nowrap="nowrap" align="left" width="50px">
                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                </td>
                <td width="20px">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" width="50px">
                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                </td>
                <td nowrap="nowrap" align="center">
                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                    </asp:TextBox>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                        Width="40px"></asp:Button>
                </td>
            </tr>
        </table>
        --%>
        <b>Bank Ledger</b>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvBankLedger" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"  OnNeedDataSource="gvBankLedger_NeedDataSource" EnableHeaderContextMenu="true"
            DataKeyNames="FLDVOUCHERLINEITEMID" OnItemDataBound="gvBankLedger_RowDataBound" OnItemCommand="gvBankLedger_RowCommand" GroupingEnabled="false">
         <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCITYID">
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
                <%--   Tag ID	Currency	Bank Amount Net	Allocated in Leger	Remaining amount	Allocation Status	Action--%>
<%--                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                <telerik:GridTemplateColumn>
                    <%--<HeaderTemplate>
                        <asp:CheckBox ID="chkAllRemittance" runat="server" Text="Check All" AutoPostBack="true"
                            OnPreRender="CheckAll" />
                    </HeaderTemplate>--%>
                    <ItemTemplate>
                        <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="BankLedgerSaveCheckedValues" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Ref.">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblRefno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO")) %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblLedgerLineitemId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOUCHERLINEITEMID")%>' Visible="false"></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Amount" AllowSorting="true" SortExpression="FLDAMOUNT">
                   <%-- <HeaderTemplate>
                        <telerik:RadLabel ID="lblAmount" runat="server" CommandName="Sort" CommandArgument="FLDAMOUNT" Text="Amount"></telerik:RadLabel>
                    </HeaderTemplate>--%>
                    <ItemStyle HorizontalAlign="RIGHT"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblRemainingamount" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:n2}" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Value Date" AllowSorting="true" SortExpression="FLDVALUEDATE">
                 <%--   <HeaderTemplate>
                        <telerik:RadLabel ID="lblValueDate" runat="server" CommandName="Sort" CommandArgument="FLDVALUEDATE" Text="Value Date"></telerik:RadLabel>
                    </HeaderTemplate>--%>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE","{0:dd/MMM/yyyy}" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher entry">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblAllocatedinLeger" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Description">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
<%--                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                            CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png  %>" ToolTip="Delete" />--%>
                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                            CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                            ToolTip="Edit"></asp:ImageButton>
                        <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
<%--                        <asp:ImageButton ID="cmdEditVoucher" runat="server" AlternateText="Archive" CommandArgument="<%# Container.DataItemIndex %>"
                            CommandName="EDITVOUCHER" ImageUrl="<%$ PhoenixTheme:images/edit-info.png %>"
                            ToolTip="Change ledger entries" />--%>
                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton ID="cmdVouchershowing" runat="server" AlternateText="Archive"
                            CommandArgument="<%# Container.DataItem %>" CommandName="VOUCHERSHOWING"
                            ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" ToolTip="Voucher showing" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
                    </MasterTableView>
        </telerik:RadGrid>
        <%--   <table width="100%" border="0" class="datagrid_pagestyle">
            <tr>
                <td nowrap="nowrap" align="center">
                    <telerik:RadLabel ID="Label1" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="Label2" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="Label3" runat="server">
                    </telerik:RadLabel>&nbsp;&nbsp;
                </td>
                <td nowrap="nowrap" align="left" width="50px">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                </td>
                <td width="20px">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" width="50px">
                    <asp:LinkButton ID="LinkButton2" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                </td>
                <td nowrap="nowrap" align="center">
                    <asp:TextBox ID="TextBox1" MaxLength="3" Width="20px" runat="server" CssClass="input">
                    </asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                        Width="40px"></asp:Button>
                </td>
            </tr>
        </table>--%>
        <br />
        <br />
        <br />
        <b>Voucher Not Showing in Current Bank Statement</b>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCurrentBankStatement" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowCustomPaging="true" OnNeedDataSource="gvCurrentBankStatement_NeedDataSource"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="true"
            DataKeyNames="FLDVOUCHERLINEITEMID" OnItemDataBound="gvCurrentBankStatement_RowDataBound" OnItemCommand="gvCurrentBankStatement_RowCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
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
                <%--   Tag ID	Currency	Bank Amount Net	Allocated in Leger	Remaining amount	Allocation Status	Action--%>
<%--                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                <telerik:GridTemplateColumn>
                    <%--<HeaderTemplate>
                        <asp:CheckBox ID="chkAllRemittance" runat="server" Text="Check All" AutoPostBack="true"
                            OnPreRender="CheckAll" />
                    </HeaderTemplate>--%>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Ref.">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblRefno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO")) %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblLineitemId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOUCHERLINEITEMID")%>'
                            Visible="false"></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Amount">
                    <%--<HeaderTemplate>
                        <telerik:RadLabel ID="lblAmount" runat="server" CommandName="Sort" CommandArgument="FLDAMOUNT" Text="Amount"></telerik:RadLabel>
                    </HeaderTemplate>--%>
                    <ItemStyle HorizontalAlign="RIGHT"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblRemainingamount" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:n2}" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Value Date" AllowSorting="true" SortExpression="FLDVALUEDATE">
                   <%-- <HeaderTemplate>
                        <telerik:RadLabel ID="lblValueDate" runat="server" CommandName="Sort" CommandArgument="FLDVALUEDATE" Text="Value Date"></telerik:RadLabel>
                    </HeaderTemplate>--%>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE","{0:dd/MMM/yyyy}" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher entry">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblAllocatedinLeger" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Description">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION" )) %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Action">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                   <%-- <HeaderTemplate>
                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                    </HeaderTemplate>--%>
                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
<%--                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                            CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png  %>" ToolTip="Delete" />--%>
<%--                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                            ToolTip="Edit"></asp:ImageButton>--%>
                        <%--<img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />--%>
<%--                        <asp:ImageButton ID="cmdEditVoucher" runat="server" AlternateText="Archive" CommandArgument="<%# Container.DataItemIndex %>"
                            CommandName="EDITVOUCHER" ImageUrl="<%$ PhoenixTheme:images/edit-info.png %>"
                            ToolTip="Change ledger entries" />--%>
                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton ID="cmdVouchernotshowing" runat="server" AlternateText="Archive"
                            CommandArgument="<%# Container.DataItem %>" CommandName="VOUCHERNOTSHOWING"
                            ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" ToolTip="Voucher not showing" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
                    </MasterTableView>
        </telerik:RadGrid>
        <%--   <table width="100%" border="0" class="datagrid_pagestyle">
            <tr>
                <td nowrap="nowrap" align="center">
                    <telerik:RadLabel ID="Label1" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="Label2" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="Label3" runat="server">
                    </telerik:RadLabel>&nbsp;&nbsp;
                </td>
                <td nowrap="nowrap" align="left" width="50px">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                </td>
                <td width="20px">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" width="50px">
                    <asp:LinkButton ID="LinkButton2" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                </td>
                <td nowrap="nowrap" align="center">
                    <asp:TextBox ID="TextBox1" MaxLength="3" Width="20px" runat="server" CssClass="input">
                    </asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                        Width="40px"></asp:Button>
                </td>
            </tr>
        </table>--%>

    </form>
</body>
</html>
