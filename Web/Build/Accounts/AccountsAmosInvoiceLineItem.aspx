<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAmosInvoiceLineItem.aspx.cs"
    Inherits="AccountsAmosInvoiceLineItem" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
         <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="Title" runat="server" TabStrip="true"></eluc:TabStrip>
        <table cellpadding="2" cellspacing="1" style="width: 100%">
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" MaxLength="25" ReadOnly="true"
                        CssClass="readonlytextbox" Width="150px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="60px" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" Width="180px" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInvoiceReference" runat="server" Text="Invoice Reference"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSupplierRefEdit" runat="server" CssClass="readonlytextbox" MaxLength="25"
                        ReadOnly="true" Width="240px">
                    </telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtCurrencyId" runat="server" CssClass="readonlytextbox" MaxLength="25"
                        ReadOnly="true" Width="240px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text="Currency Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCurrencyName" runat="server" CssClass="readonlytextbox" MaxLength="25"
                        ReadOnly="true" Width="240px">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <hr />
        <table cellpadding="1" cellspacing="1" style="width: 100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" CssClass="input" AppendDataBoundItems="true" VesselsOnly="true"
                        OnTextChangedEvent="Invoice_SetVessel" AutoPostBack="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPONumber" runat="server" Text="PO Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPOnumber" runat="server" CssClass="input" MaxLength="50" Width="120px"></telerik:RadTextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                </td>
            </tr>
        </table>
        <br />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvInvoice" Height="53%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnItemCommand="gvInvoice_ItemCommand" OnItemDataBound="gvInvoice_ItemDataBound" OnNeedDataSource="gvInvoice_NeedDataSource"
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
                    <telerik:GridTemplateColumn HeaderText=" Purchase Order No." AllowSorting="true" SortExpression="FLDSTATENAME">
                        <HeaderStyle Width="35%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkInvoiceLineItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'
                                CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' OnClick="InvoiceLineItemSelected"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="true" SortExpression="FLDSTATENAME">
                        <HeaderStyle Width="35%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PO Amount" AllowSorting="true" SortExpression="FLDSTATENAME">
                        <HeaderStyle Width="35%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPurPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALTOTAL","{0:n2}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PO Advance Amount" AllowSorting="true" SortExpression="FLDSTATENAME">
                        <HeaderStyle Width="35%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPurAdvanceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTPAYMENT","{0:n2}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PO Advance Amount" AllowSorting="true" SortExpression="FLDSTATENAME">
                        <HeaderStyle Width="35%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString().Length>100 ? DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString().Substring(0, 100) + "..." : DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' />
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
