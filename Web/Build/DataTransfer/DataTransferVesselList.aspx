<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferVesselList.aspx.cs"
    Inherits="Registers_DataTransferVesselList" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
            .divFloatLeft
            {
                height: 53px;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDataTransferVesselList" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuVesselList" runat="server" OnTabStripCommand="VesselList_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <br clear="all" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="divinstruction" DecoratedControls="All" EnableRoundedCorners="true" />
        <div runat="server" id="divinstruction">
            <asp:TextBox ID="lblGuidance" runat="server" Text="" BorderStyle="None" BorderWidth="0px"
                ReadOnly="true" Rows="4" TextMode="MultiLine" Width="100%" Font-Bold="true"></asp:TextBox>
        </div>
        <br clear="all" />
        <eluc:TabStrip ID="MenuFilter" runat="server" OnTabStripCommand="MenuFilter_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselList" runat="server" Height="98.5%"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" OnSortCommand="gvVesselList_SortCommand" OnNeedDataSource="gvVesselList_NeedDataSource"
            OnItemDataBound="gvVesselList_ItemDataBound" OnItemCommand="gvVesselList_ItemCommand"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDVESSELID" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false">
                <HeaderStyle Width="102px" HorizontalAlign="Center" />
                <CommandItemSettings ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true"
                    ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="80px" AllowSorting="true"
                        SortExpression="FLDVESSELNAME">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'>
                            </telerik:RadLabel>
                            <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Export Date" HeaderStyle-Width="134px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDataTransferDateTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPORTDATE") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Import Date" HeaderStyle-Width="134px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDataTransferStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTDATE") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle Width="50px" />
                        <ItemStyle Width="60px" Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="IMPORT" ID="Import" ToolTip="Import">
                                <span class="icon"><i class="fas fa-file-download"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="EXPORT" ID="cmdExport" ToolTip="Export">
                                        <span class="icon"><i class="fas fa-file-upload"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="SCHEDULEDJOBS" ID="cmdJobs" ToolTip="Scheduled Jobs">
                                    <span class="icon"><i class="fas fa-business-time"></i> </span> 
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="HISTORY" ID="cmdHistory" ToolTip="History">
                                        <span class="icon"><i class="fas fa-list-alt"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="ATTACHMENTHISTORY" ID="cmdAttachmentHistory"
                                ToolTip="Attachment History">
                                      <span class="icon"><i class="far fa-list-alt"></i></span> 
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="IMPORTLOG" ID="cmdImportLog" ToolTip="Import Error Log">
                                      <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                    AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <asp:Button runat="server" ID="Button1" OnClick="cmdHiddenSubmit_Click" Visible="false" />
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
