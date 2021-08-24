<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalComponentCategoryWorkOrder.aspx.cs" Inherits="Dashboard_DashboardTechnicalComponentCategoryWorkOrder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        
        <script type="text/javascript">
            function setSize() {                
                setTimeout(function () {                   
                    TelerikGridResize($find("<%= gvComponentCategpry.ClientID %>"));
                }, 200);      
            }
            window.onresize = window.onload = setSize;         
            function changeRequest() {
                top.openNewWindow('dsd', 'Change Request', 'Inventory/InventoryComponentChangeRequestList.aspx'); return false;
            }
            function componentList() {
                top.openNewWindow('dsd', 'Components List', 'Inventory/InventoryComponentTreeDashboard.aspx'); return false;
            }
            function defectJob() {
                top.openNewWindow('dsd', 'Defects / Unscheduled Jobs', 'PlannedMaintenance/PlannedMaintenanceDefectListRegister.aspx'); return false;
            }
            function maintenanceDone() {
                top.openNewWindow('dsd', 'Maintenance Done', 'PlannedMaintenance/PlannedMaintenanceWorkOrderReportList.aspx?FromDashboard=1'); return false;
            }
            function maintenanceDue() {
                top.openNewWindow('dsd', 'Maintenance Due', 'Dashboard/DashboardTechnicalPmsMaitenanceDue.aspx?d=30 D'); return false;
            }
            function overhaul() {
                top.openNewWindow('dsd', 'Over Hauls', 'Dashboard/DashboardTechnicalOverhaulWorkOrder.aspx'); return false;
            }
            function postponementRequirement() {
                top.openNewWindow('detail', 'Postponed Requirement', 'PlannedMaintenance/PlannedMaintenanceWorkOrderPostponedApproval.aspx'); return false;
            }
           
            function runningHours() {
                top.openNewWindow('dsd', 'Running Hours', 'PlannedMaintenance/PlannedMaintenanceCounterUpdate.aspx'); return false;
            }
            function workOrders() {
                top.openNewWindow('dsd', 'Work Orders', 'PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx'); return false;
            }
            function workOrderLog() {
                top.openNewWindow('dsd', 'Work Order Log', 'PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReportList.aspx'); return false;
            }
            function verificationPending() {
                top.openNewWindow('dsd', 'Verification Pending', 'PlannedMaintenance/PlannedMaintenanceWorkorderApprovalPendingList.aspx?VerifyType=1'); return false;
            }
            function vesselParameter() {
                top.openNewWindow('dsd', 'Vessel Parameter', 'PlannedMaintenance/PlannedMaintenanceJobParameterVesselMap.aspx'); return false;
            }
            function pageLoad() {
                setSize();
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .t-col, .t-container {
            padding-left: 0px !important;
            padding-right: 0px !important;
        }
        p{
            padding-left: 10px
        }
        .t-container {
            max-width: none !important;
        }
    </style>    
</head>
<body>
    <form id="form1" runat="server" >
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadPageLayout runat="server" GridType="Fluid" ShowGrid="true" HtmlTag="None">
            <telerik:LayoutRow RowType="Generic">
                <Rows>
                    <telerik:LayoutRow RowType="Generic" CssClass="content">
                        <Rows>
                            <telerik:LayoutRow RowType="Container" WrapperHtmlTag="Div">
                                <Columns>
                                    <telerik:LayoutColumn Span="10" SpanMd="10" SpanSm="10" SpanXs="10" ID="layoutGrid" Height="500px" runat="server">
                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                                            <telerik:RadGrid ID="gvComponentCategpry" runat="server" AutoGenerateColumns="false" OnItemDataBound="gvComponentCategpry_ItemDataBound"
                                                OnNeedDataSource="gvComponentCategpry_NeedDataSource" ShowFooter="True" AllowFilteringByColumn="True">
                                                <MasterTableView HeaderStyle-Font-Bold="true" FooterStyle-Font-Bold="true">
                                                    <ColumnGroups>
                                                        <telerik:GridColumnGroup HeaderText="Job Due but not Planned" Name="Cat1" HeaderStyle-HorizontalAlign="Center" ParentGroupName="cat1">
                                                        </telerik:GridColumnGroup>
                                                        <telerik:GridColumnGroup HeaderText="Work Order Issued" Name="Cat1Issued" HeaderStyle-HorizontalAlign="Center" ParentGroupName="cat1">
                                                        </telerik:GridColumnGroup>
                                                        <telerik:GridColumnGroup HeaderText="Job Due but not Planned" Name="Cat3" HeaderStyle-HorizontalAlign="Center" ParentGroupName="cat2">
                                                        </telerik:GridColumnGroup>
                                                        <telerik:GridColumnGroup HeaderText="Work Order Issued" Name="Cat3Issued" HeaderStyle-HorizontalAlign="Center" ParentGroupName="cat2">
                                                        </telerik:GridColumnGroup>
                                                        <telerik:GridColumnGroup HeaderText="" Name="cat1" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridColumnGroup>
                                                        <telerik:GridColumnGroup HeaderText="" Name="cat2" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridColumnGroup>
                                                    </ColumnGroups>
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Category" FooterText="Total">
                                                             <FilterTemplate>                                                                
                                                                <telerik:RadCheckBox runat="server" ID="chkIsCritical" Text="Is Critical" OnCheckedChanged="chkIsCritical_CheckedChanged" Checked='<%#ViewState["ISCRITICAL"].ToString() == "1" ? true : false %>'></telerik:RadCheckBox>                                                                 
                                                            </FilterTemplate>
                                                            <ItemStyle Wrap="true" />
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTCATEGORY") %>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="OverDue" ColumnGroupName="Cat1" HeaderStyle-HorizontalAlign="Center"
                                                            Aggregate="Sum" DataField="FLDCAT1AND2NOTPLANNEDOVERDUECOUNT" FooterText=" " UniqueName="FLDCAT1AND2NOTPLANNEDOVERDUECOUNT">
                                                             <ItemStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                             <FilterTemplate>
                                                                Responsibility
                                                                 <br />
                                                               <telerik:RadComboBox ID="ddlResponsibility" OnDataBinding="ddlResponsibility_DataBinding" AppendDataBoundItems="true" 
                                                                    SelectedValue='<%# ViewState["RESP"].ToString() %>' AutoPostBack="true" OnTextChanged="ddlDueDaysCat1_TextChanged"
                                                                    runat="server" Width="100%">                                                                    
                                                                </telerik:RadComboBox> 
                                                            </FilterTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkCat1and2NotPlannedOverDue" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAT1AND2NOTPLANNEDOVERDUECOUNT") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Due" UniqueName="FLDCAT1AND2NOTPLANNEDDUECOUNT" ColumnGroupName="Cat1"
                                                            Aggregate="Sum" DataField="FLDCAT1AND2NOTPLANNEDDUECOUNT" FooterText=" " HeaderStyle-HorizontalAlign="Center">
                                                            <FilterTemplate>
                                                                <telerik:RadComboBox ID="ddlDueDaysCat1" runat="server" SelectedValue='<%#ViewState["days"].ToString() %>'
                                                                    AutoPostBack="true" OnTextChanged="ddlDueDaysCat1_TextChanged" Width="80px">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="15" Text="15 Days" />
                                                                        <telerik:RadComboBoxItem Value="30" Text="30 Days" />
                                                                        <telerik:RadComboBoxItem Value="90" Text="90 Days" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </FilterTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkCat1and2NotPlanned" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAT1AND2NOTPLANNEDDUECOUNT") %>'></asp:LinkButton>                                                                
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="OverDue" ColumnGroupName="Cat1Issued" AllowFiltering="false" HeaderStyle-HorizontalAlign="Center"
                                                            Aggregate="Sum" DataField="FLDCAT1AND2ISSUEDOVERDUECOUNT" FooterText=" " UniqueName="FLDCAT1AND2ISSUEDOVERDUECOUNT">
                                                             <ItemStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkCat1and2IssueOverDue" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAT1AND2ISSUEDOVERDUECOUNT") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Due" ColumnGroupName="Cat1Issued" Aggregate="Sum" DataField="FLDCAT1AND2ISSUEDCOUNT" FooterText=" " UniqueName="FLDCAT1AND2ISSUEDCOUNT"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <FilterTemplate>
                                                                <telerik:RadComboBox ID="ddlDueDaysCat1Issue" runat="server" SelectedValue='<%#ViewState["days"].ToString() %>'
                                                                    AutoPostBack="true" OnTextChanged="ddlDueDaysCat1_TextChanged" Width="80px">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="15" Text="15 Days" />
                                                                        <telerik:RadComboBoxItem Value="30" Text="30 Days" />
                                                                        <telerik:RadComboBoxItem Value="90" Text="90 Days" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </FilterTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkCat1and2Issue" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAT1AND2ISSUEDCOUNT") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn HeaderText="OverDue" ColumnGroupName="Cat3" AllowFiltering="false" HeaderStyle-HorizontalAlign="Center"
                                                            Aggregate="Sum" DataField="FLDCAT2AND3NOTPLANNEDOVERDUECOUNT" FooterText=" " UniqueName="FLDCAT2AND3NOTPLANNEDOVERDUECOUNT">
                                                             <ItemStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkCat2and3NotPlannedOverdue" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAT2AND3NOTPLANNEDOVERDUECOUNT") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Due" ColumnGroupName="Cat3" HeaderStyle-HorizontalAlign="Center"
                                                            Aggregate="Sum" DataField="FLDCAT2AND3NOTPLANNEDDUECOUNT" FooterText=" " UniqueName="FLDCAT2AND3NOTPLANNEDDUECOUNT">
                                                            <FilterTemplate>
                                                                <telerik:RadComboBox ID="ddlDueDaysCat3" runat="server" SelectedValue='<%#ViewState["days"].ToString() %>'
                                                                    AutoPostBack="true" OnTextChanged="ddlDueDaysCat1_TextChanged" Width="80px">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="15" Text="15 Days" />
                                                                        <telerik:RadComboBoxItem Value="30" Text="30 Days" />
                                                                        <telerik:RadComboBoxItem Value="90" Text="90 Days" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </FilterTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkCat2and3NotPlanned" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAT2AND3NOTPLANNEDDUECOUNT") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="OverDue" ColumnGroupName="Cat3Issued" AllowFiltering="false" HeaderStyle-HorizontalAlign="Center"
                                                            Aggregate="Sum" DataField="FLDCAT2AND3ISSUEDOVERDUECOUNT" FooterText=" " UniqueName="FLDCAT2AND3ISSUEDOVERDUECOUNT">
                                                             <ItemStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkCat3and4IssueOverDue" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAT2AND3ISSUEDOVERDUECOUNT") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Due" ColumnGroupName="Cat3Issued" Aggregate="Sum" DataField="FLDCAT2AND3ISSUEDCOUNT" FooterText=" " UniqueName="FLDCAT2AND3ISSUEDCOUNT"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <FilterTemplate>
                                                                <telerik:RadComboBox ID="ddlDueDaysCat3Issued" runat="server" SelectedValue='<%#ViewState["days"].ToString() %>'
                                                                    AutoPostBack="true" OnTextChanged="ddlDueDaysCat1_TextChanged" Width="80px">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="15" Text="15 Days" />
                                                                        <telerik:RadComboBoxItem Value="30" Text="30 Days" />
                                                                        <telerik:RadComboBoxItem Value="90" Text="90 Days" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </FilterTemplate>
                                                              <ItemStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkCat3and4Issue" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAT2AND3ISSUEDCOUNT") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                                <ClientSettings>
                                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                </ClientSettings>
                                            </telerik:RadGrid>
                                        </telerik:RadAjaxPanel>
                                    </telerik:LayoutColumn>

                                    <telerik:LayoutColumn Span="2" SpanMd="2" SpanSm="2" SpanXs="2">

                                        <br />
                                        <p>
                                            <telerik:RadButton ID="btnPrimary" runat="server" Text="Change Request" AutoPostBack="false" Width="150px" OnClientClicked="changeRequest">
                                            </telerik:RadButton>
                                        </p>
                                        <p>
                                            <telerik:RadButton ID="RadButton1" runat="server" Text="Component Hierarchy" Width="150px" AutoPostBack="false" OnClientClicked="componentList">
                                            </telerik:RadButton>
                                        </p>
                                        <p>
                                            <telerik:RadButton ID="RadButton2" runat="server" Text="Defects / Non-Routine Jobs" Width="150px" AutoPostBack="false" OnClientClicked="defectJob">
                                            </telerik:RadButton>
                                        </p>
                                        <p>
                                            <telerik:RadButton ID="RadButton4" runat="server" Text="Maintenance Due" Width="150px" AutoPostBack="false" OnClientClicked="maintenanceDue">
                                            </telerik:RadButton>
                                        </p>
                                        <p>
                                            <telerik:RadButton ID="RadButton3" runat="server" Text="Maintenance Done" Width="150px" OnClientClicked="maintenanceDone">
                                            </telerik:RadButton>
                                        </p>                                        
                                        <p>
                                            <telerik:RadButton ID="RadButton5" runat="server" Text="Overhaul Status"  Width="150px" AutoPostBack="false" OnClientClicked="overhaul" Visible="false">
                                            </telerik:RadButton>
                                        </p>
                                        <p>
                                            <telerik:RadButton ID="RadButton6" runat="server" Text="Postponement Requests" OnClientClicked="postponementRequirement"  Width="150px">
                                            </telerik:RadButton>
                                        </p>
                                        <p>
                                            <telerik:RadButton ID="RadButton7" runat="server" Text="Running Hours"  Width="150px" AutoPostBack="false" OnClientClicked="runningHours">
                                            </telerik:RadButton>
                                        </p>                                        
                                        <p>
                                            <telerik:RadButton ID="RadButton10" runat="server" Text="Verification Pending"  Width="150px" AutoPostBack="false" OnClientClicked="verificationPending">
                                            </telerik:RadButton>
                                        </p>
                                        <p>
                                            <telerik:RadButton ID="RadButton11" runat="server" Text="Vessel Parameter"  Width="150px" AutoPostBack="false" OnClientClicked="vesselParameter">
                                            </telerik:RadButton>
                                        </p>
                                        <p>
                                            <telerik:RadButton ID="RadButton8" runat="server" Text="Work Order"  Width="150px" AutoPostBack="false" OnClientClicked="workOrders">
                                            </telerik:RadButton>
                                        </p>
                                        <p>
                                            <telerik:RadButton ID="RadButton9" runat="server" Text="Work Order Log"  Width="150px" AutoPostBack="false" OnClientClicked="workOrderLog">
                                            </telerik:RadButton>
                                        </p>                                                                                 
      								</telerik:LayoutColumn>
                                </Columns>
                            </telerik:LayoutRow>
                        </Rows>
                    </telerik:LayoutRow>
                </Rows>
            </telerik:LayoutRow>
        </telerik:RadPageLayout>
    </form>
</body>
</html>
