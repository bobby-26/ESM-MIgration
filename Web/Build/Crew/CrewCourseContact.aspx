<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseContact.aspx.cs"
    Inherits="CrewCourseContact" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Designation" Src="~/UserControls/UserControlDesignation.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Contact</title>
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
                        <asp:Literal ID="lblCourseContact" runat="server" Text="Course Contact"></asp:Literal>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureCourseContact" width="100%">
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
                                <asp:Literal ID="lblDepartment" runat="server" Text="Department"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" AutoPostBack="true">
                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                             <td>
                                <asp:Literal ID="lblContactType" runat="server" Text="Contact Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlContactType" runat="server" CssClass="input"
                                    AppendDataBoundItems="true" AutoPostBack="true">
                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <b><asp:Literal ID="lblContactList" runat="server" Text="Contact List"></asp:Literal></b>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvCourseContact" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCommand="gvCourseContact_RowCommand" OnRowDataBound="gvCourseContact_RowDataBound"
                        OnRowCancelingEdit="gvCourseContact_RowCancelingEdit" OnRowEditing="gvCourseContact_RowEditing"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowDeleting="gvCourseContact_RowDeleting"  OnRowUpdating="gvCourseContact_RowUpdating"
                        EnableViewState="false" ShowFooter="true">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblContactName" runat="server" Text="Contact Name"></asp:Literal>
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
                                    <asp:Literal ID="lblContactType" runat="server" Text="Contact Type"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblContactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTACTTYPE") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                 <asp:DropDownList ID="ddlContactTypeAdd" runat="server" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" AutoPostBack="true">
                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                                   
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblEmail" runat="server" Text="Email"></asp:Literal>
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
