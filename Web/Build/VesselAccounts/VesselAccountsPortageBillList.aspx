<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPortageBillList.aspx.cs" Inherits="VesselAccountsPortageBillList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>BOW</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function confirmunlock(args) {
                if (args) {
                    __doPostBack("<%=confirmunlock.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager2" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="FinalizePortageBill_Confirm" />
        <asp:Button ID="confirmunlock" runat="server" Text="confirm" OnClick="UnLockPortageBill_Confirm" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuPB" runat="server" OnTabStripCommand="MenuPB_TabStripCommand" Title="Portage Bill"></eluc:TabStrip>
        <telerik:RadAjaxPanel runat="server" ID="pnlNTBRManager" Height="90%" EnableAJAX="false">

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblClosingDate" runat="server" Text="Closing Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtClosginDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPortageBillHistory" runat="server" Text="Portage Bill History"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlHistory" runat="server" CssClass="input" DataTextField="FLDCLOSINGDATE"
                            DataValueField="FLDPORTAGEBILLID" AutoPostBack="true" OnSelectedIndexChanged="ddlHistory_SelectedIndexChanged">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuPBExcel" runat="server" OnTabStripCommand="MenuPBExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvPB" runat="server" AutoGenerateColumns="False" Height="85%" ShowHeader="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" ShowFooter="true"
                OnNeedDataSource="gvPB_NeedDataSource" OnItemDataBound="gvPB_ItemDataBound"
                OnExportCellFormatting="gvPB_ExportCellFormatting" OnInfrastructureExporting="gvPB_InfrastructureExporting">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100%"
                    GroupHeaderItemStyle-Wrap="false" Width="100%" CommandItemDisplay="Top" DataKeyNames="FLDPORTAGEBILLID,FLDSIGNONOFFID,FLDEMPLOYEEID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <CommandItemStyle Height="20px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Earnings" Name="Earnings" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Deductions" Name="Deductions" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="Image" HeaderStyle-Width="30px" ItemStyle-Width="30px" Exportable="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFinalBal" runat="server" ImageUrl="<%$ PhoenixTheme:images/balance-mismatch.png %>" ToolTip="Final Balance Mismatch" Visible="false" />
                                <asp:Image ID="imgNegBal" runat="server" ImageUrl="<%$ PhoenixTheme:images/negative-balance.png %>" ToolTip="Negative Balance" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." UniqueName="Fileno" HeaderStyle-Width="60px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDFILENO"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" UniqueName="Name" HeaderStyle-Width="220px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEmployeename" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"].ToString() %>'></asp:LinkButton>
                                <%--<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"].ToString() %>         --%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" UniqueName="Rank" HeaderStyle-Width="60px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDRANKCODE"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From" UniqueName="From" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDFROMDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To" UniqueName="To" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDTODATE"])%>
                            </ItemTemplate>
                            <FooterStyle Wrap="false" Font-Bold="true" />
                            <FooterTemplate>
                                Total
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days" UniqueName="Days" HeaderStyle-Width="46px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDDAYS"].ToString()%>
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
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" Scrolling-EnableNextPrevFrozenColumns="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="6" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
                <ExportSettings Excel-Format="Xlsx">
                </ExportSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
