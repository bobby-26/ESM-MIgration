<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditDebitNoteRebateReceivable.aspx.cs"
    Inherits="Accounts_AccountsCreditDebitNoteRebateReceivable" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlStockItemEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Visible="false" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuRebateReceivable" runat="server" OnTabStripCommand="MenuRebateReceivable_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <br />
            <table id="TBL1" style="width: 75%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCreditNoteRegisterNo" runat="server" Text="Credit Note Register No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRegisterNo" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSupplier" runat="server" CssClass="readonlytextbox" Width="300"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyAmount" runat="server" Text="Currency/Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCurrencyAmount" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderRebateReceivable_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRebateReceivable" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvRebateReceivable_ItemCommand" ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnNeedDataSource="gvRebateReceivable_NeedDataSource" OnItemDataBound="gvRebateReceivable_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="Row Number" AllowSorting="true">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>

                                <asp:LinkButton ID="lnkVoucherLineItemNo" runat="server"
                                    CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNO") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblCreditDebitLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEBITNOTEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVoucherLineItemAllocatedId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMALLOCATEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Code" AllowSorting="true">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListExpenseAccount">
                                    <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                        Width="65%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="hidden" MaxLength="50"
                                        Width="1%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="hidden" MaxLength="20" Width="10px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountSource" CssClass="hidden" runat="server"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountUsage" CssClass="hidden" runat="server"></telerik:RadTextBox>
                                    <br />
                                    <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Description" AllowSorting="true">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Account Code" AllowSorting="true">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListSubAccount">
                                    <telerik:RadTextBox ID="txtAccountCodeEdit" runat="server" CssClass="hidden" MaxLength="20" Width="0px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountDescriptionEdit" runat="server" CssClass="hidden" MaxLength="50" Width="0px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountIdEdit" runat="server" CssClass="hidden" MaxLength="20" Width="0px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountSourceEdit" CssClass="hidden" Width="0px" runat="server"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountUsageEdit" CssClass="hidden" Width="0px" runat="server"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Width="60px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" />
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTMAPID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Transaction Currency" AllowSorting="true">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Currency runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                    ID="ucCurrencyAdd" AppendDataBoundItems="true" Width="80px" AutoPostBack="false" CssClass="dropdown_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Long Description" AllowSorting="true">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLongDescriptionHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtLongDescription" runat="server" CssClass="input" TextMode="MultiLine">
                                </telerik:RadTextBox>
                                <br />
                                <b>
                                    <telerik:RadLabel ID="lblTotalAmount" runat="server" Text="Total"></telerik:RadLabel>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prime Amount" AllowSorting="true">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSign" runat="server" Text="-" Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBaseAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtBaseAmount" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="80px"></eluc:Number>
                                <br />
                                <b>
                                    <%=strAmountTotal%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="6%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="Edit" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
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
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
