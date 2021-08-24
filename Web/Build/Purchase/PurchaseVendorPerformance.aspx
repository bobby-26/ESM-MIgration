<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendorPerformance.aspx.cs" Inherits="Purchase_PurchaseVendorPerformance" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleetList.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <table cellpadding="2" cellspacing="2">
                <tr align="top">
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfleet" runat="server" Text="Fleet"></telerik:RadLabel>
                        <eluc:Fleet ID="ucFleet" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="170px" />
                    </td>
                    <td style="overflow-y: hidden">
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                        <telerik:RadListBox ID="ddlYear" SelectionMode="Multiple" AutoPostBack="false" AppendDataBoundItems="true" Width="150px" Height="80px" runat="server"></telerik:RadListBox>

                    </td>
                    <td style="display: inline-block;">
                        <telerik:RadLabel ID="litType" runat="server" Text="Stock Type :"></telerik:RadLabel>
                        <br />
                        <telerik:RadComboBox ID="ddlType" runat="server" AutoPostBack="false" Style="width: 80px;">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Selected="True" Text="Store"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="Spare"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="3" Text="Service"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        <br />
                        <br />
                    </td>
                    <td style="display: inline-block;">
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td style="display: inline-block;">
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorName" runat="server" Width="240px" CssClass="input"></telerik:RadTextBox>
                            <asp:TextBox ID="txtVendorId" runat="server" Width="0px" CssClass="input" BorderStyle="None"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx', true);"
                                Text=".." />
                        </span>
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsBottom" Text=".." OnClick="cmdClear_Click" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                OnItemDataBound="gvCrew_ItemDataBound" OnItemCommand="gvCrew_ItemCommand" Width="100%" Height="69%" EnableHeaderContextMenu="true" GroupingEnabled="false"
                CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvCrew_NeedDataSource"
                OnSortCommand="gvCrew_SortCommand">
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vendor" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Stock Type" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form No." HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="RFQ's Sent On" HeaderStyle-Width="13%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSENTDATE", "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="RFQ's Responded On" HeaderStyle-Width="14%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDREQUESTRESPONDDATE" , "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Issued On" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDPOISSUEDDATE" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPOISSUEDDATE" , "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Delivered On" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVENDORDELIVEREDDATE" , "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Delivery Time(Days)" HeaderStyle-Width="13%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Total(USD)" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPOAMOUNTTOTAL") %>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>


