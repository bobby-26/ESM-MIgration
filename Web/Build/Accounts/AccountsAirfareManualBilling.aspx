<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfareManualBilling.aspx.cs" Inherits="Accounts_AccountsAirfareManualBilling" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AIRFARE MANUAL BILLING</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAirfareManualBilling" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Airfare Manual Billing" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuAirfareBillingMain" runat="server" OnTabStripCommand="MenuAirfareBillingMain_TabStripCommand" Title="Airfare Manual Billing"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuAirfareBilling" runat="server" OnTabStripCommand="MenuAirfareBilling_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table id="tblAirfare" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAgent" runat="server" Text="Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtSupplierCode" runat="server" CssClass="input" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSupplierName" runat="server" CssClass="input" ReadOnly="false"
                                Width="180px">
                            </telerik:RadTextBox>
                            <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtAgentId" runat="server"></telerik:RadTextBox>
                        </span>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVessel">
                            <telerik:RadTextBox ID="txtVesselId" runat="server" CssClass="input" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="input" ReadOnly="false"
                                Width="180px">
                            </telerik:RadTextBox>
                            <img id="imgShowVessel" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTicketNo" runat="server" Text="Ticket Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTicketNo" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuAirfareBillinglist" runat="server" OnTabStripCommand="MenuAirfareBillinglist_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAirfareBilling" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Height="68%" Width="100%" CellPadding="3" OnItemCommand="gvAirfareBilling_ItemCommand" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnNeedDataSource="gvAirfareBilling_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true">
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
                        <telerik:GridTemplateColumn HeaderText="Invoice Number" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblInvoiceNumberHeader" Visible="true" runat="server">
                                    <asp:ImageButton runat="server" ID="cmdInvoiceNumber" OnClick="cmdSearch_Click" CommandName="FLDINVOICENUMBER"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkInvoiceNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDINVOICENUMBER">Invoice Number&nbsp;</asp:LinkButton>
                                <img id="FLDINVOICENUMBER" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAgentInvoiceNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTINVOICEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInvoiceNumber" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Date" HeaderStyle-Width="9%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Agent Name" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAgentName" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passenger Name" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPassengerNameHeader" Visible="true" runat="server">
                                    <asp:ImageButton runat="server" ID="cmdPassengerName" OnClick="cmdSearch_Click" CommandName="FLDPASSENGERNAME"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkPassengerNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDPASSENGERNAME">Passenger Name&nbsp;</asp:LinkButton>
                                <img id="FLDPASSENGERNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassengerName" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="9%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="12%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVesselHeader" Visible="true" runat="server">
                                    <asp:ImageButton runat="server" ID="cmdVessel" OnClick="cmdSearch_Click" CommandName="FLDVESSELNAME"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME">Vessel&nbsp;</asp:LinkButton>
                                <img id="FLDCOURSENAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVessel" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket No" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketNo" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="8%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Bill" ImageUrl="<%$ PhoenixTheme:images/priority_invoice.png %>"
                                    CommandName="Bill" CommandArgument='<%# Container.DataItem %>' ID="cmdBill"
                                    ToolTip="Bill"></asp:ImageButton>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
