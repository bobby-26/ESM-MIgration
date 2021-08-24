<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseContent.aspx.cs"
    Inherits="CrewCourseContent"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Faculty List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseContent" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
<%--   <asp:UpdatePanel runat="server" ID="pnlCrewCourseCertificateEntry">
        <ContentTemplate>--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <asp:Literal ID="lblCourseContent" runat="server" Text="Course Content"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCourseContent" runat="server" OnTabStripCommand="CourseContent_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblrevision" runat="server" Text="Note: For revision Please enter the Revision Date and save."
                                CssClass="guideline_text"></asp:Label>
                        </td>
                    </tr>
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
                            <asp:Literal ID="lblRevisionDate" runat="server" Text="Revision Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucRevisionDate" runat="server" CssClass="input" />
                            <asp:ImageButton ID ="imgRevision" runat ="server" ImageUrl="<%$ PhoenixTheme:images/copy-requisition.png %>" ToolTip ="Revision History" />
                        </td>
                        <td>
                            <asp:Literal ID="lblLastReviewedBy" runat="server" Text="Last Reviewed By"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReviewedBy" runat="server" CssClass="input" Width="220px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSubjectMatter" runat="server" Text="Subject Matter"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubjectMatter" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblLearningObjective" runat="server" Text="Learning Objective"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLearningTarget" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMethodology" runat="server" Text="Methodology"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMethodology" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblMinimumRequirementsforcandidates" runat="server" Text="Minimum Requirements for candidates"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRequirement" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblNotes" runat="server" Text="Notes"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" runat="server" CssClass="input" TextMode="MultiLine" Width="420px"
                                Height="40px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Literal ID="lbladditional" runat="server" Text="additional text  on certificate"></asp:Literal>
                    </td>
                    <td>
                    <asp:TextBox ID="txtAdditionaltext" runat="server" CssClass="input" TextMode="MultiLine" Width="420px"
                                Height="40px"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblCourseOutline" runat="server" Text="Course Outline"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <eluc:Custom ID="txtCourseOutline" runat="server" Width="100%" Height="320px" PictureButton="true"
                                DesgMode="true" HTMLMode="true" PrevMode="true" OnFileUploadEvent="btnInsertPic_Click" />
                        </td>
                    </tr>
                    <%-- <tr>
                        <td>
                           
                            <b>Session Contents</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                    Content
                    </td>
                        <td colspan="6">
                           <asp:TextBox ID="txtContent" runat="server" 
                                    CssClass="gridinput_mandatory" Height="50px" TextMode="MultiLine" Width="690px"></asp:TextBox>
                        </td>
                    </tr>--%>
                </table>
                <%-- 
                 <div style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewSessionContent" runat="server" OnTabStripCommand="CrewSessionContent_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvCrewSessionContent" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCrewSessionContent_RowCommand"
                        OnRowDataBound="gvCrewSessionContent_RowDataBound"  OnRowDeleting="gvCrewSessionContent_RowDeleting"
                        OnRowCancelingEdit="gvCrewSessionContent_RowCancelingEdit" 
                        OnRowEditing="gvCrewSessionContent_RowEditing" OnRowUpdating="gvCrewSessionContent_RowUpdating"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSessionNoHeader" runat="server">Session No &nbsp;                                        
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSessionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSESSIONID") %>'></asp:Label>
                                    <asp:Label ID="lblSessionno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSESSIONNO") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSessionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSESSIONID") %>'></asp:Label>
                                    <asp:TextBox ID="txtSessionNoEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSESSIONNO") %>'
                                        MaxLength="30"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSessionNoAdd" runat="server" CssClass="gridinput_mandatory" 
                                        MaxLength="30"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80%"></ItemStyle>
                                <HeaderTemplate>
                                  Content
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkContent" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSESSIONCONTENT") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtContentEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSESSIONCONTENT") %>'
                                        CssClass="gridinput_mandatory" MaxLength="500"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtContentAdd" runat="server" CssClass="gridinput_mandatory" 
                                        MaxLength="200"  ></asp:TextBox>
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
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
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
                <font color="blue">Note:One day has four Session</font>--%>
            </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
