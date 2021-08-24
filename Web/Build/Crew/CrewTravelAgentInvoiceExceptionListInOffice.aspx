<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelAgentInvoiceExceptionListInOffice.aspx.cs"
    Inherits="CrewTravelAgentInvoiceExceptionListInOffice" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Travel Invoice Office</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            document.onkeydown = function (e) {
                var keyCode = (e) ? e.which : event.keyCode;
                if (keyCode == 13) {
                    WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('MenutravelList$dlstTabs$ctl02$btnMenu', '', false, '', 'CrewTravelAgentInvoiceExceptionListInOffice.aspx', false, true));
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewTravelInvoiceExceptionInOffice" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="85%">
            <eluc:TabStrip ID="MenuTitle" runat="server" Title="Travel Invoice Exception List"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table id="tbl1" cellpadding="1" cellspacing="2" width="90%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" Text="Vessel" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlvessel" runat="server"  AppendDataBoundItems="true"
                            AssignedVessels="true" Entitytype="VSL" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" Text="Name" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAgent" runat="server" Text="Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListAgent">
                            <telerik:RadTextBox ID="txtAgentName" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAgentNumber" runat="server" Width="240px" CssClass="input"></telerik:RadTextBox>
                            <img runat="server" id="cmdShowAgent" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListAgent', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=135&CalledFrom=travelinvoiceexception', true); " />
                            <telerik:RadTextBox ID="txtAgentId" runat="server" CssClass="input" Width="1px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoidceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceDateBetween" runat="server" Text="Invoice Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtstartdate" runat="server" CssClass="input_mandatory" />
                        &nbsp; to &nbsp;
                    <eluc:Date ID="txtenddate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTicketno" runat="server" Text="Ticket No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTicketNo" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowArchievedInvoice" runat="server" Text="Show Archived Invoice"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkarchievedyn" runat="server" AutoPostBack="true" OnCheckedChanged="chkarchievedyn_OnCheckedChanged" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenutravelList" runat="server" OnTabStripCommand="MenutravelList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCT" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCCT_ItemCommand" OnNeedDataSource="gvCCT_NeedDataSource" Height="75%"
                OnItemDataBound="gvCCT_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblInvoiceNumberHeader" runat="server">Invoice Number</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblagentinvoiceid" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAGENTINVOICEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblarchievedyn" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDISARCHIVEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblInvoiceDateHeader" runat="server">Invoice Date</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvDate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPNRHeader" runat="server">PNR</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPNR" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPassengerNameHeader" runat="server">Passenger Name</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepDateHeader" runat="server">Dep Date</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepDate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVesselHeader" runat="server">Vessel</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselname" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTicketHeader" runat="server">Ticket</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTicket" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTotalChargesHeader" runat="server">Total Charges</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChargers" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblImportedDateHeader" runat="server">Imported Date</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblimpoteddate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Archive" ToolTip="Archive" Width="20PX" Height="20PX"
                                    CommandName="ARCHIEVE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdarchieve">
                              <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Dearchieve" ToolTip="De archieve" Width="20PX" Height="20PX"
                                    CommandName="DEARCHIEVE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmddearchieve" Visible="false">
                              <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>                 
                     <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
