<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportAvailabilityReportFormat2.aspx.cs"
    Inherits="CrewReportAvailabilityReportFormat2" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCVesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Statuslist" Src="~/UserControls/UserControlStatusList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Availability Report Format2</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
        <eluc:TabStrip ID="MenuRecStatTabs" runat="server" OnTabStripCommand="MenuRecStatTabs_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <table width="100%">
            <tr style="color: Blue">
                &nbsp;&nbsp;
                    <span id="Span3" class="icon" style="align-self: center" runat="server"><font color="blue" size="2px">Note : To view the Guidelines, place the mouse pointer over the&nbsp;&nbsp;<i class="fas fa-question-circle"></i></span>&nbsp;&nbsp;button.</font>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="GuidlinesTooltip" Width="300px" ShowEvent="onmouseover" 
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span3" IsClientID="true"
                        HideEvent="LeaveToolTip" Position="MiddleRight" EnableRoundedCorners="true" HideDelay="5000"
                        Text="Please note: <br/> 1)If include past experience is checked, the filter will search for vessel type experience in full past experience, if not it will check only last vessel/onboard vessel. <br/> 2) Press Control for Multiple Selection">
                    </telerik:RadToolTip>
            </tr>
        </table>
        <table width="100%" border="1" style="border-collapse: collapse;">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text="Availability Between"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDate" runat="server" />
                                <eluc:Date ID="ucDate1" runat="server" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtName" runat="server" ToolTip="Enter the Name"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Principal ID="ucPrinicpal" runat="server" AddressType="128" AppendDataBoundItems="true" />
                            </td>
                       
                            <td colspan="3">
                                <telerik:RadCheckBox ID="chkNewApp" runat="server" Text="New Applicant" />
                                <telerik:RadCheckBox ID="chkInactive" runat="server" Text="Inactive/NTBR/MGR.NTBR" />
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkIncludepastexp" runat="server" Text="Include past experience" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                        <tr valign="top">
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="Server" Text="Vessel Type"></telerik:RadLabel>
                                <eluc:UCVesselType ID="ddlVesselType" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text=" Pool"></telerik:RadLabel>
                                <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                                <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" ZoneList="<%# PhoenixRegistersMiscellaneousZoneMaster.ListMiscellaneousZoneMaster() %>" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" AutoPostBack="false"
                                    RankList="<%# PhoenixRegistersRank.ListRank() %>" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                                <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                                <eluc:Statuslist ID="lstStatus" runat="server" AppendDataBoundItems="true" AutoPostBack="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            </td> </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="53%"
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnNeedDataSource="gvCrew_NeedDataSource" OnItemDataBound="gvCrew_ItemDataBound"
            OnItemCommand="gvCrew_ItemCommand" ShowFooter="False" OnSortCommand="gvCrew_SortCommand"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDEMPLOYEEID" CommandItemDisplay="Top">
                <HeaderStyle Width="102px" />
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="Last Sign Off" Name="LastSignOff" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                    <telerik:GridColumnGroup HeaderText="Last" Name="Last" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                </ColumnGroups>
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />

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
                     <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="50px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDROW")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="220px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                            <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDBATCH")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="160px" ColumnGroupName="LastSignOff"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSEL") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" HeaderStyle-Width="80px" ColumnGroupName="LastSignOff"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE")) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="DOA" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDOA"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Contact By" AllowSorting="false" HeaderStyle-Width="150px" ColumnGroupName="Last"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDLASTCONTACT")%>
                            <%# DataBinder.Eval(Container,"DataItem.FLDLASTCONTACT").ToString()==""?"":"On"%>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" HeaderStyle-Width="160px" ColumnGroupName="Last"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLASTREMARKS").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDLASTREMARKS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDLASTREMARKS").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipLeaveRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTREMARKS") %>'
                                Width="360px" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Promotion Category" AllowSorting="false" HeaderStyle-Width="130px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDPROMOTIONCATEGORY")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Requirement" AllowSorting="false" HeaderStyle-Width="130px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDSEAFARERREQUIREMENTNAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Licence Level" AllowSorting="false" HeaderStyle-Width="130px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                  
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container,"DataItem.FLDLICENCENAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                        <ItemTemplate>
                           <asp:LinkButton runat="server" AlternateText="Suitability Check" CommandName="SUITABILITYCHECK"
                                CommandArgument="<%# Container.DataSetIndex %>" ID="imgSuitableCheck" ToolTip="Suitability Check">
                                <span class="icon"><i class="fas  fa-user-astronaut"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
