<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaFeedbackForm.aspx.cs"
    Inherits="Presea_PreSeaFeedbackForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Medical</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

   </telerik:RadCodeBlock>
    <style type="text/css">
        .Sftblclass tr td
        {
            border: 1px solid black;
        }
    </style>
</head>
<body>
    <form id="frmPreSeaFeedBackForm" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaFeedBackForm">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="FeedbackForm"></eluc:Title>
                    </div>
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td rowspan="3">
                                <asp:Image ID="imgSims" runat="server" Width="75px" Height="75px" ImageUrl="<%$ PhoenixTheme:images/sims.png %>" />
                            </td>
                            <td>
                                <center>
                                    <h2>
                                        Samundra Institute Of Maritime Studies</h2>
                                </center>
                            </td>
                            <td rowspan="3">
                                <asp:Image ID="imgDnv" runat="server" ImageUrl="<%$ PhoenixTheme:images/dnv.png %>"
                                    Width="80px" Height="75px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <h3>
                                        <b>Participant Feedback Form</b></h3>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <h3>
                                        <b>Adminstration Approved Courses (Pre - Sea)</b></h3>
                                </center>
                            </td>
                        </tr>
                    </table>
                    <table class="Sftblclass" width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <center>
                                    <h4>
                                        Form No: F-2B</h4>
                                </center>
                            </td>
                            <td>
                                <center>
                                    <h4>
                                        Rev No:1</h4>
                                </center>
                            </td>
                            <td>
                                <center>
                                    <h4>
                                        Prep. by : VRK</h4>
                                </center>
                            </td>
                            <td>
                                <center>
                                    <h4>
                                        App. by: SV</h4>
                                </center>
                            </td>
                            <td>
                                <center>
                                    <h4>
                                        <asp:Label ID="lblCurrentDate" runat="server"></asp:Label></h4>
                                </center>
                            </td>
                            <td>
                                <center>
                                    <h4>
                                        Page 1/1</h4>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table>
                        <tr>
                            <td>
                                <ul>
                                    <li>
                                        <h4>
                                            Thank you for completing this form. It’ll give us valuable inputs towards continual
                                            improvements.</h4>
                                    </li>
                                    <li>
                                        <h4>
                                            This form should be completed and handed over to the faculty, twice in each semester.</h4>
                                    </li>
                                    <li>
                                        <h4>
                                            If more space is needed, please use the space at the back of this sheet or use an
                                            extra sheet.</h4>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" class="Sftblclass">
                        <tr>
                            <td align="center">
                                Name:<b>
                                    <asp:Label ID="lblName" runat="server"></asp:Label>
                                </b>
                            </td>
                            <td align="center">
                                Course Name:<b> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblCourseName"
                                    runat="server"></asp:Label>
                                </b>
                            </td>
                            <td align="center">
                                Batch No: <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblBatchNo" runat="server"></asp:Label>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                Feedback Period
                            </td>
                            <td align="center">
                                <b>From Date: </b>
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b> To Date: </b>
                                <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td align="center">
                                Occassion<eluc:Quick ID="ucOccassion" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="106" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblCOurseId" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblSemesterId" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblBatchId" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuPreSeaFeedBackForm" runat="server" OnTabStripCommand="PreSeaFeedBackForm_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div id="divForm" runat="server" visible="false">
                    <div>
                        <asp:GridView ID="gvFeedbackQuestions" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvFeedbackQuestions_RowCommand"
                            OnRowDataBound="gvFeedbackQuestions_ItemDataBound" OnRowCancelingEdit="gvFeedbackQuestions_RowCancelingEdit"
                            OnRowEditing="gvFeedbackQuestions_RowEditing" ShowFooter="false" Style="margin-bottom: 0px"
                            EnableViewState="false" OnSorting="gvFeedbackQuestions_Sorting" AllowSorting="true"
                            OnRowCreated="gvFeedbackQuestions_RowCreated" OnRowUpdating="gvFeedbackQuestions_RowUpdating">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" Width="40%" />
                                    <HeaderTemplate>
                                        Question
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuestionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Answer
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOptionType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONTYPE") %>'></asp:Label>
                                        <asp:Label ID="lblOptions" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWER") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblOptionTypeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONTYPE") %>'></asp:Label>
                                        <asp:Label ID="lblQuestionIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblFeedBackQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKQUESTIONID") %>'></asp:Label>
                                        <asp:CheckBoxList ID="chkOptions" runat="server" Visible="false" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                        <asp:RadioButtonList ID="rblOptions" runat="server" Visible="false" RepeatDirection="Horizontal"
                                            RepeatColumns="3">
                                        </asp:RadioButtonList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" Width="30%" />
                                    <HeaderTemplate>
                                        Comments
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblComments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENTS") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtComments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENTS") %>'
                                            TextMode="MultiLine" Width="300px" Height="75px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
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
                    <br />
                    <table>
                        <tr>
                            <td>
                                <h3>
                                    Course Evaluation</h3>
                                <h4 style="color: Blue;">
                                    Give your grading on a scale 1 to 10<br />
                                    For example :- 1 to 6 = Poor, 6 to 7.5 = Satisfactory; 8 = Good; 9 = V. Good ; 10
                                    = Excellent
                                </h4>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:GridView ID="gvCourseEvaluation" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCourseEvaluation_RowCommand"
                            OnRowDataBound="gvCourseEvaluation_ItemDataBound" OnRowCancelingEdit="gvCourseEvaluation_RowCancelingEdit"
                            OnRowEditing="gvCourseEvaluation_RowEditing" ShowFooter="false" Style="margin-bottom: 0px"
                            EnableViewState="false" OnSorting="gvFeedbackQuestions_Sorting" AllowSorting="true"
                            OnRowCreated="gvCourseEvaluation_RowCreated" OnRowUpdating="gvCourseEvaluation_RowUpdating">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Course Evaluation
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCourseEvaluation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Grade
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrade" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSERATING") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblFeedbackCOurseId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKCOURSEID") %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblQuestionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKCODE") %>'
                                            Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtGrade" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSERATING") %>'
                                            CssClass="input_mandatory"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                            Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
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
                    <br />
                    <div>
                        <table runat="server" width="100%">
                             <tr>
                                <td style="color:Blue">
                                    <b>Suggestions for Future Improvement of the Course.<br />
                                    Please Click Save after the Suggestions is entered.</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtFutureImprovements" runat="server" CssClass="input" TextMode="MultiLine"
                                        Width="100%" Height="75px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="color:Blue">
                                   
                                   <b> Suggestions For Improving The Administrative Arrangements For Joining This Course.<br />
                                    Please Click Save after the Suggestions is entered </b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtSuggestionComments" runat="server" CssClass="input" TextMode="MultiLine"
                                        Width="100%" Height="75px"></asp:TextBox>
                                </td>
                            </tr>
                           
                        </table>
                    </div>
                    <br />
                    <b>
                        <h3>
                            <asp:Label ID="lblText" runat="server">Faculty Evaluation</asp:Label>
                        </h3>
                        <h4 style="color: Blue;">
                            Give your grading on a scale 1 to 10<br />
                            For example :- 1 to 6 = Poor, 6 to 7.5 = Satisfactory; 8 = Good; 9 = V. Good ; 10
                            = Excellent
                        </h4>
                        <asp:Label ID="lblAddText" runat="server" ForeColor="Blue">Click on the '+' to add score for faculty.</asp:Label>
                    </b>
                    <br />
                    <div style="position: absolute">
                        <eluc:TabStrip ID="MenuFacultyFeedback" runat="server" OnTabStripCommand="MenuFacultyFeedback_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <br />
                    <br />
                    <div id="divGrid" runat="server">
                        <asp:GridView ID="gvPreSea" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvPreSea_RowDataBound" ShowHeader="true"
                            ShowFooter="false" EnableViewState="false">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Faculty Evaluation">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuestionId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDQUESTIONID"].ToString()%>'></asp:Label>
                                        <asp:Label ID="lblQuestionName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDQUESTIONNAME"].ToString()%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
