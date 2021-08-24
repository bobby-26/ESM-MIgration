<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseFacultyEvaluationList.aspx.cs"
    Inherits="CrewCourseFacultyEvaluationList" MaintainScrollPositionOnPostback="true" %>

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
<body>
    <form id="frmCrewFeedback" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAssessmentList">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblHeadingFacultyEvaluation" runat="server" Text="Faculty Evaluation"></asp:Literal>
                    </div>
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
                                <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBatchNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="360px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <asp:Literal ID="lblSelectaFaculty" runat="server" Text="Select a Faculty"></asp:Literal></b>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFaculty" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div id="divGrid" style="position: relative; z-index: +1">
                        <b>
                            <asp:Literal ID="lblFacultyEvaluation" runat="server" Text="Faculty Evaluation"></asp:Literal></b>
                        <asp:GridView ID="gvFacultyEvaluation" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvFacultyEvaluation_RowCommand"
                            OnRowDataBound="gvFacultyEvaluation_ItemDataBound" OnRowEditing="gvFacultyEvaluation_RowEditing"
                            OnRowCancelingEdit="gvFacultyEvaluation_RowCancelingEdit" ShowFooter="true" OnRowUpdating="gvFacultyEvaluation_RowUpdating"
                            OnRowCreated="gvFacultyEvaluation_RowCreated" ShowHeader="true" OnRowDeleting="gvFacultyEvaluation_RowDeleting"
                            EnableViewState="false" OnSelectedIndexChanging="gvFacultyEvaluation_SelectedIndexChanging">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblFacultyEvaluationHeader" runat="server" Text="Faculty Evaluation"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEvaluationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVALUATIONID") %>'></asp:Label>
                                        <asp:Label ID="lblFacultyEvaluationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYEVALUATIONID") %>'></asp:Label>
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
                                        <eluc:Number ID="txtMarksEdit" runat="server" CssClass="input_mandatory txtNumber"
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
