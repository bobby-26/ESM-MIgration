<%@ Page Language="C#" AutoEventWireup="True" CodeFile="PreSeaTrainingStaff.aspx.cs"
    Inherits="PreSeaTrainingStaff" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Training Staff</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTrainingStaff" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlTrainingStaff">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Staff"></eluc:Title>
                    </div>
                </div>
                 <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuTraining" runat="server" OnTabStripCommand="Training_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2;">
                    <table id="tblConfigureDesignation" width="30%">
                        <tr>
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
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuTrainingStaff" runat="server" OnTabStripCommand="TrainingStaff_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div1" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvMapping" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvMapping_RowCommand" OnRowDataBound="gvMapping_ItemDataBound"
                        OnRowCreated="gvMapping_RowCreated" OnRowCancelingEdit="gvMapping_RowCancelingEdit"
                        OnRowUpdating="gvMapping_RowUpdating" OnRowDeleting="gvMapping_RowDeleting" AllowSorting="true"
                        OnRowEditing="gvMapping_RowEditing" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMappingIdadd" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPPINGID") %>'></asp:Label>
                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    Code
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFacultyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFacultyCodeEdit" runat="server" MaxLength="5" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYCODE") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Designation
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblUserCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></asp:Label>
                                    <asp:Label ID="lblMappingIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPPINGID") %>'></asp:Label>
                                    <asp:CheckBoxList ID="cblDesignationEdit" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" RepeatColumns="6" DataSource='<%#  PhoenixCrewCourseDesignation.ListDesignation() %>'
                                        DataTextField="FLDDESIGNATIONNAME" DataValueField="FLDDESIGNATIONID">
                                    </asp:CheckBoxList>
                                    <%--<asp:DropDownList ID="ddlDesignationEdit" runat="server" DataSource='<%#  PhoenixCrewCourseDesignation.ListDesignation() %>'
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" DataTextField="FLDDESIGNATIONNAME"
                                        DataValueField="FLDDESIGNATIONID">
                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Role
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRoleName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDROLENAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <eluc:Quick ID="ucRoleEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" QuickTypeCode="123"
                                        SelectedQuick='<%#  DataBinder.Eval(Container,"DataItem.FLDROLEID") %>' />                             
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Load
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFacultyLoad" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYLOADPERSEM") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFacultyLoadEdit" runat="server" MaxLength="5" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYLOADPERSEM") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
