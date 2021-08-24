<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselComponentExceptionRAList.aspx.cs"
    Inherits="PlannedMaintenanceVesselComponentExceptionRAList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>RA Exception List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvParameterList.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

        </script>
    </telerik:RadCodeBlock>
</head>
<body>

    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true" Width="100%">
            <telerik:RadNotification ID="RadNotification1" runat="server" AutoCloseDelay="2000" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"
                EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="Center" Font-Bold="true" Font-Size="X-Large" Animation="Fade" BackColor="#99ccff" ShowTitleMenu="false">
            </telerik:RadNotification>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table>
                <tr>
                    <td><telerik:RadLabel runat="server" ID="lblvessel" Text="Vessel"></telerik:RadLabel></td>
                    <td><eluc:Vessel ID="ucVessel" runat="server" AssignedVessels="true" Entitytype="VSL" VesselsOnly="true"
                            AutoPostBack="true" OnTextChangedEvent="ucVessel_TextChangedEvent" Width="200px" /></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuParameterList" runat="server" OnTabStripCommand="MenuParameterList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvParameterList" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" 
                CellSpacing="0" GridLines="None" EnableViewState="true" AllowMultiRowEdit="true" OnNeedDataSource="gvParameterList_NeedDataSource"
                Width="100%" OnItemCommand="gvParameterList_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component Number" HeaderStyle-Width="25%">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblComponentNumber" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"] %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblcomponentid" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTID"] %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblglobalRAid" Text='<%#((DataRowView)Container.DataItem)["FLDGLOBALRAID"] %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblvesselid" Text='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="65%">
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblComponentName" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Map" CommandName="MAP" ID="cmdMap" ToolTip="Map" >
                                <span class="icon"><i class="fas fa-user-check-approved"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
