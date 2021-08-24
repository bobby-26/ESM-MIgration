<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsSeniorOfficersPlannerSearch.aspx.cs"
    Inherits="Crew_CrewReportsSeniorOfficersPlannerSearch" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeniorRank" Src="~/UserControls/UserControlSeniorRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Senior Officers Planner Search</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .mlabel {
                color: blue !important;                
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblnote" runat="server" Text="Note: Pool and Zone are for the onboard seafarers."
                        CssClass="mlabel">
                    </telerik:RadLabel>
                </td>
            </tr>
        </table>
        <table border="1" style="border-collapse: collapse;">
            <tr valign="top">
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBatch" runat="server" Text="Batch">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true" width = "100%"/>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type">
                                </telerik:RadLabel>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                                </telerik:RadLabel>
                                <eluc:Vessel ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true" runat="server"
                                     Width="190px" Entitytype="VSL" ActiveVesselsOnly="true"  AssignedVessels="true"/>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool">
                                </telerik:RadLabel>
                                <eluc:Pool ID="lstPool" runat="server" AppendDataBoundItems="true"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone">
                                </telerik:RadLabel>
                                <eluc:Zone ID="lstZone" runat="server" AppendDataBoundItems="true"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                                </telerik:RadLabel>
                                <eluc:SeniorRank ID="lstSeniorRank" runat="server" AppendDataBoundItems="true"  />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="65%"
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
            EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound" OnItemCommand="gvCrew_ItemCommand"
            ShowFooter="False" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" >
                <HeaderStyle Width="102px" />
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
                <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="65px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="On Signer" AllowSorting="false" HeaderStyle-Width="190px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOnSignerId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERID") %>' />
                            <asp:LinkButton ID="lnkOnSigner" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNER") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Off Signer" AllowSorting="false" HeaderStyle-Width="190px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOffSignerId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>' />
                            <asp:LinkButton ID="lnkOffSigner" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNER") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="EX/NEW" AllowSorting="false" HeaderStyle-Width="70px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblExNew" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXNEW") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="On Signing Port/Date" AllowSorting="false" HeaderStyle-Width="150px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                    
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOnSignPortDate" Visible="true" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPORTDATE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Owners Briefing" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOwnersBriefing" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBRIEFING") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="S'pore Briefing" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSporeBriefing" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPOREBRIEFING") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Parallel Sailing" AllowSorting="false" HeaderStyle-Width="58px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblParallelSailing" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARELLELSAILING") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemarks" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
