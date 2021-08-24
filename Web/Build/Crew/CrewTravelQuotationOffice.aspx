<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuotationOffice.aspx.cs"
    Inherits="CrewTravelQuotationOffice" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register Src="../UserControls/UserControlCurrency.ascx" TagName="UserControlCurrency" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel Request Quotation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body style="margin: 0; padding: 0px; text-align: center;">
    <form id="frmCrewTravelQuotationItems" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />       
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="60%">
             <eluc:TabStrip ID="MenuTitle" runat="server"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button runat="server" ID="cmdHiddenSubmitclose" OnClick="cmdHiddenSubmitclose_Click" />
            <telerik:RadLabel ID="lblNote" runat="server" Visible="false"></telerik:RadLabel>
            <iframe runat="server" id="ifMoreInfo" scrolling="auto" frameborder="0" style="min-height: 460px; width: 100%"></iframe>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvAgent" runat="server" EnableViewState="false" ShowFooter="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvAgent_NeedDataSource" OnItemCommand="gvAgent_ItemCommand" OnItemDataBound="gvAgent_ItemDataBound"
               EnableHeaderContextMenu="true"  AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblQuotationNoHeader" CommandArgument="FLDQUOTATIONNO" runat="server"
                                    CommandName="Sort">
                                    Quotation Number
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuotationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAgentID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblfinalizeyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINALIZEDYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblconfirmedyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lblQuotationNo" CommandName="Select" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONREFNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblQuotationCurrencyHeader" CommandArgument="FLDCURRENCYID" runat="server"
                                    CommandName="Sort">
                                    Currency
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="ucCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblStatusHeader" runat="server">Status</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblconfirmStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIRMEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lnkAmountHeader" CommandArgument="FLDAMOUNT" runat="server" CommandName="Sort" >
                                    Fare
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lnkTaxHeader" CommandArgument="FLDTAX" runat="server" CommandName="Sort">
                                    Tax
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTotalAmountHeader" runat="server">Total Amount</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblQuotedStatusHeader" runat="server">Quoted Status</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblMarineNonMarineHeader" runat="server">Marine/Non Marine
                                    <br />
                                    Fare</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmarinefare" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMARINEFAREYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Chat" ToolTip="Chat" Width="20PX" Height="20PX"
                                    CommandName="CHAT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdChat">
                                <span class="icon"><i class="fas fa-comments"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
