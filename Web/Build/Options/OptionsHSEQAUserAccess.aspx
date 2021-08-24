<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsHSEQAUserAccess.aspx.cs" Inherits="Options_OptionsHSEQAUserAccess" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader">
                        <div class="divFloatLeft">
                            <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="false"></eluc:Title>
                            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        </div>
                        <div style="position: absolute; right: 0px">
                            <eluc:TabStrip ID="MenuUserAccessList" runat="server" OnTabStripCommand="UserAccessList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                        </div>
                    </div>

                    <table width="100%">
                         <tr>
                            <td>
                                <asp:Literal ID="lblGroupName" runat="server" Text="Group"></asp:Literal>
                            </td>
                            <td>
                                 <asp:TextBox runat="server" ID="txtGroupName" Width="260px" CssClass="readonlytextbox"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:TextBox>
                               <%--<asp:Label ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:Label>--%>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <asp:Literal ID="lblDefaultCompany" runat="server" Text="Company"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompany" runat="server" DataTextField="FLDSHORTCODE" DataValueField="FLDCOMPANYID"
                                    CssClass="input" OnTextChanged="ddlCompany_TextChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Literal ID="lblDMSCategory" runat="server" Text="DMS Category"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlDMSCategoryList" AutoPostBack="true" OnTextChanged="ddlDMSCategoryList_TextChanged" CssClass="input" AppendDataBoundItems="true" DataTextField="FLDROOT" DataValueField="FLDCATEGORYID">
                                    <asp:ListItem Text="--Select--" Value="Dummy"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <th>
                                <asp:Literal ID="lblCategoryAccess" runat="server" Text="HSEQA Access"></asp:Literal>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvMenu" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" OnRowCommand="gvMenu_RowCommand" OnRowDataBound="gvMenu_RowDataBound"
                                    OnRowEditing="gvMenu_RowEditing" ShowHeader="true" EnableViewState="false">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkMenuRights" OnCheckedChanged="CheckBoxClicked" AutoPostBack="true" Text='<%# Container.DataItemIndex %>' BackColor="Transparent" ForeColor="Transparent" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CategoryName">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCategoryNameHeader" runat="server">Category Name&nbsp;
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROOTID") %>'></asp:Label>
                                                <asp:Label ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROOT") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>

                    <eluc:Status runat="server" ID="ucStatus" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>
