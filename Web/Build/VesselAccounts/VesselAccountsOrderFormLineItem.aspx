<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOrderFormLineItem.aspx.cs"
    Inherits="VesselAccountsOrderFormLineItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
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
    <title>Crew Vessel Sign-On</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmBondReq" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmBondReq" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="llbvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtvessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrderNo" runat="server" Text="Order No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"
                            Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrderDate" runat="server" Text="Order Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtOrderDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuStoreItem" runat="server" OnTabStripCommand="MenuStoreItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" Height="90%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewSearch_ItemCommand" OnItemDataBound="gvCrewSearch_ItemDataBound" EnableHeaderContextMenu="true" EnableViewState="false" OnPreRender="gvCrewSearch_PreRender" OnEditCommand="gvCrewSearch_EditCommand" ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvCrewSearch_NeedDataSource" OnUpdateCommand="gvCrewSearch_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Number" UniqueName="ORDERLINE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblorderlineid" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDORDERLINEID"] %>'></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDNUMBER"] %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr>
                                        <td valign="middle" align="right" style="width: 90%">
                                            <telerik:RadLabel ID="lblTotalAmountasperInvoice" runat="server" Font-Bold="true" Text="Total Amount as per Invoice (Local Currency)"></telerik:RadLabel>
                                        </td>
                                        <td valign="middle"  align="right" style="width: 10%">
                                            <telerik:RadLabel ID="lbltotamtinvoice" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="right">
                                            <telerik:RadLabel ID="lblLessDiscountLocalCurrency" runat="server" Font-Bold="true" Text="Less: Discount (Local Currency)"></telerik:RadLabel>
                                        </td>
                                        <td  valign="middle" align="right">
                                            <telerik:RadLabel ID="lblLessDiscountLocalCurValue" runat="server" Font-Bold="true" Text=""></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="right">
                                            <telerik:RadLabel ID="lblCharges" runat="server" Text="Add: Delivery Charges(After Discount)" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                        <td  valign="middle" align="right">
                                            <telerik:RadLabel ID="lblchargesValue" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="right">
                                            <telerik:RadLabel ID="lblNetAmountLocalCurrency" runat="server" Text="Net Amount (Local Currency)" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                        <td  valign="middle" align="right">
                                            <telerik:RadLabel ID="lblNetAmtLocalCurValue" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="right">
                                            <telerik:RadLabel ID="lblNetAmountUSD" runat="server" Text="Net Amount" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                        <td  valign="middle" align="right">
                                            <telerik:RadLabel ID="lblnetAmountValue" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" UniqueName="ORDERLINE">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDNAME"] %>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="ORDERLINE">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDUNITNAME"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ordered Quantity" UniqueName="ORDERLINE">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDORDEREDQUANTITY"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="ORDERLINE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDQUANTITY"] %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="lblQuantity" runat="server" Visible="false" CssClass="input_mandatory"
                                    Width="90px" Text='<%#((DataRowView)Container.DataItem)["FLDQUANTITY"] %>' />
                                <eluc:Number ID="txtQuantity" runat="server" CssClass="input_mandatory" Width="90px"
                                    Text='<%#((DataRowView)Container.DataItem)["FLDQUANTITY"] %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit Price" UniqueName="ORDERLINE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDUNITPRICE"] %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="lblUnitPrice" runat="server" Visible="false" CssClass="input"
                                    Width="90px" Text='<%#((DataRowView)Container.DataItem)["FLDUNITPRICE"] %>' />
                                <eluc:Number ID="txtUnitPrice" runat="server" CssClass="input" Width="90px"
                                    Text='<%#((DataRowView)Container.DataItem)["FLDUNITPRICE"] %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Price" UniqueName="ORDERLINE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDTOTALPRICE"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem%>" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdUpdate"
                                    ToolTip="Update"><span class="icon"><i class="fas fa-save"></i></span>
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
                <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <KeyboardNavigationSettings EnableKeyboardShortcuts="true" AllowSubmitOnEnter="true"
                        AllowActiveRowCycle="true" MoveDownKey="DownArrow" MoveUpKey="UpArrow"></KeyboardNavigationSettings>

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
