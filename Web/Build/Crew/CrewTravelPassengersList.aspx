<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelPassengersList.aspx.cs"
    Inherits="CrewTravelPassengersList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Passengers List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>   
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmtravelrequest" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="50%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="menutab" runat="server" Title="" TabStrip="true"></eluc:TabStrip>
            <table width="100%" style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNotes" runat="server" Font-Bold="true" Text="Notes :"></telerik:RadLabel>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                            Text="1. To create new travel request click on new button.</br>2. To add additional passengers to the request, select the request from the bottom scroll list and click on passenger entry button.</br>3. To change the passengers details, click on the passenger name in the scroll list.">
                        </telerik:RadToolTip>

                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCT" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCCT_ItemCommand" OnNeedDataSource="gvCCT_NeedDataSource"
                OnItemDataBound="gvCCT_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                        <telerik:GridTemplateColumn HeaderText="S.No.">
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRowNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport No.">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassportNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOriginName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE","{0:HH}:{0:mm}").ToString() == "00:00" ? DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MM/yyyy}") : DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MM/yyyy HH:mm}").ToString()%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:HH}:{0:mm}").ToString() == "00:00" ? DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MM/yyyy}") : DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MM/yyyy HH:mm}").ToString()%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action">
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
