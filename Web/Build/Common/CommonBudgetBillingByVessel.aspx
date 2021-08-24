<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetBillingByVessel.aspx.cs" Inherits="CommonBudgetBillingByVessel" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetBillingItem" Src="~/UserControls/UserControlBudgetBillingItem.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget Break Down</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCommonBudgetGroupAllocation" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlCommonBudgetGroupAllocation" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuBudgetTab" runat="server" TabStrip="true" OnTabStripCommand="BudgetTab_TabStripCommand"></eluc:TabStrip>

            <eluc:TabStrip ID="subMenuBudgetTab" runat="server" TabStrip="true" OnTabStripCommand="BudgetTab_TabStripCommand"></eluc:TabStrip>



            <table id="tblBudgetGroupAllocationSearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Financial Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucFinancialYear" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            CssClass="input" Enabled="false" QuickTypeCode="55" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEffectiveDate" runat="server" Text="Effective Date "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEffectiveDate" runat="server" CssClass="input" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselCode" runat="server" CssClass="input" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCommonBudgetGroupAllocation" runat="server" OnTabStripCommand="CommonBudgetGroupAllocation_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselAllocation" runat="server" AutoGenerateColumns="False" Height="60%"
                Font-Size="11px" Width="100%" CellPadding="3"
                OnItemCommand="gvVesselAllocation_ItemCommand"
                OnItemDataBound="gvVesselAllocation_ItemDataBound"
                OnSelectedIndexChanging="gvVesselAllocation_SelectedIndexChanging"
                ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvVesselAllocation_NeedDataSource"
                DataKeyNames="FLDBUDGETBILLINGBYVESSELID" OnDeleteCommand="gvVesselAllocation_DeleteCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnUpdateCommand="gvVesselAllocation_UpdateCommand" OnSortCommand="gvVesselAllocation_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Billing Item" HeaderStyle-Width="162px">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblBillingItemDescriptionHeader" runat="server" CommandName="Sort"
                                    CommandArgument="FLDBILLINGITEMDESCRIPTION">Billing Item&nbsp;</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBillingItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGITEMDESCRIPTION") %>'
                                    CommandName="SELECT" CommandArgument='<%# Container.DataItem%>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblBillingItemDescription" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGITEMDESCRIPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetBillingId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETBILLINGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetBillingByVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETBILLINGBYVESSELID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblBillingItemDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGITEMDESCRIPTION") %>'></telerik:RadLabel>
                                <eluc:BudgetBillingItem ID="ucBudgetBillingItemEdit" runat="server" CssClass="dropdown_mandatory"
                                    AutoPostBack="false" OnTextChangedEvent="EditBillingItem_Changed" Width="200px"
                                    Visible="false" AppendDataBoundItems="true" SelectedBillingItem="FLDBUDGETBILLINGID" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:BudgetBillingItem ID="ucBudgetBillingItemAdd" runat="server" CssClass="dropdown_mandatory"
                                    AutoPostBack="true" OnTextChangedEvent="AddBillingItem_Changed" Width="140px"
                                    AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency" HeaderStyle-Width="85px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequencyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Billing Basis" HeaderStyle-Width="105px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBillingBasisName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGBASISNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Billing Unit" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBillingUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLINGUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Budget Code" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="175px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVisitType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISITTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlVisitTypeEdit" runat="server" Width="200px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                        <telerik:RadComboBoxItem Text="IT Visit" Value="1" />
                                        <telerik:RadComboBoxItem Text="Superintendent Visit" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlVisitTypeAdd" runat="server" >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                        <telerik:RadComboBoxItem Text="IT Visit" Value="1" />
                                        <telerik:RadComboBoxItem Text="Superintendent Visit" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budgeted Quantity" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselBudgetedQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETEDQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtBudgetedQuantityEdit" runat="server" Width="90px" CssClass="input"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETEDQUANTITY","{0:n2}") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtBudgetedQuantityAdd" runat="server" Width="90px" CssClass="input" Mask="9999999.99" Text="0.00" />
                            </FooterTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrencyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:UserControlCurrency ID="ddlCurrencyCodeEdit" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                    CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:UserControlCurrency ID="ddlCurrencyCodeAdd" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                    CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="false" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budget Amount" HeaderStyle-Width="135px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" Width="90px" CssClass="input_mandatory"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAmountAdd" runat="server" Width="90px" CssClass="input_mandatory"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="140px" HeaderText="Owner Budget Code">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'
                                        MaxLength="20" CssClass="input" Width="60%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input_mandatory"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="ibtnShowOwnerBudgetEdit" runat="server" CommandArgument="<%# Container.DataItem %>">
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETID") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListOwnerBudgetAdd">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCodeAdd" runat="server" MaxLength="20" CssClass="input"
                                        Width="60%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetNameAdd" runat="server" Width="0px" CssClass="input_mandatory"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="ibtnShowOwnerBudgetAdd" runat="server" CommandArgument="<%# Container.DataItem %>">
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtOwnerBudgetIdAdd" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupIdAdd" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="130px">
                           <ItemTemplate>
                                <telerik:RadLabel ID="lblComapanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Company ID="ucCompanyEdit" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Company ID="ucCompanyAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="100%"
                                    SelectedCompany="12" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="88px" />
                            <ItemStyle Width="20px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
