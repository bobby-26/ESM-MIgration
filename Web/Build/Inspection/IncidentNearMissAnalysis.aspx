<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IncidentNearMissAnalysis.aspx.cs" Inherits="Inspection_IncidentNearMissAnalysis" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressTypeList.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident Analysis</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvCrew.ClientID %>"));
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Incident / Near Miss Analysis" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2">
                <tr align="top">
                    <td style="display: inline-block;">
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                        <br />
                        <telerik:RadComboBox ID="ddlYear" runat="server" AutoPostBack="false" Width="100px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                        <br />
                        <br />
                        <telerik:RadLabel ID="litEventType" runat="server" Text="Event Type"></telerik:RadLabel>
                        <br />
                        <telerik:RadComboBox ID="ddlEventType" runat="server" AutoPostBack="false"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" Width="100px">
                            <Items>
                                <telerik:RadComboBoxItem Value="Incident" Text="Incident" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="Near Miss" Text="Near Miss"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="litQuarter" runat="server" Text="Quarter"></telerik:RadLabel>
                        <br />
                        <asp:ListBox ID="lstQuarter" SelectionMode="Multiple" AppendDataBoundItems="true" Width="100px" Height="80px" CssClass="input" runat="server">
                            <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Q1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Q2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Q3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Q4"></asp:ListItem>
                        </asp:ListBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfleet" runat="server" Text="Fleet"></telerik:RadLabel>
                        <eluc:Fleet ID="ucFleet" runat="server" AppendDataBoundItems="true" vesselsonly="true" Width="100px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                        <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                        <br />
                        <eluc:Address ID="ucPrincipal" AddressType="128" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" GroupingEnabled="false" EnableHeaderContextMenu="true"
                CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvCrew_NeedDataSource" GridLines="None">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Event Date">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDEVENTDATE", "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref. No">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDEVENTREFNO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Event Type">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDEVENTTYPE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDEVENTSTATUS") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cons. Cat">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDEVENTCONSEQUENCECATEGORY") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDEVENTCATEGORY") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub. Category">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDEVENTSUBCATEGORY") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>



