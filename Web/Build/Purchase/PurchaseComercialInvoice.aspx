<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseComercialInvoice.aspx.cs" Inherits="PurchaseComercialInvoice" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Commercial Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= rgvForm.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="panel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" border="0" cellpadding="10">
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlFormNo" Text="Inv Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtFormNo" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlCompany" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="ltrlConsigny" runat="server" Text="Consigny"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiAddress runat="server" ID="ucAddrAgent"
                            Width="80%" />
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlFromDate" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="ltrlToDate" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuNewRequisition" runat="server" OnTabStripCommand="MenuNewRequisition_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="rgvForm" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="false" OnDeleteCommand="rgvForm_DeleteCommand"
                CellSpacing="0" GridLines="None" OnNeedDataSource="rgvForm_NeedDataSource" OnItemDataBound="rgvForm_ItemDataBound" OnSortCommand="rgvForm_SortCommand" OnItemCommand="rgvForm_ItemCommand" ShowFooter="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCOMRTIALINVOICEID" TableLayout="Fixed">
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Invoive Number" UniqueName="NUMBER" AllowSorting="true" SortExpression="FLDINVOICENUMBER">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton RenderMode="Lightweight" ID="lblInvoiceNo" runat="server" CommandArgument="INVNUMBER" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></asp:LinkButton>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblInoviceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMRTIALINVOICEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Invoice Date" UniqueName="Date" AllowSorting="true" SortExpression="FLDINVOICEDATE">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblFormTitle" runat="server"
                                    Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDINVOICEDATE").ToString())%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Company" UniqueName="FLDCOMPANYNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Consigny" UniqueName="FLDCONSIGNY">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblConsignyName" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDCONSIGNY").ToString()%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="ACTION" HeaderText="Action">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" CommandName="EDITINV" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCOMRTIALINVOICEID") %>' ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    ID="cmdEdit" ToolTip="Edit"></asp:ImageButton>
                                <asp:LinkButton runat="server" AlternateText="PDF" CommandName="PDF" ID="cmdPDF" ToolTip="Export PDF">
                                    <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                </asp:LinkButton>
                                <asp:ImageButton runat="server" AlternateText="Delete" CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCOMRTIALINVOICEID") %>' ImageUrl="<%$ PhoenixTheme:images/Delete.png %>"
                                    ID="cmdDelete" ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                        PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="false" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

