<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewIncidentsEmployeeList.aspx.cs" Inherits="CrewIncidentsEmployeeList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewList" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="MenuCrewList" runat="server" OnTabStripCommand="CrewList_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncidentDate" runat="server" Text="Incident Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtIncidentDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                </tr>
            </table>
            <br />
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%--  <asp:GridView ID="gvCrewList" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnRowDataBound="gvCrewList_RowDataBound"
                    CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewList" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvCrewList_NeedDataSource"
                    OnItemCommand="gvCrewList_ItemCommand"
                    OnItemDataBound="gvCrewList_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Select">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Passport No">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPASSPORTNO")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Employee No">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEECODE")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Nationality">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITYNAME")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sign On">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sign Off">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE", "{0:dd/MMM/yyyy}") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </div>
            <eluc:Status ID="ucStatus" runat="server" />
        </div>

    </form>
</body>
</html>
