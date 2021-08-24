<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelDetailsForAgent.aspx.cs"
    Inherits="CrewTravelDetailsForAgent" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucVessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReqNo" runat="server" Width="60%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ucVessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" Entitytype="VSL" Width="60%"
                            AssignedVessels="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Width="60%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTicketNo" runat="server" Text="Ticket No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTicketNo" runat="server" Width="60%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListOriginCity">
                            <telerik:RadTextBox ID="txtOriginCity" runat="server" ReadOnly="false" CssClass="readonlytextbox"
                                MaxLength="20" Width="150px">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                ID="ImgShowOrigin" ToolTip="Select Destination"
                                OnClientClick="return showPickList('spnPickListOriginCity', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); ">
                                        <span class="icon"><i class="fas fa-list-alt"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtOriginId" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListDestinationCity">
                            <telerik:RadTextBox ID="txtDestinationCity" runat="server" ReadOnly="false" CssClass="readonlytextbox"
                                MaxLength="20" Width="150px">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                ID="ImgShowDestination" ToolTip="Select Destination"
                                OnClientClick="return showPickList('spnPickListDestinationCity', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); ">
                                        <span class="icon"><i class="fas fa-list-alt"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtDestinationId" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeparture" runat="server" Text="Departure Between"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <eluc:Date ID="txtStartDate" CssClass="input_mandatory" Width="120px" runat="server"
                            DatePicker="true" />
                        &nbsp;to&nbsp;
                                <eluc:Date ID="txtEndDate" CssClass="input_mandatory" Width="120px" runat="server"
                                    DatePicker="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenutravelList" runat="server" OnTabStripCommand="MenutravelList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelDetails" runat="server" AutoGenerateColumns="False" Height="85%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="true"
                OnNeedDataSource="gvTravelDetails_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvTravelDetails_ItemCommand"
                OnItemDataBound="gvTravelDetails_ItemDataBound">
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
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Request No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequisitionNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrigin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tax" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
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
