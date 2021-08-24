<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelAgentInvoiceExceptionList.aspx.cs"
    Inherits="CrewTravelAgentInvoiceExceptionList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Invoice Exception List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewChangeTravel" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table id="tbl1" cellpadding="1" cellspacing="2" width="90%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtvessel" runat="server" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="Invoice From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtstartdate" runat="server" Width="80%"/>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="Invoice To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtenddate" runat="server" Width="80%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRequestNo" runat="server" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassengerName" runat="server" Text="Passenger Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassengerName" runat="server" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTicketNo" runat="server" Text="Ticket Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTicketNo" runat="server" Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowArchievedInvoice" runat="server" Text="Show Archive Invoice"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadCheckBox ID="chkarchievedyn" runat="server" AutoPostBack="true" OnCheckedChanged="chkarchievedyn_OnCheckedChanged" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenutravelList" runat="server" OnTabStripCommand="MenutravelList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCT" runat="server" AutoGenerateColumns="False" Height="85%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="true"
                OnNeedDataSource="gvCCT_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvCCT_ItemCommand"
                OnItemDataBound="gvCCT_ItemDataBound">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Date (DD/MM/YYYY)" Name="Date" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Invoice No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblagentinvoiceid" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAGENTINVOICEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblarchievedyn" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDISARCHIVEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice" ColumnGroupName="Date" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure" ColumnGroupName="Date" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PNR No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPNR" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passenger" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselname" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTicket" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'
                                    CommandArgument="<%# Container.DataSetIndex %>"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Charges" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChargers" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true">                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblstatus" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Imported On" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblimporteddate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
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
