<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentDashboard.aspx.cs" Inherits="Inventory_InventoryComponentDashboard" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskedTextBox" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Component" Src="../UserControls/UserControlMultiColumnComponents.ascx" %>
<%@ Register TagPrefix="eluc" TagName="budget" Src="~/UserControls/UserControlMultiColumnBudgetRemainingBalance.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvWorkOrder.ClientID %>"), true, 30);
                }, 200);
            }
            window.onresize = window.onload = Resize;
            function ChangeRequest() {
                top.openNewWindow('dsd', 'Change Request', 'Inventory/InventoryComponentChangeRequestList.aspx'); return false;
            }
            function CompParts() {
                top.openNewWindow('dsd', 'Compo Parts', 'Inventory/InventoryComponentSpareItem.aspx'); return false;
            }
            function Attachments() {
                top.openNewWindow('dsd', 'Attachments', 'PlannedMaintenance/PlannedMaintenanceWorkOrderRequisition.aspx'); return false;
            }
            function Counters() {
                top.openNewWindow('dsd', 'Counter', 'Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx'); return false;
            }
            function TechnicalSpecification() {
                top.openNewWindow('dsd', 'Technical Specifications', 'PlannedMaintenance/PlannedMaintenanceCounterUpdate.aspx'); return false;
            }
            function pageLoad() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvWorkOrder.ClientID %>"), true, 30);
                }, 200);
            }
            function CloseWindow() {
                    document.getElementById('<%=ValidationSummary1.ClientID%>').style.display = 'none';
            }
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                    radwindow.close();
                    var masterTable = $find("<%= gvWorkOrder.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }

        

        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="Resize()" onload="Resize()">
    <form id="frmPlannedMaintenanceComponentTypeGeneral" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ucConfirm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pnlComponentGeneral" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxPanel ID="pnlComponentGeneral" runat="server">


            <eluc:TabStrip ID="MenuComponentGeneral" runat="server" OnTabStripCommand="PlannedMaintenanceComponent_TabStripCommand"></eluc:TabStrip>

            <br clear="all" />

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadPageLayout runat="server" GridType="Fluid" ShowGrid="true" HtmlTag="None">
                <telerik:LayoutRow RowType="Generic" CssClass="content">
                    <Rows>
                        <telerik:LayoutRow RowType="Container" WrapperHtmlTag="Div">
                            <Columns>
                                <telerik:LayoutColumn Span="8" SpanMd="8" SpanSm="8" SpanXs="8" ID="layoutGrid" runat="server">
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:MaskedTextBox MaskText="###.##.##" runat="server" ID="txtComponentNumber" Width="100px" />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>

                                            </td>
                                            <td>
                                                <eluc:UserControlAddress ID="txtMakerId" AddressType="130,131" runat="server" Width="100%" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblName" runat="server" Text="Component Name"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="200" Width="100%">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtType" runat="server" MaxLength="50" Width="100%"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblParentComponent" runat="server" Text="Parent Component"></telerik:RadLabel>
                                            </td>
                                            <td>

                                                <eluc:Component ID="txtParentComponentID" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblSerialNumber" runat="server" Text="Serial Number"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtSerialNumber" runat="server" MaxLength="50"
                                                    Width="80px">
                                                </telerik:RadTextBox>

                                            </td>

                                        </tr>

                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblClassCode" runat="server" Text="Class Code"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtClassCode" runat="server" MaxLength="200" Width="100%"></telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                                            </td>
                                            <td>

                                                <eluc:UserControlAddress ID="txtVendorId" AddressType="130,131" runat="server" Width="100%" />
                                            </td>
                                            <%--    <td>
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                   
                        <eluc:budget ID="txtBudgetId" runat="server" budgetgroup="106" hardtypecode="30" vesselid="<%# PhoenixSecurityContext.CurrentSecurityContext.VesselID %>" />
                    </td>--%>
                                        </tr>
                                        <tr>
                                            <%--<td>
                                                <telerik:RadLabel ID="lblCritical" runat="server" Text="Critical"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadCheckBox ID="chkIsCritical" Checked="false" runat="server" />
                                            </td>--%>
                                            <td>
                                                <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel> 
                                            </td>
                                            <td>
                                                <eluc:UserControlQuick ID="ucCompCategory" runat="server" QuickTypeCode="166" Width="100%" CssClass="input_mandatory" AppendDataBoundItems="true" />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:UserControlHard runat="server" ID="ucStatus" HardTypeCode="13" AppendDataBoundItems="true" Width="100%" CssClass="input_mandatory" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblintalltion" runat="server" Text="Installation Date"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Date ID="txtinstallation" runat="server" DatePicker="true" MaxLength="200" Width="100%"></eluc:Date>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <%--<telerik:RadTextBox ID="txtLocation" runat="server" MaxLength="200" Width="240px"></telerik:RadTextBox>--%>
                                                <telerik:RadComboBox ID="ddlLocation" runat="server" DataTextField="FLDCOMPONENTNAME" 
                                                    DataValueField="FLDGLOBALCOMPONENTID" EnableLoadOnDemand="true" Width="100%">

                                                </telerik:RadComboBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblclassification" Text="Critical Category" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td colspan="2">
                                                <telerik:RadRadioButtonList ID="rbCompClasification" runat="server" Direction="Horizontal">
                                                    <Items>
                                                        <telerik:ButtonListItem Text="NA" Value="0" />
                                                        <telerik:ButtonListItem Text="Safety" Value="1" />
                                                        <telerik:ButtonListItem Text="Operational" Value="2" />
                                                        <telerik:ButtonListItem Text="Environmental" Value="3" />
                                                    </Items>
                                                </telerik:RadRadioButtonList>
                                            </td>
                                        </tr>
                                        <tr style="visibility:hidden;">
                                            <td>
                                                <telerik:RadLabel ID="lblOperational" runat="server" Text="Operational & Safety Critical"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadCheckBox ID="chkOperationalCritical" runat="server" />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblEnvironmental" runat="server" Text="Environmental Critical"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadCheckBox ID="chkEnvironmentalCritical" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <eluc:Confirm ID="ucConfirm" runat="server" Visible="false" OnConfirmMesage="ucConfirm_ConfirmMesage" OKText="Yes" CancelText="No" />
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="2" SpanMd="2" SpanSm="2" SpanXs="2">


                                    <p>
                                        <telerik:RadButton ID="btnchangerequest" runat="server" Text="Change Request" AutoPostBack="false" Width="150px">
                                        </telerik:RadButton>
                                    </p>
                                    <p>
                                        <telerik:RadButton ID="btnparts" runat="server" Text="Parts" Width="150px" AutoPostBack="false">
                                        </telerik:RadButton>

                                    </p>
                                    <p>
                                        <telerik:RadButton ID="btnattachment" runat="server" Text="Attachments" Width="150px" AutoPostBack="false">
                                        </telerik:RadButton>
                                    </p>
                                    <p>
                                        <telerik:RadButton ID="btncounter" runat="server" Text="Counter" Width="150px">
                                        </telerik:RadButton>
                                    </p>
                                    <p>
                                        <telerik:RadButton ID="btntechspec" runat="server" Text="Technical Specifications" Width="150px" AutoPostBack="false">
                                        </telerik:RadButton>
                                    </p>
                                    <%--<p>
                                        <telerik:RadButton ID="btnWorkorderDue" runat="server" Text="Maintenance Due" Width="150px" AutoPostBack="false">
                                        </telerik:RadButton>
                                    </p>--%>
                                    <p>
                                        <telerik:RadButton ID="btnAdditions" runat="server" Text="Additions to Component" Width="150px" AutoPostBack="false">
                                        </telerik:RadButton>
                                    </p>

                                </telerik:LayoutColumn>
                                <telerik:LayoutColumn Span="2" SpanMd="2" Spansm="2" SpanXs="2">
                                    <p>
                                        <telerik:RadButton ID="btnManuals" runat="server" Text="Manuals" AutoPostBack="false" Width="150px">
                                        </telerik:RadButton>
                                    </p>
                                    <p>
                                        <telerik:RadButton ID="btnDocument" runat="server" Text="Other Documents" AutoPostBack="false" Width="150px">
                                        </telerik:RadButton>
                                    </p>
                                    <p>
                                        <telerik:RadButton ID="btnRA" runat="server" Text="Risk Assessments" AutoPostBack="false" Width="150px">
                                        </telerik:RadButton>
                                    </p>
                                    <p>
                                        <telerik:RadButton ID="btnReportingTemp" runat="server" Text="Report Template" AutoPostBack="false" Width="150px">
                                        </telerik:RadButton>
                                    </p>
                                </telerik:LayoutColumn>
                            </Columns>
                        </telerik:LayoutRow>
                    </Rows>
                </telerik:LayoutRow>
            </telerik:RadPageLayout>
            <eluc:TabStrip ID="MenuWorkOrderRequestion" runat="server" OnTabStripCommand="MenuWorkOrderRequestion_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemDataBound="gvWorkOrder_ItemDataBound"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" AllowFilteringByColumn="true" 
            OnItemCommand="gvWorkOrder_ItemCommand" OnSortCommand="gvWorkOrder_SortCommand" EnableLinqExpressions="false" EnableViewState="true">

            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                AllowFilteringByColumn="True" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="" AllowSorting="false" UniqueName="FLDISCRITICAL"
                        ShowSortIcon="false" DataField="FLDISCRITICAL" FilterControlWidth="50px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" AllowFiltering="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20px"></ItemStyle>
                        <HeaderStyle Width="50px" />
                        <FilterTemplate>
                            <telerik:RadCheckBox runat="server" ID="chkIsCritical" OnCheckedChanged="chkIsCritical_CheckedChanged"
                                Checked='<%#ViewState["ISCRITICAL"].ToString() == "1" ? true : false %>'>
                            </telerik:RadCheckBox>
                            <br />
                            <telerik:RadLabel ID="lblFIsCritial" runat="server" Text="Is Critical" AssociatedControlID="chkIsCritical"></telerik:RadLabel>
                        </FilterTemplate>
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component No." DataField="FLDCOMPONENTNUMBER" UniqueName="FLDCOMPONENTNUMBER"
                        AllowSorting="true" SortExpression="FLDCOMPONENTNUMBER" CurrentFilterFunction="Contains" AllowFiltering="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <HeaderStyle Width="90px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRespId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDPLANNINGDISCIPLINE"]%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERID"]%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJobID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCategoryId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDCATEGORYID"]%>'></telerik:RadLabel>
                            <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component Name" AllowSorting="true" SortExpression="FLDCOMPONENTNAME" UniqueName="FLDCOMPONENTNAME"
                        DataField="FLDCOMPONENTNAME" AllowFiltering="false">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="240px"></ItemStyle>
                        <HeaderStyle Width="240px" />
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Code & Title" AllowSorting="true" SortExpression="FLDWORKORDERNAME" UniqueName="FLDWORKORDERNAME"
                        DataField="FLDWORKORDERNAME" FilterControlWidth="80px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="280px"></ItemStyle>
                        <HeaderStyle Width="280px" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnktitle" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"] %>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Category" AllowSorting="true" UniqueName="FLDJOBCATEGORY"
                        ShowSortIcon="false" DataField="FLDJOBCATEGORYID" FilterControlWidth="80px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="180px"></ItemStyle>
                        <HeaderStyle Width="180px" />
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
                    <telerik:GridTemplateColumn HeaderText="Maintenance Interval" AllowSorting="false" AllowFiltering="true" ShowFilterIcon="false"
                        UniqueName="FLDFREQUENCYTYPE" AutoPostBackOnFilter="false" HeaderStyle-Width="110px">
                        <FilterTemplate>
                            <telerik:RadTextBox ID="txtFrequency" runat="server" Width="40%" Text='<%# ViewState["FREQUENCY"].ToString() %>'></telerik:RadTextBox>
                            <telerik:RadComboBox ID="cblFrequencyType" runat="server" OnDataBinding="cblFrequencyType_DataBinding" AutoPostBack="false" Width="60%" AppendDataBoundItems="true"
                                OnClientSelectedIndexChanged="FrequencyIndexChanged" SelectedValue='<%# ViewState["FREQUENCYTYPE"].ToString() %>'>
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
                        AllowSorting="true" FilterDelay="2000" AutoPostBackOnFilter="false" CurrentFilterFunction="Between">
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlDueDays" runat="server" Width="80px" SelectedValue='<%# ViewState["DUE"].ToString() %>'
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
                                        var val = 0;
                                        if (args.get_item().get_value() != "") {
                                            val = args.get_item().get_value();
                                            var today = new Date();
                                            var newdate = new Date();
                                            newdate.setDate(today.getDate() + Number(val));
                                            tableView.filter("FLDDUEDATE", today.toShortFormat() + "~" + newdate.toShortFormat(), "Between");
                                        }
                                        else {
                                            tableView.filter("FLDDUEDATE", "~", "Between");
                                        }
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <HeaderStyle Width="100px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDue" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Done On" AllowSorting="false" AllowFiltering="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <HeaderStyle Width="90px" />
                        <ItemTemplate>
                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" UniqueName="FLDWORKORDERSTATUS" AllowSorting="false" 
                        AllowFiltering="true" AutoPostBackOnFilter="false" FilterDelay="2000" CurrentFilterFunction="EqualTo">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <HeaderStyle Width="100px" />
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlStatus" AppendDataBoundItems="true" OnDataBinding="ddlStatus_DataBinding" Width="100%"
                                SelectedValue='<%# ViewState["STATUS"].ToString() %>' OnClientSelectedIndexChanged="ddlStatusIndexChanged"
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
                            <telerik:RadLabel ID="lblStaus" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERSTATUS")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Work Order No." AllowSorting="false" AllowFiltering="true" ShowFilterIcon="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <HeaderStyle Width="100px" />
                        <FilterTemplate>
                            <telerik:RadCheckBox runat="server" ID="ChkNotPlanned" OnCheckedChanged="ChkNotPlanned_CheckedChanged"
                                Checked='<%#ViewState["JOBNOTPLAN"].ToString() == "1" ? true : false %>' AutoPostBack="true">
                            </telerik:RadCheckBox>
                            <br />
                            <telerik:RadLabel ID="lblNotplan" runat="server" Text="Jobs without WO." AssociatedControlID="ChkNotPlanned"></telerik:RadLabel>
                        </FilterTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPID") %>' Visible="false"></telerik:RadLabel>
                            <asp:LinkButton ID="lnkGroupNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPNO") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true">
                    </telerik:GridClientSelectColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                        <HeaderStyle Width="60px" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkFind" runat="server" ToolTip="Find Related" Visible="false"
                                Text="Find Related" CommandName="FIND">
                                <span class="icon"><i class="fas fa-search"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="View" Visible="false"
                                    CommandName="VIEW" CommandArgument='<%# Container.DataSetIndex %>' ID="lnkJobView"
                                    ToolTip="View" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-binoculars"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Maintenance History" Visible="false"
                                    CommandName="WOHISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdWoHistory"
                                    ToolTip="Maintenance History" Width="20px" Height="20px">
                               <span class="icon"><i class="fas fa-newspaper"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="RA History" Visible="false"
                                    CommandName="RAHISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRaHistory"
                                    ToolTip="RA History" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="PTW History" Visible="false"
                                    CommandName="PTWHISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPtwHistory"
                                    ToolTip="PTW History" Width="20px" Height="20PX">
                                <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Change Request"
                                    CommandName="CHANGEREQ" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdChangeReq"
                                    ToolTip="Change Request" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-receipt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Postpone job">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
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
        </telerik:RadAjaxPanel>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="400px" Height="365px" Modal="true" OffsetElementID="main" OnClientClose="CloseWindow">
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1">
                    <eluc:TabStrip ID="menuWorkorderCreate" runat="server" OnTabStripCommand="menuWorkorderCreate_TabStripCommand" />
                    <table border="0">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input_mandatory upperCase"
                                    MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text="Planned Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtDueDate" runat="server" Width="100%" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Assigned To"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Discipline ID="ddlResponsible" runat="server" CssClass="input_mandatory" Width="100%"
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

