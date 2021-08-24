<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRPMSOverdueDetails.aspx.cs" Inherits="CrewOffshoreDMRPMSOverdueDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PMS Overdue</title>
    <telerik:RadCodeBlock ID="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">

          <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
          
            <%--  <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>
            <div id="divGrid" runat="server" style="position: relative; overflow: auto; z-index: 0">
                <%--  <asp:GridView ID="gvDMRPMSOverdue" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" RowStyle-Wrap="false" Width="100%" CellPadding="3" ShowHeader="true"
                        OnRowDataBound="gvDMRPMSOverdue_RowDataBound" EnableViewState="false" AllowSorting="true">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <Columns>--%>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvDMRPMSOverdue" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    OnItemDataBound="gvDMRPMSOverdue_ItemDataBound"
                    OnNeedDataSource="gvDMRPMSOverdue_NeedDataSource"
                    OnItemCommand ="gvDMRPMSOverdue_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn>
                                <headertemplate>
                                     <telerik:RadLabel ID="lblWONumberHeader" runat="server" Text="Work Order Number"></telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <asp:LinkButton ID="lnkWONumber"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>' ></asp:LinkButton>
                                    <telerik:RadLabel ID="lblWorkOrderId"   Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>' ></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <headertemplate>
                                     <telerik:RadLabel ID="lblWOTitleHeader" runat="server" Text="Work Order Title"></telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblWorkOrderTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>' ></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <headertemplate>
                                     <telerik:RadLabel ID="lblComponentNumberHeader" runat="server" Text="Component Number"></telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblComponentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>' ></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <headertemplate>
                                     <telerik:RadLabel ID="lblComponentNameHeader" runat="server" Text="Component Name"></telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>' ></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <headertemplate>
                                     <telerik:RadLabel ID="lblDueDateHeader" runat="server" Text="Due Date"></telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>' ></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
            </div>
        
        </div>

    </form>
</body>
</html>
