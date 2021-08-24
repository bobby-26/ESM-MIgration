<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRVesselMovements.aspx.cs"
    Inherits="CrewOffshoreDMRVesselMovements" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Movements</title>
    <telerik:RadCodeBlock runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLocation" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <div class="subHeader" style="position: relative">
                <div class="subHeader" style="position: relative">
                    <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>

                    <eluc:Status runat="server" ID="ucStatus" />
                </div>
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuNewSaveTabStrip" TabStrip="true" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>
                </div>
            </div>
            <br clear="all" />
            <%--  <asp:GridView ID="gvVesselMovements" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                    EnableViewState="false" OnRowDataBound="gvVesselMovements_ItemDataBound">
                    <FooterStyle CssClass="datagrid_footerstyle" />
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselMovements" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                OnNeedDataSource="gvVesselMovements_NeedDataSource"
                OnItemDataBound ="gvVesselMovements_ItemDataBound1">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>

                        <telerik:GridTemplateColumn>
                            <itemstyle wrap="False" horizontalalign="Left" width="30%"></itemstyle>
                            <footerstyle wrap="false" horizontalalign="Left" width="30%" />
                            <headertemplate>
                                <telerik:RadLabel ID="lblActivityHeader" runat="server" Text="Activity"></telerik:RadLabel>
                            </headertemplate>
                            <itemtemplate>
                                <telerik:RadDropDownList ID="ddlActivityAdd" DefaultMessage="--Select--" runat="server" CssClass="input_mandatory" Width="150px"
                                    DataTextField="FLDTASKNAME" DataValueField="FLDOPERATIONALTASKID">
                                </telerik:RadDropDownList>
                            </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <itemstyle wrap="False" horizontalalign="Left" width="30%"></itemstyle>
                            <footerstyle wrap="false" horizontalalign="Left" width="30%" />
                            <headertemplate>
                                <telerik:RadLabel ID="lblDescriptionHeader" runat="server" Text="Description"></telerik:RadLabel>
                            </headertemplate>
                            <itemtemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" Width="500px" CssClass="input"></telerik:RadTextBox>
                            </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <itemstyle wrap="False" horizontalalign="left" width="10%"></itemstyle>
                            <footerstyle wrap="false" horizontalalign="Left" width="10%" />
                            <headertemplate>
                                <telerik:RadLabel ID="lblTimeHeader" runat="server" Text="Time"></telerik:RadLabel>
                            </headertemplate>
                            <itemtemplate>
                                <eluc:Number ID="txtFromTimeAdd" runat="server" CssClass="input" Width="50px" MaskText="##:##" />
                               
                            </itemtemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle  Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>

    </form>
</body>
</html>
