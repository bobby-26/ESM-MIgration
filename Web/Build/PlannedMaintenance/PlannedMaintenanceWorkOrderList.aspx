<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderList.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderList" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />


        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--<asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>

        <table width="90%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text="Work Order Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWorkorderNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblWorkorderName" runat="server" Text="Work Order Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWorkorderName" runat="server" CssClass="input" Width="250"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtComponentNumber" runat="server" CssClass="input" Width="150"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblmonth" runat="server" Text="Month"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList ID="ddlMonth" runat="server" CssClass="input" AutoPostBack="false">
                        <Items>
                            <telerik:DropDownListItem Value="" Text="--Select--" />
                            <telerik:DropDownListItem Value="1" Text="Jan" />
                            <telerik:DropDownListItem Value="2" Text="Feb" />
                            <telerik:DropDownListItem Value="3" Text="Mar" />
                            <telerik:DropDownListItem Value="4" Text="Apr" />
                            <telerik:DropDownListItem Value="5" Text="May" />
                            <telerik:DropDownListItem Value="6" Text="Jun" />
                            <telerik:DropDownListItem Value="7" Text="Jul" />
                            <telerik:DropDownListItem Value="8" Text="Aug" />
                            <telerik:DropDownListItem Value="9" Text="Sep" />
                            <telerik:DropDownListItem Value="10" Text="Oct" />
                            <telerik:DropDownListItem Value="11" Text="Nov" />
                            <telerik:DropDownListItem Value="12" Text="Dec" />
                        </Items>
                    </telerik:RadDropDownList>
                </td>
                <td>
                    <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ddlyear" runat="server" CssClass="input" QuickTypeCode="55" Width="100px" AutoPostBack="true" />
                </td>
            </tr>
        </table>

        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <eluc:TabStrip ID="MenuDivWorkOrderList" runat="server" OnTabStripCommand="MenuDivWorkOrderList_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender"
                OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView AllowPaging="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCOMPONENTJOBID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Work Order No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERNUMBER">
                            <HeaderStyle Width="113px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Order Title" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERNAME">
                            <HeaderStyle Width="320px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentJobID" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTJOBID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJobID" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDJOBID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDTKEY"] %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="Select" CommandArgument='<%#Container.DataItem %>'
                                    Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                                <%--<asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER">
                            <HeaderStyle Width="113px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME">
                            <HeaderStyle Width="240px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Class Code">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCLASSCODE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency">
                            <HeaderStyle Width="155px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Done On" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKDONEDATE">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDWORKDONEDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Priority" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDPLANINGPRIORITY">
                            <HeaderStyle Width="65px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Resp Discipline" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDDISCIPLINENAME">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" ScrollHeight="" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
