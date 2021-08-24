<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComJobListForPlan.aspx.cs"
    Inherits="PlannedMaintenanceComJobListForPlan" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Jobs</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuComponentType" runat="server" OnTabStripCommand="MenuComponentType_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvComponentJob" runat="server" RenderMode="Lightweight" AutoGenerateColumns="False" CellPadding="3" AllowFilteringByColumn="true"
                Font-Size="11px" OnItemCommand="gvComponentJob_ItemCommand" OnNeedDataSource="gvComponentJob_NeedDataSource" AllowMultiRowSelection="true"
                OnItemDataBound="gvComponentJob_ItemDataBound" ShowHeader="true" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" Height="90%" EnableViewState="false" AllowSorting="true" OnSortCommand="gvComponentJob_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component Number" DataField="FLDCOMPONENTNUMBER" AllowFiltering="true" 
                            AllowSorting="true" SortExpression="FLDCOMPONENTNUMBER">
                            <FilterTemplate>
                                <telerik:RadTextBox ID="txtCompNoFilter" runat="server" Text='<%# ViewState["CompNoFilter"].ToString() %>'
                                   OnTextChanged="txtCompNoFilter_TextChanged"  AutoPostBack="true" Width="100%"></telerik:RadTextBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name" DataField="FLDCOMPONENTNAME"  AllowFiltering="true" 
                            AllowSorting="true" SortExpression="FLDCOMPONENTNAME">
                            <FilterTemplate>
                                <telerik:RadTextBox ID="txtCompNameFilter" runat="server" Text='<%# ViewState["CompNameFilter"].ToString() %>'
                                   OnTextChanged="txtCompNameFilter_TextChanged" AutoPostBack="true" Width="100%"></telerik:RadTextBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Code/Title" DataField="FLDJOBTITLE" AllowFiltering="false" SortExpression="FLDJOBTITLE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Category" AllowFiltering="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Class" AllowFiltering="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Maintenance Interval" AllowSorting="true">
                            <FilterTemplate>
                                <eluc:Hard ID="ucFrequencyType" runat="server" AppendDataBoundItems="true" Width="80px" AutoPostBack="true"
                                    HardList='<%# PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,7) %>' HardTypeCode="7"
                                    OnTextChangedEvent="ucFrequencyType_TextChangedEvent" />
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBFREQUENCY") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFrequencyType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBFREQUENCYTYPE") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFrequencyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Responsibility" ShowFilterIcon="false" HeaderStyle-Width="150px" AllowSorting="true" SortExpression="FLDDISCIPLINENAME">
                            <FilterTemplate>
                                <eluc:Discipline ID="GriducDiscipline" runat="server" CssClass="input"
                                    DisciplineList='<%# PhoenixRegistersDiscipline.ListDiscipline() %>' AutoPostBack="true"
                                    SelectedDiscipline='<%# ViewState["filterDiscipline"].ToString() %>' OnTextChangedEvent="GriducDiscipline_TextChangedEvent" />
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDisciplineId" runat="server" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDISCIPLINE") %>'>
                                </telerik:RadLabel>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date" AllowFiltering="false" AllowSorting="true" SortExpression="FLDJOBNEXTDUEDATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBNEXTDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="Find Related" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkFind" runat="server" Text="Find" ToolTip="Find Related Jobs" CommandName="FIND">
                                    <span class="icon"><i class="fas fa-search"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" OnConfirmMesage="ucConfirm_OnClick" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
