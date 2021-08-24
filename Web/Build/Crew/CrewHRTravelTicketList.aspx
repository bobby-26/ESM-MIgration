<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHRTravelTicketList.aspx.cs" Inherits="CrewHRTravelTicketList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Family Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewHrTravelTicket" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTravelTicketMain" runat="server" OnTabStripCommand="TravelTicketMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuTitle" runat="server"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="88%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadLabel ID="lblTitle" runat="server" Visible="false"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuTravelTicket" runat="server" OnTabStripCommand="TravelTicket_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelTicket" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvTravelTicket_ItemCommand" OnNeedDataSource="gvTravelTicket_NeedDataSource" Height="90%"
                OnItemDataBound="gvTravelTicket_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    DataKeyNames="FLDTRAVELHOPLINEID">
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
                        <telerik:GridTemplateColumn HeaderText="S.No.">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblNameHeader" runat="server" Text="Name"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelPassengerId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelHopLineId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELHOPLINEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblattachmentMappingyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTMAPPINGYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAttachment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENT") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAttachmentCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepatureCityHeader" runat="server" Text="Origin"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepatureCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURECITYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDepatureCityId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDestinationCityHeader" runat="server" Text="Destination"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDestinationCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDestinationCityId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepartureDateHeader" runat="server" Text="Departure"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDDEPARTURETIME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrivalDateHeader" runat="server" Text="Arrival"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDARRIVALTIME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="left" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTicketNoHeader" runat="server" Text="Ticket No"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="left" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTotalAmountHeader" runat="server" Text="Amount"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Travel Status">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTravelStatusHeader" runat="server">Travel Status</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltravelstatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTICKETSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="left" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepatureDatePassedHeader" runat="server" Text="Obsolete Ticket"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepatureDatePassed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATUREDATEPASSEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ToolTip="Ticket" Width="20PX" Height="20PX"
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachmentMapping">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
