<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelRequestAdd.aspx.cs" Inherits="Crew_CrewTravelRequestAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuHeader" runat="server" TabStrip="true" OnTabStripCommand="MenuHeader_TabStripCommand"
                 Title="Travel Request"></eluc:TabStrip>
            <eluc:TabStrip ID="Menuapprove" runat="server" OnTabStripCommand="Menuapprove_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucvessel" CssClass="dropdown_mandatory" runat="server" AssignedVessels="true" AutoPostBack="true"
                            OnTextChangedEvent="ucVessel_OnTextChanged" Width="200px" Entitytype="VSL" ActiveVesselsOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME"
                            DataValueField="FLDACCOUNTID" Width="200px"
                            EnableLoadOnDemand="True"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrigin" runat="server" Text="Departure"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtOrigin" runat="server" CssClass="input_mandatory" Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="DepatureDate" runat="server" Text="Departure Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtorigindate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        <telerik:RadComboBox ID="ddlampmOriginDate" runat="server" CssClass="input_mandatory" Width="55px"
                            EnableLoadOnDemand="True"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--" Value="" />
                                <telerik:RadComboBoxItem Text="AM" Value="1" />
                                <telerik:RadComboBoxItem Text="PM" Value="2" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="txtDestination" runat="server" CssClass="input_mandatory" Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblArrivalDate" runat="server" Text="Arrival Date"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Date ID="txtDestinationdate" runat="server" CssClass="input" DatePicker="true" />
                        <telerik:RadComboBox ID="ddlampmarrival" runat="server" CssClass="input" Width="55px"
                            EnableLoadOnDemand="True"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--" Value="" />
                                <telerik:RadComboBoxItem Text="AM" Value="1" />
                                <telerik:RadComboBoxItem Text="PM" Value="2" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucpurpose" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            ReasonFor="2" Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Payment Mode"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="Payment" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" Width="200px" />
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuTravel" runat="server" OnTabStripCommand="MenuTravel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" runat="server" AllowCustomPaging="true" AllowSorting="true" Height="45%"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewSearch_ItemCommand" OnNeedDataSource="gvCrewSearch_NeedDataSource"
                OnItemDataBound="gvCrewSearch_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false">
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
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lnkFileNoHeader" runat="server" Text="File No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFirstnameHeader" runat="server" Text="Name"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkEployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAppliedRankHeader" runat="server">Rank&nbsp;</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppliedRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPassportHeader" runat="server" Text="Passport No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassport" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
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
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:status ID="ucstatus" runat="server" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
