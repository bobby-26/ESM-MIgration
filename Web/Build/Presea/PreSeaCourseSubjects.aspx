<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaCourseSubjects.aspx.cs"
    Inherits="PreSeaCourseSubjects" %>

<%@ Import Namespace="SouthNests.Phoenix.PreSea" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Exam" Src="~/UserControls/UserControlPreSeaExam.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaSemester.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MainSubject" Src="~/UserControls/UserControlPreSeaMainSubject.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MCSubject" Src="~/UserControls/UserControlMultiColumnPreSeaSubject.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
    <form id="frmPreSeaExam" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaRoom">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Subjects" ShowMenu="false"></eluc:Title>
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCourseMaster" runat="server" OnTabStripCommand="CourseMaster_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divSearch">
                    <table id="tblSearch" width="50%">
                        <tr>
                            <td>
                                Course
                            </td>
                            <td>
                                <eluc:Course ID="ddlCourse" runat="server" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaCourseSubjects" runat="server" OnTabStripCommand="MenuPreSeaCourseSubjects_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPreSeaExam" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCancelingEdit="gvPreSeaExam_RowCancelingEdit"
                        OnRowCommand="gvPreSeaExam_RowCommand" OnRowDataBound="gvPreSeaExam_RowDataBound"
                        OnRowDeleting="gvPreSeaExam_RowDeleting" OnRowEditing="gvPreSeaExam_RowEditing"
                        OnRowUpdating="gvPreSeaExam_RowUpdating" ShowFooter="true" EnableViewState="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />                          
                            <asp:TemplateField HeaderText="Subject">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Subject
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSESUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCourseSubjectIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSESUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:MCSubject ID="ucSubjectAdd" runat="server" CssClass="input_mandatory" Width="200" />                                   
                                </FooterTemplate>
                            </asp:TemplateField>  
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Semester
                                </HeaderTemplate>
                                <ItemTemplate>
                                  <asp:Label ID="lblSemester" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSemesterEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>' Visible="false"></asp:Label>
                                  <asp:DropDownList ID="ddlSemesterEdit" runat="server" AppendDataBoundItems="true" Width="120px" CssClass="input" >
                                  <asp:ListItem Text="--Select--" Value =""></asp:ListItem>
                                  <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                  <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                  <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                  <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                  <asp:ListItem Text="5" Value="5"></asp:ListItem>                                  
                                  <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                  <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                  <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                  <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                  <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                  </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate >
                                  <asp:DropDownList ID="ddlSemesterAdd" runat="server" AppendDataBoundItems="true" Width="120px" CssClass="input" >
                                  <asp:ListItem Text="--Select--" Value =""></asp:ListItem>
                                  <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                  <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                  <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                  <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                  <asp:ListItem Text="5" Value="5"></asp:ListItem>                                  
                                  <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                  <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                  <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                  <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                  <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                  </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                 <asp:Literal ID="lblSubjectCode" Text="Subject Code" runat="server"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                  <asp:Label ID="lblSubjectCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSubjectCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTCODE") %>' CssClass="input_mandatory"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSubjectCodeAdd" runat="server" CssClass="input_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Active
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkAciveEdit" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1")? true: false %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkAciveAdd" runat="server" Checked="true" />
                                </FooterTemplate>
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
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" visible="false" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
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
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
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
