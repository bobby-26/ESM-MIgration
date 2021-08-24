<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSundryPurchaseLineItem.aspx.cs" Inherits="AccountsSundryPurchaseLineItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Accounts Sundry Purchase LineItem</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


            <eluc:TabStrip ID="menulineitem" runat="server"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuStoreItem" runat="server" OnTabStripCommand="MenuStoreItem_TabStripCommand"></eluc:TabStrip>

            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvCrewSearch" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvCrewSearch_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false"
                OnItemDataBound="gvCrewSearch_ItemDataBound" OnItemCommand="gvCrewSearch_ItemCommand"
                ShowFooter="true" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERLINEID">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="102px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblorderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDNUMBER"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="370px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDNAME"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit" HeaderStyle-Width="102px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDUNITNAME"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity" HeaderStyle-Width="102px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDQUANTITY"] %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtQuantity" runat="server" CssClass="input_mandatory" Width="90px" Text='<%#((DataRowView)Container.DataItem)["FLDQUANTITY"] %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit Price" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDUNITPRICE"] %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtUnitPrice" runat="server" CssClass="input_mandatory" Width="90px" Text='<%#((DataRowView)Container.DataItem)["FLDUNITPRICE"] %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotalAmountasperInvoiceLocalCurrency" runat="server" Text="Total Amount as per Invoice (Local Currency)"></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lblLessDiscountLocalCurrency" runat="server" Text="Less: Discount (Local Currency)"></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lblNetAmountLocalCurrency" runat="server" Text="Net Amount (Local Currency)"></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lblNetAmountUSD" runat="server" Text="Net Amount (USD)"></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Price" HeaderStyle-Width="102px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDTOTALPRICE"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="102px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>


