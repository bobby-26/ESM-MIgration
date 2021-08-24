<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDashboardOwnerDebitCreditNoteGenerateRegister.aspx.cs" Inherits="Accounts_AccountsDashboardOwnerDebitCreditNoteGenerateRegister" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Debit/Credit Note</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDebitCreditNote" runat="server" submitdisabledcontrols="true">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                    CssClass="hidden" />
                <eluc:TabStrip ID="MenuDebitCreditNoteGrid" runat="server" OnTabStripCommand="MenuDebitCreditNoteGrid_TabStripCommand"></eluc:TabStrip>
                <asp:HiddenField ID="hdnScroll" runat="server" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvDebitCreditNote" runat="server" Height="300px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvDebitCreditNote_NeedDataSource"
                    OnItemCommand="gvDebitCreditNote_ItemCommand"
                    OnItemDataBound="gvDebitCreditNote_ItemDataBound1"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDOWNERDEBITCREDITNOTEID" CommandItemDisplay="Top">
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
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="100px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reference No." HeaderStyle-Width="150px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDebitCreditNoteId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERDEBITCREDITNOTEID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkReferenceNo" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER")  %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Billing Company" HeaderStyle-Width="80px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBillingCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="100px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblType" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Sub Type" HeaderStyle-Width="150px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSubType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Bank Receiving Funds" HeaderStyle-Width="200px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBank" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblBankDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Line Time" HeaderStyle-Width="200px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLineItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEM") %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMALL") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="50px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Received Amt" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReceivedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory txtNumber"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDAMOUNT") %>' Width="100%">
                                    </eluc:Number>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Difference" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDifference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDIFFERENCE","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Received Date" HeaderStyle-Width="150px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReceivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucReceivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE")) %>' CssClass="input_mandatory" DatePicker="true" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Received Status" HeaderStyle-Width="100px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReceivedStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDSTATUSNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="ddlReceivedStatus" runat="server" CssClass="dropdown_mandatory"
                                        DataSource='<%# PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 141)%>'
                                        DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" Width="100%" Filter="Contains" EmptyMessage="Type to select">
                                    </telerik:RadComboBox>
                                    <telerik:RadLabel ID="lblReceivedStatusEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDSTATUS") %>'></telerik:RadLabel>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="200px">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Width="100%" TextMode="MultiLine"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'>
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Open/Close" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOpenClose" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENCLOSE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="ddlOpenClose" runat="server" CssClass="dropdown_mandatory" Width="100%" Filter="Contains" EmptyMessage="Type to select">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Open" Text="Open"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="Close" Text="Close"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadLabel ID="lblOpenCloseEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENCLOSE") %>'></telerik:RadLabel>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Voucher No." HeaderStyle-Width="100px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVoucherNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="200px">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="GETEDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEditopen"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="Edit" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                        ToolTip="Attachment"></asp:ImageButton>
                                    <img id="Img5" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Reject" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Delete" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdHistory" runat="server" AlternateText="History" CommandArgument='<%# Container.DataSetIndex %>'
                                        CommandName="History" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png%>" ToolTip="History" />
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEditCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                <div>
                    <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px; width: 100%;"></iframe>
                </div>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
