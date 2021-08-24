<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSignOffReasonWise.aspx.cs"
    Inherits="Crew_CrewSignOffReasonWise" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Reason" Src="~/UserControls/UserControlSignOffReasonList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PoolList" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EmpFleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Off Report Reasonwise</title>
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
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <table border="1" style="border-collapse: collapse;">
            <tr>
                <td>
                    <table>
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
                                <telerik:RadLabel ID="lblManager" runat="server" Text="Manager">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" Width="130px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblReason" runat="server" Text="Reason">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:SignOffReason ID="ddlSignOffReason" runat="server" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" Width="130px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Address ID="ucPrincipal" AddressType="128" runat="server" AppendDataBoundItems="true"
                                    Width="130px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBatch" runat="server" Text="Batch">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" Width="130px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblNoReliever" runat="server" Text="No Reliever">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="ChkNoReliever" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality">
                                </telerik:RadLabel>
                                <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone">
                                </telerik:RadLabel>
                                <eluc:Zone runat="server" ID="ucZone" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type">
                                </telerik:RadLabel>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool">
                                </telerik:RadLabel>
                                <eluc:PoolList ID="ucPool" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status">
                                </telerik:RadLabel>
                                <telerik:RadListBox ID="lstStatus" runat="server" SelectionMode="Multiple" DataTextField="FLDHARDNAME"
                                    DataValueField="FLDHARDCODE" Width="280px" Height="100px">
                                </telerik:RadListBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblEmpFleet" runat="server" Text="Emp Fleet">
                                </telerik:RadLabel>
                                <eluc:EmpFleet ID="ucEmpFleet" runat="server" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" EnableViewState="false"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
            EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound" OnItemCommand="gvCrew_ItemCommand"
            ShowFooter="False" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false"
                    ShowRefreshButton="false" />
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
                    <telerik:GridTemplateColumn HeaderText="Emp No." AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Emp Name" AllowSorting="false" HeaderStyle-Width="220px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                            <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Civil Status" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMaritalStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARITALSTATUS") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign On" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignOnDate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign Off" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignedOff" Visible="true" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Duration" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDuration" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Age" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAge" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Nationality" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="120px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Available Date" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAvailableDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Signed Off Port" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselType" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Reliever Name" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRelieverName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' />
                            <telerik:RadLabel ID="lblOffsignerID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Join Date" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblJoinDate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEXPECTEDJOINDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Signed On Port" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignedOnPort" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONPORT") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Recmd Course" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSE").ToString()%> '>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFREMARKSHINT") %>'>
                            </telerik:RadLabel>
                            <eluc:Tooltip runat="server" ID="ucRemarks" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFREMARKS") %>'
                                Width="100px" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Appraisal Recd" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAppraisalRecd" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALRECD") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblHardName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>' />
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
