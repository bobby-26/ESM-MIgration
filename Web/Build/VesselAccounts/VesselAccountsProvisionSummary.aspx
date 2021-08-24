<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsProvisionSummary.aspx.cs"
    Inherits="VesselAccountsProvisionSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Provision</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="97%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPettyCash" runat="server" OnTabStripCommand="MenuPettyCash_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbllog" runat="server" Text="Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtlog" runat="server" Text="" ReadOnly="true" Enabled="false" Width="240px"></telerik:RadTextBox></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCTM" runat="server" OnTabStripCommand="MenuCTM_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBondReq" Height="96%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvBondReq_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Order No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDREFERENCENO"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Required Port">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSEAPORTNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Order Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDORDERDATE", "{0:dd/MMM/yyyy}"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received On">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:dd/MMM/yyyy}"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Currency">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDPAYMENTCURRENCYCODE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDAMOUNTPAID"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDTOTALAMOUNT"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Currency Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDAMOUNTPAIDINREPORT"]%>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
