<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreEmployeeCompletedTrainingNeeds.aspx.cs" Inherits="CrewOffshoreEmployeeCompletedTrainingNeeds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Completed Training Needs</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewRecommendedCourses" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">

                <%-- <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                     <eluc:TabStrip ID="TrainingNeed" runat="server" TabStrip="false" OnTabStripCommand="TrainingNeed_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>

                <eluc:TabStrip ID="RecommendedCourses" Title="Completed Training Needs" runat="server"></eluc:TabStrip>

                <div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
                <br />
                <b>Completed Recommended CBT</b>

                <eluc:TabStrip ID="MenuOffshoreCompletedCBTTraining" runat="server" OnTabStripCommand="MenuOffshoreCompletedCBTTraining_TabStripCommand"></eluc:TabStrip>

                <div id="div3" style="position: relative; z-index: 1">
                    <%-- <asp:GridView ID="gvOffshoreCCBTTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowFooter="false" OnRowCommand="gvOffshoreCCBTTraining_RowCommand" OnRowCreated="gvOffshoreCCBTTraining_RowCreated"
                    OnRowEditing="gvOffshoreCCBTTraining_RowEditing" OnRowCancelingEdit="gvOffshoreCCBTTraining_RowCancelingEdit"
                    OnRowUpdating="gvOffshoreCCBTTraining_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDTRAININGNEEDID"
                    OnRowDataBound="gvOffshoreCCBTTraining_RowDataBound" OnRowDeleting="gvOffshoreCCBTTraining_OnRowDeleting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshoreCCBTTraining" runat="server"  AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOffshoreCCBTTraining_NeedDataSource"
                        OnItemCommand="gvOffshoreCCBTTraining_ItemCommand"
                        OnItemDataBound="gvOffshoreCCBTTraining_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView DataKeyNames="FLDTRAININGNEEDID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" >
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            
                            <HeaderStyle Width="100px" />
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="As reported by the vessel" Name="Vessel" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="To be Completed by Manning Office / Agent" Name="Office" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>

                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Vessel">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIsRaisedFromCBT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Category">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Category"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblIsRaisedFromCBTEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                        <telerik:RadComboBox ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input" AutoPostBack="true"
                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged"
                                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox Width="100%" ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged"
                                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Sub Category">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input" AppendDataBoundItems="true"
                                            Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Identified Training Need">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="200px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtTrainingNeedEdit" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'
                                            CssClass="input">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtTrainingNeedAdd" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Level of Improvement">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ucImprovementEdit" Enabled="false" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                            AppendDataBoundItems="true" QuickTypeCode="128" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                            AppendDataBoundItems="true" QuickTypeCode="128" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Type of Training">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                            AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Course/CBT">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="imgCourseName" runat="server" CommandArgument='<%# Container.DataSetIndex %>'>
                                            <span class="icon"><i class="fas fa-glasses"></i></span>
                                        </asp:LinkButton>
                                        <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To be done by">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:ToBeDoneBy ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                        <%--  <eluc:Quick ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBY") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="130" />--%>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDetailsHeader" runat="server" Text="Details of Training Imparted / Course Attended"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                            CssClass="input">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTrainerHeader" runat="server" Text="Name of Trainer / Institute"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                            CssClass="input">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Date Completed">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="130px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' Visible="false" />
                                        <telerik:RadLabel ID="lblDateCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Office" HeaderText="Score %">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Result" ColumnGroupName="Office">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt" Visible="false"
                                            ToolTip="Upload Attachment"></asp:ImageButton>
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Initiate Course Request" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>"
                                            CommandName="Initiate Course Request" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCourseReq" Visible="false"
                                            ToolTip="View Requested Course"></asp:ImageButton>
                                        <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" FrozenColumnsCount="3" EnableNextPrevFrozenColumns="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

                <br />
                <b>Completed CBT</b>

                <eluc:TabStrip ID="MenuOffshoreTraining" runat="server" OnTabStripCommand="MenuOffshoreTraining_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 1">
                    <%-- <asp:GridView ID="gvOffshoreTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowFooter="false" OnRowCommand="gvOffshoreTraining_RowCommand" OnRowCreated="gvOffshoreTraining_RowCreated"
                    OnRowEditing="gvOffshoreTraining_RowEditing" OnRowCancelingEdit="gvOffshoreTraining_RowCancelingEdit"
                    OnRowUpdating="gvOffshoreTraining_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDTRAININGNEEDID"
                    OnRowDataBound="gvOffshoreTraining_RowDataBound" OnRowDeleting="gvOffshoreTraining_OnRowDeleting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshoreTraining" runat="server"  AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOffshoreTraining_NeedDataSource"
                        OnItemCommand="gvOffshoreTraining_ItemCommand"
                        OnItemDataBound="gvOffshoreTraining_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView DataKeyNames="FLDTRAININGNEEDID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" >
                            

                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="As reported by the vessel" Name="Vessel" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="To be Completed by Manning Office / Agent" Name="Office" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <HeaderStyle Width="102px" />
                            <Columns>

                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Vessel">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIsRaisedFromCBT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Category">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblIsRaisedFromCBTEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select" ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select" ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Sub Category">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select" ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select" ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Identified Training Need">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="200px" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtTrainingNeedEdit" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'
                                            CssClass="gridinput_mandatory">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtTrainingNeedAdd" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Level of Improvement">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ucImprovementEdit" Enabled="false" runat="server" CssClass="input_mandatory" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                            AppendDataBoundItems="true" QuickTypeCode="128" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                            AppendDataBoundItems="true" QuickTypeCode="128" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Type of Training">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                            AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Course/CBT">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="imgCourseName" runat="server" CommandArgument='<%# Container.DataSetIndex %>'>
                                            <span class="icon"><i class="fas fa-glasses"></i></span>
                                        </asp:LinkButton>
                                        <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To be done by">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:ToBeDoneBy ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" AppendDataBoundItems="true" />

                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDetailsHeader" runat="server" Text="Details of Training Imparted / Course Attended"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                            CssClass="input">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTrainerHeader" runat="server" Text="Name of Trainer / Institute"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                            CssClass="input">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Date Completed">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="130px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' Visible="false" />
                                        <telerik:RadLabel ID="lblDateCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Office" HeaderText="Score %">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Office" HeaderText="Result">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt" Visible="false"
                                            ToolTip="Upload Attachment"></asp:ImageButton>
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Initiate Course Request" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>"
                                            CommandName="Initiate Course Request" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCourseReq" Visible="false"
                                            ToolTip="View Requested Course"></asp:ImageButton>
                                        <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" FrozenColumnsCount="3" EnableNextPrevFrozenColumns="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

                <br />
                <b>Completed Training Course</b>

                <eluc:TabStrip ID="MenuOffshoreCompletedCourseTraining" runat="server" OnTabStripCommand="MenuOffshoreCompletedCourseTraining_TabStripCommand"></eluc:TabStrip>

                <div id="div4" style="position: relative; z-index: 1">
                    <%-- <asp:GridView ID="gvOffshoreCCourseTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowFooter="false" OnRowCommand="gvOffshoreCCourseTraining_RowCommand" OnRowCreated="gvOffshoreCCourseTraining_RowCreated"
                    OnRowEditing="gvOffshoreCCourseTraining_RowEditing" OnRowCancelingEdit="gvOffshoreCCourseTraining_RowCancelingEdit"
                    OnRowUpdating="gvOffshoreCCourseTraining_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDTRAININGNEEDID"
                    OnRowDataBound="gvOffshoreCCourseTraining_RowDataBound" OnRowDeleting="gvOffshoreCCourseTraining_OnRowDeleting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshoreCCourseTraining" runat="server"  AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOffshoreCCourseTraining_NeedDataSource"
                        OnItemCommand="gvOffshoreCCourseTraining_ItemCommand"
                        OnItemDataBound="gvOffshoreCCourseTraining_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView DataKeyNames="FLDTRAININGNEEDID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" >
                            
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="As reported by the vessel" Name="Vessel" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="To be Completed by Manning Office / Agent" Name="Office" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <HeaderStyle Width="102px" />

                            <Columns>

                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Vessel">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsRaisedFromCBT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Category">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblIsRaisedFromCBTEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select" ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select" ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Sub Category">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select" ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to Select" ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Identified Training Need">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="200px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtTrainingNeedEdit" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'
                                            CssClass="gridinput_mandatory">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtTrainingNeedAdd" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Vessel" HeaderText="Level of Improvement">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ucImprovementEdit" Enabled="false" runat="server" CssClass="input_mandatory" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                            AppendDataBoundItems="true" QuickTypeCode="128" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                            AppendDataBoundItems="true" QuickTypeCode="128" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Type of Training" ColumnGroupName="Vessel">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                            AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Course/CBT">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="imgCourseName" runat="server" CommandArgument='<%# Container.DataSetIndex %>'>
                                            <span class="icon"><i class="fas fa-glasses"></i></span>
                                        </asp:LinkButton>
                                        <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDonebyHeader" runat="server" Text="To be done by"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:ToBeDoneBy ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                        <%-- <eluc:Quick ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBY") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="130" />     --%>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Details of Training Imparted / Course Attended">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="200px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                            CssClass="input">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Office" HeaderText="Name of Trainer / Institute">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="200px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                            CssClass="input">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="Office" HeaderText="Date Completed">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="130px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="Office">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton runat="server" AlternateText="Attachment" 
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Upload Attachment">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                     
                                        <asp:LinkButton runat="server" AlternateText="Initiate Course Request" 
                                            CommandName="Initiate Course Request" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCourseReq" Visible="false"
                                            ToolTip="View Requested Course">
                                           <span class="icon"><i class="fas fa-user-graduate"></i></span> 
                                        </asp:LinkButton>
                                        <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Score %" Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblScoreHeader" runat="server" Text="Score %"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Result" Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblResultHeader" runat="server" Text="Result"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" FrozenColumnsCount="3" EnableNextPrevFrozenColumns="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>

</body>
</html>
