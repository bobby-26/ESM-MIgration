<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReconcilationStagingInvoiceDiff.aspx.cs"
    Inherits="AccountsReconcilationStagingInvoiceDiff" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagName="UserControlCurrency" TagPrefix="eluc" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body style="margin: 0; padding: 0px; text-align: center;">
    <form id="frmPurchaseSatging" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="TabStripTitle" runat="server"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <br />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblpnlLineItemEntryBalanceAmount" runat="server" Text="Balance Amount:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBalanceAmount" runat="server" Font-Bold="true"></telerik:RadLabel>
                        <%--<telerik:RadTextBox ID="txtBalanceAmount" runat="server" CssClass="input"></telerik:RadTextBox>--%>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuRegistersStockItem" runat="server" OnTabStripCommand="RegistersStockItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOrderLine" Height="50%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvOrderLine_ItemCommand" OnItemDataBound="gvOrderLine_ItemDataBound" OnNeedDataSource="gvOrderLine_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" OnSortCommand="gvOrderLine_SortCommand" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=">Part Number" Visible="false" AllowSorting="true" SortExpression="FLDPARTNUMBER">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="35%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderLineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPartNumber" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="true" SortExpression="FLDSERIALNO">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="5%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadLabel4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Line Item" AllowSorting="true" SortExpression="FLDPARTNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lblSerialNo" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtTotalPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Difference">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<telerik:RadTextBox ID="TextBox1" runat="server" MaxLength="22" Width="120px" CssClass="input_mandatory" Style="text-align: right;"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNT","{0:n2}") %>'></telerik:RadTextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtInvoiceEdit" AcceptNegative="Left"
                                    Mask="9999999.99" MaskType="Number" InputDirection="RightToLeft">
                                </ajaxToolkit:MaskedEditExtender>--%>
                                <eluc:Number ID="txtInvoiceEdit" runat="server" Width="90px" CssClass="input_mandatory"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNT","{0:n2}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtFinalPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINALAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Difference Remarks">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtOrderLineRemarksEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEREMARKS") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img5" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdUpdate"
                                    ToolTip="Update"></asp:ImageButton>
                                <img id="Img6" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
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
            <br>
            <table>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblAdditionalChargeItem" runat="server" Text="Additional Charge Item:"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="AdditionalChargeItem" runat="server" OnTabStripCommand="AdditionalChargeItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTax" Height="50%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvTax_ItemCommand" OnItemDataBound="gvTax_ItemDataBound" OnNeedDataSource="gvTax_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="Charge Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="35%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdditionalDiscountYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDITIONALDISCOUNTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCRIPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsGst" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISGST") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="35%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderTaxCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERTAXCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOriginalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Difference">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTaxInvoice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--asp:TextBox ID="txtTaxInvoiceEdit" runat="server" MaxLength="22" Width="120px" CssClass="input_mandatory" Style="text-align: right;"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNT","{0:n2}") %>'>
                                </asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="txtTaxInvoiceEditMask" runat="server" TargetControlID="txtTaxInvoiceEdit" AcceptNegative="Left"
                                    Mask="9999999.99" MaskType="Number" InputDirection="RightToLeft">
                                </ajaxToolkit:MaskedEditExtender> --%>
                                <eluc:Number ID="txtTaxInvoiceEdit" runat="server" Width="90px" CssClass="input_mandatory"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNT","{0:n2}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTaxTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINALAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Difference Remarks">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="35%" />
                            <ItemTemplate>
                                <%--<telerik:RadLabel ID="lblAdditionalDiscountYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDITIONALDISCOUNTYN") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsGst" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISGST") %>'></telerik:RadLabel>--%>
                                <telerik:RadLabel ID="lblInvoiceRemarkEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarksEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEREMARKS") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Visible="false"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Save" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
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
