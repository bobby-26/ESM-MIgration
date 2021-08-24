<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceCompJobSummaryDetail.aspx.cs" Inherits="PlannedMaintenanceCompJobSummaryDetail" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Summary</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= GvPMS.ClientID %>"));
                }, 200);
            }
        </script>
       
    </telerik:RadCodeBlock>

</head>
<body onresize="Resize();" onload="Resize();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="GvPMS">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="GvPMS"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip id="menuPms" runat="server" OnTabStripCommand="menuPms_TabStripCommand" />
        <telerik:RadGrid ID="GvPMS" runat="server" AllowPaging="true" AllowCustomPaging="true" GridLines="None" AllowSorting="true"
            OnNeedDataSource="GvPMS_NeedDataSource1" OnItemDataBound="GvPMS_ItemDataBound" EnableHeaderContextMenu="true"
            OnPreRender="GvPMS_PreRender" OnItemCommand="GvPMS_ItemCommand" OnSortCommand="GvPMS_SortCommand"
            AllowFilteringByColumn="true" EnableLinqExpressions="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <ColumnGroups>
                </ColumnGroups>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowFiltering="true" AllowSorting="true" ShowSortIcon="true" DataField="FLDVESSELNAME" SortExpression="FLDVESSELNAME"
                        HeaderStyle-Width="80px" ShowFilterIcon="false" UniqueName="FLDVESSELNAME" FilterControlWidth="100%" FilterDelay="3000">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component" AllowFiltering="true" AllowSorting="true" ShowSortIcon="true" DataField="FLDCOMPONENT" SortExpression="FLDCOMPONENT"
                        HeaderStyle-Width="70px" UniqueName="FLDCOMPONENT" ShowFilterIcon="false" FilterControlWidth="100%" FilterDelay="3000">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCompNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job" AllowFiltering="true" AllowSorting="false" ShowSortIcon="true" 
                        HeaderStyle-Width="150px" UniqueName="FLDJOB" ShowFilterIcon="false" FilterControlWidth="100%" FilterDelay="3000">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkJobcode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDJOB") %>' CommandName="JOB"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Category" AllowFiltering="true" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="120px" 
                        UniqueName="FLDCATEGORY" ShowFilterIcon="false" FilterDelay="2000">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblJobCategory" runat="server" RenderMode="Lightweight" OnDataBinding="cblJobCategory_DataBinding" AutoPostBack="true" Width="100%" AppendDataBoundItems="true"
                                    OnSelectedIndexChanged="cblJobCategory_SelectedIndexChanged" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("FLDCATEGORY").CurrentFilterValue %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Frequency" AllowFiltering="false" AllowSorting="true" ShowSortIcon="true" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Done" AllowFiltering="false" AllowSorting="true" ShowSortIcon="true" HeaderStyle-Width="70px" DataField="FLDJOBLASTDONEDATE" SortExpression="FLDJOBLASTDONEDATE">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastDone" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDJOBLASTDONEDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Next Due" AllowFiltering="true" AllowSorting="true" ShowSortIcon="true" UniqueName="FLDJOBNEXTDUEDATE" DataField="FLDJOBNEXTDUEDATE" SortExpression="FLDJOBNEXTDUEDATE"
                        HeaderStyle-Width="100px" ShowFilterIcon="false" FilterDelay="3000">
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FROM"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["TO"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDJOBNEXTDUEDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                            tableView.filter("FLDJOBNEXTDUEDATE", fromDate + "~" + toDate, "Between");
                                        }
                                        function FormatSelectedDate(picker) {
                                            var date = picker.get_selectedDate();
                                            var dateInput = picker.get_dateInput();
                                            var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());

                                            return formattedDate;
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNextDue" runat="server" ForeColor="Red" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDJOBNEXTDUEDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false" AllowSorting="false" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="MaintenanceForm"
                                CommandName="MAINTENANCEFORM" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdTemplates"
                                ToolTip="Reporting Templates" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-file"></i></span>
                            </asp:LinkButton>
                            <asp:ImageButton runat="server" ID="cmdAttachments" ToolTip="Attachment" 
                                CommandName="ATTACHMENTS" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                            <asp:LinkButton runat="server" AlternateText="Parts"
                                CommandName="PARTS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdParts"
                                ToolTip="Parts" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-cogs"></i></span>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="false">
                <Selecting AllowRowSelect="false" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                Sys.Application.add_load(function () {
                    setTimeout(function () {
                        TelerikGridResize($find("<%= GvPMS.ClientID %>"));
                    }, 200);
                });
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
