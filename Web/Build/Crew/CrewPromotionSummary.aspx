<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPromotionSummary.aspx.cs" Inherits="CrewPromotionSummary" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Promotion</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" DestroyOnClose="true"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true" Width="100%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNumber" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankTo" runat="server" Text="Rank To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" CssClass="dropdown_mandatory" ID="ucRankTo" runat="server" EnableLoadOnDemand="True" EmptyMessage="Type to select Rank"
                            Filter="Contains" Width="200px" AppendDataBoundItems="true" OnTextChanged="ucRankTo_TextChanged"
                            AutoPostBack="true" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <telerik:RadLabel ID="lblPromotionCOC" runat="server" Font-Bold="true" Text="Certicate Of Competency & Task"></telerik:RadLabel>
            <eluc:TabStrip ID="CrewPromotionMenu" runat="server" OnTabStripCommand="CrewPromotionMenu_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvPromotion" runat="server" AutoGenerateColumns="False" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="true"
                OnNeedDataSource="gvPromotion_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvPromotion_ItemCommand" AllowMultiRowSelection="true"
                OnItemDataBound="gvPromotion_ItemDataBound" OnSortCommand="gvPromotion_SortCommand">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="true" GroupLoadMode="Client"
                    GroupHeaderItemStyle-CssClass="center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <GroupHeaderTemplate>
                        <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPENAME").ToString() %>'></telerik:RadLabel>
                    </GroupHeaderTemplate>
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDDOCUMENTTYPENAME" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <%-- <telerik:GridClientSelectColumn UniqueName="chkDocument"></telerik:GridClientSelectColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Document Name" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentType" runat="server" Width="30px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReqDocTaskID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDDOCANDTASKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Required Document/Task" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReqDocTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDDOCANDTASK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Avail. Document/Task" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="400px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAvailableTaskId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCANDTASKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAvailableDocTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCANDTASK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued/Completed" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuedDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expired" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Missing/Not Completed" HeaderStyle-Width="12%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMissing" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMISSINGYESNO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMissingYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMISSINGYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpiredYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIREDYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expired YN" HeaderStyle-Width="12%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIREDYESNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <telerik:RadLabel ID="RadLabel1" runat="server" Font-Bold="true" Text="Recommended Appraisals"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPromotionAppraisal" runat="server" AutoGenerateColumns="False" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="true"
                OnNeedDataSource="gvPromotionAppraisal_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvPromotionAppraisal_ItemCommand" AllowMultiRowSelection="true"
                OnItemDataBound="gvPromotionAppraisal_ItemDataBound" OnSortCommand="gvPromotionAppraisal_SortCommand">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="true" GroupLoadMode="Client"
                    GroupHeaderItemStyle-CssClass="center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <GroupHeaderTemplate>
                        <telerik:RadLabel ID="lblRequired" runat="server" Text='<%# "  Required[ " +DataBinder.Eval(Container,"DataItem.FLDAPPRAISALREQUIRED").ToString() + " ]" %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text='<%# "  - Available["  %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblCount" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAPPRAISALCOUNT").ToString()  %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="RadLabel7" runat="server" Text='<%#  "]" %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblIssuitable" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUITABLE").ToString() %>'></telerik:RadLabel>
                    </GroupHeaderTemplate>
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <GroupByFields>                            
                                <telerik:GridGroupByField FieldName="FLDAPPRAISALREQUIRED" SortOrder="Ascending" />
                                <telerik:GridGroupByField FieldName="FLDAPPRAISALCOUNT" SortOrder="Ascending" />
                                 <telerik:GridGroupByField FieldName="FLDISSUITABLE" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Document Name" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentType" runat="server" Width="30px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReqApp" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALREQUIRED") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAppraisalid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtkey" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Occasion" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOccasion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCASION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDFROMDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblToDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDTODATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Master Comment" HeaderStyle-Width="22%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMasterComment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMASTERCOMMENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HOD Comment" HeaderStyle-Width="22%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHODComment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEADDEPTCOMMENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadLabel ID="RadLabel2" runat="server" Font-Bold="true" Text="Rank Experience"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPromotionRank" runat="server" AutoGenerateColumns="False" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="true"
                OnNeedDataSource="gvPromotionRank_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvPromotionRank_ItemCommand" AllowMultiRowSelection="true"
                OnItemDataBound="gvPromotionRank_ItemDataBound" OnSortCommand="gvPromotionRank_SortCommand">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="true" GroupLoadMode="Client"
                    GroupHeaderItemStyle-CssClass="center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <GroupHeaderTemplate>
                        <telerik:RadLabel ID="lblRequired" runat="server" Text='<%# "  Required Experience  [ " +DataBinder.Eval(Container,"DataItem.FLDRANKEXPREQUIRED").ToString() + " ]" %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text='<%# "  - Combined Experience [" %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblCmbRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMBINEDRANKEXP").ToString() %>'></telerik:RadLabel>
                         <telerik:RadLabel ID="RadLabel5" runat="server" Text='<%#  "]" %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblIssuitable" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUITABLE").ToString() %>'></telerik:RadLabel>
                    </GroupHeaderTemplate>
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDRANKEXPREQUIRED" SortOrder="Ascending" />
                                <telerik:GridGroupByField FieldName="FLDCOMBINEDRANKEXP" SortOrder="Ascending" />
                                <telerik:GridGroupByField FieldName="FLDISSUITABLE" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Experience" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrentExp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTEXPERIENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Required Experience" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReqExp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKEXPREQUIRED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Combined Experience" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCombExp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMBINEDRANKEXP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadLabel ID="RadLabel3" runat="server" Font-Bold="true" Text="Promotion Plan"></telerik:RadLabel>

            <eluc:TabStrip ID="CrewPromotionPlanMenu" runat="server" OnTabStripCommand="CrewPromotionPlanMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPromotionPlan" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvPromotionPlan_NeedDataSource"
                OnItemCommand="gvPromotionPlan_ItemCommand" OnItemDataBound="gvPromotionPlan_ItemDataBound" OnSortCommand="gvPromotionPlan_SortCommand" OnDeleteCommand="gvPromotionPlan_DeleteCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Rank From">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROMOTIONPLANID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankFrom" Width="400px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposed Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDPROMOTIONPLANDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActive" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVEYESNO").ToString() %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="NAVIGATEEDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>


        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
