<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRVoyageROBDetails.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreDMRVoyageROBDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewOffshore" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPStatus" Src="~/UserControls/UserControlSEPStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOffshoreVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVoyageData" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div style="z-index: 1;">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="MenuVoyageTap" TabStrip="true" runat="server" OnTabStripCommand="VoyageTap_TabStripCommand"></eluc:TabStrip>
            <telerik:RadButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <div class="subHeader">
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="VoyageNewSaveTap_TabStripCommand"></eluc:TabStrip>
                </div>
            </div>
        </div>
        <br clear="all" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>

                <telerik:AjaxSetting AjaxControlID="gvTradingArea">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ucUnitEdit" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <div>

                <table>
                    <tr runat="server" visible="false">
                        <td>
                            <telerik:RadLabel ID="lblAtSea" runat="server" Text="At Sea">
                            </telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAtSea" runat="server" />
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                            <telerik:RadLabel ID="lblLocation" runat="server" Text="Location">
                            </telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input" Width="230px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDeckConditions" runat="server" Text="Deck Condition Remarks">
                            </telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtDeckConditionRemarks" runat="server" TextMode="MultiLine" CssClass="input" Width="230px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div id="divGrid" style="position: relative; z-index: 0">
                <%-- <asp:GridView ID="gvTradingArea" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvTradingArea_RowCommand" OnRowDataBound="gvTradingArea_ItemDataBound"
                    OnRowCancelingEdit="gvTradingArea_RowCancelingEdit" OnRowDeleting="gvTradingArea_RowDeleting"
                    AllowSorting="true" OnRowEditing="gvTradingArea_RowEditing" OnRowUpdating="gvTradingArea_RowUpdating"
                    ShowFooter="true" ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>--%>
                <telerik:RadGrid ID="gvTradingArea" RenderMode="Lightweight" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    OnNeedDataSource="gvTradingArea_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDVOYAGEID" TableLayout="Fixed" >
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
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
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblTradingAreaHeader" Text="Liquids" runat="server">
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOilTypeName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMANUALOILTYPE")%>'></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOthersOilType" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOTHERSOILTYPE")%>' CssClass="input" Width="350px"></telerik:RadTextBox>
                                    <telerik:RadLabel ID="lblOilTypeCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPECODE")%>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblUnitHeader" Text="Unit" runat="server">
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <eluc:Unit ID="ucUnitEdit" runat="server" AppendDataBoundItems="true" UnitList='<%#PhoenixRegistersUnit.ListDMRProductUnit() %>'
                                        SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>' CssClass="input" />
                                    <%--<asp:Literal ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUNITNAME")%>'></asp:Literal>--%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblAmountROB" Text="Amount ROB" runat="server">
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <eluc:Number ID="ucROB" runat="server" Width="65px" CssClass="input" Enabled="true" MaskText="##########"
                                        IsInteger="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROB")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Charter per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
