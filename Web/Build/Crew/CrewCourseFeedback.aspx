<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseFeedback.aspx.cs"
    Inherits="CrewCourseFeedback" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Feedback</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body >
    <form id="frmCrewFeedback" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAssessmentList">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Status runat="server" ID="ucStatusConf" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <asp:HiddenField ID="hdnScroll" runat="server" />
                <div id="divScroll" style="position: relative; z-index: 1; width: 100%; height: 100%;
                    overflow: auto;" onscroll="javascript:setScroll('divScroll', 'hdnScroll');">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <asp:Literal ID="lblParticipantFeedback" runat="server" Text="Participant Feedback"></asp:Literal>
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuCourseFeedback" runat="server" OnTabStripCommand="CourseFeedback_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div id="divFind" style="position: relative; z-index: 2">
                        <table id="tblConfigureAssessmentList" width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                        Enabled="false" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblCourseType" runat="server" Text="Course Type"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard runat="server" ID="ucCourseType" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                        Enabled="false" HardTypeCode="103" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b><asp:Literal ID="lblSelectaCandidate" runat="server" Text="Select a Candidate"></asp:Literal></b>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCandidate" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        AutoPostBack="true" OnTextChanged="SelectCandidate">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBatchNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="360px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lbl1Whatdoyouhopetogainfromattendingthiscourse" runat="server" Text="1. What do you hope to gain from attending this course?"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="lbl2Doyouhaveanysuggestionsforimprovingtheadministrativearrangementsforbookingjoiningthiscourse" runat="server" Text="2. Do you have any suggestions for improving the administrative arrangements for booking /joining this course?"></asp:Literal><br />
                                    <asp:Literal ID="lblIfYespleasespecify" runat="server" Text="(If “Yes” please specify:)"></asp:Literal>
                                    <asp:RadioButtonList ID="rbComment2" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="N/A"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtComment1" runat="server" CssClass="input_mandatory" Width="460px"
                                        Height="50px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComment2" runat="server" CssClass="input" Width="460px" Height="50px"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lbl3DidyoureceiveinformationregardingtheCourseandModulesObjectivesScopeandSubjectareas" runat="server" Text="3. Did you receive information regarding the Course and Modules Objectives, Scope and Subject areas?"></asp:Literal><br />
                                    <asp:Literal ID="lblYesNoAnysuggestionstoimprovethisinformation" runat="server" Text="(Yes / No. Any suggestions to improve this information)"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="lbl4Wasthecoursedurationnumberofdayshoursperday" runat="server" Text="4. Was the course duration, number of days & hours per day?"></asp:Literal><br />
                                    <asp:Literal ID="lblPleasetickasappropriateIfyouhavetickedthe1or3pleasecomment" runat="server" Text="Please tick as appropriate.(If you have ticked the (1) or (3) please comment)"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbComment3" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="N/A"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:TextBox ID="txtComment3" runat="server" CssClass="input" Width="460px" Height="50px"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbComment4" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="Too Long"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Just right to cover topics fully"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Too Short"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:TextBox ID="txtComment4" runat="server" CssClass="input" Width="460px" Height="50px"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lbl5Topicsthatwere" runat="server" Text="5. Topics that were:"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblAOfmostinteresttoyou" runat="server" Text="A*: Of most interest to you"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="lblBOfleastinteresttoyou" runat="server" Text="B*. Of least interest to you"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtComment5" runat="server" CssClass="input" Width="460px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComment6" runat="server" CssClass="input" Width="460px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lbl6Doyouconsiderthattheobjectivesofthecourseweremet" runat="server" Text="6. Do you consider that the objectives of the course were met?"></asp:Literal><br />
                                    <asp:Literal ID="lblIfNopleasecomment" runat="server" Text="(If “No”, please comment)"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="lbl7Anysuggestionsforfutureimprovementofthecourse" runat="server" Text="7. Any suggestions for future improvement of the course;"></asp:Literal>
                                    <br />
                                    <asp:Literal ID="lblieamendmentsdeletionsoradditionstothetopicspresentedinthecourseoronfaculty" runat="server" Text="i.e. amendments, deletions or additions to the topics presented in the course or on faculty:-"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbComment5" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:TextBox ID="txtComment7" runat="server" CssClass="input" Width="460px" Height="50px"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComment8" runat="server" CssClass="input" Width="460px" Height="50px"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Literal ID="lbl8Pleasegiveyourgradingaboutthecourseonascale1to10where10Excellent9VGood8Good6to75SatisfactoryorAverage6andbelowFairtoPoor" runat="server" Text="8. Please give your grading about the course on a scale 1 to 10, where 10 = Excellent, 9 = V.Good, 8 = Good, 6 to 7.5 = Satisfactory or Average, 6 and below = Fair to Poor"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divGrid" style="position: relative; z-index: +1">
                                        <b><asp:Literal ID="lblCourseEvaluation" runat="server" Text="Course Evaluation"></asp:Literal></b>
                                        <asp:GridView ID="gvCourseEvaluation" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCourseEvaluation_RowCommand" OnRowCreated="gvCourseEvaluation_RowCreated"
                                            OnRowDataBound="gvCourseEvaluation_ItemDataBound" OnRowEditing="gvCourseEvaluation_RowEditing" OnSelectedIndexChanging="gvCourseEvaluation_SelectedIndexChanging"
                                            OnRowCancelingEdit="gvCourseEvaluation_RowCancelingEdit" ShowFooter="false" OnRowUpdating="gvCourseEvaluation_RowUpdating"
                                            ShowHeader="true" OnRowDeleting="gvCourseEvaluation_RowDeleting" EnableViewState="false">
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                            <RowStyle Height="10px" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:Literal ID="lblCourseEvaluation" runat="server" Text="Course Evaluation"></asp:Literal>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEvaluationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVALUATIONID") %>'></asp:Label>
                                                        <asp:Label ID="lblCourseEvaluationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEEVALUATIONID") %>'></asp:Label>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle HorizontalAlign="Right" Width="20%"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:Literal ID="lblGrade" runat="server" Text="Grade"></asp:Literal>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRADE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number ID="txtMarksEdit" runat="server" CssClass="input_mandatory"
                                                            MaxLength="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRADE") %>' IsInteger="true">
                                                        </eluc:Number>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblActionHeader" runat="server">
                                                        Action
                                                        </asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                                            ToolTip="Edit"></asp:ImageButton>
                                                        <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                                            ToolTip="Delete"></asp:ImageButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                            CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                                            ToolTip="Save"></asp:ImageButton>
                                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                            width="3" />
                                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                            ToolTip="Cancel"></asp:ImageButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                                <td>
                                    <div class="navSelect" style="position: relative; width: 400px">
                                        <asp:Label ID="lblNote" runat="server" Text="Note:Click on the add button to Assign/Edit Faculty Evaluation"
                                            CssClass="guideline_text"></asp:Label>
                                        <eluc:TabStrip ID="MenuFacultyEvaluation" runat="server" OnTabStripCommand="FacultyEvaluation_TabStripCommand">
                                        </eluc:TabStrip>
                                    </div>
                                    <div id="div1" style="position: relative; z-index: 0; width: 100%;">
                                        <asp:GridView ID="gvBindFacultyEvaluation" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false" OnRowDataBound="gvBindFacultyEvaluation_RowDataBound"
                                            OnRowEditing="gvBindFacultyEvaluation_RowEditing" OnRowCancelingEdit="gvBindFacultyEvaluation_RowCancelingEdit"
                                            OnRowUpdating="gvBindFacultyEvaluation_RowUpdating" EnableViewState="False">
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                            <Columns>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lbl9CourseRemarks" runat="server" Text="9.Course Remarks"></asp:Literal>
                                </td>
                                <td>
                                   <asp:Literal ID="lbl10FacultyRemarks" runat="server" Text="10.Faculty Remarks"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCourseRemarks" runat="server" CssClass="input" Width="460px" Height="50px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFacultyRemarks" runat="server" CssClass="input" Width="460px" Height="50px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
