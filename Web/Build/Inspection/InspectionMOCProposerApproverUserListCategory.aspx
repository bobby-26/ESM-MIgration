<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCProposerApproverUserListCategory.aspx.cs"
    Inherits="InspectionMOCProposerApproverUserListCategory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Users</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div runat="server" id="DivHeader">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersUser" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlUserEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:TabStrip ID="MenuUser" runat="server" OnTabStripCommand="MenuUser_TabStripCommand">
            </eluc:TabStrip>
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divFind">
            </telerik:RadFormDecorator>
            <div id="divFind" style="position: relative; z-index: 2">
                <table id="tblConfigureUser" width="100%">
                    <tr>
                        <td style="width: 15%">
                            <asp:Literal ID="lblrole" runat="server" Text="Role"></asp:Literal>
                        </td>
                        <td style="width: 35%">
                            <asp:TextBox ID="txtRole" runat="server" ReadOnly="true" Width="300px" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                        <td style="width: 15%">
                            <asp:Literal ID="lblDesignation" runat="server" Text="Designation"></asp:Literal>
                        </td>
                        <td style="width: 35%">
                            <telerik:RadDropDownList ID="ddlDesignation" runat="server" Width="300px" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged"
                                AutoPostBack="true" AppendDataBoundItems="true">
                            </telerik:RadDropDownList>
                            <%-- <asp:DropDownList ID="ddlDesignation" runat="server" Width="300px" CssClass="input"
                                OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged" AppendDataBoundItems="true"
                                AutoPostBack="true" />--%>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="gvUser" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowCommand="gvUser_RowCommand" OnRowDataBound="gvUser_RowDataBound"
                OnRowEditing="gvUser_RowEditing" ShowHeader="true" AllowSorting="true" OnSorting="gvUser_Sorting">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                    <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblUserHeader" runat="server">Department&nbsp;
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUser" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></asp:Label>
                            <asp:Label ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblUserNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDUSERNAME"
                                ForeColor="White">Username&nbsp;</asp:LinkButton>
                            <img id="FLDUSERNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUserCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></asp:Label>
                            <asp:Label ID="lblEmail" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL") %>'></asp:Label>
                            <%--<asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></asp:Label>--%>
                            <asp:LinkButton ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblFirstNameHeader" runat="server">
                                    First Name
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblLastNameHeader" runat="server">
                                    Last Name
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblMiddleNameHeader" runat="server">
                                    Middle Name
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMiddleName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblDesignationHeader" runat="server">
                                    Designation
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDesignation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvUser" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
