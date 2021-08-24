<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsAdminEarningDeduction.aspx.cs"
    Inherits="VesselAccountsAdminEarningDeduction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlVesselMappingCurrency.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Earnings/Deductions Admin</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlType" runat="server" EnableLoadOnDemand="True" CssClass="input_mandatory"
                            EmptyMessage="Type to select" AutoPostBack="true" OnTextChanged="ddlEmployee_TextChangedEvent" Filter="Contains" MarkFirstMatch="true" Width="180px">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="Onboard Earnings/Deduction"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="3" Text="Cash Advance"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="4" Text="Allotment"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="5" Text="Radio Log"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="7" Text="Special Allotment"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="8" Text="Sign off Allotment"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBal" runat="server" Width="80px" CssClass="input" Enabled="false"
                            Style="text-align: right;" Text="0.00" Visible="false">
                        </telerik:RadTextBox>
                        <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ddlEmployee_TextChangedEvent" Width="280px" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Month ID="ddlMonth" runat="server" Width="120px" CssClass="input_mandatory" AutoPostBack="true"></eluc:Month>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Year ID="ddlYear" runat="server" Width="120px" OrderByAsc="false" CssClass="input_mandatory" AutoPostBack="true" OnTextChanged="ddlEmployee_TextChangedEvent"></eluc:Year>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuBondIssue" runat="server" OnTabStripCommand="MenuBondIssue_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" Height="92%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvCrewSearch_ItemCommand" OnItemDataBound="gvCrewSearch_ItemDataBound" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvCrewSearch_NeedDataSource" OnPreRender="gvCrewSearch_PreRender">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEARNINGDEDUCTIONID">
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Transaction" Name="Transaction" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Report" Name="Report" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselCurrencyid" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDCURRENCY"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAmount" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDAMOUNT"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEDid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEARNINGDEDUCTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWageHeadId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDWAGEHEADID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPurpose" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDPURPOSE"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblsignonoffid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblED" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTION"] %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblemployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Entry Type" UniqueName="wage">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEnry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDREIMBURSMENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <HeaderStyle Width="13%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurposeDESC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPurposeEdit" runat="server" Width="98%" CssClass="gridinput_mandatory" Text='<%#((DataRowView)Container.DataItem)["FLDPURPOSE"]%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" ColumnGroupName="Transaction">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCURRENCY" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDBASECURRENCYCODE"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Currency ID="ddlCurrencyEdit" runat="server" CssClass="input_mandatory"
                                    VesselId='<%#((DataRowView)Container.DataItem)["FLDVESSELID"]%>' AutoPostBack="true" Width="70px"
                                    CurrencyList='<%#PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(((DataRowView)Container.DataItem)["FLDVESSELID"].ToString()), 1)%>'
                                    SelectedCurrency='<%#((DataRowView)Container.DataItem)["FLDBASECURRENCY"]%>'
                                    OnTextChangedEvent="ddlCurrency_SelectedIndexChanged" />
                                <%----%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Transaction">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDBASEAMOUNT"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" MaxLength="8"
                                    Width="99%" Text='<%#((DataRowView)Container.DataItem)["FLDBASEAMOUNT"]%>' />
                                <%-- IsPositive='<%#((DataRowView)Container.DataItem)["FLDWAGEHEADID"].ToString() == ViewState["BRF"].ToString() ? false : true %>'--%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" ColumnGroupName="Report">
                            <HeaderStyle Width="6%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVESSELCURRENCY" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVCCURRENCYCODE"] %>' ToolTip='<%# ((DataRowView)Container.DataItem)["FLDVCCURRENCYCODE"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Report">
                            <HeaderStyle Width="6%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVESSELAmount" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDAMOUNT"] %>' ToolTip='<%# ((DataRowView)Container.DataItem)["FLDAMOUNT"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exchange Rate" HeaderStyle-Wrap="true" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblexchangerate" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDEXCHANGERATE"]%>' ToolTip='<%# ((DataRowView)Container.DataItem)["FLDEXCHANGERATE"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtExchangeRateEdit" runat="server" CssClass="input" DecimalPlace="17" MaxLength="25"
                                    Width="100%" Text='<%#((DataRowView)Container.DataItem)["FLDEXCHANGERATE"]%>' />
                                <telerik:RadLabel ID="lblExchangeRate" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEXCHANGERATE"]%>'></telerik:RadLabel>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank Account/ Beneficiary Name" UniqueName="Bank">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDACCOUNTNUMBER"] %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:BankAccount ID="ddlBankAccount" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    EmployeeId='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>' BankAccountList='<%#PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(((DataRowView)Container.DataItem)["FLDEMPLOYEEID"].ToString()))%>' Width="99%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" UniqueName="MonthYear">
                            <HeaderStyle Width="7%" />
                            <ItemTemplate>
                                <%#string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDMONTHYEAR"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" UniqueName="Date">
                            <HeaderStyle Width="13%" />
                            <ItemTemplate>
                                <%#string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDDATE"]) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory"
                                    Text='<%# ((DataRowView)Container.DataItem)["FLDDATE"] %>' Width="99%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" Visible="true" AlternateText="Confirm" CommandName="CONFIRM" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdConfirm"
                                    ToolTip="Confirm">
                                    <span class="icon"><i class="fas fa-award"></i></span>
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
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" OnConfirmMesage="ucConfirm_OnClick" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
