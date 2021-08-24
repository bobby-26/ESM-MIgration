<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionInspectorSchedule.aspx.cs"
    Inherits="InspectionInspectorSchedule" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITopic" Src="~/UserControls/UserControlInspectionTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Checklist" Src="~/UserControls/UserControlInspectionChecklist.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspection List"</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleMaster" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:Title runat="server" ID="frmTitle" Text="Inspection List" Visible="false"></eluc:Title>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>
        <div style="position: relative; width: 100%">
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="&nbsp&nbsp&nbsp Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" OnTextChangedEvent="ucVessel_Changed"
                            VesselsOnly="true" AutoPostBack="true" Width="200px"/>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInspector" runat="Server" Text="Inspector"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInspector" runat="server" Width="200px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="position: relative; width: 100%">
            <table width="100%">
                <tr>
                    <td valign="top">
                        <div id="divGrid" style="position: relative; z-index: 0">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <eluc:TabStrip ID="MenuInspectionScheduleSearch" runat="server" OnTabStripCommand="InspectionScheduleSearch_TabStripCommand"></eluc:TabStrip>

                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvInspectionSchedule" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                            OnItemCommand="gvInspectionSchedule_ItemCommand" OnItemDataBound="gvInspectionSchedule_ItemDataBound" OnNeedDataSource="gvInspectionSchedule_NeedDataSource"
                                            OnSortCommand="gvInspectionSchedule_SortCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                            ShowFooter="false" ShowHeader="true" EnableViewState="true">
                                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                AutoGenerateColumns="false" DataKeyNames="FLDINSPECTIONSCHEDULEID" TableLayout="Fixed">
                                                <NoRecordsTemplate>
                                                    <table width="100%" border="0">
                                                        <tr>
                                                            <td align="center">
                                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                                <HeaderStyle Width="102px" />
                                                <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="150px">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <asp:LinkButton ID="lnkVesselCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME">Vessel&nbsp;</asp:LinkButton>
                                                        <img id="FLDVESSELNAME" runat="server" visible="false" />
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblDTkey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>' Visible="false"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Reference Number" HeaderStyle-Width="200px">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <asp:LinkButton ID="lnkInspectionRefNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDREFERENCENUMBER">Reference Number&nbsp;</asp:LinkButton>
                                                        <img id="FLDREFERENCENUMBER" runat="server" visible="false" />
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblInspectionRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <%--<telerik:GridTemplateColumn HeaderText="Inspection Type">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="lnkInspectionTypeHeader" runat="server" CommandName="Sort" CommandArgument="FLDINSPECTIONTYPENAME"
                                                                    ForeColor="White">Type &nbsp;</asp:LinkButton>
                                                                <img id="FLDINSPECTIONTYPENAME" runat="server" visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblInspectionTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONTYPENAME") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>--%>
                                                    <telerik:GridTemplateColumn HeaderText="Inspection Category" Visible="false">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <asp:LinkButton ID="lnkInspectionCategoryHeader" runat="server" CommandName="Sort"
                                                            CommandArgument="FLDINSPECTIONCATEGORY">Category &nbsp;</asp:LinkButton>
                                                        <img id="FLDINSPECTIONCATEGORY" runat="server" visible="false" />
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblInspectionCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCATEGORY") %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="170px">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblInspectionCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCOMPANYNAME") %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Vetting" HeaderStyle-Width="120px">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <asp:LinkButton ID="lnkInspectionHeader" runat="server" CommandName="Sort" CommandArgument="FLDINSPECTIONNAME">Vetting&nbsp;</asp:LinkButton>
                                                        <img id="FLDINSPECTIONNAME" runat="server" visible="false" />
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblInspectionScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONSCHEDULEID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblInspectionDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                                        <asp:LinkButton ID="lnkInspection" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNAME") %>' Visible="false"></asp:LinkButton>
                                                        <telerik:RadLabel ID="lblInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNAME") %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Visible="false">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <telerik:RadLabel ID="lblISHeader" runat="server">I/S</telerik:RadLabel>
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblInspectionScreening" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONSCREENING") %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Visible="false">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <telerik:RadLabel ID="lblBasisHeader" runat="server">Basis</telerik:RadLabel>
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblBasisId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISID") %>'></telerik:RadLabel>
                                                        <asp:LinkButton ID="lnkBasis" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISREFERENCENUMBER") %>'></asp:LinkButton>

                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Done Date" HeaderStyle-Width="150px">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <asp:LinkButton ID="lnkLastDoneDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDVETTINGCOMPLETIONDATE">Done Date&nbsp;</asp:LinkButton>
                                                        <img id="FLDVETTINGCOMPLETIONDATE" runat="server" visible="false" />
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Port" Visible="false">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <telerik:RadLabel ID="lblPortHeader" runat="server">Port</telerik:RadLabel>
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Inspector" Visible="false">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <telerik:RadLabel ID="lblInspectorHeader" runat="server"> Inspector</telerik:RadLabel>
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Attending Supt" Visible="false">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <headertemplate>
                                                        <telerik:RadLabel ID="lblAttendingSuptHeader" runat="server"> Attending Supt</telerik:RadLabel>
                                                    </headertemplate>
                                                        <itemtemplate>
                                                        <telerik:RadLabel ID="lblAttendingSupt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTENDINGSUPT") %>'></telerik:RadLabel>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Deficiency" HeaderStyle-Width="310px">
                                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                                        <itemtemplate>
                                                        <asp:LinkButton ID="lnkMinorNc" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYCOUNT") %>'></asp:LinkButton>
                                                        <%--<asp:LinkButton ID="lnkMajorNc" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAJORNCCOUNT") %>'></asp:LinkButton>
                                                                <asp:LinkButton ID="lnkObservation" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBSERVATIONCOUNT") %>'></asp:LinkButton>--%>
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Visible="false">
                                                        <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                                        <headertemplate>
                                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                                                    Action
                                                        </telerik:RadLabel>
                                                    </headertemplate>
                                                        <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                                        <itemtemplate>
                                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                            CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                                            ToolTip="Edit"></asp:ImageButton>
                                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                    </itemtemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                        <%--  <div id="divPage" style="position: relative;">
                                            <table width="100%" border="0" class="datagrid_pagestyle">
                                                <tr>
                                                    <td nowrap align="center">
                                                        <telerik:RadLabel ID="lblPagenumber" runat="server">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblPages" runat="server">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblRecords" runat="server">
                                                        </telerik:RadLabel>&nbsp;&nbsp;
                                                    </td>
                                                    <td nowrap align="left" width="50px">
                                                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                                    </td>
                                                    <td width="20px">&nbsp;
                                                    </td>
                                                    <td nowrap align="right" width="50px">
                                                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                                    </td>
                                                    <td nowrap align="center">
                                                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                                        </asp:TextBox>
                                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                                            Width="40px"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>--%>

                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <eluc:Confirm ID="ucConfirmComplete" runat="server" OnConfirmMesage="btnComplete_Click"
            OKText="Yes" CancelText="No" />
    </form>
</body>
</html>
