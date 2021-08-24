<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceAdditionsToComponent.aspx.cs" Inherits="PlannedMaintenanceAdditionsToComponent" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagName="TabStrip" TagPrefix="eluc" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagName="Error" TagPrefix="eluc" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Additions To Component</title>
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
       <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager"></telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" ID="RadSkinManager"></telerik:RadSkinManager>

        <telerik:RadAjaxManager ID="AjaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvAdditions">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvAdditions" UpdatePanelHeight="40%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvBatteries">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvBatteries" UpdatePanelHeight="40%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <eluc:Error ID="ucError" runat="server" Visible="false" />
        <eluc:TabStrip runat="server" ID="ucMenu" Title="" TabStrip="true" />

        <telerik:RadGrid runat="server" ID="gvAdditions" OnNeedDataSource="gvAdditions_NeedDataSource" OnItemCommand="gvAdditions_ItemCommand" OnBatchEditCommand="gvAdditions_BatchEditCommand"
            RenderMode="Lightweight" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" Height="100%" CellSpacing="0" GridLines="None" OnItemDeleted="gvAdditions_ItemDeleted"
            AllowAutomaticDeletes="true">
            <MasterTableView EditMode="BATCH" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDID,FLDCOMPONENTID,FLDITEM,FLDVESSELID" CommandItemDisplay="Top">
                <BatchEditingSettings EditType="Row" HighlightDeletedRows="true"/>
                <CommandItemSettings AddNewRecordText="Add" ShowRefreshButton="false" ShowExportToExcelButton="false" />
                <Columns>
                    <telerik:GridDropDownColumn HeaderText="Item" UniqueName="FLDITEM" DataType="System.String" DataField="FLDITEM" ListValueField="FLDQUICKCODE" ListTextField="FLDQUICKNAME"
                        DataSourceID="ItemList" DropDownControlType="RadComboBox" EmptyListItemText="--Select Item--" EmptyListItemValue="" EnableEmptyListItem="true" AllowAutomaticLoadOnDemand="true">
                        <HeaderStyle Width="30%" />
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" Text="*This field is required" Display="Dynamic">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridDropDownColumn>
                    <telerik:GridBoundColumn HeaderText="Value" UniqueName="FLDVALUE" DataType="System.String" DataField="FLDVALUE" >
                        <HeaderStyle Width="60%" />
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" Text="*This field is required" Display="Dynamic">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn Text="Delete" HeaderText="Action" ConfirmDialogType="RadWindow" ConfirmText="Are you sure you want to delete this record?" 
                        CommandName="Delete"  UniqueName="DELETE" ButtonCssClass="fa fa-trash-alt">
                        <HeaderStyle Width="10%" />
                    </telerik:GridButtonColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

        <br clear="all" />
        <telerik:RadGrid runat="server" ID="gvBatteries" OnNeedDataSource="gvBatteries_NeedDataSource" OnItemCommand="gvBatteries_ItemCommand" OnBatchEditCommand="gvBatteries_BatchEditCommand"
            RenderMode="Lightweight" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" Height="100%" CellSpacing="0" GridLines="None" OnItemDeleted="gvBatteries_ItemDeleted"
            AllowAutomaticDeletes="true">
            <MasterTableView EditMode="BATCH" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDID,FLDCOMPONENTID,FLDVESSELID" CommandItemDisplay="Top">
                <BatchEditingSettings EditType="Row" HighlightDeletedRows="true"/>
                <CommandItemSettings AddNewRecordText="Add" ShowRefreshButton="false" ShowExportToExcelButton="false" />
                <Columns>
                    <telerik:GridDropDownColumn HeaderText="Type" UniqueName="FLDTYPE" DataType="System.String" DataField="FLDTYPE" ListValueField="FLDQUICKCODE" ListTextField="FLDQUICKNAME"
                        DataSourceID="TYPELIST" DropDownControlType="RadComboBox" EmptyListItemText="--Select Item--" EmptyListItemValue="" EnableEmptyListItem="true" AllowAutomaticLoadOnDemand="true">
                        <HeaderStyle Width="20%" />
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" Text="*This field is required" Display="Dynamic">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridDropDownColumn>
                    <telerik:GridBoundColumn HeaderText="Specification" UniqueName="FLDSPECIFICATION" DataType="System.String" DataField="FLDSPECIFICATION" >
                        <HeaderStyle Width="20%" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Voltage" UniqueName="FLDVOLTAGE" DataType="System.String" DataField="FLDVOLTAGE" >
                        <HeaderStyle Width="20%" />
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn HeaderText="Last Replaced" UniqueName="FLDLASTREPLACED" DataType="System.DateTime" DataField="FLDLASTREPLACED" DataFormatString="{0:dd/MM/yyyy}" >
                        <HeaderStyle Width="20%" />
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn HeaderText="Suggested Replace Date" UniqueName="FLDSUGGESTEDREPLACEDDATE" DataType="System.DateTime" DataField="FLDSUGGESTEDREPLACEDDATE" DataFormatString="{0:dd/MM/yyyy}" >
                        <HeaderStyle Width="20%" />
                    </telerik:GridDateTimeColumn>
                    <telerik:GridButtonColumn Text="Delete" HeaderText="Action" ConfirmDialogType="RadWindow" ConfirmText="Are you sure you want to delete this record?" 
                        CommandName="Delete"  UniqueName="DELETE" ButtonCssClass="fa fa-trash-alt">
                        <HeaderStyle Width="10%" />
                    </telerik:GridButtonColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

        <asp:SqlDataSource ID="ItemList" runat="server" DataSourceMode="DataReader" SelectCommand="SELECT [FLDQUICKCODE],[FLDQUICKNAME] FROM [TBLQUICK] (NOLOCK) WHERE [FLDQUICKTYPECODE]=177 ORDER BY [FLDQUICKNAME]"
            ConnectionString="<%$ ConnectionStrings:ConnectionString%>" >
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="TypeList" runat="server" DataSourceMode="DataReader" SelectCommand="SELECT [FLDQUICKCODE],[FLDQUICKNAME] FROM [TBLQUICK] (NOLOCK) WHERE [FLDQUICKTYPECODE]=178 ORDER BY [FLDQUICKNAME]"
            ConnectionString="<%$ ConnectionStrings:ConnectionString%>" >
        </asp:SqlDataSource>
            
    </form>
</body>
</html>
