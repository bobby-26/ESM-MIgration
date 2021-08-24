<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingNeedsVessel.aspx.cs" Inherits="CrewOffshoreTrainingNeedsVessel" %>

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
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewRecommendedCourses" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:Title runat="server" ID="ucTitle" Text="Pending Training Needs" ShowMenu="true" Visible="false" />
            <eluc:TabStrip ID="TrainingNeed" runat="server" TabStrip="true" OnTabStripCommand="TrainingNeed_TabStripCommand"></eluc:TabStrip>
            <%--<div class="subHeader" style="position: relative;">               
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">                    
                        <eluc:TabStrip ID="CourseRequest" runat="server" TabStrip="true" OnTabStripCommand="CourseRequest_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>--%>
            <eluc:TabStrip ID="MenuOffshoreTraining" runat="server" OnTabStripCommand="MenuOffshoreTraining_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshoreTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" Height="86%" CellPadding="3" ShowFooter="false" OnItemCommand="gvOffshoreTraining_ItemCommand" OnRowCreated="gvOffshoreTraining_RowCreated"
                EnableHeaderContextMenu="true" GroupingEnabled="false"
                ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvOffshoreTraining_NeedDataSource"
                OnItemDataBound="gvOffshoreTraining_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDTRAININGNEEDID">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="GridColumnGroup1" HeaderText="As Reported by the Vessel" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="GridColumnGroup2" HeaderText="To be Completed by Manning Office/Agent" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <NestedViewTemplate>
                        <table style="font-size: 11px;">
                            <tr>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>

                                </td>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel4" runat="server" Text="File No:"></telerik:RadLabel>

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblFileNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>

                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="lblCategory" runat="server" Text="Category :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>

                                </td>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="lblSubCategory" runat="server" Text="SubCategory :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel3" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>

                                </td>
                            </tr>
                        </table>
                    </NestedViewTemplate>
                    <Columns>
                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="7%" ColumnGroupName="GridColumnGroup1" UniqueName="Statusid">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="ImgFlagP" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                    Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="13%" ColumnGroupName="GridColumnGroup1" AllowSorting="true" SortExpression="FLDNAME" ShowSortIcon="true">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExamID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEREQUESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExamRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMREQUESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTrainingNeedId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEEDID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataItem%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="8%" HeaderText="Rank" AllowSorting="true" SortExpression="FLDRANKNAME" ShowSortIcon="true" ColumnGroupName="GridColumnGroup1">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="8%" HeaderText="File No" ColumnGroupName="GridColumnGroup1" Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="10%" HeaderText="Vessel" ColumnGroupName="GridColumnGroup1" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="10%" HeaderText="Category" ColumnGroupName="GridColumnGroup1" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="8%" HeaderText="Sub Category" ColumnGroupName="GridColumnGroup1" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="12%" HeaderText="Identified Training Need" ColumnGroupName="GridColumnGroup1">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="12%" HeaderText="Level of Improvement" ColumnGroupName="GridColumnGroup1">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="12%" HeaderText="Type of Training" ColumnGroupName="GridColumnGroup1">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                    AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course/CBT" HeaderStyle-Width="12%" ColumnGroupName="GridColumnGroup1">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></telerik:RadLabel>
                                <asp:ImageButton ID="imgCourseName" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItem %>'></asp:ImageButton>
                                <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Material" HeaderStyle-Width="10%" ColumnGroupName="GridColumnGroup1">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Material" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>"
                                    CommandName="MATERIAL" CommandArgument='<%# Container.DataItem %>' ID="cmdMaterial"
                                    ToolTip="Download Material"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="16%" HeaderText="To be done by" ColumnGroupName="GridColumnGroup2" UniqueName="Tobedonebyid">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoneby" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:ToBeDoneBy ID="ucDonebyEdit" Width="100%" runat="server" CssClass="input" AppendDataBoundItems="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="15%" UniqueName="DetailsofTrainingImparted" HeaderText="Details of Training Imparted / Course Attended" ColumnGroupName="GridColumnGroup2">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                    CssClass="input">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="15%" UniqueName="NameofTrainer" HeaderText="Name of Trainer / Institute" ColumnGroupName="GridColumnGroup2">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                    CssClass="input">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="12%" HeaderText="Date Completed" ColumnGroupName="GridColumnGroup2">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucCompletionDate" Width="100%" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Test" HeaderStyle-Width="14%" ColumnGroupName="GridColumnGroup2">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExam" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No. Of Attempts" HeaderStyle-Width="10%" ColumnGroupName="GridColumnGroup2">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoOfAttempts" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFATTEMPTS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Score %" HeaderStyle-Width="10%" ColumnGroupName="GridColumnGroup2">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Result" HeaderStyle-Width="7%" ColumnGroupName="GridColumnGroup2" UniqueName="Resultid">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Attended" HeaderStyle-Width="12%" ColumnGroupName="GridColumnGroup2" UniqueName="DateAttendedid">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateAttended" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEATTENDED")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Override Y/N" Visible="false" HeaderStyle-Width="5%" ColumnGroupName="GridColumnGroup2" UniqueName="OverrideY/Nid">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkOverrideYN" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDEYN").ToString().Equals("1")? true : false %>' Enabled="false" />
                                <telerik:RadLabel ID="lblOverrideYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDE") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="13%" HeaderText="Action" ColumnGroupName="GridColumnGroup2" UniqueName="Actionid">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                    ToolTip="Edit" Visible="false"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItem %>' ID="cmdAtt"
                                    ToolTip="Upload Attachment"></asp:ImageButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Initiate Course Request" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>"
                                    CommandName="Initiate Course Request" CommandArgument="<%# Container.DataItem %>" ID="cmdCourseReq" Visible="false"
                                    ToolTip="View Requested Course"></asp:ImageButton>
                                <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Initiate Test Request" ImageUrl="<%$ PhoenixTheme:images/course-request.png %>"
                                    CommandName="INITIATEEXAMREQ" CommandArgument="<%# Container.DataItem %>"
                                    ID="cmdExamReq" ToolTip="Test"></asp:ImageButton>
                                <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Initiate Course Request" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                    CommandName="Test Request History" CommandArgument="<%# Container.DataItem %>" ID="cmdExamReqHistory"
                                    ToolTip="Test History"></asp:ImageButton>
                                <img id="Img6" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" visible="false" />
                                <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                    CommandName="Archive" CommandArgument='<%# Container.DataItem %>' ID="cmdArchive" Visible="false"
                                    ToolTip="Archive"></asp:ImageButton>
                                <img id="Img7" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" visible="false" />
                                <asp:ImageButton runat="server" AlternateText="De-Archive" ImageUrl="<%$ PhoenixTheme:images/dearchive.png %>"
                                    CommandName="DeArchive" CommandArgument='<%# Container.DataItem %>' ID="cmdDeArchive" Visible="false"
                                    ToolTip="De-Archive"></asp:ImageButton>
                                <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                <img id="Img8" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Override" ImageUrl="<%$ PhoenixTheme:images/28.png %>"
                                    CommandName="OVERRIDE" CommandArgument="<%# Container.DataItem %>" ID="cmdOverride" Visible="false"
                                    ToolTip="Training Need Override"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="UPDATE" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
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
                        <telerik:RadLabel ID="lblOverDue" runat="server" Text=" * Overdue"></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="lblGreen" runat="server" src="<%$ PhoenixTheme:images/green-symbol.png %>" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDue" runat="server" Text=" * Due"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
