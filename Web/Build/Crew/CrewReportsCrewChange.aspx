<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsCrewChange.aspx.cs"
    Inherits="Crew_CrewReportsCrewChange" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Change Report</title>
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
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="MenuReport_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <b style="color: Blue; font-size: small">
            <telerik:RadLabel ID="ltlText" runat="server" Text="Please Select JSU as the Union since this report is for JSU Vessels Only.">
            </telerik:RadLabel>
        </b>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="Period Between">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                    <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblUnion" runat="server" Text="Union">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlUnion" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                        EmptyMessage="Type to Select Union" ToolTip="Type to Select Union" Filter="Contains"
                        MarkFirstMatch="true" CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddlUnion_SelectedIndexChanged"
                        Width="100%">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel" Width="50%">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlVessel" runat="server" CssClass="input_mandatory" AutoPostBack="false"
                        EmptyMessage="Type to Select Vessel" ToolTip="Type to Select Vessel" Filter="Contains"
                        MarkFirstMatch="true" Width="100%">
                        <Items>
                            <telerik:RadComboBoxItem Selected="True" Value="0" Text="--Select--"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                    <telerik:RadRadioButtonList ID="rblFormats" runat="server" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="rblFormats_SelectedIndexChanged" AutoPostBack="true"
                        Visible="false">
                        <Items>
                            <telerik:ButtonListItem Text="Format1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="Format2"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="Format3"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="70%"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" OnSortCommand="gvCrew_SortCommand" OnNeedDataSource="gvCrew_NeedDataSource"
            OnItemDataBound="gvCrew_ItemDataBound" ShowFooter="false" EnableViewState="false"
            OnItemCommand="gvCrew_ItemCommand" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEID" TableLayout="Fixed">
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
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="68px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="190px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                            <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="DOB" AllowSorting="false" HeaderStyle-Width="88px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBirthDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date/Place on SD" AllowSorting="false" HeaderStyle-Width="120px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDatePlaceSignOn" Visible="true" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEPLACESIGNEDON", "{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Address" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAddress" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESS") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Passport NO/DOE" AllowSorting="false" HeaderStyle-Width="122px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPassportDoeDoi" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPPTDOE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="SIB No/DOI/DOE" AllowSorting="false" HeaderStyle-Width="120px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFlagSeamanBook" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGSIBDOIDOE") %>' />
                            <telerik:RadLabel ID="lblFlagCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGCODE") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblFlagName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAG") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Indian Licence No/DOI/DOE" AllowSorting="false"
                        HeaderStyle-Width="174px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblIndianLicence" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINDIANLICENCE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Seaman Book No/DOI/DOE" AllowSorting="false"
                        HeaderStyle-Width="210px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSeamanBook" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIBDOIDOE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                    EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
