<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSingleVesselActual.aspx.cs"
    Inherits="Crew_CrewSingleVesselActual" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew List Actual</title>
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
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
        TabStrip="false"></eluc:TabStrip>
    <table>
        <tr>
            <td>
                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel">
                </telerik:RadLabel>
            </td>
            <td>
                <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true"
                    Width="80%" CssClass="dropdown_mandatory" Entitytype="VSL" AssignedVessels="true" />
            </td>
            <td>
                <telerik:RadLabel ID="lblSailOnly" runat="server" Text="Sail Only">
                </telerik:RadLabel>
            </td>
            <td>
                <telerik:RadCheckBox ID="ckbSailOnly" runat="server" />
            </td>
        </tr>
    </table>
    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
    </eluc:TabStrip>
    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSingleVessel" runat="server"
        Height="80%" EnableViewState="false" AllowCustomPaging="true" AllowSorting="true"
        AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrewSingleVessel_NeedDataSource"
        EnableHeaderContextMenu="true" OnItemDataBound="gvCrewSingleVessel_ItemDataBound"
        OnItemCommand="gvCrewSingleVessel_ItemCommand" ShowFooter="False" AutoGenerateColumns="false">
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
                <telerik:GridTemplateColumn HeaderText="Emp Name" AllowSorting="false" HeaderStyle-Width="210px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblEmpno" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                        <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Emp No." AllowSorting="false" HeaderStyle-Width="90px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblFileEmpno" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="70px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblRankCode" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Nationality" AllowSorting="false" HeaderStyle-Width="110px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" HeaderStyle-Width="110px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblStatus" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Passport No" AllowSorting="false" HeaderStyle-Width="110px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblPassportNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="(Exp)Join Date" AllowSorting="false" HeaderStyle-Width="110px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblExpJoin" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEXPECTEDJOINDATE","{0:dd/MMM/yyyy}")) %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="S/on Date" AllowSorting="false" HeaderStyle-Width="110px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblSignOnDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINALCOC") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Relief Date" AllowSorting="false" HeaderStyle-Width="110px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblRelifDate" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE","{0:dd/MMM/yyyy}")) %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="SMNBK No." AllowSorting="false" HeaderStyle-Width="110px"
                    ShowSortIcon="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <telerik:RadLabel ID="lblSMNBKNOHeader" runat="server">
                            SMNBK No.</telerik:RadLabel>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblSmbnkno" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>' />
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
