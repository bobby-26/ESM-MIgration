<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectLineItemConfirmationAdd.aspx.cs" Inherits="Accounts_AccountsProjectLineItemConfirmationAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Awaiting Confirmation Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmConfirmation" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table cellpadding="2" cellspacing="1" style="width: 100%">
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="ltType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <telerik:RadDropDownList ID="ddlType" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="DisableType">
                        <Items>
                            <telerik:DropDownListItem Value="" Text="--Select--"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Value="1" Text="Purchase Form"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Value="2" Text="Direct PO"></telerik:DropDownListItem>
                        </Items>
                    </telerik:RadDropDownList>
                </td>
                <td width="15%">
                    <telerik:RadLabel ID="lblFormType" runat="server" Text="Form Type"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <eluc:Hard ID="ucFormType" AppendDataBoundItems="true" CssClass="input" Enabled="false" AutoPostBack="true" runat="server" />
                </td>
            </tr>
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblFormNumber" runat="server" Text="Form Number"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <telerik:RadTextBox ID="txtFormNumber" runat="server" CssClass="input">
                    </telerik:RadTextBox>
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
                    <telerik:RadLabel ID="lblFormDescription" runat="server" Text="Form Description"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <telerik:RadTextBox ID="txtFormDescription" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvGrid" runat="server" AutoGenerateColumns="False" CellPadding="3" AllowPaging="true" AllowCustomPaging="true"
            Height="74%" OnItemCommand="gvGrid_ItemCommand" EnableViewState="False" EnableHeaderContextMenu="true" GroupingEnabled="false"
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
                    <telerik:GridTemplateColumn HeaderText="Order Date">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOrderDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblorderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Form Number">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFormNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Form Type">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFormType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Requisition Status">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFormStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSTATUS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALTOTAL") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Description">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center" HeaderText="Action">
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
