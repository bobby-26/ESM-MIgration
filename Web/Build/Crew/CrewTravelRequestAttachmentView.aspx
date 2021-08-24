<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelRequestAttachmentView.aspx.cs" Inherits="Crew_CrewTravelRequestAttachmentView" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ticket Attachment</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvTravel.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTravelAttachment" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravel" runat="server" Width="100%"
                EnableViewState="true" AllowCustomPaging="true" AllowSorting="true"
                AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvTravel_NeedDataSource"
                EnableHeaderContextMenu="true" OnItemDataBound="gvTravel_ItemDataBound" OnSortCommand="gvTravel_SortCommand"
                OnItemCommand="gvTravel_ItemCommand" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">

                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="125px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin" AllowSorting="false" HeaderStyle-Width="80px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrigin" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDORIGINNAME").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" AllowSorting="false" HeaderStyle-Width="80px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDestination" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PNR" AllowSorting="false" HeaderStyle-Width="60px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPNR" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket No" AllowSorting="false" HeaderStyle-Width="80px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPPINGDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAttachmentCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAttachmentdtkey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lblTicketNo" runat="server"
                                    CommandName="Select"></asp:LinkButton>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>' runat="server"
                                    Height="14px" ToolTip="View File">
                                </asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="File Name" AllowSorting="false" HeaderStyle-Width="85px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></telerik:RadLabel>
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
