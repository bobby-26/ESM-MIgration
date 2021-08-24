<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaFacultyFeedback.aspx.cs"
    Inherits="Presea_PreSeaFacultyFeedback" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Feedback</title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmFacultyFeedback" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlFacultyFeedback">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        Faculty Evaluation
                    </div>
                </div>
                
                <h4><b style="color:Blue">***Important: Where grading given is 6 or below;supporting comments to be mentioned***</b></h4>
                
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureAssessmentList" width="100%">
                        <tr>
                            <td>
                                <b>Select a Faculty</b>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFaculty" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    AutoPostBack="true">
                                    <%--<asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div id="divGrid" style="position: relative; z-index: +1">
                        <asp:GridView ID="gvFacultyEvaluation" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvFacultyEvaluation_RowCommand"
                            OnRowDataBound="gvFacultyEvaluation_ItemDataBound" OnRowEditing="gvFacultyEvaluation_RowEditing"
                            OnRowCancelingEdit="gvFacultyEvaluation_RowCancelingEdit" ShowFooter="true" OnRowUpdating="gvFacultyEvaluation_RowUpdating"
                            ShowHeader="true" OnRowDeleting="gvFacultyEvaluation_RowDeleting" EnableViewState="false">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                    <HeaderTemplate>
                                        Faculty Evaluation
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEvaluationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKID") %>'></asp:Label>
                                        <asp:Label ID="lblFacultyEvaluationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKFACULTYID") %>'></asp:Label>
                                        <asp:Label ID="lblQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYQUESTIONID") %>'></asp:Label>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Right" Width="20%"></ItemStyle>
                                    <HeaderTemplate>
                                        Grade
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYRATING") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblFacultyid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYID") %>'></asp:Label>
                                        <asp:Label ID="lblQuestionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYQUESTIONID") %>'></asp:Label>
                                        <asp:Label ID="lblFacultyEvaluationIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKFACULTYID") %>'></asp:Label>
                                        <eluc:Number ID="txtMarksEdit" runat="server" CssClass="input_mandatory txtNumber"
                                            MaxLength="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYRATING") %>' IsInteger="true" DecimalPlace="0">
                                        </eluc:Number>
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
