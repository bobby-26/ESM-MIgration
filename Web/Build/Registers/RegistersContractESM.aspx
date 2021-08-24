<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersContractESM.aspx.cs"
    Inherits="RegistersContractESM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Contract" Src="~/UserControls/UserControlContractCBA.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Budget" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="GWC" Src="~/UserControls/UserControlGlobalWageComponent.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Standard Wage Components</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWageComponents" runat="server" Text="Wage Components"></telerik:RadLabel>
                        <eluc:Hard ID="ucWageComponents" runat="server" HardTypeCode="156" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" AutoPostBack="true" OnTextChangedEvent="ucWageComponents_Changed" Width="240px" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersCity" runat="server" OnTabStripCommand="RegistersCity_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" Height="45%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvCrew_ItemCommand" OnItemDataBound="gvCrew_ItemDataBound" EnableViewState="false" ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvCrew_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Budget Code" Name="Budget" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblComponentIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtShortCodeEdit" runat="server" CssClass="input_mandatory" MaxLength="3"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>' Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtShortCodeAdd" runat="server" CssClass="input_mandatory" MaxLength="3"
                                    ToolTip="Enter Short Code" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtComponentNameEdit" runat="server" CssClass="input_mandatory"
                                    MaxLength="100" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'
                                    ToolTip="Enter Component Name" Width="95%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtComponentNameAdd" runat="server" CssClass="input_mandatory" MaxLength="100"
                                    ToolTip="Enter Component Name" Width="95%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Global Wage Component">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWageComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWAGECOMPONENTNAME")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDWAGECOMPONENTNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:GWC ID="ucGlobalWageComponentEdit" runat="server" AppendDataBoundItems="true" CssClass="input" Width="160px"
                                    ComponentList='<%#PhoenixRegisterGlobalWageComponent.GloabalWageComponentList(1) %>'
                                    SelectedComponent='<%# DataBinder.Eval(Container, "DataItem.FLDWAGECOMPONENTID")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:GWC ID="ucGlobalWageComponentAdd" runat="server" AppendDataBoundItems="true" CssClass="input" Width="160px"
                                    ComponentList='<%#PhoenixRegisterGlobalWageComponent.GloabalWageComponentList(1) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Calculation Basis">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCALCULATIONBASISNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ddlCalBasisEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    HardTypeCode="72" HardList="<%#PhoenixRegistersHard.ListHard(1,72) %>" SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDCALCULATIONBASIS")%>'
                                    Width="95%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ddlCalBasisAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    HardTypeCode="72" HardList="<%#PhoenixRegistersHard.ListHard(1,72) %>" Width="95%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payable Basis">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEBASISNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ddlPayBasisEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    HardTypeCode="72" ShortNameFilter="BOC,EOC,MOC,SCC,BNC,CNC" HardList='<%#PhoenixRegistersHard.ListHard(1, 72, 0, "BOC,EOC,MOC,SCC,BNC,CNC")%>'
                                    SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEBASIS")%>' Width="99%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ddlPayBasisAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    HardTypeCode="72" ShortNameFilter="BOC,EOC,MOC,SCC,BNC,CNC" HardList='<%#PhoenixRegistersHard.ListHard(1, 72, 0, "BOC,EOC,MOC,SCC,BNC,CNC")%>'
                                    Width="99%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Posting" ColumnGroupName="Budget">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDBUDGETCODE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Budget ID="ddlBudgetEdit" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>" SelectedBudgetCode='<%# DataBinder.Eval(Container, "DataItem.FLDBUDGETID")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Budget ID="ddlBudgetAdd" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charging" ColumnGroupName="Budget">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCHARGINGBUDGETCODE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Budget ID="ddlChargingBudgetEdit" runat="server" AppendDataBoundItems="true"
                                    CssClass="input" BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>" SelectedBudgetCode='<%# DataBinder.Eval(Container, "DataItem.FLDCHARGINGBUDGETID")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Budget ID="ddlChargingBudgetAdd" runat="server" AppendDataBoundItems="true"
                                    CssClass="input" BudgetCodeList="<%#PhoenixRegistersBudget.ListBudget() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Contract Setting">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShowtoOwner" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHOWTOOWNERDESC")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox DropDownPosition="Static" ID="ddlContactEdit" runat="server" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select Contract" Filter="Contains" MarkFirstMatch="true" CssClass="input_mandatory" Width="100px">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--select--" />
                                        <telerik:RadComboBoxItem Value="0" Text="Main" />
                                        <telerik:RadComboBoxItem Value="1" Text="SideLetter1" />
                                        <telerik:RadComboBoxItem Value="2" Text="SideLetter2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox DropDownPosition="Static" ID="ddlContactAdd" runat="server" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select Contract" Filter="Contains" MarkFirstMatch="true" CssClass="input_mandatory" Width="100px">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--select--" />
                                        <telerik:RadComboBoxItem Value="0" Text="Main" />
                                        <telerik:RadComboBoxItem Value="1" Text="SideLetter1" />
                                        <telerik:RadComboBoxItem Value="2" Text="SideLetter2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Is Offer Letter Check">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblofferletter" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISCHECKOFFERLETTERDESC")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadRadioButtonList ID="rblIsCheckOfferLetterEdit" runat="server" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Text="Yes" Value="1" />
                                        <telerik:ButtonListItem Text="No" Value="0" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadRadioButtonList ID="rblIsCheckOfferLetterAdd" runat="server" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Text="Yes" Value="1" />
                                        <telerik:ButtonListItem Text="No" Value="0" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="Rank" runat="server" Text="Rank"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            CssClass="dropdown_mandatory" OnTextChangedEvent="ddlRankAdd_TextChangedEvent" Width="240px" />
                        <telerik:RadLabel ID="lblNote" runat="server" Text="Note: Select a Rank to Update the Component Wages."></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCR" Height="40%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvCR_ItemCommand" OnItemDataBound="gvCR_ItemDataBound" EnableViewState="false" ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvCR_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Last Modified" Name="Modified" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDMAINCOMPONENTNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" Visible="false">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentIdRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSubComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBCOMPONENTID")%>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblComponentIdRankEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSubComponentIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBCOMPONENTID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKID")%>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCY")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Currency ID="ddlCurrencyEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    CurrencyList="<%#PhoenixRegistersCurrency.ListActiveCurrency(1, true) %>" SelectedCurrency='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYID")%>' Width="99%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT").ToString().Replace(".00", "") + " " + DataBinder.Eval(Container, "DataItem.FLDCALCULATIONNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>' IsPositive="true" Width="120px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="by" ColumnGroupName="Modified">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDMODIFIEDBY").ToString().Equals("") ? " " :DataBinder.Eval(Container, "DataItem.FLDMODIFIEDBY")+ " on " + string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDMODIFIEDDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Modified">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastModifiedDate" runat="server" Text='<%# string.Format("{0:dd/MMM/yyyy}",DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="History" CommandName="HISTORY" ID="cmdHistory" ToolTip="Component History">
                                    <span class="icon"><i class="fas  fa-receipt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>

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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
