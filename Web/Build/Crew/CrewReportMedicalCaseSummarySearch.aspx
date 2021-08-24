<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMedicalCaseSummarySearch.aspx.cs"
    Inherits="Crew_CrewReportMedicalCaseSummarySearch" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PoolList" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Medical Case Summary</title>
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

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text=" Illness / Injury Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                        <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server"  Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server"  EmptyMessage="Type to select status" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Open" Value="0"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Closed" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="ReOpen" Value="2"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBatch" runat="Server" Text="Batch"></telerik:RadLabel>
                        <br />
                    </td>
                    <td>
                        <div runat="server" id="DivBatch" style="overflow-y: auto; overflow-x: hidden;">
                            <eluc:BatchList ID="ucBatch" AppendDataBoundItems="true" runat="server" Width="240px" />
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                        <br />
                    </td>
                    <td>

                        <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="240px" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        <br />
                    </td>
                    <td>

                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true"
                            VesselsOnly="true" Width="240px" Entitytype="VSL" AssignedVessels="true" ActiveVesselsOnly="true" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                        <br />
                    </td>
                    <td>

                        <eluc:PoolList ID="ucPool" runat="server" AppendDataBoundItems="true" Width="240px" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                        <br />
                    </td>
                    <td>

                        <eluc:Zone runat="server" ID="ucZone" AppendDataBoundItems="true" Width="240px" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        <br />
                    </td>
                    <td>

                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="240px" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeafarerStatus" Text="Seafarer Status" runat="server"></telerik:RadLabel>
                        <br />
                    </td>
                    <td>

                        <telerik:RadListBox ID="lstStatus" runat="server" SelectionMode="Multiple"
                            DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" Width="240px" Height="80px"></telerik:RadListBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl" Text="Manager" runat="server"></telerik:RadLabel>
                        <br />
                    </td>
                    <td>

                        <eluc:Manager ID="ucManager" AddressType="126" runat="server" AppendDataBoundItems="true"
                            Width="240px" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                        <br />
                    </td>
                    <td>

                        <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128"
                            AppendDataBoundItems="true" Width="240px" />

                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvMedicalCase" runat="server"
                Height="45%" EnableViewState="false" AllowCustomPaging="true" AllowSorting="true"
                AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvMedicalCase_NeedDataSource"
                EnableHeaderContextMenu="true" OnItemDataBound="gvMedicalCase_ItemDataBound" OnSortCommand="gvMedicalCase_SortCommand"
                OnItemCommand="gvMedicalCase_ItemCommand" ShowFooter="False" AutoGenerateColumns="false">
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

                    <%--<asp:GridView ID="gvMedicalCase" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false"
                                OnRowDataBound="gvMedicalCase_RowDataBound">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />--%>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No" AllowSorting="false" HeaderStyle-Width="150px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Medical Case Summary" AllowSorting="false" HeaderStyle-Width="450px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <table width="100%" style="border-collapse: collapse;">
                                    <tr>
                                        <td style="width: 7%; font-weight: bold;">
                                            <telerik:RadLabel ID="lblEmpNoHeader" runat="server">Request No.</telerik:RadLabel>

                                        </td>
                                        <td style="width: 25%;">
                                            <telerik:RadLabel ID="lblPNIMedicalCaseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNIMEDICALCASEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRefNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                                        </td>
                                        <td style="width: 8%; font-weight: bold;">
                                            <telerik:RadLabel ID="lblRankHeader" runat="server">Rank</telerik:RadLabel>

                                        </td>
                                        <td style="width: 15%;">
                                            <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                                        </td>
                                        <td style="width: 10%; font-weight: bold;">
                                            <telerik:RadLabel ID="lblFromDateHeader" runat="server">Date</telerik:RadLabel>

                                        </td>
                                        <td style="width: 18%;">
                                            <telerik:RadLabel ID="lblFromDate" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDFROMDATE")) %>' />
                                        </td>
                                        <td style="width: 12%; font-weight: bold;">
                                            <telerik:RadLabel ID="lblDateofClosureHeader" runat="server">Date of Closure</telerik:RadLabel>

                                        </td>
                                        <td style="width: 8%;">
                                            <telerik:RadLabel ID="lblDateofClosure" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFCLOSURE"))%> '></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblFileNoHeader" runat="server">File No</telerik:RadLabel>

                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblCDCNumberHeader" runat="server">CDC Number</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblCDCNumber" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>' />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblStatusHeader" runat="server">Status</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                        </td>
                                        <td style="width: 11%; font-weight: bold;">
                                            <telerik:RadLabel ID="lblTotalCostIncurredHeader" runat="server">Total Cost Incurred</telerik:RadLabel>
                                        </td>
                                        <td style="width: 10%;">
                                            <telerik:RadLabel ID="lblTotalCostIncurred" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALEXPENSE") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblEmpNameHeader" runat="server">Name</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                            <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblSignOffDateHeader" runat="server">Sign Off Date</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblSignedOff" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE")) %>' />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblApplicableCBAHeader" runat="server">Applicable CBA</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblApplicableCBA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCBA") %>'></telerik:RadLabel>
                                        </td>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblDeductibleAmountHeader" runat="server">Deductible Amount</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblDeductibleAmount" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWDEDUCTABLE") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblVesselHeader" runat="server">Vessel</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblVesselName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                            <br />
                                            <telerik:RadLabel ID="lblVesselType" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>' />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblIllnessInjuryDateHeader" runat="server">Illness/Injury</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblIllnessCategory" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFMEDICALCASE") %>' />
                                            <span style="font-weight: bold;">/</span>
                                            <telerik:RadLabel ID="lblIllnessInjuryDate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFILLNESS")) %>' />
                                            <br />
                                            <telerik:RadLabel ID="lblPendingWith" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGWITH") %>'></telerik:RadLabel>
                                            <br />
                                            <telerik:RadLabel ID="lblIllnessType" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPESOFINJURY") %>' />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblLimitationPeriodHeader" runat="server">Limitation Period</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblLimitationPeriod" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMELIMIT") %>' />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblClaimableAmountHeader" runat="server">Claimable Amount</telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblClaimableAmount" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCLAIMABLE") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblRemarksHeader" runat="server">Remarks</telerik:RadLabel>
                                        </td>
                                        <td colspan="7">
                                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
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
