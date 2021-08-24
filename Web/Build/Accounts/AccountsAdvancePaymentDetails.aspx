<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdvancePaymentDetails.aspx.cs" Inherits="Accounts_AccountsAdvancePaymentDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PO AdvancePaymentDetails</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .style1 {
                width: 127px;
            }

            .style2 {
                width: 172px;
            }

            .style3 {
                width: 130px;
            }

            .style4 {
                width: 102px;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAdvancePaymentBreakUps" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--<eluc:Title runat="server" ID="ucTitle" Text="PO AdvancePaymentDetails" ShowMenu="false"></eluc:Title>--%>
            <br />
            <table>
                <tr>
                    <td class="style4">
                        <telerik:RadLabel ID="lblPONumber" runat="server" Text="PO Number :"></telerik:RadLabel>
                    </td>
                    <td class="style1">
                        <telerik:RadTextBox ID="txtPoNumber" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td class="style2">
                        <telerik:RadLabel ID="lblAdvancePaymentNumber" runat="server" Text="Advance Payment Number"></telerik:RadLabel>
                    </td>
                    <td class="style3">
                        <telerik:RadTextBox ID="txtAdvancePaymentNumber" runat="server" CssClass="input"
                            MaxLength="20" Width="187px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuAdvanceItem" runat="server" OnTabStripCommand="MenuAdvanceItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAdvancePayment" Height="82%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvAdvancePayment_ItemCommand" OnItemDataBound="gvAdvancePayment_ItemDataBound" OnNeedDataSource="gvAdvancePayment_NeedDataSource"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="Advance Payment Number">
                            <HeaderStyle Width="60%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%--<asp:Label ID="lblPurPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPAYABLEAMOUNT","{0:n2}") %>'></asp:Label>--%>
                                <%--<asp:LinkButton ID="lnkPurPayableAmount" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPAYABLEAMOUNT","{0:n2}") %>'></asp:LinkButton>
                                    <asp:Label ID="lblInvoicePayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEPAYABLEAMOUNT","{0:n2}") %>' Visible ="false" ></asp:Label>--%>
                                <telerik:RadLabel ID="lblAdvancePaymentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYMENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotal" runat="server" Text="Total"></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" Visible="false">
                            <HeaderStyle Width="" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                <%--<asp:TextBox ID="txtInvoiceLineItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICELINEITEMCODE") %>'
                                        MaxLength="10" Visible="false"></asp:TextBox>
                                    <asp:Label ID="lblStockType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></asp:Label>
                                    <asp:TextBox ID="txtOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                        MaxLength="10" Visible="false"></asp:TextBox>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPayDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                <%--<asp:TextBox ID="txtInvoiceLineItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICELINEITEMCODE") %>'
                                        MaxLength="10" Visible="false"></asp:TextBox>
                                    <asp:Label ID="lblStockType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></asp:Label>
                                    <asp:TextBox ID="txtOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                        MaxLength="10" Visible="false"></asp:TextBox>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text="lbl"></telerik:RadLabel>
                            </FooterTemplate>
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
