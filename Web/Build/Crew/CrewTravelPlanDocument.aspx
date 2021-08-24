<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelPlanDocument.aspx.cs"
    Inherits="CrewTravelPlanDocument" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Medical Documents</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
        function Resize() {
             
        var $ = $telerik.$;
                var height = $(window).height();
                var gvCrewTravelPlan = $find("<%= gvCrewTravelPlan.ClientID %>");
                var gvHistory = $find("<%= gvHistory.ClientID %>");
           
                var gvCrewTravelPlanPagerHeight = (gvCrewTravelPlan.PagerControl) ? gvCrewTravelPlan.PagerControl.offsetHeight : 0;
                var gvHistoryPagerHeight = (gvHistory.PagerControl) ? gvHistory.PagerControl.offsetHeight : 0;
              
                gvCrewTravelPlan.GridDataDiv.style.height = (Math.round(height / 3) - gvCrewTravelPlanPagerHeight - 19) + "px";
                gvHistory.GridDataDiv.style.height = (Math.round(height / 3) - gvHistoryPagerHeight - 19) + "px";

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
    <form id="frmCrewMedical" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTab" runat="server" Title="Travel Plan"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                       <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                    <%--<td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>--%>
               <%-- </tr>
                <tr>--%>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNumber" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                    </table>
                </div>
            <telerik:RadLabel ID="lblTravelplanned" runat="server" Font-Bold="true" Text="Travel Planned"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuCrewTravelPlan" runat="server" OnTabStripCommand="MenuCrewTravelPlan_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewTravelPlan" runat="server" EnableViewState="true" 
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnItemCommand="gvCrewTravelPlan_ItemCommand"
                OnNeedDataSource="gvCrewTravelPlan_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCrewTravelPlan_ItemDataBound"
                ShowFooter="false"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Vessel">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Vessel Account">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselac" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOrigHeader" runat="server">Origin</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDORIGIN")%>
                                <telerik:RadLabel ID="lblTravelHistoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELHISTORYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDestHeader" runat="server">Destination</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepDateHeader" runat="server">Departure</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE", "{0:dd/MM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrDateHeader" runat="server">Arrival</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDAteEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Ticket No.">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="PNR No.">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpnrno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Airline Code" HeaderStyle-Wrap="false">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAirlineCode" runat="server" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Class">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Tax">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Cancelled Y/N">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketCancelyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELTICKETYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="PLANEDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="View Ticket" ToolTip="View Ticket" Width="20PX" Height="20PX"
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAtt">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px"/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <telerik:RadLabel ID="lblTravelHistory" runat="server" Font-Bold="true" Text="Travel History"></telerik:RadLabel>
            <eluc:TabStrip ID="CrewTravelHistory" runat="server" OnTabStripCommand="CrewTravelHistory_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvHistory" runat="server" EnableViewState="true" OnItemCommand="gvHistory_ItemCommand"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" 
                OnNeedDataSource="gvHistory_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvHistory_ItemDataBound"
                ShowFooter="false"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Vessel">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Vessel Account">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselac" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Origin" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOrigHeader" runat="server">Origin</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrigin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelHistoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELHISTORYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDestHeader" runat="server">Destination</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepDateHeader" runat="server">Departure</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE", "{0:dd/MM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrDateHeader" runat="server">Arrival</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDAteEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Ticket No.">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="PNR No.">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpnrno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Airline Code" HeaderStyle-Wrap="false">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAirlineCode" runat="server" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Class">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Tax">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Cancelled Y/N">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketCancelyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELTICKETYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="MEDICALEDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="View Ticket" ToolTip="View Ticket" Width="20PX" Height="20PX"
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAtt">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
