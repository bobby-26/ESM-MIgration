<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelBookingRequest.aspx.cs"
    Inherits="CrewHotelBookingRequest" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hotel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewHotelRequest" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:TextBox ID="lblBookingId" runat="server"></asp:TextBox>
            <eluc:TabStrip ID="MenuHotelBooking" runat="server" OnTabStripCommand="MenuHotelBooking_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvBookingDetails" runat="server" Height="90%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvBookingDetails_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvBookingDetails_ItemDataBound"
                OnItemCommand="gvBookingDetails_ItemCommand" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="FLDBOOKINGID" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <%--  <asp:Image ID="imgFlag" runat="server" Visible="false" />--%>
                                <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false">
                                 <span class="icon" id="imgFlagcolor" style="color:orange;" ><i class="fas fa-star"></i></span>      
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reference No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBookingId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBOOKINGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBOOKINGSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferenceNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkReferenceNumberName" runat="server" CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hotel" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHotelName" CssClass="tooltip" ClientIDMode="AutoID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDHOTELNAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDHOTELNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDHOTELNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="uclblHotelNameTT" TargetControlId="lblHotelName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOTELNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="City" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requested Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestedDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrentStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBOOKINGSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="1" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <span class="icon" style="color: green;"><i class="fas fa-star-blue"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompletedRequest" runat="server" Text="* Completed Request"></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon" style="color: orange;"><i class="fas fa-star-yellow"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPendingRequest" runat="server" Text="* Pending Request"></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon" style="color: red;"><i class="fas fa-star-red"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCancelledRequest" runat="server" Text="* Cancelled Request"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" Font-Size="Large"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                            Text="By default Hotel request list will show only the pending requests. You have to filter to view Confirmed/cancelled requests.">
                        </telerik:RadToolTip>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="* Note"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
