<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InjuryAnalysis.aspx.cs" Inherits="Inspection_InjuryAnalysis" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressTypeList.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Injury Analysis" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2">
                <tr align="top">
                    <td style="overflow-y: hidden">
                        <telerik:RadLabel ID="litYear" runat="server" Text="Year"></telerik:RadLabel>
                        <br />
                        <asp:ListBox ID="ddlYear" SelectionMode="Multiple" AppendDataBoundItems="true" Width="100px" Height="80px" runat="server"></asp:ListBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="litQuarter" runat="server" Text="Quarter"></telerik:RadLabel>
                        <br />
                        <asp:ListBox ID="lstQuarter" SelectionMode="Multiple" AppendDataBoundItems="true" Width="100px" Height="80px" runat="server">
                            <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Q1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Q2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Q3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Q4"></asp:ListItem>
                        </asp:ListBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="litMonth" runat="server" Text="Month"></telerik:RadLabel>
                        <br />
                        <asp:ListBox ID="lstMonth" SelectionMode="Multiple" AppendDataBoundItems="true" Width="100px" Height="80px" runat="server">
                            <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                            <asp:ListItem Value="5" Text="May"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                            <asp:ListItem Value="7" Text="July"></asp:ListItem>
                            <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                            <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                            <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                            <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                            <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                        </asp:ListBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfleet" runat="server" Text="Fleet"></telerik:RadLabel>
                        <eluc:Fleet ID="ucFleet" runat="server" AppendDataBoundItems="true" vesselsonly="true" Width="100px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel1" runat="server" Text="Vessel"></telerik:RadLabel>
                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="240px" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                OnRowDataBound="gvCrew_RowDataBound" OnRowCommand="gvCrew_RowCommand" Width="100%" GroupingEnabled="false" EnableHeaderContextMenu="true"
                CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvCrew_NeedDataSource"
                OnSorting="gvCrew_Sorting" GridLines="None">
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
                        <telerik:GridTemplateColumn HeaderText="Employee Name">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDINJUREDEMPLOYEENAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Age">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDAGE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SIM's Y/N">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSIMS") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPECATEGORYNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cons. Category">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCONSEQUENCECATEGORYNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Incident">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTDATE","{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type Of Injury">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFINJURYNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Part Of the body">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPARTOFTHEBODYINJUREDNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Day/Night">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDAYNIGHT") %>
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
            <%--            <br />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="ltGrid" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
            </table>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
