<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsFXContractAssignment.aspx.cs"
    Inherits="AccountsFXContractAssignment" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFXContractAssignment.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="Form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlStockItemEntry" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <%--                        <eluc:Title runat="server" ID="Title1" Text="FX Contract"></eluc:Title>--%>
            <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFXContractAssignment" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowPaging="true" AllowCustomPaging="true" EnableHeaderContextMenu="true"
                AllowSorting="true" OnNeedDataSource="gvFXContractAssignment_NeedDataSource" GroupingEnabled="false" Height="94%"
                OnItemCommand="gvFXContractAssignment_RowCommand"
                OnItemDataBound="gvFXContractAssignment_RowDataBound">
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
                        <telerik:GridTemplateColumn HeaderText="Bank Account">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBankAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank Account Currency">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBankAccountCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Currency">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTCURRENCYCODE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALPAYABLEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Contract Applicability">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblContractType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ddlContractType" runat="server" CssClass="dropdown_mandatory" HardTypeCode="137"
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 137) %>' AppendDataBoundItems="true"
                                    SelectedHard='<%#DataBinder.Eval(Container, "DataItem.FLDFXCONTRACTTYPE")%>'
                                    Width="300px" />
                                <telerik:RadLabel ID="lblBankaccountid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRemittancecurrencyid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="FX Contract Ref">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFXContractRef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFXCONTRACTREF") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtFXContractRefEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFXCONTRACTREF") %>'
                                    CssClass="input_mandatory" MaxLength="25" Width="240px">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exchange Rate">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExchangeRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFXEXCHANGERATE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtExchangeRateEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFXEXCHANGERATE","{0:n2}") %>'
                                    Wrap="False" Width="150px">
                                </telerik:RadTextBox>
                                <%--                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExchangeRate" runat="server" AutoComplete="true"
                                        InputDirection="RightToLeft" Mask="9,999.999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                        TargetControlID="txtExchangeRateEdit" />--%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center" HeaderText="Action">
                            <HeaderStyle />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="SAVE" CommandArgument='<%# Container.DataItem %>' ID="cmdUpdate"
                                    ToolTip="Update"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItem %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
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

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
