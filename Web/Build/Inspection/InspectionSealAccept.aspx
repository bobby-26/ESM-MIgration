<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealAccept.aspx.cs" Inherits="InspectionSealAccept" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Accept</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }

            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSeal.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealStatusReport" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
         <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
         <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:TabStrip ID="MenuSeal" runat="server" OnTabStripCommand="MenuSeal_TabStripCommand"></eluc:TabStrip>
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:VesselByCompany ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" AutoPostBack="true"
                                    VesselsOnly="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSealType" runat="server" Text="Seal Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Quick ID="ucSealType" Width="200px" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="87" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFromSealNo" runat="server" Text="From Seal Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFromSealNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblToSealNo" runat="server" Text="To Seal Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtToSealNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ucStatus" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    HardTypeCode="197" AutoPostBack="true" ShortNameFilter="SFO,WMS,ISS,NRD" />
                            </td>
                        </tr>
                    </table>
                    <eluc:TabStrip ID="MenuSealExport" runat="server" OnTabStripCommand="MenuSealExport_TabStripCommand"></eluc:TabStrip>
                    <%-- <asp:GridView ID="gvSeal" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnRowCommand="gvSeal_RowCommand" 
                        Width="100%" CellPadding="3" OnRowDataBound="gvSeal_RowDataBound" ShowHeader="true"
                        EnableViewState="false" OnSorting="gvSeal_Sorting" AllowSorting="true" DataKeyNames="FLDSEALID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSeal" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" Width="99.8%"
                        OnNeedDataSource="gvSeal_NeedDataSource">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDSEALID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                            <Columns>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="40px" />
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" Width="80px" />
                                    <HeaderTemplate>
                                        <telerik:RadCheckBox ID="chkAllSeal" runat="server" Text="Select All" AutoPostBack="true"
                                            OnPreRender="CheckAll" />
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Seal Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>

                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDSEALNO"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblVesselHeader" runat="server">Vessel</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblSealTypeHeader" runat="server">Seal Type</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDSEALTYPENAME"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblStatusHeader" runat="server">Status</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDSTATUSNAME"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

            <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
            <%-- <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
            CancelText="No" />--%>
             </telerik:RadAjaxPanel>
    </form>
</body>
</html>
