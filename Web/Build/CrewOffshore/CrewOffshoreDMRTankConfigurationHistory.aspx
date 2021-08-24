<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRTankConfigurationHistory.aspx.cs"
    Inherits="CrewOffshoreDMRTankConfigurationHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tank History</title>
    <telerik:RadCodeBlock ID="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLocation" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

            <br />
            <u><b>
                <telerik:RadLabel ID="lblCargo" runat="server" Text="Cargo"></telerik:RadLabel>
            </b></u>
            <br />
            <br />

            <eluc:TabStrip ID="MenuTankHistory" runat="server" OnTabStripCommand="MenuTankHistory_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative; z-index: 0">
                <%--  <asp:GridView ID="gvTankHistory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvTankHistory_ItemDataBound" AllowSorting="true"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvTankHistory" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    OnNeedDataSource="gvTankHistory_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed"  Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" HeaderText="Date" />
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReportDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREPORTDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Port/Location">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPortLocation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Operation">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOperation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Product">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblProduct" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Quantity">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                             
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQuantityName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ROB">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblROBName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBCUM") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>


            </div>

            <br />
            <u><b>
                <telerik:RadLabel ID="lblTankCleaning" runat="server" Text="Tank Cleaning"></telerik:RadLabel>
            </b></u>
            <br />
            <br />
           
                <eluc:TabStrip ID="MenuTankCleaned" runat="server" OnTabStripCommand="MenuTankCleaned_TabStripCommand"></eluc:TabStrip>
           
            <table cellpadding="2" cellspacing="2" width="60%">
                <tr>
                    <td style="vertical-align: top;" width="100%">
                        <div id="div2" style="position: relative; z-index: 0">
                            <%--<asp:GridView ID="gvTankCleaned" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" OnRowDataBound="gvTankCleaned_ItemDataBound" AllowSorting="true"
                                    ShowHeader="true" EnableViewState="false">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvTankCleaned" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                CellSpacing="0" GridLines="None"
                                OnNeedDataSource="gvTankCleaned_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed"  Height="10px">
                                    <HeaderStyle Width="102px" />
                                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                        <telerik:GridTemplateColumn HeaderText="Date">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                          
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblReportDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Last Product Loaded"> 
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                      
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblProduct" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>

                        </div>

                    </td>
                </tr>
            </table>
            <br />
            <u><b>
                <telerik:RadLabel ID="lblAlertPostponing" runat="server" Text="Alert postponing"></telerik:RadLabel>
            </b></u>
            <br />
            <br />
          
                <eluc:TabStrip ID="MenuAlertPostpone" runat="server" OnTabStripCommand="MenuAlertPostpone_TabStripCommand"></eluc:TabStrip>
        
            <table cellpadding="2" cellspacing="2" width="60%">
                <tr>
                    <td style="vertical-align: top;" width="100%">
                        <div id="div3" style="position: relative; z-index: 0">
                            <%-- <asp:GridView ID="gvAlertPostpone" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3"  AllowSorting="true"
                                    ShowHeader="true" EnableViewState="false">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>

                            <telerik:RadGrid RenderMode="Lightweight" ID="gvAlertPostpone" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                CellSpacing="0" GridLines="None"
                                OnNeedDataSource="gvAlertPostpone_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed"  Height="10px">
                                    <HeaderStyle Width="102px" />
                                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                        <telerik:GridTemplateColumn HeaderText="Date">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                           
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblReportDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Product">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                          
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblProduct" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Remarks">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                         
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblpostponeremarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTPONEREMARKS") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>


                        </div>

                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
