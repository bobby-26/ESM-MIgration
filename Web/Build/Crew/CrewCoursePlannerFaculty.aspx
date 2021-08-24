<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCoursePlannerFaculty.aspx.cs"
    Inherits="CrewCoursePlannerFaculty" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlInstitution.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="FacultyCode" Src="~/UserControls/UserControlFacultyCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmExtraApproval" Src="~/UserControls/UserControlCourseExtraApproval.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Faculty Planner</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewFacultyPlanner" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlFacultyPlanner">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblFacultyPlanner" runat="server" Text="Faculty Planner"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
          
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewFacultyList" runat="server" OnTabStripCommand="CrewFacultyList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvCoursePlanner" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCoursePlanner_RowCommand" OnRowDataBound="gvCoursePlanner_ItemDataBound"
                        OnRowEditing="gvCoursePlanner_RowEditing" OnRowCancelingEdit="gvCoursePlanner_RowCancelingEdit"
                        ShowFooter="false" OnRowUpdating="gvCoursePlanner_RowUpdating" ShowHeader="true"
                        EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                         <asp:TemplateField>
                             <ItemStyle HorizontalAlign="Left"></ItemStyle>
                             <HeaderTemplate>
                                 <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                             </HeaderTemplate>
                             <ItemTemplate>
                                 <asp:Label ID="lblBatchNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSplitAM" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPLITAM") %>'></asp:Label>
                                    <asp:Label ID="lblSplitPM" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPLITPM") %>'></asp:Label>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></asp:Label>
                                    <asp:Label ID="lblHoliday" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOLIDAYYN") %>'></asp:Label>
                                     <asp:Label ID="lblSundayholidayyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUNDAYHOLIDAYYN") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                         
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAM" runat="server" Text="AM"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFacultyAM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYNAMEAM") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                        <eluc:FacultyCode ID="ucFacultyAMEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                         FacultyCodeList='<%# PhoenixRegistersFaculty.ListFacultyCode(ViewState["courseid"].ToString()==""?1:Convert.ToInt32(ViewState["courseid"].ToString())) %>'
                                          SelectedFacultyCode='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYAM") %>'
                                         />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPM" runat="server" Text="PM"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFacultyPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYNAMEPM") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                         <eluc:FacultyCode ID="ucFacultyPMEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                         FacultyCodeList='<%# PhoenixRegistersFaculty.ListFacultyCode(ViewState["courseid"].ToString()==""?1:Convert.ToInt32(ViewState["courseid"].ToString())) %>'
                                          SelectedFacultyCode='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYPM") %>'
                                         />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
