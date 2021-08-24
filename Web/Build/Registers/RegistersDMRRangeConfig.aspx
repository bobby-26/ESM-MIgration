<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDMRRangeConfig.aspx.cs"
    Inherits="Registers_RegistersDMRRangeConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOffshoreVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Noon Report Range Config</title>
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

    <form id="frmNoonReportRangeConfig" runat="server" submitdisabledcontrols="true">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>
            </div>

            <div id="divFind" style="position: relative; z-index: 2">
                <table id="tblSearch" width="100%">
                    <tr>
                        <%--<td>
                                <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                            </td>
                            <td>
                               <asp:TextBox ID="txtAgentName" CssClass="input" runat="server" />
                            </td>--%>
                        <td>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                                VesselsOnly="true" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTargetVessel" runat="server" Text="Copy to Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucTargetVessel" runat="server" CssClass="input" AutoPostBack="true"
                                VesselsOnly="true" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <%--<div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuDMRRangeConfig" runat="server" OnTabStripCommand="MenuDMRRangeConfig_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>
            <u><b>
                <telerik:RadLabel ID="lblMeteorologyData" runat="server" Text="Meteorology Data"></telerik:RadLabel>
            </b></u>
            <br />
            <div id="divGrid" style="position: relative; z-index: 1" width="100%">
                <%-- <asp:GridView ID="gvMetrology" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowDataBound="gvMetrology_ItemDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvMetrology" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None"
                    OnItemDataBound="gvMetrology_ItemDataBound1"
                    OnNeedDataSource="gvMetrology_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDCONFIGID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Field Name">
                                <ItemStyle Wrap="True" Width="45%" HorizontalAlign="Left"></ItemStyle>
                             
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblConfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYTEXT") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblcolumnname" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel" Visible="false">
                                <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Min Value">
                                <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                        CssClass="input" MaxLength="5" Width="80px" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Max Value">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                        CssClass="input" MaxLength="5" Width="80px" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Effective Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <eluc:Date ID="txtEffectiveDate" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEFFECTIVEDATE") %>' DatePicker="true" Enabled="true" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Active Y/N">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="170px"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkRequiredEdit" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUIREDYN").ToString().Equals("1") ? true : false %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                      <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="History" 
                                    CommandName="HISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdHistory"
                                    ToolTip="History">
                                    <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
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
                <telerik:RadLabel ID="lblFOFlowmeter" runat="server" Text="FO Flow Meter Readings"></telerik:RadLabel>
            </b></u>
            <br />
            <div id="divGrid1" style="position: relative; z-index: 1" width="100%">
               <%-- <asp:GridView ID="gvFOFlowmeter" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDataBound="gvFOFlowmeter_ItemDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                 <telerik:RadGrid RenderMode="Lightweight" ID="gvFOFlowmeter" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None"
                    OnItemDataBound="gvFOFlowmeter_ItemDataBound1"
                    OnNeedDataSource="gvFOFlowmeter_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDCONFIGID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                        <telerik:GridTemplateColumn HeaderText="Field Name">
                            <ItemStyle Wrap="True" Width="45%" HorizontalAlign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYTEXT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcolumnname" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Value">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Value">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Effective Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Date ID="txtEffectiveDate" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEFFECTIVEDATE") %>' DatePicker="true" Enabled="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="170px"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkRequiredEdit" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUIREDYN").ToString().Equals("1") ? true : false %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="History" 
                                    CommandName="HISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdHistory"
                                    ToolTip="History">
                                    <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
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
                <telerik:RadLabel ID="lblOperationCons" runat="server" Text="Operation Summary Cons/Hr"></telerik:RadLabel>
            </b></u>
            <br />
            <div id="div2" style="position: relative; z-index: 1" width="100%">
               <%-- <asp:GridView ID="gvOperationCons" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDataBound="gvOperationCons_ItemDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvOperationCons" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None"
                    OnItemDataBound="gvOperationCons_ItemDataBound1"
                    OnNeedDataSource="gvOperationCons_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDCONFIGID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                        <telerik:GridTemplateColumn HeaderText="Field Name">
                            <ItemStyle Wrap="True" Width="45%" HorizontalAlign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYTEXT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcolumnname" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Value">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Value">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Effective Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Date ID="txtEffectiveDate" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEFFECTIVEDATE") %>' DatePicker="true" Enabled="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="170px"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkRequiredEdit" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUIREDYN").ToString().Equals("1") ? true : false %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="History" 
                                    CommandName="HISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdHistory"
                                    ToolTip="History">
                                    <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
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
                <telerik:RadLabel ID="lblOperationSpeed" runat="server" Text="Operation Summary Speed"></telerik:RadLabel>
            </b></u>
            <br />
            <div id="div3" style="position: relative; z-index: 1" width="100%">
              <%--  <asp:GridView ID="gvOperationSpeed" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDataBound="gvOperationSpeed_ItemDataBound" Width="100%" CellPadding="3"
                    ShowHeader="true" EnableViewState="false" AllowSorting="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                 <telerik:RadGrid RenderMode="Lightweight" ID="gvOperationSpeed" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None"
                    OnItemDataBound="gvOperationSpeed_ItemDataBound1"
                    OnNeedDataSource="gvOperationSpeed_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDCONFIGID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                        <telerik:GridTemplateColumn HeaderText="Field Name">
                            <ItemStyle Wrap="True" Width="45%" HorizontalAlign="Left"></ItemStyle>
                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYTEXT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcolumnname" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Value">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Value">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Effective Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Date ID="txtEffectiveDate" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEFFECTIVEDATE") %>' DatePicker="true" Enabled="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="170px"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkRequiredEdit" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUIREDYN").ToString().Equals("1") ? true : false %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="History" 
                                    CommandName="HISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdHistory"
                                    ToolTip="History">
                                    <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
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
                <telerik:RadLabel ID="lblBulks" runat="server" Text="Bulks"></telerik:RadLabel>
            </b></u>
            <br />
            <div id="div5" style="position: relative; z-index: 1" width="100%">
               <%-- <asp:GridView ID="gvBulks" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDataBound="gvBulks_ItemDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                 <telerik:RadGrid RenderMode="Lightweight" ID="gvBulks" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None"
                    OnItemDataBound="gvBulks_ItemDataBound1"
                    OnNeedDataSource="gvBulks_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDCONFIGID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                        <telerik:GridTemplateColumn HeaderText="Field Name">
                            <ItemStyle Wrap="True" Width="45%" HorizontalAlign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYTEXT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcolumnname" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Value">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Value">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Effective Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Date ID="txtEffectiveDate" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEFFECTIVEDATE") %>' DatePicker="true" Enabled="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="170px"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkRequiredEdit" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUIREDYN").ToString().Equals("1") ? true : false %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                 <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="History" 
                                    CommandName="HISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdHistory"
                                    ToolTip="History">
                                    <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
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
                <telerik:RadLabel ID="lblOthers" runat="server" Text="Others"></telerik:RadLabel>
            </b></u>
            <br />
            <div id="div4" style="position: relative; z-index: 1" width="100%">
               <%-- <asp:GridView ID="gvOthers" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDataBound="gvOthers_ItemDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                 <telerik:RadGrid RenderMode="Lightweight" ID="gvOthers" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None"
                    OnItemDataBound="gvOthers_ItemDataBound1"
                    OnNeedDataSource="gvOthers_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDCONFIGID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                        <telerik:GridTemplateColumn HeaderText="Field Name">
                            <ItemStyle Wrap="True" Width="45%" HorizontalAlign="Left"></ItemStyle>
                         
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYTEXT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcolumnname" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Value">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Value">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="80px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Effective Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <eluc:Date ID="txtEffectiveDate" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEFFECTIVEDATE") %>' DatePicker="true" Enabled="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="170px"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkRequiredEdit" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUIREDYN").ToString().Equals("1") ? true : false %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="History" 
                                    CommandName="HISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdHistory"
                                    ToolTip="History">
                                    <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
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
        </div>
    </form>

</body>
</html>
