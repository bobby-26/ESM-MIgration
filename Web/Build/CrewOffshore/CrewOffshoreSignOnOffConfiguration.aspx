<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSignOnOffConfiguration.aspx.cs" Inherits="CrewOffshoreSignOnOffConfiguration" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>RH Lock/Unlock</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
          <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvSignOnOffConfiguration.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSignOnOffConfiguration" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="CrewQuery" runat="server" TabStrip="true" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width="200px" />
                    </td>
                </tr>
            </table>
        </div>
        <eluc:TabStrip ID="MenuSignOnOffConfiguration" runat="server" OnTabStripCommand="SignOnOffConfiguration_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
        <%--         <asp:GridView ID="gvSignOnOffConfiguration" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowCreated="gvSignOnOffConfiguration_RowCreated" Width="100%" CellPadding="3" OnRowCommand="gvSignOnOffConfiguration_RowCommand"
                    OnRowDataBound="gvSignOnOffConfiguration_ItemDataBound" OnRowCancelingEdit="gvSignOnOffConfiguration_RowCancelingEdit"
                    OnRowUpdating="gvSignOnOffConfiguration_RowUpdating" OnRowEditing="gvSignOnOffConfiguration_RowEditing"
                    ShowFooter="true" ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSignOnOffConfiguration" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvSignOnOffConfiguration_NeedDataSource"
                OnItemCommand="gvSignOnOffConfiguration_ItemCommand"
                OnItemDataBound="gvSignOnOffConfiguration_ItemDataBound1"
                OnUpdateCommand="gvSignOnOffConfiguration_UpdateCommand"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                        <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <telerik:GridTemplateColumn HeaderText="ID">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConfigurationId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGURATIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DMR Operational Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <asp:CheckBox ID="chkEnableDMRYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDDMROPERATIONALYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkEnableDMRYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDDMROPERATIONALYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Crew Operational Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <asp:CheckBox ID="chkEnableCrewYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCREWENABLEDYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkEnableCrewYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCREWENABLEDYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Crew Sign On/Off Access By">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnOffAccessBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFACCESSBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlSignOnOffAccessByEdit" runat="server" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                        <telerik:RadComboBoxItem Text="Office" Value="0" />
                                        <telerik:RadComboBoxItem Text="Vessel" Value="1" />
                                    </Items>

                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Laid up date">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLaidup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLAIDUPDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:UserControlDate ID="txtLaidupDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLAIDUPDATE","{0:dd/MMM/yyyy}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Re-Activate Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReactiveDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREACTIVEDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:UserControlDate ID="txtReactiveDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREACTIVEDATE","{0:dd/MMM/yyyy}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="UPDATECHEKLICT"
                                    CommandName="UPDATECHEKLICT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdchecklist"
                                    ToolTip="Refresh checklist">
                                    <span class="icon"><i class="fas fa-redo-refresh"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save">
                                      <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
        </telerik:RadAjaxPanel>




    </form>
</body>
</html>
