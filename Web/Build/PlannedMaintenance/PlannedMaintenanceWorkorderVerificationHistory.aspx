<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkorderVerificationHistory.aspx.cs"
    Inherits="PlannedMaintenanceWorkorderVerificationHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Verification History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function setSize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvWorkorderList.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = setSize;
            function pageLoad() {
                setSize();
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="setSize()" onload="setSize()">
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true" Width="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWorkOrder" runat="server" TabStrip="true" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvWorkorderList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="true" AllowMultiRowEdit="true" Width="100%"
                OnNeedDataSource="gvWorkorderList_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Comp. No.">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblCompNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNO")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkorderName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlannedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Assigned To">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAssignedTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Verification Rank">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELVERIFICATIONRANK") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Verification">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselVerified" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELVERIFICATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Remarks">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELVERIFYREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supnt Verification">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupntVerified" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEVERIFICATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supnt Remarks">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupntRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEVERIFYREMARKS") %>'></telerik:RadLabel>
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
