<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDashboardERMVoucherDetail.aspx.cs" Inherits="Accounts_AccountsDashboardERMVoucherDetail" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Unbilled Vouchers</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand"></eluc:TabStrip>

            <br />
            <b>
                <telerik:RadLabel ID="lblAccountCode" runat="server"
                    Text='Excel Export will only export the first 500 records starting from current page.'
                    Style="color: #3333FF">
                </telerik:RadLabel>
            </b>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCountry" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCountry_NeedDataSource"
                OnItemCommand="gvCountry_ItemCommand"
                OnUpdateCommand="gvCountry_UpdateCommand"
                OnItemDataBound="gvCountry_ItemDataBound1"
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

                        <telerik:GridTemplateColumn HeaderText="Account" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVoucherDetailId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDETAILID") %>'
                                    CssClass="gridinput">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVoucherLineItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    CssClass="gridinput">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'
                                    CssClass="gridinput">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVoucherDetailIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDETAILID") %>'
                                    CssClass="gridinput">
                                </telerik:RadLabel>
                                <span id="spnPickListCreditAccount">
                                    <telerik:RadTextBox ID="txtAccountCodeEdit" runat="server" CssClass="input_mandatory" Enabled="false"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>' MaxLength="20" Width="60px">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtCreditAccountDescription" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="0px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCreditAccountId" runat="server" CssClass="input_mandatory" MaxLength="20"
                                        Width="0px">
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAccountDescription" runat="server" Text="Account Description"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtAccountDescriptionEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTDESCRIPTION") %>'
                                    TextMode="MultiLine" Rows="2">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget code" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblESMBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESMBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtESMBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESMBUDGETCODE") %>'
                                    CssClass="gridinput" MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBudgetCodeDescription" runat="server" Text="Budget Code Description"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtBudgetCodeDescriptionEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODEDESCRIPTION") %>'
                                    TextMode="MultiLine" Rows="2">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" MaxLength="20" CssClass="input"
                                        Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voucher Date" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtVoucherDate" runat="server" CssClass="gridinput" DatePicker="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Debit Note Reference" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDebitNoteReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDebitNoteReferenceEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'
                                    CssClass="gridinput">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Phoenix Voucher" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhoenixVoucher" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHOENIXVOUCHER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblPhoenixVoucherEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHEONIXVOUCHERNUMBER") %>' Visible="false" />
                                <telerik:RadTextBox ID="txtPhoenixVoucherEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHOENIXVOUCHER") %>'
                                    CssClass="gridinput" MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reference" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblReference" runat="server" Text="Reference"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtReferenceEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCE") %>'
                                    CssClass="gridinput" MaxLength="100" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmount" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                    IsPositive="false" MaxLength="18" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    TextMode="MultiLine" Rows="3">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Included in Owner SOA" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <asp:Literal ID="lblIncludeSOA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCULDEINOWNERREPORT") %>'></asp:Literal>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIncludeSOA" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDINCULDEINOWNERREPORT").ToString().Equals("Yes"))?true:false %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Show Separately in the Vessel Summary Balance" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <asp:Literal ID="lblShowinSummaryBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHOWINSUMMARYBALANCE") %>'></asp:Literal>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkShowinSummaryBalance" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSHOWINSUMMARYBALANCE").ToString().Equals("Yes"))?true:false %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                    ToolTip="Attachment" Visible="false"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                    CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                    ToolTip="No Attachment" Visible="false"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
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

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
