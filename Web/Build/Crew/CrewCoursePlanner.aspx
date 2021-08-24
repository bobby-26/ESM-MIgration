<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCoursePlanner.aspx.cs"
    Inherits="CrewCoursePlanner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Faculty" Src="~/UserControls/UserControlFaculty.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Planner</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCoursePlanner" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionRecordAndResponseEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Label ID="lblCourseId" runat="server" Visible="false"></asp:Label>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="CoursePlanner"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewMenuCoursePlanner" runat="server" OnTabStripCommand="CrewMenuCoursePlanner_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%" id="tblCoursePlanner">
                        <tr>
                            <td>
                                <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                                    Enabled="true" EnableViewState="true" AutoPostBack="true" OnTextChangedEvent="WeekSetting" />
                            </td>
                            <td>
                                <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtToDate" runat="server" CssClass="readonlytextbox" DatePicker="true"
                                    Enabled="false" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkExcludeSunday" runat="server" Text="Exclude Sunday" AutoPostBack="true"
                                    OnCheckedChanged="ExcludeSunday" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIncludeHoliday" runat="server" Text="Include Holidays" AutoPostBack="true"
                                    OnCheckedChanged="IncludeHoliday" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtSearchCourse" runat="server" MaxLength="100" CssClass="input"
                                    Width="260px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewCourseList" runat="server" OnTabStripCommand="CrewCourseList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div style="position: relative; width: 100%">
                    <table width="100%" cellpadding="2" cellspacing="2" runat="server">
                        <tr>
                            <td valign="top" width="100%">
                                <div id="div2" style="position: relative; z-index: +1">
                                    <asp:GridView ID="gvCourseList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="3" OnRowCommand="gvCourseList_RowCommand" OnSorting="gvCourseList_Sorting"
                                        AllowSorting="true" OnRowEditing="gvCourseList_RowEditing" ShowFooter="false" OnRowDataBound="gvCourseList_ItemDataBound"
                                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDDOCUMENTID">
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Abbreviation">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lblAbbreviationHeader" runat="server" CommandName="Sort" CommandArgument="FLDABBREVIATION"
                                                        ForeColor="White">Code&nbsp;</asp:LinkButton>
                                                    <img id="FLDABBREVIATION" runat="server" visible="false" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></asp:Label>
                                                    <asp:Label ID="lblAbbreviation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Course">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDCOURSE"
                                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                                    <asp:LinkButton ID="lblCourseHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOURSE"
                                                        ForeColor="White">Course&nbsp;</asp:LinkButton>
                                                    <img id="FLDCOURSE" runat="server" visible="false" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkCourse" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Document Type">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblDocumentTypeHeader" runat="server">Course Type&nbsp;
                                                    </asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></asp:Label>
                                                    <asp:Label ID="lblDocumentType" runat="server" Width="120px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPENAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nomination List">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Literal ID="lblNominationList" runat="server" Text="Nomination List"></asp:Literal>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNominationList" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOMINATIONLIST") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div id="divPage" style="position: relative; width: auto">
                                        <table width="100%" border="0" class="datagrid_pagestyle">
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
                                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                                    </asp:TextBox>
                                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                                        Width="40px"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                               <div style="color: Blue; font-size: small;">
                                    <b><asp:Literal ID="lblNote" runat="server" Text="Note:"></asp:Literal></b><asp:Literal ID="lblFirstselectacoursethencreateabatchandthenassignfaculty" runat="server" Text="First select a course then create a batch and then assign faculty."></asp:Literal>
                                    <br />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:Literal ID="lblOnlycourseswithnominationwillbelistedintheabovelist" runat="server" Text="Only courses with nomination will be listed in the above list."></asp:Literal>
                                </div>
                                <br />
                                <div style="text-align: center; margin:10px; vertical-align:bottom;">
                                    <asp:Button ID="btnPrevWeek" runat="server" CssClass="input" Text="View Previous Week"
                                        OnClick="PrevClick" />
                                    <asp:Button ID="btnNextWeek" runat="server" CssClass="input" Text="View Next Week"
                                        OnClick="NextClick" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rblPlannerList" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                                    <asp:ListItem Value="1" Text="Column wise planner"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Row wise planner"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                                    <eluc:TabStrip ID="MenuGridCoursePlannerList" runat="server" OnTabStripCommand="MenuGridCoursePlannerList_TabStripCommand">
                                    </eluc:TabStrip>
                                </div>
                                <div id="div1" runat="server" style="position: relative; z-index: 0; width: 100%;">
                                    <asp:GridView ID="gvCoursePlannerList" runat="server" AutoGenerateColumns="False"  OnDataBound="gvCoursePlannerList_DataBound"
                                        Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false" OnRowDataBound="gvCoursePlannerList_RowDataBound"
                                        EnableViewState="False">
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
                    </table>
                </div>
                  <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
