<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalPmsMaitenanceDue.aspx.cs"
    Inherits="DashboardTechnicalPmsMaitenanceDue" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseWindow() {
                document.getElementById('<%=txtDueDate.ClientID%>').innerHTML = "";
                document.getElementById('<%=ValidationSummary1.ClientID%>').style.display = 'none';
            }
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                    radwindow.close();
                    var masterTable = $find("<%= gvWorkOrder.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>
        <style type="text/css">
        .backcolor {
            background-color: #FFFF03 !important;
        }

        
    </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadSkinManager runat="server"></telerik:RadSkinManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvWorkOrder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvWorkOrder" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="92%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuWorkOrder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuWorkOrder" />
                        <telerik:AjaxUpdatedControl ControlID="gvWorkOrder" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="92%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>        
        <telerik:RadGrid ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemDataBound="gvWorkOrder_ItemDataBound" GroupingEnabled="false"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" AllowFilteringByColumn="true"  OnPreRender="gvWorkOrder_PreRender"
            OnItemCommand="gvWorkOrder_ItemCommand" Height="100%" OnSortCommand="gvWorkOrder_SortCommand" EnableLinqExpressions="false" EnableViewState="true">

            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                AllowFilteringByColumn="True" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID" FilterExpression="">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" UniqueName="FLDVESSELNAME" AllowFiltering="false" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                           <%#((DataRowView)Container.DataItem)["FLDVESSELNAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Critical" AllowSorting="false" UniqueName="FLDISCRITICAL"
                        ShowSortIcon="false" AutoPostBackOnFilter="false" ShowFilterIcon="false" >
                        <ItemStyle HorizontalAlign="Left" Width="20px"></ItemStyle>
                        <HeaderStyle Width="41px" />
                        <FilterTemplate>
                            <telerik:RadCheckBox runat="server" ID="chkIsCritical" OnCheckedChanged="chkIsCritical_CheckedChanged"
                                Checked='<%#GetFilter("ISCRITICAL") == "1" ? true : false %>'>
                            </telerik:RadCheckBox>
                            <br />
                            <telerik:RadLabel ID="lblFIsCritial" runat="server" ToolTip="Is Critical" AssociatedControlID="chkIsCritical"></telerik:RadLabel>                            
                        </FilterTemplate>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDISCRITICAL"].ToString().Equals("1") ? "Yes" : "No"%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component No."  UniqueName="FLDCOMPONENTNUMBER"
                        AllowSorting="true" SortExpression="FLDCOMPONENTNUMBER" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRespId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDPLANNINGDISCIPLINE"]%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERID"]%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCategoryId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDCATEGORYID"]%>'></telerik:RadLabel>
                            <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component" AllowSorting="true" SortExpression="FLDCOMPONENTNAME" UniqueName="FLDCOMPONENTNAME"
                         FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"] + " " + ((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"] + " " + ((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Code & Title" AllowSorting="true" SortExpression="FLDWORKORDERNAME" UniqueName="FLDWORKORDERNAME"
                        FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnktitle" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"] + " - "+ ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"] %>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"] + " - "+ ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Category" AllowSorting="true" UniqueName="FLDJOBCATEGORY"
                        ShowSortIcon="false" FilterControlWidth="99%"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlJobCategory" runat="server" OnDataBinding="ddlJobCategory_DataBinding" AppendDataBoundItems="true"
                                SelectedValue='<%# GetFilter("JCATNAME") %>' OnClientSelectedIndexChanged="JobCategoryIndexChanged">
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function JobCategoryIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            tableView.filter("FLDJOBCATEGORY", args.get_item().get_value(), "EqualTo");
                                        }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDJOBCATEGORY"] %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Maintenance Interval" AllowSorting="false" AllowFiltering="true" ShowFilterIcon="false"
                        UniqueName="FLDFREQUENCYTYPE" AutoPostBackOnFilter="false" HeaderStyle-Width="110px">
                        <FilterTemplate>
                            <telerik:RadTextBox ID="txtFrequency" runat="server" Width="40%" Text='<%# GetFilter("FREQUENCY") %>'></telerik:RadTextBox>
                            <telerik:RadComboBox ID="cblFrequencyType" runat="server" OnDataBinding="cblFrequencyType_DataBinding" AutoPostBack="false" Width="60%" AppendDataBoundItems="true"
                                OnClientSelectedIndexChanged="FrequencyIndexChanged" SelectedValue='<%# GetFilter("FREQUENCYTYPE") %>'>
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function FrequencyIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var frequency = $find('<%# ((GridItem)Container).FindControl("txtFrequency").ClientID %>');
                                        var freqtype = args.get_item().get_value();
                                        tableView.filter("FLDFREQUENCYTYPE", frequency.get_value() + "~" + freqtype, "EqualTo");
                                        }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Responsibility" HeaderStyle-Width="100px" UniqueName="FLDPLANNINGDISCIPLINE"
                        AllowSorting="true" SortExpression="FLDDISCIPLINENAME" FilterControlWidth="98%" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlResponsibility" OnDataBinding="ddlResponsibility_DataBinding" AppendDataBoundItems="true"
                                SelectedValue='<%# GetFilter("ucRank") %>' OnClientSelectedIndexChanged="ResponsibilityIndexChanged"
                                runat="server">
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function ResponsibilityIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            tableView.filter("FLDPLANNINGDISCIPLINE", args.get_item().get_value(), "EqualTo");
                                        }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                         <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Due On" UniqueName="FLDDUEDATE" SortExpression="FLDDUEDATE" HeaderStyle-Width="95px"
                        AllowSorting="true" FilterDelay="2000" AutoPostBackOnFilter="false" CurrentFilterFunction="Between">
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlDueDays" runat="server" Width="98%" SelectedValue='<%# GetFilter("DUE") %>'
                                OnClientSelectedIndexChanged="DueIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Value="" Text="--All--" />
                                    <telerik:RadComboBoxItem Value="0" Text="Over Due" />
                                    <telerik:RadComboBoxItem Value="15" Text="15 Days" />
                                    <telerik:RadComboBoxItem Value="30" Text="30 Days" />
                                    <telerik:RadComboBoxItem Value="60" Text="60 Days" />
                                    <telerik:RadComboBoxItem Value="90" Text="90 Days" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function DueIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDDUEDATE", args.get_item().get_value(), "EqualTo");
                                            //var val = 0;
                                            //if (args.get_item().get_value() != "") {
                                            //    val = args.get_item().get_value();
                                            //    var today = new Date();
                                            //    var newdate = new Date();
                                            //    newdate.setDate(today.getDate() + Number(val));
                                            //    tableView.filter("FLDDUEDATE", today.toShortFormat() + "~" + newdate.toShortFormat(), "Between");
                                            //}
                                            //else {
                                            //    tableView.filter("FLDDUEDATE", "~", "Between");
                                            //}
                                        }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>                        
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="95px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDue" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Done On" AllowSorting="false" AllowFiltering="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <HeaderStyle Width="65px" />
                        <ItemTemplate>
                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" UniqueName="FLDWORKORDERSTATUS" AllowSorting="false" HeaderStyle-Width="75px" 
                        AllowFiltering="true" AutoPostBackOnFilter="false" FilterDelay="2000" CurrentFilterFunction="EqualTo">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="75px"></ItemStyle>
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlStatus" AppendDataBoundItems="true" OnDataBinding="ddlStatus_DataBinding" Width="98%"
                                SelectedValue='<%# GetFilter("STATUS") %>' OnClientSelectedIndexChanged="ddlStatusIndexChanged"
                                runat="server">
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function ddlStatusIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDWORKORDERSTATUS", args.get_item().get_value(), "EqualTo");
                                        }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStaus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERSTATUS")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Job Type" UniqueName="FLDDEFECT" SortExpression="FLDDEFECT" HeaderStyle-Width="75px"
                        AllowSorting="true" FilterDelay="2000" AutoPostBackOnFilter="false" CurrentFilterFunction="EqualTo">
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlDefect" runat="server" Width="98%" SelectedValue='<%# GetFilter("chkDefect") %>'
                                OnClientSelectedIndexChanged="DefectIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Value="" Text="--All--" />
                                    <telerik:RadComboBoxItem Value="0" Text="Planned" />
                                    <telerik:RadComboBoxItem Value="1" Text="Un Planned" />
                                    <telerik:RadComboBoxItem Value="2" Text="Defect" />                                   
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function DefectIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDDEFECT", args.get_item().get_value(), "EqualTo");
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="75px"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDDEFECT") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Priority" UniqueName="FLDPLANINGPRIORITY" AllowFiltering="true" FilterControlWidth="35px"
                        ShowFilterIcon="false" AllowSorting="false" FilterDelay="1000" CurrentFilterFunction="EqualTo">
                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                        <HeaderStyle Width="50px"/>
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANINGPRIORITY") %>' ID="lblPriority"></telerik:RadLabel>
                       </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Work Order" UniqueName="FLDISJOBPLANNED" AllowSorting="false" AllowFiltering="true" ShowFilterIcon="false" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <FilterTemplate>
                            <telerik:RadCheckBox runat="server" ID="ChkNotPlanned" OnCheckedChanged="ChkNotPlanned_CheckedChanged"
                                Checked='<%# GetFilter("JOBNOTPLAN") == "1" ? true : false %>' AutoPostBack="true">
                            </telerik:RadCheckBox>
                            <br />
                            <telerik:RadLabel ID="lblNotplan" runat="server" Text="Jobs without WO." ToolTip="Jobs without WO." AssociatedControlID="ChkNotPlanned"></telerik:RadLabel>
                        </FilterTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPID") %>' Visible="false"></telerik:RadLabel>
                            <asp:LinkButton ID="lnkGroupNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPNO") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true">
                    </telerik:GridClientSelectColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkFind" runat="server" ToolTip="Find Related" Text="Find Related" CommandName="FIND">
                                <span class="icon"><i class="fas fa-search"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Reschedule Job">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Docking" ID="cmdDocking" CommandName="DOCKING" ToolTip="Add To Drydock">
                                 <span class="icon"><i class="fas fa-docker"></i></span>
                            </asp:LinkButton>
                            <asp:ImageButton runat="server" AlternateText="History"
                                ImageUrl="<%$ PhoenixTheme:images/showlist.png %>" ID="cmdView" ToolTip="Reschedule History" CommandName="VIEW"></asp:ImageButton>
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
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="400px" Height="365px" Modal="true" OffsetElementID="main" OnClientClose="CloseWindow">
            <ContentTemplate>
                 <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server">
                </telerik:RadAjaxLoadingPanel>
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel2">
                    <eluc:TabStrip ID="menuWorkorderCreate" runat="server" OnTabStripCommand="menuWorkorderCreate_TabStripCommand" />
                    <table border="0">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input_mandatory upperCase"
                                    MaxLength="200" Width="180px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text="Planned Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtDueDate" runat="server" Width="120px" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Assigned To"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Discipline ID="ddlResponsible" runat="server" CssClass="input_mandatory"
                                    AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPlannedJob" runat="server" Text="Routine WO. ?"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadRadioButtonList ID="rblPlannedJob" runat="server" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Text="Yes" Value="0" />
                                        <telerik:ButtonListItem Text="No" Value="1" Selected="true" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="400px"
                                    BorderWidth="1px" HeaderText="List of errors"></asp:ValidationSummary>
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
