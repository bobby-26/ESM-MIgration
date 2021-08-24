<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaCourseContact.aspx.cs"
    Inherits="PreSeaCourseContact" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Contact</title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCourseContact" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCourseListEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Course Master"></eluc:Title>
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCourseMaster" runat="server" OnTabStripCommand="CourseMaster_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureCourseContact" width="100%">
                        <tr>
                            <td>
                                Course
                            </td>
                            <td>
                                <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Enabled="false" />
                            </td>
                            <td>
                                Department
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" AutoPostBack="true">
                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <b>Contact List</b>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvCourseContact" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCommand="gvCourseContact_RowCommand" OnRowDataBound="gvCourseContact_RowDataBound"
                        OnRowCancelingEdit="gvCourseContact_RowCancelingEdit" OnRowEditing="gvCourseContact_RowEditing"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowDeleting="gvCourseContact_RowDeleting"
                        OnRowUpdating="gvCourseContact_RowUpdating" EnableViewState="false" ShowFooter="true">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Contact Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblContactId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTACTID") %>'></asp:Label>
                                    <asp:Label ID="lblContactName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTACTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlCourseContactAdd" runat="server" CssClass="input_mandatory"
                                        AppendDataBoundItems="true" Width="70%">
                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Code
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFacultyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Contact Type
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblContactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTACTTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Email
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL") %>'></asp:Label>
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
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
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
