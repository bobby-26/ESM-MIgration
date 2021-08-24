<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPettyCashGeneral.aspx.cs"
    Inherits="VesselAccountsPettyCashGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Log</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
            function unconfirm(args) {
                if (args) {
                    __doPostBack("<%=btnunconfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPettyCash" runat="server" OnTabStripCommand="MenuPettyCash_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            <asp:Button ID="btnconfirm" runat="server" Text="confirm" OnClick="LockCTM_Confirm" />
            <asp:Button ID="btnunconfirm" runat="server" Text="unconfirm" OnClick="UnLockCTM_Confirm" />
            <eluc:TabStrip ID="MenuPettyCash1" runat="server" OnTabStripCommand="MenuPettyCash1_TabStripCommand"></eluc:TabStrip>
            <table width="50%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbllog" runat="server" Text="Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtlog" runat="server" Text="" ReadOnly="true" Enabled="false" Width="240px"></telerik:RadTextBox></td>
                
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCTM" Width="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                            CellSpacing="0" GridLines="None" OnItemDataBound="gvCTM_ItemDataBound" EnableHeaderContextMenu="true"
                            ShowFooter="true" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvCTM_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="">

                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDPURPOSE"] %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblTotal" runat="server" Text="Total" Font-Bold="true"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Receipts">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <%#",1,".Contains("," + ((DataRowView)Container.DataItem)["FLDCREDITDEBIT"].ToString() + ",") ? ((DataRowView)Container.DataItem)["FLDAMOUNT"] : string.Empty%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Payments">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <%#!",1,".Contains("," + ((DataRowView)Container.DataItem)["FLDCREDITDEBIT"].ToString() + ",") ? ((DataRowView)Container.DataItem)["FLDAMOUNT"] : string.Empty%>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblTotalamount" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </FooterTemplate>
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
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <br />
            <div id="div1" runat="server">
                <table width="50%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Receipts"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="(+)"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtReceipts" runat="server" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true" Width="90px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Payments"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="(-)"></telerik:RadLabel>
                        </td>

                        <td>
                            <telerik:RadTextBox ID="txtPayments" runat="server" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true" Width="90px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel5" runat="server" Text="Closing Balance"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel6" runat="server" Text="(=)"></telerik:RadLabel>
                        </td>

                        <td>
                            <telerik:RadTextBox ID="txtclosingbalance" runat="server" CssClass="readonlytextbox txtNumber"
                                ReadOnly="true" Width="90px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <br />

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
