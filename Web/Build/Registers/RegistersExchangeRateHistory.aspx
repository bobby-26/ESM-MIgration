<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersExchangeRateHistory.aspx.cs"
    Inherits="RegistersExchangeRateHistory" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PageNumber" Src="~/UserControls/UserControlPageNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Exchange Rate</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersExchangeRate" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadInputManager RenderMode="Lightweight" ID="RadInputManager1" runat="server">
            <telerik:NumericTextBoxSetting Type="Currency" DecimalDigits="17" KeepTrailingZerosOnFocus="false" AllowRounding="false" BehaviorID="settings1"
                MaxValue="99999999.99999999999999999" DecimalSeparator="." ErrorMessage="????">
                <TargetControls>
                    <telerik:TargetInput ControlID="txtExchangerateEdit" Enabled="true" />
                </TargetControls>
            </telerik:NumericTextBoxSetting>
        </telerik:RadInputManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand" TabStrip="True"></eluc:TabStrip>
            <table id="tblExchangeRate" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyName" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtCurrencyName" runat="server" MaxLength="100" CssClass="input"
                            Width="236px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtCurrencyCode" runat="server" MaxLength="5" CssClass="input" Width="60px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="Modified From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtFromdate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="Modified To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtTodate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="chkActive" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuExchangeRate" runat="server" OnTabStripCommand="ExchangeRate_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="dgExchangerate" Height="72%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="dgExchangerate_ItemCommand"
                OnNeedDataSource="dgExchangerate_NeedDataSource" OnItemDataBound="dgExchangerate_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEXCHANGERATEID" TableLayout="Fixed" CommandItemDisplay="Top">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="true" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Currency" AllowSorting="true" SortExpression="FLDCURRENCYNAME">
                            <HeaderStyle Width="14.8%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadlblExchangeRateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrencyname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" UniqueName="Code" SortExpression="FLDCURRENCYCODE">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrencycode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblCurrencyId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></telerik:RadLabel>
                                <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListAllActiveCurrency(iUserCode)%>'
                                    runat="server" ActiveCurrency="true" AppendDataBoundItems="true" CssClass="gridinput_mandatory" Width="100%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="USD to Currency" AllowSorting="true" SortExpression="FLDEXCHANGERATE">
                            <HeaderStyle Width="15%" HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkexchangerate" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE","{0:f17}") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnInDirectExchangeEdit">
                                    <%--<telerik:RadNumericTextBox ID="txtExchangerateEdit" RenderMode="Lightweight" runat="server" CssClass="input_mandatory" MaxLength="25"  Width="120px" Style="text-align: right;"
                                        Type="Number" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE") %>' >
                                        <NumberFormat AllowRounding="false" DecimalSeparator="." DecimalDigits="17" KeepTrailingZerosOnFocus="true" KeepNotRoundedValue="false" />
                                    </telerik:RadNumericTextBox>--%>
                                    <%-- <eluc:Decimal ID="txtExchangerateEdit" runat="server" CssClass="input_mandatory"
                                        Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE") %>' /> --%>
                                      </span>
                                    <telerik:RadTextBox ID="txtExchangerateEdit" RenderMode="Lightweight" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE") %>'></telerik:RadTextBox>
                              
                                <telerik:RadTextBox ID="txtCurrencyid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'
                                    Visible="false">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency to USD" AllowSorting="true" SortExpression="FLDEXCHANGERATEUSD">
                            <HeaderStyle Width="15%" HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lnkexchangerateuSD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATEUSD" ,"{0:f17}") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Modified Date" AllowSorting="true" SortExpression="FLDMODIFIEDDATE" Visible="false">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkModifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Variance (%)" AllowSorting="true" SortExpression="FLDCHANGEPERCENTAGE">
                            <HeaderStyle Width="10%" HorizontalAlign="Right"/>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvarieschange" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCHANGEPERCENTAGE","{0:n2}" )%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Modified By" AllowSorting="true" SortExpression="FLDMODIFIEDUSERNAME">
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmodifiedby" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMODIFIEDUSERNAME" )%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="cmdEdit" runat="server" AlternateText="Edit"
                                    CommandName="EDIT" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdHistory" runat="server" AlternateText="History" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID")  %>'
                                    CommandName="History" ToolTip="History">
                                    <span class="icon"><i class="fas fa-history"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="cmdSave" runat="server" AlternateText="Save"
                                    CommandName="Save" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdCancel" runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
            <%--<asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
