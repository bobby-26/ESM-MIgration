<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchExamSchedule.aspx.cs"
    Inherits="PreSeaBatchExamSchedule" %>

<%@ Import Namespace="SouthNests.Phoenix.PreSea" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Exam" Src="~/UserControls/UserControlPreSeaExam.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaSemester.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Subject" Src="~/UserControls/UserControlPreSeaSubject.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Section" Src="~/UserControls/UserControlPreseaSection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaCourseSemester.ascx" %>
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
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Exam Planner"></eluc:Title>
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBatchPlanner" runat="server" OnTabStripCommand="BatchPlanner_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divSearch">
                    <table id="tblSearch" cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                Course
                            </td>
                            <td>
                                <eluc:Course ID="ddlCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox" />
                            </td>
                            <td>
                                Batch
                            </td>
                            <td>
                                <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Width="240px"/>
                            </td>
                            <td>
                                Exam
                            </td>
                            <td>
                                <eluc:Exam ID="ucExam" runat="server" AppendDataBoundItems="true" CssClass="input" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Semester
                            </td>
                            <td>           
                                  <eluc:Semester ID="ddlSemester" runat="server" Width="200px"  
                                      CssClass="dropdown_mandatory" AppendDataBoundItems="true"/>                     
                             <%--   <asp:DropDownList ID="ucSemester" runat="server" AppendDataBoundItems="true" Width="120px"
                                    AutoPostBack="true" CssClass="input">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
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
                                </asp:DropDownList>--%>
                            </td>
                            <td>
                                Section
                            </td>
                            <td>                                
                                <asp:DropDownList runat="server" ID="ucSection" AppendDataBoundItems="true" CssClass="input"
                                    Width="240px">
                                    <asp:ListItem Text="--Select--" Value="">--Select--</asp:ListItem>
                                    <asp:ListItem Text="A" Value="1">A</asp:ListItem>
                                    <asp:ListItem Text="B" Value="2">B</asp:ListItem>
                                    <asp:ListItem Text="C" Value="3">C</asp:ListItem>
                                    <asp:ListItem Text="D" Value="4">D</asp:ListItem>
                                    <asp:ListItem Text="E" Value="5">E</asp:ListItem>
                                    <asp:ListItem Text="F" Value="6">F</asp:ListItem>
                                    <asp:ListItem Text="G" Value="7">G</asp:ListItem>
                                    <asp:ListItem Text="H" Value="8">H</asp:ListItem>
                                    <asp:ListItem Text="I" Value="9">I</asp:ListItem>
                                    <asp:ListItem Text="J" Value="10">J</asp:ListItem>
                                    <asp:ListItem Text="K" Value="11">K</asp:ListItem>
                                    <asp:ListItem Text="L" Value="12">L</asp:ListItem>
                                    <asp:ListItem Text="M" Value="13">M</asp:ListItem>
                                    <asp:ListItem Text="N" Value="14">N</asp:ListItem>
                                    <asp:ListItem Text="O" Value="15">O</asp:ListItem>
                                    <asp:ListItem Text="P" Value="16">P</asp:ListItem>
                                    <asp:ListItem Text="Q" Value="17">Q</asp:ListItem>
                                    <asp:ListItem Text="R" Value="18">R</asp:ListItem>
                                    <asp:ListItem Text="S" Value="19">S</asp:ListItem>
                                    <asp:ListItem Text="T" Value="20">T</asp:ListItem>
                                    <asp:ListItem Text="U" Value="21">U</asp:ListItem>
                                    <asp:ListItem Text="V" Value="22">V</asp:ListItem>
                                    <asp:ListItem Text="W" Value="23">W</asp:ListItem>
                                    <asp:ListItem Text="X" Value="24">X</asp:ListItem>
                                    <asp:ListItem Text="Y" Value="25">Y</asp:ListItem>
                                    <asp:ListItem Text="Z" Value="26">Z</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                From Date - To Date
                            </td>
                            <td>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                               <asp:Literal ID="ltrdash" runat="server" Text="-"></asp:Literal>
                                <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaExam" runat="server" OnTabStripCommand="PreSeaExam_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPreSeaExam" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCancelingEdit="gvPreSeaExam_RowCancelingEdit"
                        OnRowCommand="gvPreSeaExam_RowCommand" OnRowDataBound="gvPreSeaExam_RowDataBound"
                        OnRowDeleting="gvPreSeaExam_RowDeleting" OnRowEditing="gvPreSeaExam_RowEditing"
                        OnSelectedIndexChanging="gvPreSeaExam_SelectedIndexChanging" OnRowUpdating="gvPreSeaExam_RowUpdating"
                        ShowFooter="true" EnableViewState="false" DataKeyNames="FLDEXAMSCHEDULEID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="S.No.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    S.No.
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exam Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Exam
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMSCHEDULEID") %>'></asp:Label>
                                    <asp:Label ID="lblPreSeaCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESEACOURSEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkExamName" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                        CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAM") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exam Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Exam Type
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblExamType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'></asp:Label>
                                    <asp:Label ID="lblExamTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMID") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <eluc:Exam ID="ucExamTypeAdd" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Semester Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Semester
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSemesterName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>'></asp:Label>
                                    <asp:Label ID="lblSemesterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                      <eluc:Semester ID="ucSemesterAdd" runat="server" Width="120px"   
                                     CssClass="dropdown_mandatory" AppendDataBoundItems="true"/>   
                                    <%--<asp:DropDownList ID="ucSemesterAdd" runat="server" AppendDataBoundItems="true" Width="120px"
                                        AutoPostBack="true" CssClass="input">
                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
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
                                    </asp:DropDownList>     --%>                           
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    From Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblBatchIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHID") %>'></asp:Label>
                                    <asp:Label ID="lblSemesterIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>'></asp:Label>
                                    <asp:Label ID="lblExamTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMID") %>'></asp:Label>
                                    <asp:Label ID="lblScheduleIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMSCHEDULEID") %>'></asp:Label>
                                    <eluc:Date ID="txtFromDateEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="txtFromdateAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    To Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblToDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtToDateEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="txtToDateAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Section
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                    
                                    <asp:DropDownList runat="server" ID="ucSectionEdit" AppendDataBoundItems="true" 
                                        CssClass="input_mandatory"  Width="120px"> 
                                        <asp:ListItem Text="--Select--" Value="">--Select--</asp:ListItem>                                       
                                        <asp:ListItem Text="A" Value="1">A</asp:ListItem>
                                        <asp:ListItem Text="B" Value="2">B</asp:ListItem>
                                        <asp:ListItem Text="C" Value="3">C</asp:ListItem>
                                        <asp:ListItem Text="D" Value="4">D</asp:ListItem>
                                        <asp:ListItem Text="E" Value="5">E</asp:ListItem>
                                        <asp:ListItem Text="F" Value="6">F</asp:ListItem>
                                        <asp:ListItem Text="G" Value="7">G</asp:ListItem>
                                        <asp:ListItem Text="H" Value="8">H</asp:ListItem>
                                        <asp:ListItem Text="I" Value="9">I</asp:ListItem>
                                        <asp:ListItem Text="J" Value="10">J</asp:ListItem>
                                        <asp:ListItem Text="K" Value="11">K</asp:ListItem>
                                        <asp:ListItem Text="L" Value="12">L</asp:ListItem>
                                        <asp:ListItem Text="M" Value="13">M</asp:ListItem>
                                        <asp:ListItem Text="N" Value="14">N</asp:ListItem>
                                        <asp:ListItem Text="O" Value="15">O</asp:ListItem>
                                        <asp:ListItem Text="P" Value="16">P</asp:ListItem>
                                        <asp:ListItem Text="Q" Value="17">Q</asp:ListItem>
                                        <asp:ListItem Text="R" Value="18">R</asp:ListItem>
                                        <asp:ListItem Text="S" Value="19">S</asp:ListItem>
                                        <asp:ListItem Text="T" Value="20">T</asp:ListItem>
                                        <asp:ListItem Text="U" Value="21">U</asp:ListItem>
                                        <asp:ListItem Text="V" Value="22">V</asp:ListItem>
                                        <asp:ListItem Text="W" Value="23">W</asp:ListItem>
                                        <asp:ListItem Text="X" Value="24">X</asp:ListItem>
                                        <asp:ListItem Text="Y" Value="25">Y</asp:ListItem>
                                        <asp:ListItem Text="Z" Value="26">Z</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblSectionEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>                                    
                                    <asp:DropDownList runat="server" ID="ucSectionAdd" AppendDataBoundItems="true" CssClass="input"
                                        Width="120px">
                                        <asp:ListItem Text="--Select--" Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Text="A" Value="1">A</asp:ListItem>
                                        <asp:ListItem Text="B" Value="2">B</asp:ListItem>
                                        <asp:ListItem Text="C" Value="3">C</asp:ListItem>
                                        <asp:ListItem Text="D" Value="4">D</asp:ListItem>
                                        <asp:ListItem Text="E" Value="5">E</asp:ListItem>
                                        <asp:ListItem Text="F" Value="6">F</asp:ListItem>
                                        <asp:ListItem Text="G" Value="7">G</asp:ListItem>
                                        <asp:ListItem Text="H" Value="8">H</asp:ListItem>
                                        <asp:ListItem Text="I" Value="9">I</asp:ListItem>
                                        <asp:ListItem Text="J" Value="10">J</asp:ListItem>
                                        <asp:ListItem Text="K" Value="11">K</asp:ListItem>
                                        <asp:ListItem Text="L" Value="12">L</asp:ListItem>
                                        <asp:ListItem Text="M" Value="13">M</asp:ListItem>
                                        <asp:ListItem Text="N" Value="14">N</asp:ListItem>
                                        <asp:ListItem Text="O" Value="15">O</asp:ListItem>
                                        <asp:ListItem Text="P" Value="16">P</asp:ListItem>
                                        <asp:ListItem Text="Q" Value="17">Q</asp:ListItem>
                                        <asp:ListItem Text="R" Value="18">R</asp:ListItem>
                                        <asp:ListItem Text="S" Value="19">S</asp:ListItem>
                                        <asp:ListItem Text="T" Value="20">T</asp:ListItem>
                                        <asp:ListItem Text="U" Value="21">U</asp:ListItem>
                                        <asp:ListItem Text="V" Value="22">V</asp:ListItem>
                                        <asp:ListItem Text="W" Value="23">W</asp:ListItem>
                                        <asp:ListItem Text="X" Value="24">X</asp:ListItem>
                                        <asp:ListItem Text="Y" Value="25">Y</asp:ListItem>
                                        <asp:ListItem Text="Z" Value="26">Z</asp:ListItem>
                                    </asp:DropDownList>
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
                                        ToolTip="Delete" Visible="false"></asp:ImageButton>
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
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </div>
                </br> <b>&nbsp;Exam Details</b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaExamSub" runat="server" OnTabStripCommand="MenuPreSeaExamSub_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvExamDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCancelingEdit="gvExamDetails_RowCancelingEdit"
                    OnRowCommand="gvExamDetails_RowCommand" OnRowDataBound="gvExamDetails_RowDataBound"
                    OnRowDeleting="gvExamDetails_RowDeleting" OnRowEditing="gvExamDetails_RowEditing"
                    OnRowUpdating="gvExamDetails_RowUpdating" ShowFooter="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                S.No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exam Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Exam Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBatchExamId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHEXAMID") %>'></asp:Label>
                                <asp:Label ID="lblExamDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMDATE") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtExamDateEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtExamDateAdd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Time">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Start Time
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTTIME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStartTimeEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTTIME") %>'
                                    Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="txtStartTimeMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtStartTimeEdit" UserTimeFormat="TwentyFourHour" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtStartTimeAdd" runat="server" CssClass="input_mandatory" Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="txtStartTimeAddMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtStartTimeAdd" UserTimeFormat="TwentyFourHour" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Time">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                End Time
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEndTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDTIME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndTimeEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDTIME") %>'
                                    Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="txtEndTimeMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtEndTimeEdit" UserTimeFormat="TwentyFourHour" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtEndTimeAdd" runat="server" CssClass="input_mandatory" Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="txtEndTimeAddMask" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtEndTimeAdd" UserTimeFormat="TwentyFourHour" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subject">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Subject
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                <asp:Label ID="lblSubjectIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <span id="spnSubjectAdd">
                                    <asp:TextBox ID="txtSubjectNameAdd" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="120px"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgSubjectAdd" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:TextBox ID="txtsubjectId" runat="server" CssClass="input" Width="0px" Enabled="false"
                                        MaxLength="50"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtSubjectType" CssClass="input" Width="0px" MaxLength="20">
                                    </asp:TextBox>
                                </span>                                
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                Max Score
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblMaxMark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXMARKS") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblBatchExamIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHEXAMID") %>'></asp:Label>
                                <asp:Label ID="lblExamIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMID") %>'></asp:Label>
                                <asp:Label ID="lblSemesterIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>'></asp:Label>
                                <asp:Label ID="lblBatchIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHID") %>'></asp:Label>
                                <eluc:Number ID="txtMaxMarkEdit" runat="server" CssClass="input_mandatory" Mask="999.99"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXMARKS") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtMaxMarkAdd" runat="server" CssClass="input_mandatory" Mask="999.99" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                Pass Score
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPassMark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSMARKS") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtPassMarkEdit" runat="server" CssClass="input_mandatory" Mask="999.99"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSMARKS") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtPassMarkAdd" runat="server" CssClass="input_mandatory" Mask="999.99" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBTYPENAME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBTYPENAME") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblInvListHeader" runat="server">Invigilators&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblInvList" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVIGILATORS") %>'
                                    Visible="false"></asp:Label>
                                <img id="imgInvList" runat="server" src="<%$ PhoenixTheme:images/te_view.png %>"
                                    onmousedown="javascript:closeMoreInformation()" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Active YN
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1")? "Yes" : "No"%>'></asp:Label>
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
                                <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" visible="false" />
                                <asp:ImageButton runat="server" AlternateText="Details" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                    CommandName="INVIGILATOR" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdInvigilator"
                                    ToolTip="Invigilator Details"></asp:ImageButton>
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
