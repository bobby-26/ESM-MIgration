<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportBriefingandDeBriefing.aspx.cs"
    Inherits="CrewReportBriefingandDeBriefing" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CompanyName" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Briefing" Src="~/UserControls/UserControlBriefingTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="user" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MCUser" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>briefinganddebriefing</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvBriefDebrief.ClientID %>"));
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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblBriefing"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuTravel" runat="server" OnTabStripCommand="MenuTravel_TabStripCommand" TabStrip="false"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfilenumber" runat="server" Text="File Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtfilenumber" runat="server" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbllocation" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:CompanyName ID="ucCompanyName" runat="server" AppendDataBoundItems="true"
                            Width="160px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblinstructor" runat="server" Text="Instructor"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MCUser ID="RadMcUserTD" runat="server" Width="80%" emailrequired="false" designationrequired="false" ItemsPerRequest="50" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true"
                            HardTypeCode="54" Width="120px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblbriefingid" runat="server" Text="Briefing Subject"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Briefing ID="ucbriefingid" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="Sign On Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtsugnonStartDate" runat="server" />
                        <eluc:Date ID="txtsignonenddate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="Sign Off Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtsignoffstartdate" Width="80px" runat="server" />
                        <eluc:Date ID="txtsignoffenddate" Width="80px" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="DivRank" style="overflow-y: auto; overflow-x: hidden;">
                            <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="240px" />
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="DivVessel" style="overflow-y: auto; overflow-x: hidden;">
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true"
                                VesselsOnly="true" Width="240px" Entitytype="VSL" AssignedVessels="true" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblpool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="DivPool" style="overflow-y: auto; overflow-x: hidden;">
                            <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="240px" />
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblbatchid" runat="server" Text="Batch"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="Divbatchid" style="overflow-y: auto; overflow-x: hidden; height: 80px;">
                            <eluc:BatchList ID="ucBatchList" runat="server" AppendDataBoundItems="true" Width="240px" />
                        </div>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuTravelStatus" runat="server" TabStrip="false" OnTabStripCommand="MenuTravelStatus_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvBriefDebrief" runat="server"
                EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvBriefDebrief_NeedDataSource" OnItemCommand="gvBriefDebrief_ItemCommand" OnItemDataBound="gvBriefDebrief_ItemDataBound"
                ShowFooter="False" OnSortCommand="gvBriefDebrief_SortCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="File Number" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee Name" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDZONENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Training Batch" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDTRAININGBATCHNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Vessel" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDLASTVESSALNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off Date" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Pressent Vessel" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDPRESSENTVESSELNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign on Date" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDLOCATION")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Briefing Detail" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBRIEFINGDETAIL") %>' />
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBRIEFINGDETAIL") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblBriefingId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEBRIEFINGDEBRIEFINGID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <img id="imgRemarks" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>"
                                    onmousedown="javascript:closeMoreInformation()" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
