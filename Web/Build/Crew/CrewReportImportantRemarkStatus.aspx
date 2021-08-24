<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportImportantRemarkStatus.aspx.cs"
    Inherits="CrewReportImportantRemarkStatus" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselList" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Important Remarks</title>
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
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <table border="1" style="border-collapse: collapse;">
            <tr valign="top">
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtFileNo" Width="100px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlstatus" runat="server" 
                                    Filter="Contains" MarkFirstMatch="true" Width="100px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Completed" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Not Completed" Value="3"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td colspan="2">
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                                </telerik:RadLabel>
                                <eluc:UserControlVesselList ID="ucVessel" runat="server" AppendDataBoundItems="true"
                                    Width="180px" Entitytype="VSL" ActiveVesselsOnly="true" AssignedVessels="true" VesselsOnly="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool">
                                </telerik:RadLabel>
                                <eluc:Pool ID="lstPool" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone">
                                </telerik:RadLabel>
                                <eluc:Zone ID="lstZone" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                                </telerik:RadLabel>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblManager" runat="server" Text="Manager">
                                </telerik:RadLabel>
                                <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal">
                                </telerik:RadLabel>
                                <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true" Width ="160px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvImportantRemark" runat="server" Height="68%"
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnNeedDataSource="gvImportantRemark_NeedDataSource" OnItemDataBound="gvImportantRemark_ItemDataBound"
            OnItemCommand="gvImportantRemark_ItemCommand" ShowFooter="False" OnSortCommand="gvImportantRemark_SortCommand"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDEMPLOYEEID">
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
                    <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="65px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="185px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemStyle HorizontalAlign="Left" Wrap="true" Width="200px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCrewId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'>
                            </telerik:RadLabel>
                            <asp:LinkButton ID="lnkCrew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="55px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Posted By" AllowSorting="false" HeaderStyle-Width="160px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPostedName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTEDBYNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Comments" AllowSorting="false" HeaderStyle-Width="125px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPostedComments" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPOSTEDDESCRIPTION").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDPOSTEDDESCRIPTION").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDPOSTEDDESCRIPTION").ToString() %>'>
                            </telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipPostedComment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTEDDESCRIPTION") %>'
                                Width="220px" TargetControlId="lblPostedComments"/>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Posted Date" AllowSorting="false" HeaderStyle-Width="92px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPostedDate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPOSTEDDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action By" AllowSorting="false" HeaderStyle-Width="150px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblActionName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONBYNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Comments" AllowSorting="false" HeaderStyle-Width="125px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblActionComment" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIONDESCRIPTION").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDACTIONDESCRIPTION").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDACTIONDESCRIPTION").ToString() %>'>
                            </telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipActionComment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONDESCRIPTION") %>'
                                Width="220px" TargetControlId="lblActionComment"/>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action Date" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblActionDate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTIONDATE","{0:dd/MMM/yyyy}")) %>' />
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
