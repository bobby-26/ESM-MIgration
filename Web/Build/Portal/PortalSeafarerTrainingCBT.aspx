<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalSeafarerTrainingCBT.aspx.cs" Inherits="PortalSeafarerTrainingCBT" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewRecommendedCourses" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1"
            runat="server">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlCrewRecommendedCourses">
           
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="TrainingNeed" runat="server" TabStrip="true" OnTabStripCommand="TrainingNeed_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr align="left">
                    <td>
                        <telerik:RadLabel ID="lblfileno" runat="server" Text="File No."></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtfileno" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfirstname" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblfname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRANKname" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadTextBox ID="lblrname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadRadioButtonList ID="rbtoption" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbtoption_SelectedIndexChanged" Direction="Horizontal">
                <Items>
                    <telerik:ButtonListItem Text="Pending" Value="PENDING"></telerik:ButtonListItem>
                    <telerik:ButtonListItem Text="Completed" Value="COMPLETED"></telerik:ButtonListItem>
                    <telerik:ButtonListItem Text="Overriden" Value="OVERRIDEN"></telerik:ButtonListItem>
                </Items>
            </telerik:RadRadioButtonList>
            <telerik:RadLabel ID="lblPendingRecommendedcbt" runat="server" Text="Pending from Appraisal"
                Font-Bold="true">
            </telerik:RadLabel>
            <eluc:TabStrip ID="MenuOffshoreTraining1" runat="server" OnTabStripCommand="MenuOffshoreTraining1_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTrainingbyreg" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowSorting="true"
                Width="100%" CellPadding="3" ShowFooter="false" OnItemCommand="gvTrainingbyreg_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnRowCreated="gvTrainingbyreg_RowCreated" OnNeedDataSource="gvTrainingbyreg_NeedDataSource" AllowPaging="true" AllowCustomPaging="true"
                OnUpdateCommand="gvTrainingbyreg_UpdateCommand" Height=""
                ShowHeader="true" EnableViewState="false" DataKeyNames="FLDTRAININGNEEDID" OnItemDataBound="gvTrainingbyreg_ItemDataBound"
                OnDeleteCommand="gvTrainingbyreg_DeleteCommand">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />

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
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="header text">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="To be Completed by Manning Office / Agent" HeaderStyle-HorizontalAlign="Right" Name="To be Completed by Manning Office / Agent">
                        </telerik:GridColumnGroup>


                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status1" ColumnGroupName="header text" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Image ID="ImgFlagP" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                    Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Name" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExamID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEREQUESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExamRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMREQUESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTrainingNeedId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEEDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsRaisedFromCBT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataItem%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Rank" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="File No" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" ColumnGroupName="header text" HeaderStyle-Width="170px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn Visible="false" HeaderText="Type of Training" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                    AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course/CBT" ColumnGroupName="header text" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseName" Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Material" ColumnGroupName="header text" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Material"
                                    CommandName="MATERIAL" CommandArgument='<%# Container.DataItem %>' ID="cmdMaterial"
                                    ToolTip="Download Material"> <span class="icon"><i class="fas fa-download"></i></span> </asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date" ColumnGroupName="header text" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoneby1" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE"))  %>'></telerik:RadLabel>
                                <%--<telerik:RadLabel ID="lblDoneby" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:ToBeDoneBy ID="ucDonebyEdit" runat="server" CssClass="input"
                                    AppendDataBoundItems="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Details of Training Imparted / Course Attended" ColumnGroupName="header text" HeaderStyle-Width="130px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                    CssClass="input">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name of Trainer / Institute" ColumnGroupName="header text" HeaderStyle-Width="130px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                    CssClass="input">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Completed" UniqueName="Complete12" ColumnGroupName="header text" HeaderStyle-Width="110px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                <telerik:RadLabel ID="lblDateCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Test" ColumnGroupName="header text" HeaderStyle-Width="130px">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblExamHeader" runat="server" Text="Test"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExam" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No. of Attempts" UniqueName="Attempt14" ColumnGroupName="header text" Visible="false">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblNoOfAttemptHeader" runat="server" Text="No of Attempts"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoOfAttempts" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFATTEMPTS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Score" UniqueName="Score15" ColumnGroupName="header text" Visible="false">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblScoreHeader" runat="server" Text="Score %"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScore" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Result" ColumnGroupName="To be Completed by Manning Office / Agent" HeaderStyle-Width="60px" Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Attended" UniqueName="Attend17" ColumnGroupName="To be Completed by Manning Office / Agent" HeaderStyle-Width="105px" Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateAttended" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEATTENDED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Archived" UniqueName="Archive18" Visible="false" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateArchived" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEARCHIVED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Override Y/N" UniqueName="Override19" Visible="false" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkOverrideYN" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDEYN").ToString().Equals("1")? true : false %>'
                                    Enabled="false" />
                                <telerik:RadLabel ID="lblOverrideYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDEYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="To be Completed by Manning Office / Agent" UniqueName="Action20" HeaderText="Action" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" CommandArgument="<%# Container.DataItem %>" ID="cmdAtt" ToolTip="Upload Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>


                                <asp:LinkButton runat="server" AlternateText="Initiate Course Request" CommandName="Initiate Course Request" Visible="false" CommandArgument="<%# Container.DataItem %>" ID="cmdCourseReq" ToolTip="View Requested Course">
                                    <span class="icon"><i class="fas fa-book"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Initiate Test Request" CommandName="INITIATEEXAMREQ" CommandArgument="<%# Container.DataItem %>" ID="cmdExamReq" ToolTip="Test">
                                    <span class="icon"><i class="fas fa-book"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Initiate Course Request" CommandName="Test Request History" CommandArgument="<%# Container.DataItem %>" ID="cmdExamReqHistory" ToolTip="Test History">
                                    <span class="icon"><i class="fas fa-list-alt-annexure"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Archive" CommandName="Archive" Visible="false" CommandArgument="<%# Container.DataItem %>" ID="cmdArchive" ToolTip="Archive">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="De-Archive" CommandName="DeArchive" Visible="false" CommandArgument="<%# Container.DataItem %>" ID="cmdDeArchive" ToolTip="De-Archive">
                                  <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>


                                <asp:LinkButton runat="server" AlternateText="Override" CommandName="OVERRIDE" CommandArgument="<%# Container.DataItem %>" ID="cmdOverride" ToolTip="Training Need Override">
                                  <span class="icon"><i class="fas fa-calendar-times"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <telerik:RadLabel ID="Label1" runat="server" Text="Pending CBT "
                Font-Bold="true">
            </telerik:RadLabel>
            <eluc:TabStrip ID="MenuOffshoreTraining" runat="server" OnTabStripCommand="MenuOffshoreTraining_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshoreTraining" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
                Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false" OnItemCommand="gvOffshoreTraining_ItemCommand"
                OnRowCreated="gvOffshoreTraining_RowCreated" OnNeedDataSource="gvOffshoreTraining_NeedDataSource" AllowSorting="true"
                OnUpdateCommand="gvOffshoreTraining_UpdateCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                ShowHeader="true" EnableViewState="false" DataKeyNames="FLDTRAININGNEEDID" OnItemDataBound="gvOffshoreTraining_ItemDataBound"
                OnDeleteCommand="gvOffshoreTraining_DeleteCommand">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
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
                        <telerik:GridColumnGroup HeaderText="" HeaderStyle-HorizontalAlign="Center" Name="header text">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="As Reported By Vessel" HeaderStyle-HorizontalAlign="Center" Name="As Reported By Vessel">
                        </telerik:GridColumnGroup>
                           <telerik:GridColumnGroup HeaderText="To be Completed by Manning Office / Agent" HeaderStyle-HorizontalAlign="Center" Name="To be Completed by Manning Office / Agent">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status1" ColumnGroupName="As Reported By Vessel" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="52px">
                            <ItemTemplate>
                                <asp:Image ID="ImgFlagP" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                    Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" UniqueName="Name2" HeaderText="Name" ColumnGroupName="As Reported By Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExamID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEREQUESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExamRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMREQUESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTrainingNeedId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEEDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsRaisedFromCBT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataItem%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" UniqueName="Rank3" HeaderText="Rank" ColumnGroupName="As Reported By Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" UniqueName="FileNo4" HeaderText="File No" ColumnGroupName="As Reported By Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="Vessel5" ColumnGroupName="As Reported By Vessel" HeaderStyle-Width="140px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" UniqueName="Category6" ColumnGroupName="As Reported By Vessel" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString().Length > 40 ? DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblIsRaisedFromCBTEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                <telerik:RadDropDownList ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory"
                                    AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Category" UniqueName="Sub7" ColumnGroupName="As Reported By Vessel" HeaderStyle-Width="140px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString().Length > 40 ? DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory"
                                    AppendDataBoundItems="true">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory"
                                    AppendDataBoundItems="true">
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Identified Training Need" UniqueName="Training8" ColumnGroupName="As Reported By Vessel" HeaderStyle-Width="155px">
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
                        <telerik:GridTemplateColumn HeaderText="Level of Improvement" UniqueName="Improvement9" ColumnGroupName="header text" HeaderStyle-Width="95px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucImprovementEdit" Enabled="false" runat="server" CssClass="input_mandatory"
                                    SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                    AppendDataBoundItems="true" QuickTypeCode="128" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="128" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Type of Training" UniqueName="Type10" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                    AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course/CBT" UniqueName="Course11" ColumnGroupName="header text" HeaderStyle-Width="155px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Material" UniqueName="Material12" ColumnGroupName="header text" HeaderStyle-Width="62px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Material"
                                    CommandName="MATERIAL" CommandArgument='<%# Container.DataItem %>' ID="cmdMaterial"
                                    ToolTip="Download Material"> <span class="icon"><i class="fas fa-download"></i></span> </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To be done by" UniqueName="done13" ColumnGroupName="header text" HeaderStyle-Width="155px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoneby" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:ToBeDoneBy ID="ucDonebyEdit" runat="server" CssClass="input"
                                    AppendDataBoundItems="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Details of Training Imparted / Course Attended" UniqueName="Course14" ColumnGroupName="header text" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                    CssClass="input">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name of Trainer / Institute" UniqueName="Institute15" ColumnGroupName="header text" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                    CssClass="input">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Completed" UniqueName="Date16" ColumnGroupName="header text" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                <telerik:RadLabel ID="lblDateCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Test" UniqueName="Exam17" ColumnGroupName="header text" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExam" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No. of Attempts" UniqueName="Attempt18" ColumnGroupName="To be Completed by Manning Office / Agent" HeaderStyle-Width="90px" Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoOfAttempts" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFATTEMPTS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Score" UniqueName="Score19" ColumnGroupName="To be Completed by Manning Office / Agent" HeaderStyle-Width="80px" Visible="false">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblScoreHeader" runat="server" Text="Score %"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Result" UniqueName="Result20" ColumnGroupName="To be Completed by Manning Office / Agent" HeaderStyle-Width="60px" Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Attended" UniqueName="Date21" ColumnGroupName="To be Completed by Manning Office / Agent" HeaderStyle-Width="100px" Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateAttended" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEATTENDED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Archived" UniqueName="Archive22" Visible="false" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateArchived" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEARCHIVED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Override Y/N" Visible="false" UniqueName="Check23" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkOverrideYN" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDEYN").ToString().Equals("1")? true : false %>'
                                    Enabled="false" />
                                <telerik:RadLabel ID="lblOverrideYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDEYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="To be Completed by Manning Office / Agent" UniqueName="Action24" HeaderStyle-Width="100px" HeaderText="Action" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" CommandArgument="<%# Container.DataItem %>" ID="cmdAtt" ToolTip="Upload Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>


                                <asp:LinkButton runat="server" AlternateText="Initiate Course Request" CommandName="Initiate Course Request" Visible="false" CommandArgument="<%# Container.DataItem %>" ID="cmdCourseReq" ToolTip="View Requested Course">
                                    <span class="icon"><i class="fas fa-book"></i></span>
                                </asp:LinkButton>


                                <asp:LinkButton runat="server" AlternateText="Initiate Test Request" CommandName="INITIATEEXAMREQ" CommandArgument="<%# Container.DataItem %>" ID="cmdExamReq" ToolTip="Test">
                                    <span class="icon"><i class="fas fa-pen-alt"></i></span>
                                </asp:LinkButton>


                                <asp:LinkButton runat="server" AlternateText="Initiate Course Request" CommandName="Test Request History" CommandArgument="<%# Container.DataItem %>" ID="cmdExamReqHistory" ToolTip="Test History">
                                    <span class="icon"><i class="fas fa-list-alt-annexure"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Archive" CommandName="Archive" Visible="false" CommandArgument="<%# Container.DataItem %>" ID="cmdArchive" ToolTip="Archive">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="De-Archive" CommandName="DeArchive" Visible="false" CommandArgument="<%# Container.DataItem %>" ID="cmdDeArchive" ToolTip="De-Archive">
                                  <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Override" CommandName="OVERRIDE" CommandArgument="<%# Container.DataItem %>" ID="cmdOverride" ToolTip="Training Need Override">
                                  <span class="icon"><i class="fas fa-calendar-times"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="lblRed" runat="server" src="<%$ PhoenixTheme:images/red-symbol.png %>" />
                    </td>
                    <td>
                        <asp:Literal ID="lblOverDue" runat="server" Text=" * Overdue"></asp:Literal>
                    </td>
                    <td>
                        <img id="lblGreen" runat="server" src="<%$ PhoenixTheme:images/green-symbol.png %>" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDue" runat="server" Text=" * Due"></asp:Literal>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
