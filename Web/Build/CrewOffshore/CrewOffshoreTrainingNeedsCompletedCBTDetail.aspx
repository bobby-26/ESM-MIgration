<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingNeedsCompletedCBTDetail.aspx.cs" Inherits="CrewOffshore_CrewOffshoreTrainingNeedsCompletedCBTDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
    <title>Crew Recommended Courses</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewRecommendedCourses" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlCrewRecommendedCourses" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="TrainingNeed" runat="server" TabStrip="true" OnTabStripCommand="TrainingNeed_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr align="left">
                    <td>
                        <telerik:RadLabel ID="lblfileno" runat="server" Text="File No."></telerik:RadLabel>

                    </td>
                    <td></td>
                    <td>
                        <telerik:RadTextBox ID="txtfileno" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblfirstname" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblfname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td></td>
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
                    <telerik:ButtonListItem Text="Completed after due date" Value="OVERDUE"></telerik:ButtonListItem>
                </Items>
            </telerik:RadRadioButtonList>
            <br />
            <telerik:RadLabel ID="lblcompheading" runat="server" Text="Completed CBT"
                Font-Bold="true">
            </telerik:RadLabel>
            <eluc:TabStrip ID="MenuOffshoreTraining3" runat="server" OnTabStripCommand="MenuOffshoreTraining3_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvcmtcompleted" runat="server" Height="70%" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true"
                Width="100%" CellPadding="3" ShowFooter="false" OnItemCommand="gvcmtcompleted_ItemCommand" OnRowCreated="gvcmtcompleted_RowCreated" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnUpdateCommand="gvcmtcompleted_UpdateCommand" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDTRAININGNEEDID" OnNeedDataSource="gvcmtcompleted_NeedDataSource"
                OnItemDataBound="gvcmtcompleted_ItemDataBound" OnDeleteCommand="gvcmtcompleted_DeleteCommand">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDTRAININGNEEDID" CommandItemDisplay="Top">
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
                        <telerik:GridColumnGroup HeaderText="As Reported By Vessel" HeaderStyle-HorizontalAlign="Center" Name="As Reported By Vessel">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="To be Completed by Manning Office / Agent" HeaderStyle-HorizontalAlign="Center" Name="To be Completed by Manning Office / Agent">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn Visible="false" UniqueName="Name1" HeaderText="Name" HeaderStyle-Width="" ColumnGroupName="As Reported By Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTrainingNeedId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEEDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsRaisedFromCBT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataItem%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" UniqueName="Rank2" HeaderStyle-Width="60px" ColumnGroupName="As Reported By Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="File No" UniqueName="FileNo3" HeaderStyle-Width="" ColumnGroupName="As Reported By Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="Vessel4" HeaderStyle-Width="150px" ColumnGroupName="As Reported By Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" UniqueName="Category5" HeaderStyle-Width="140px" ColumnGroupName="As Reported By Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString().Length > 40 ? DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblIsRaisedFromCBTEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                <telerik:RadDropDownList ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Category" UniqueName="Sub6" HeaderStyle-Width="140px" ColumnGroupName="As Reported By Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString().Length > 40 ? DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Identified Training Need" UniqueName="Training7" HeaderStyle-Width="140px" ColumnGroupName="As Reported By Vessel">
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
                        <telerik:GridTemplateColumn HeaderText="Level of Improvement" UniqueName="Level8" HeaderStyle-Width="90px" ColumnGroupName="header text">
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
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Type of Training" UniqueName="Type9" HeaderStyle-Width="" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                    AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course/CBT" UniqueName="Course10" HeaderStyle-Width="140px" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date" UniqueName="done11" HeaderStyle-Width="100px" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoneby1" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE"))  %>'></telerik:RadLabel>

                                <%--<telerik:RadLabel ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:ToBeDoneBy ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" AppendDataBoundItems="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Details of Training Imparted / Course Attended" UniqueName="Imparted12" HeaderStyle-Width="" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                    CssClass="input">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name of Trainer / Institute" UniqueName="Institute13" HeaderStyle-Width="" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                    CssClass="input">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Completed" UniqueName="Date14" HeaderStyle-Width="80px" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                <telerik:RadLabel ID="lblDateCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' Visible="false"></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Test" UniqueName="Test15" HeaderStyle-Width="65px" ColumnGroupName="header text">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExam" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No. Of Attempts" UniqueName="Attempt16" HeaderStyle-Width="75px" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoOfAttempts" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFATTEMPTS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Score" UniqueName="Score17" HeaderStyle-Width="80px" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblScoreHeader" runat="server" Text="Score %"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Result" UniqueName="Result18" HeaderStyle-Width="70px" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Attended" UniqueName="Attend19" HeaderStyle-Width="80px" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateAttended" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEATTENDED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Archived" Visible="false" UniqueName="Archive20" HeaderStyle-Width="" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateArchived" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEARCHIVED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action21" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" ColumnGroupName="To be Completed by Manning Office / Agent">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" CommandArgument="<%# Container.DataItem %>" ID="cmdAtt" ToolTip="Upload Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Initiate Course Request" Visible="false" CommandName="Initiate Course Request" CommandArgument="<%# Container.DataItem %>" ID="cmdCourseReq" ToolTip="View Requested Course">
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

                                <asp:LinkButton ID="imgCertificate" AlternateText="Generate Certificate" runat="server" CommandName="CERTIFICATE"
                                    ToolTip="Generate Certificate" CommandArgument='<%# Container.DataItem %>' Visible="false"> <span class="icon"><i class="fas fa-graduation-cap"></i></span></asp:LinkButton>
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
