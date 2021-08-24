<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSeniorityWageScale.aspx.cs"
    Inherits="RegistersSeniorityWageScale" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="SeniorityScale" Src="~/UserControls/UserControlSeniorityScale.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Wage Scale</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCity" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="tblConfigureCity" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureCity" width="90%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWageDescription" runat="server" Text="Wage Scale"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SeniorityScale ID="ddlSeniority" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" AutoPostBack="true" OnTextChangedEvent="ddlSeniory_Changed" Width="320px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>

                    <td>
                        <eluc:Currency ID="ddlCurrencyAdd" runat="server" OnTextChangedEvent="ddlSeniory_Changed" AutoPostBack="true" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' SelectedCurrency="10" AppendDataBoundItems="true" CssClass="input_mandatory" Width="100px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWagerank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true"
                            Width="280px" AutoPostBack="true" OnTextChangedEvent="ddlSeniory_Changed" />
                    </td>

                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersCity" runat="server" OnTabStripCommand="RegistersCity_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSWS" Height="93%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvSWS_ItemCommand" OnItemDataBound="gvSWS_ItemDataBound"
                ShowFooter="true" ShowHeader="true" OnSortCommand="gvSWS_SortCommand" EnableViewState="true" OnNeedDataSource="gvSWS_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSCALEID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" SortExpression="FLDSHORTCODE">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScaleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCALEID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCityName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblScaleIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCALEID") %>'></telerik:RadLabel>
                                <eluc:Rank ID="ddlRankEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    RankList="<%#PhoenixRegistersRank.ListRank() %>" SelectedRank='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Rank ID="ddlRankAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    RankList="<%#PhoenixRegistersRank.ListRank() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Experience in Months">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCE") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtMonthsEdit" runat="server" CssClass="input_mandatory" Width="100px" MaxLength="3"
                                    IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtMonthsAdd" runat="server" CssClass="input_mandatory" Width="100px" MaxLength="3"
                                    IsInteger="true" IsPositive="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'
                                    Width="100px" MaxLength="7" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAmountAdd" CssClass="input_mandatory" runat="server" Width="100px"
                                    MaxLength="7" />
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
