<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectLineItemPurchaseOrderAdd.aspx.cs" Inherits="Accounts_AccountsProjectLineItemPurchaseOrderAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Order Detail</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPO" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadLabel ID="lblPurchaseOrderDetails" runat="server" Text="Purchase Order Details" Visible="false"></telerik:RadLabel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table cellpadding="2" cellspacing="1" style="width: 100%">
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblPoType" runat="server" Text="PO Type"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <telerik:RadDropDownList ID="ddlPOType" runat="server" CssClass="input"
                        OnDataBound="ddlPOType_DataBound" DataTextField="FLDTYPENAME"
                        AutoPostBack="true" DataValueField="FLDTYPEID">
                    </telerik:RadDropDownList>
                </td>
                <td width="15%">
                    <telerik:RadLabel ID="lblCreatedDate" runat="server" Text="Created From Date"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <eluc:Date ID="txtCreatedDate" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblPONumber" runat="server" Text="PO Number"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <telerik:RadTextBox ID="txtPoNumber" runat="server" CssClass="input">
                    </telerik:RadTextBox>
                </td>
                <td width="15%">
                    <telerik:RadLabel ID="lblPODescription" runat="server" Text="PO Description"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <telerik:RadTextBox ID="txtPoDescription" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvGrid" runat="server" AutoGenerateColumns="False" CellPadding="3" AllowPaging="true" AllowCustomPaging="true"
            Height="80%" OnItemCommand="gvGrid_ItemCommand" EnableViewState="False" EnableHeaderContextMenu="true" GroupingEnabled="false"
            Font-Size="11px" ShowFooter="False" Width="100%" OnNeedDataSource="gvGrid_NeedDataSource">
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
                    <telerik:GridTemplateColumn HeaderText="Order Date" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCommittedcostbreakupid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDCOSTBREAKUPID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PO Number" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PO Type" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPOType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPODESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center" HeaderText="Action"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                        <HeaderStyle />
                        <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                        <ItemTemplate>
                            <img id="Img7" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/add.png %>"
                                CommandName="ADD" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                ToolTip="Add"></asp:ImageButton>
                            <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        </ItemTemplate>
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
    </form>
</body>
</html>
