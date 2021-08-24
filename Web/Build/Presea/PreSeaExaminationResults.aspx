<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaExaminationResults.aspx.cs"
    Inherits="Presea_PreSeaExaminationResults" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaSemester.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ExamType" Src="~/UserControls/UserControlPreSeaExam.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchExam" Src="~/UserControls/UserControlPreSeaBatchExam.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaCourseSemester.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pre Sea Exam</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaExamResults" runat="server" submitdisabledcontrols="true">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlPreSeaExamResults">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Exam Results" ShowMenu="false"></eluc:Title>
                        </div>
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                            <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_OnTabStripCommand"></eluc:TabStrip>
                        </div>
                    </div>
                    <table width="100%" cellpadding="2" cellspacing="2">
                        <tr>
                            <td>First Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>Middle Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>Last Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>Batch
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBatch" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <hr />
                    <table id="tblFind" runat="server" width="100%">
                        <tr>
                            <td>Course
                            </td>
                            <td>
                                <eluc:Course ID="ucCourse" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    Enabled="false" Width="240px" />
                            </td>
                            <td>Semester
                            </td>
                            <td>
                                <eluc:Semester ID="ddlSemester" runat="server" AutoPostBack="true" 
                                    CssClass="dropdown_mandatory" AppendDataBoundItems="true" OnTextChangedEvent="ucSemester_Changed" />
                            </td>
                               <td>Batch Exam
                            </td>
                            <td>
                                <eluc:BatchExam ID="ucBatchExam" runat="server" CssClass="dropdown_mandatory" Enabled="false"
                                    AppendDataBoundItems="true" AutoPostBack="true"  Width="240"
                                    OnTextChangedEvent="ucBatchExam_TextChangedEvent"></eluc:BatchExam>
                            </td>
                             <td>Subject Code
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsubjectCode" runat="server"  CssClass="input" Width="120px"></asp:DropDownList>
                                <%--<asp:TextBox runat="server" ID="txtsubjectcode" CssClass="input" Width="120px"></asp:TextBox>--%>
                            </td>
                            </tr>
                        <tr>
                         
                        </tr>
                        <tr>
                           
                            <td>Subject
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsubject" runat="server" Width="240px" CssClass="input"></asp:DropDownList>
                                <%--<asp:TextBox runat="server" ID="txtsubjectname" CssClass="input" Width="240px"></asp:TextBox>--%>
                                <asp:HiddenField ID="hdnBatchId" runat="server" />
                            </td>

                        </tr>
                    </table>
                    <br />
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="PreExamResultsMenu" runat="server" OnTabStripCommand="PreExamResultsMenu_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvPreSeaExam" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCancelingEdit="gvPreSeaExam_RowCancelingEdit"
                            OnRowCommand="gvPreSeaExam_RowCommand" OnRowDataBound="gvPreSeaExam_RowDataBound"
                            OnRowDeleting="gvPreSeaExam_RowDeleting" OnRowEditing="gvPreSeaExam_RowEditing"
                            OnRowUpdating="gvPreSeaExam_RowUpdating" ShowFooter="false" EnableViewState="false"
                            Visible="true">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <asp:TemplateField HeaderText="SNo" Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Sl No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subject Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Subject Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblExamScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMSCHEDULEID") %>'></asp:Label>
                                        <asp:Label ID="lblResultDetailId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESULTDETAILID") %>'></asp:Label>
                                        <asp:Label ID="lblTraineeExamId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEEEXAMID") %>'></asp:Label>
                                        <asp:Label ID="lblSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subject Code">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Subject Code
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubjectCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        Max Score
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMaxMark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXMARKS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        Pass Score
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPassMark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSMARKS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Type
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        Exam Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblExamDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMDATE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblTraineeExamIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEEEXAMID") %>'></asp:Label>
                                        <asp:Label ID="lblExamDateEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMDATE") %>'></asp:Label>
                                        <asp:Label ID="lblExamScheduleIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMSCHEDULEID") %>'></asp:Label>
                                        <asp:Label ID="lblSubjectIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                        <eluc:Date ID="txtExamDateEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMDATE") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        Marks Obtained
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKOBTAINED") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblMarksEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKOBTAINED") %>'></asp:Label>
                                        <eluc:Number ID="txtMarkObtainedEdit" runat="server" CssClass="input_mandatory" Mask="999.99"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKOBTAINED") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        Result
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPassFailName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSFAILNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        IsAbsent
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPassFail" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSFAIL") %>'></asp:Label>
                                        <asp:Label ID="lblAbsentYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABSENTYNNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblPassFailEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSFAIL") %>'></asp:Label>
                                        <asp:CheckBox ID="chkAbsentYNEdit" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDPASSFAIL").ToString() == "2" ?  true : false %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        Attempt
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAttempts" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTEMPTS") %>'></asp:Label>
                                    </ItemTemplate>
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
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit" Visible="false"></asp:ImageButton>
                                        <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" Visible="false" AlternateText="Details" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                            CommandName="HISTORY" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdHistory"
                                            ToolTip="History Details"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                            ToolTip="Save" Visible="false"></asp:ImageButton>
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel" Visible="false"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
