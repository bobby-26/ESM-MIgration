<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalJobCategoryMaintanaceDone.aspx.cs" Inherits="DashboardTechnicalJobCategoryMaintanaceDone" %>
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
      <title>Maintenance Done</title>
    
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseWindow() {
                document.getElementById('<%=TextBoxRequiredFieldValidator.ClientID%>').innerHTML = "";
                document.getElementById('<%=txtDueDate.ClientID%>').innerHTML = "";
                document.getElementById('<%=ResponsibilityRequiredFieldValidator.ClientID%>').innerHTML = "";
                document.getElementById('<%=ValidationSummary1.ClientID%>').style.display = 'none';                
            }
        </script>
    </telerik:RadCodeBlock>    
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager runat="server"></telerik:RadSkinManager>
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
        <telerik:RadGrid ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemDataBound="gvWorkOrder_ItemDataBound"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" AllowFilteringByColumn="true"
            OnItemCommand="gvWorkOrder_ItemCommand" Height="100%" OnSortCommand="gvWorkOrder_SortCommand" EnableLinqExpressions="false" EnableViewState="true">
            
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                AllowFilteringByColumn="True" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID">
                <HeaderStyle Width="102px" />
                <Columns>                        
                    <telerik:GridTemplateColumn HeaderText="Component No." DataField="FLDCOMPONENTNUMBER" UniqueName="FLDCOMPONENTNUMBER"
                        AllowSorting="true" SortExpression="FLDCOMPONENTNUMBER" FilterControlWidth="50px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRespId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDPLANNINGDISCIPLINE"]%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERID"]%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCategoryId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDCATEGORYID"]%>'></telerik:RadLabel>
                            <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component Name" AllowSorting="true" SortExpression="FLDCOMPONENTNAME" UniqueName="FLDCOMPONENTNAME"
                        DataField="FLDCOMPONENTNAME" FilterControlWidth="80px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains"> 
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Code & Title" AllowSorting="true" SortExpression="FLDWORKORDERNAME" UniqueName="FLDWORKORDERNAME"
                        DataField="FLDWORKORDERNAME" FilterControlWidth="80px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">                           
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"] %>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Category" AllowSorting="true" UniqueName="FLDJOBCATEGORY"
                        ShowSortIcon="false" DataField="FLDJOBCATEGORYID" FilterControlWidth="80px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false"  CurrentFilterFunction="EqualTo">                       
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlJobCategory" runat="server" OnDataBinding="ddlJobCategory_DataBinding" AppendDataBoundItems="true"
                                    SelectedValue='<%# ViewState["JCATNAME"].ToString() %>' OnClientSelectedIndexChanged="JobCategoryIndexChanged">                                                                    
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
                    <telerik:GridTemplateColumn HeaderText="Maintenance Interval" AllowSorting="false" AllowFiltering="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Responsibility" HeaderStyle-Width="150px" UniqueName="FLDPLANNINGDISCIPLINE"
                        AllowSorting="true" SortExpression="FLDDISCIPLINENAME" DataField="FLDPLANNINGDISCIPLINE" FilterControlWidth="80px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo"> 
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlResponsibility" OnDataBinding="ddlResponsibility_DataBinding" AppendDataBoundItems="true" 
                                SelectedValue='<%# ViewState["RESP"].ToString() %>' OnClientSelectedIndexChanged="ResponsibilityIndexChanged"
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
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>                       
                    <telerik:GridTemplateColumn HeaderText="Due On" UniqueName="FLDDUEDATE" SortExpression="FLDDUEDATE" DataField="FLDDUEDATE"
                        AllowSorting="true"  FilterDelay="2000" AutoPostBackOnFilter="false" CurrentFilterFunction="Between" AllowFiltering="false">
                        <FilterTemplate>
                            <telerik:RadLabel runat="server" AssociatedControlID="FromOrderDatePicker" Text="From"></telerik:RadLabel>
                            <telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="100px" ClientEvents-OnDateSelected="FromDateSelected"
                                DbSelectedDate='<%# ViewState["FDATE"].ToString() %>' />
                            <br />
                            <telerik:RadLabel runat="server" AssociatedControlID="ToOrderDatePicker" Text="To" Style="padding-left: 5px;"></telerik:RadLabel>
                            <telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="100px" ClientEvents-OnDateSelected="ToDateSelected"
                                DbSelectedDate='<%# ViewState["TDATE"].ToString() %>'/>
                            <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                <script type="text/javascript">
                                    function FromDateSelected(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(sender);
                                        var toDate = FormatSelectedDate(ToPicker);

                                        tableView.filter("FLDDUEDATE", fromDate + "~" + toDate, "Between");

                                    }
                                    function ToDateSelected(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(FromPicker);
                                        var toDate = FormatSelectedDate(sender);

                                        tableView.filter("FLDDUEDATE", fromDate + "~" + toDate, "Between");
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
                        <HeaderStyle Width="150px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                            
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDue" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>                            
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>                    
                    <telerik:GridTemplateColumn HeaderText="Last Done On"  AllowSorting="false" AllowFiltering="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>
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
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="LoadingPanel1">       
                    <table border="0">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input_mandatory upperCase"
                                    MaxLength="200" Width="180px">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator
                                    ID="TextBoxRequiredFieldValidator"
                                    runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="txtTitle"
                                    EnableClientScript="true" ForeColor="Red"
                                    ErrorMessage="* Title can not be empty!">*
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text="Planned Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtDueDate" runat="server" Width="120px" CssClass="input_mandatory" />
                                <asp:RequiredFieldValidator ID="PickerRequiredFieldValidator" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtDueDate" ErrorMessage="* select a due date">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Assigned To"></telerik:RadLabel>
                            </td>
                            <td>
                                 <eluc:Discipline ID="ddlResponsible" runat="server" CssClass="input_mandatory"
                                    AppendDataBoundItems="true"  />
                                <asp:RequiredFieldValidator ID="ResponsibilityRequiredFieldValidator" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="ddlResponsible:ddlDiscipline" ErrorMessage="* Assigned To is required!">*</asp:RequiredFieldValidator>                                 
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <telerik:RadButton ID="btnCreateWO" Text="Create" runat="server" OnClick="btnCreateWO_Click"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                 <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="200px"
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
