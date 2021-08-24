<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelAgentInvoiceListInOffice.aspx.cs"
    Inherits="CrewTravelAgentInvoiceListInOffice" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Travel Invoice Office</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewTravelInvoiceOffice" runat="server">
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
                        <eluc:Vessel ID="ddlvessel" runat="server" AppendDataBoundItems="true" Width="40%"
                            AssignedVessels="true" Entitytype="VSL" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Passenger"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Width="40%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTicket" runat="server" Text="Ticket No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTicket" runat="server" Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepDate" runat="server" Text="Departure"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDepDate" runat="server" Width="40%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucInvoiceStatus" runat="server" AppendDataBoundItems="true" Width="40%"
                            HardList="<%# PhoenixRegistersHard.ListHard(1,200) %>" HardTypeCode="200" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceDateBetween" runat="server" Text="Invoice Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtstartdate" runat="server" CssClass="input_mandatory" Width="35%" />
                        &nbsp; And &nbsp;
                    <eluc:Date ID="txtenddate" runat="server" CssClass="input_mandatory" Width="35%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAgent" runat="server" Text="Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListAgent">
                            <telerik:RadTextBox ID="txtAgentName" runat="server" Width="30%" CssClass="input_mandatory"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAgentNumber" runat="server" Width="45%" CssClass="input_mandatory"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                ID="cmdShowAgent" ToolTip="Select Agent">                                
                                <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtAgentId" runat="server" Width="1px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenutravelList" runat="server" OnTabStripCommand="MenutravelList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCT" runat="server" AutoGenerateColumns="False" Height="82%" EnableViewState="false"
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
                    <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDAGENTINVOICEID" DetailKeyField="FLDAGENTINVOICEID" />
                        </ParentTableRelation>
                    </NestedViewSettings>
                    <NestedViewTemplate>
                        <table style="font-size: 11px; width: 60%">
                            <tr>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Imported On :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblimporteddate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></telerik:RadLabel>
                                </td>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="Paid Date :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPaidDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDPAIDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NestedViewTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Date (DD/MM/YYYY)" Name="Date" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdFilter" ToolTip="Filter"
                                    CommandName="INVOICESEARCH" CommandArgument="<%# Container.DataSetIndex %>" Width="15px" Height="15px">
                                 <span class="icon" id="imgfilter"><i class="fas fa-search"></i></span>      
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdClearFilter" Visible="false" ToolTip="Clear Filter"
                                    CommandName="CLEARFILTER" CommandArgument="<%# Container.DataSetIndex %>" Width="15px" Height="15px">
                                 <span class="icon" id="imgclear"><i class="fa fa-ban fa-eraser"></i></span>      
                                </asp:LinkButton>
                                <telerik:RadLabel ID="lblAgentInvoiceid" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAGENTINVOICEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice No." AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="105px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice" ColumnGroupName="Date" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="81px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure" ColumnGroupName="Date" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="81px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Agent" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAgentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
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
                        <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="62px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChargers" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="92px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblstatus" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voucher No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPaymentVoucher" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action" HeaderStyle-Width="82px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Report"
                                    CommandName="REPORT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdReport"
                                    ToolTip="Report">
                                <span class="icon"><i class="fas fa-file"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdedit"
                                    ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Repost"
                                    CommandName="Repost" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRepost"
                                    ToolTip="Repost">
                                <span class="icon"><i class="fas fa-redo"></i></span>
                                </asp:LinkButton>
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
