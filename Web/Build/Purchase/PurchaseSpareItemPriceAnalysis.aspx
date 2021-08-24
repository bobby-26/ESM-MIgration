<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSpareItemPriceAnalysis.aspx.cs" Inherits="Purchase_PurchaseSpareItemPriceAnalysis" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnVendorKeyPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlSpareItem" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--            <eluc:Title runat="server" ID="pageTitle" Text="Spare Item"></eluc:Title>--%>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>


            <table id="tblSpare" width="100%">

                <tr>

                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPartNumber" runat="server" BorderWidth="1px" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPartName" runat="server" BorderWidth="1px" Width="210px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>

                    </td>
                    <td>
                        <span id="spnPickComponent">
                            <telerik:RadTextBox ID="txtComponent" runat="server" Width="80px" Enabled="true" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" Width="160px" Enabled="true" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="cmdShowComponent" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickComponent', 'codehelp1', '', '../Common/CommonPickListComponentPurchase.aspx', true);"
                                Text=".." />
                            <telerik:RadTextBox ID="txtComponentID" runat="server" Width="0px" />
                        </span>
                    </td>
                </tr>

            </table>
            <eluc:TabStrip ID="SpareItemMenu" runat="server" OnTabStripCommand="SpareItem_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvSpare" runat="server" AutoGenerateColumns="False" Font-Size="11px" Height="76%" OnNeedDataSource="gvSpare_NeedDataSource"
                OnItemDataBound="gvSpare_RowDataBound" OnItemCommand="gvSpare_RowCommand" Width="100%" AllowPaging="true" AllowCustomPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                OnSortCommand="gvSpare_Sorting">
                  <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
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

                        <telerik:GridTemplateColumn HeaderText="Number" AllowSorting="true" SortExpression="FLDITEMNUMBER">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDITEMNUMBER") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDITEMNAME">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDITEMNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Component" AllowSorting="true" SortExpression="FLDCOMPONENTNAME">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vendor" AllowSorting="true" SortExpression="FLDVENDORNAME">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Port" AllowSorting="true" SortExpression="FLDPORT">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPORT") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Quantity" AllowSorting="true" SortExpression="FLDQUANTITY">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Unit" AllowSorting="true" SortExpression="FLDUNITNAME">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Price(USD)" AllowSorting="true" SortExpression="">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSINGLEUNITPRICE", "{0:n}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Price(USD)" AllowSorting="true" SortExpression="TOTALPRICE">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.TOTALPRICE","{0:n}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Of Purchase" AllowSorting="true" SortExpression="FLDDATEOFPURCHASE">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDATEOFPURCHASE" , "{0:dd-MM-yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <%--        <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAction" runat="server">Action</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
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
