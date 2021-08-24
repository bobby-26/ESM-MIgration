<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchExamResultDetails.aspx.cs"
    Inherits="PreSeaBatchExamResultDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaCurrentBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaSemester.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ExamType" Src="~/UserControls/UserControlPreSeaExam.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchExam" Src="~/UserControls/UserControlPreSeaBatchExam.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Section" Src="~/UserControls/UserControlPreSeaSection.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Exam Results</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCostEvaluationRequest" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="frmTitle" Text="Exam Results"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
        <div id="divFind">
            <table cellpadding="2" cellspacing="2" width="80%">
                <tr>
                    <td>
                        Batch
                    </td>
                    <td>
                        <eluc:PreSeaBatch ID="ucBatch" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            OnTextChangedEvent="ucBatch_Changed" AutoPostBack="true" />
                    </td>
                    <td>
                        Semester
                    </td>
                    <td>
                        <eluc:Semester ID="ucSemester" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                            OnTextChangedEvent="ucSemester_Changed" Enabled="false" AppendDataBoundItems="true" />
                    </td>                    
                </tr>
                <tr>
                    <td>
                        Exam Type
                    </td>
                    <td>
                        <eluc:ExamType ID="ucExamType" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            OnTextChangedEvent="ucExamType_Changed" AutoPostBack="true" />
                    </td>
                    <td>
                        Exam
                    </td>
                    <td>
                        <eluc:BatchExam ID="ucBatchExam" runat="server" CssClass="dropdown_mandatory" Enabled="false"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuExamResults" runat="server" OnTabStripCommand="MenuExamResults_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divGrid" style="position: relative; z-index: 1">
            <asp:GridView ID="gvExamResults" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowCommand="gvExamResults_RowCommand" OnRowDataBound="gvExamResults_ItemDataBound"
                AllowSorting="true" ShowHeader="true" EnableViewState="false" OnSelectedIndexChanging="gvExamResults_SelectedIndexChanging"
                DataKeyNames="FLDRESULTDETAILID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="DoubleClick"
                                CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Roll Number">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Roll Number
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblResultDetailId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESULTDETAILID") %>'></asp:Label>
                            <asp:Label ID="lblExamScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMSCHEDULEID") %>'></asp:Label>
                            <asp:Label ID="lblBatchRollNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHROLLNUMBER") %>'></asp:Label>
                            <asp:LinkButton ID="lnkBatchRollNo" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHROLLNUMBER") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Name
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblStudentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTUDENTID") %>'></asp:Label>
                            <asp:Label ID="lblStudentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="semester">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Semester
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSemesterName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Exam">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Exam
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblExamName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Number of Subjects">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            Subjects
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSubjectsCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFSUBJECTS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pass Subjects">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            Pass Subjects
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPassSubjectsCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFSUBPASS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Number of Absent" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Absent Subjects
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAbsentSubjectsCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFABSENT") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Status
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div id="divPage" style="position: relative; z-index: 0">
            <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                <tr>
                    <td nowrap align="center">
                        <asp:Label ID="lblPagenumber" runat="server">
                        </asp:Label>
                        <asp:Label ID="lblPages" runat="server">
                        </asp:Label>
                        <asp:Label ID="lblRecords" runat="server">
                        </asp:Label>&nbsp;&nbsp;
                    </td>
                    <td nowrap align="left" width="50px">
                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                    </td>
                    <td width="20px">
                        &nbsp;
                    </td>
                    <td nowrap align="right" width="50px">
                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                    </td>
                    <td nowrap align="center">
                        <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                        </asp:TextBox>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
