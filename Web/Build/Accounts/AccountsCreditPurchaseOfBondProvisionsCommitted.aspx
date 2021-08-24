<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditPurchaseOfBondProvisionsCommitted.aspx.cs" Inherits="AccountsCreditPurchaseOfBondProvisionsCommitted" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Credit Purchase Of Bond/Provisions Committed</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmVesselCreditPurchaseCommitted" DecoratedControls="All" />
    <form id="frmVesselCreditPurchaseCommitted" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />
            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuGeneralSub" runat="server" OnTabStripCommand="MenuGeneralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table id="tblGuidance" runat="server">
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblNote" runat="server" Text="Note: <br>*Budget details can be changed as long as the PO is not tagged to the Invoice." CssClass="guideline_text"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuOffice" runat="server" OnTabStripCommand="MenuOffice_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCreditPurchase" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCreditPurchase_ItemCommand" OnItemDataBound="gvCreditPurchase_ItemDataBound" OnNeedDataSource="gvCreditPurchase_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Order" Name="Order" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Supplier" Name="Supplier" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Invoice" Name="Invoice" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Invoice Amount Discount" Name="Invoice Amount Discount" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Advance" Name="Advance" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Budget Code" Name="Budget Code" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStockType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselsupplierid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Order">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkOrderNumber" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status" ColumnGroupName="Order">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERSTATUSNAME")%>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDORDERSTATUSNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Order" AllowSorting="true" SortExpression="FLDORDERDATE">
                            <HeaderStyle Width="9%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsMapped" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMAPPED") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERDATE") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDORDERDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="Supplier">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME")%>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Code" ColumnGroupName="Supplier">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupplierCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMappedSupplierCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE")%>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkSupplierCode" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Invoice">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Currency" ColumnGroupName="Invoice">
                            <HeaderStyle Width="13%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Before" ColumnGroupName="Invoice Amount Discount">
                            <HeaderStyle Width="10%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmtBeforeDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMTBEFOREDISCOUNT") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDAMTBEFOREDISCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="After" ColumnGroupName="Invoice Amount Discount">
                            <HeaderStyle Width="10%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmtAfterDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMTAFTERDISCOUNT") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDAMTAFTERDISCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Advance">
                            <HeaderStyle Width="10%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdvanceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budget" ColumnGroupName="Budget Code">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE")%>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:BudgetCode runat="server" ID="ucBudgetCodeEdit" AppendDataBoundItems="true" Width="60px"
                                    CssClass="input_mandatory" BudgetCodeList='<%# PhoenixRegistersBudget.ListBudget() %>'
                                    SelectedBudgetSubAccount='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Owner" ColumnGroupName="Budget Code">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOwnerBudgetcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOWNERBUDGETCODE") %>'
                                        MaxLength="20" CssClass="input" Width="47px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETID") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Bill to Company">
                            <HeaderStyle Width="14%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBilltoCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLTOCOMPANYNAME")%>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDBILLTOCOMPANYNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBILLTOCOMPANY") %>'></telerik:RadLabel>
                                <eluc:Company ID="ddlCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' Width="70px"
                                    CssClass="dropdown_mandatory" runat="server" SelectedCompany='<%# DataBinder.Eval(Container, "DataItem.FLDBILLTOCOMPANY") %>'
                                    AppendDataBoundItems="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                    CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
