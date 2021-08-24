<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsNotRelievedOnTimeOnLeave.aspx.cs"
    Inherits="CrewReportsNotRelievedOnTimeOnLeave" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vesseltype" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Not Relieved On Time</title>
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
</script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblReportsNotRelievedOnTimeOnLeave"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="MenuReport_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
    
        <table border="1" width="140%" style="border-collapse: collapse;">
            <tr>
                <td>
                    <table >
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text="Relief Delayed From">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" Width="100%"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblToDate" runat="server" Text="Relief Delayed To">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0" width="60%">
                        <tr valign="top">
                            <td colspan="2">
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type">
                                </telerik:RadLabel>
                                <eluc:Vesseltype ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="200px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool">
                                </telerik:RadLabel>
                                <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="120px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone">
                                </telerik:RadLabel>
                                <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="110px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                                </telerik:RadLabel>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="170px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBatch" runat="server" Text="Batch">
                                </telerik:RadLabel>
                                <eluc:BatchList ID="ucBatch" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                                </telerik:RadLabel>
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                                    Width="200px" Entitytype="VSL" ActiveVessels="true" AssignedVessels="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal">
                                </telerik:RadLabel>
                                <eluc:AddressType ID="ucPrincipal" runat="server" AddressType="128" Width="120px"
                                    AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblManager" runat="server" Text="Manager">
                                </telerik:RadLabel>
                                <eluc:AddressType ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                                    Width="120px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvCrew" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" 
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" OnSortCommand="gvCrew_SortCommand" OnNeedDataSource="gvCrew_NeedDataSource"
            OnItemDataBound="gvCrew_ItemDataBound" ShowFooter="false" EnableViewState="false"
            OnItemCommand="gvCrew_ItemCommand" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDRANKID" TableLayout="Fixed"
                >
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false"
                    ShowRefreshButton="false" />
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
                <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="150px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRankId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="D(-30 TO -15)" AllowSorting="false" HeaderStyle-Width="45px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkD1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD1") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblD1" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD1") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="D(-14 TO 0)" AllowSorting="false" HeaderStyle-Width="45px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkD2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD2") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblD2" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD2") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="D(+1 TO +7)" AllowSorting="false" HeaderStyle-Width="45px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkD3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD3") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblD3" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD3") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="D(+8 TO +15)" AllowSorting="false" HeaderStyle-Width="45px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkD4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD4") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblD4" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD4") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="D(+16 TO +30)" AllowSorting="false" HeaderStyle-Width="45px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkD5" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD5") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblD5" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD5") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="D(>+30)" AllowSorting="false" HeaderStyle-Width="45px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkD6" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD6") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblD6" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDD6") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="false" HeaderStyle-Width="45px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblTotal" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
