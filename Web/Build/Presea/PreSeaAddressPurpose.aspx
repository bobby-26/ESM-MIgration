<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaAddressPurpose.aspx.cs"
    Inherits="PreSeaAddressPurpose" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>purpose</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="div1">
            <eluc:Title runat="server" ID="ucTitle" Text="Contacts" ShowMenu="false" />
        </div>
    </div>
    <div runat="server" id="divSubHeader" class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="" Width="360px"></asp:Label>
        </div>
    </div>
    <div class="navSelectHeader" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAddressMain" runat="server" OnTabStripCommand="AddressMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <div class="navSelect" style="top: 28px; right: 0px; position: absolute;">
        <eluc:TabStrip ID="MenuAddressPurpose" runat="server" OnTabStripCommand="AddressPurpose_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <asp:UpdatePanel runat="server" ID="pnlAddressPurpose">
        <ContentTemplate>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        Purpose
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPurpose" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            AutoPostBack="true" OnTextChanged="ddlPurpose_TextChanged">
                        </asp:DropDownList>
                    </td>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        Name
                    </td>
                    <td>
                        <asp:Label ID="lblAddressContactID" Width="1px" runat="server" Visible="false" />
                        <asp:TextBox ID="txtName" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        Title
                    </td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        Department
                    </td>
                    <td>
                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Phone 1
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone1" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        Phone 2
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone2" runat="server" CssClass="input" />
                    </td>
                    <td>
                        Fax
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Email 1
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail1" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        Email 2
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail2" runat="server" CssClass="input" />
                    </td>
                    <td>
                        Mobile
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 15%">
                        Accessible Companies
                    </td>
                    <td style="width: 85%" colspan="5">
                        <div id="dvClass" runat="server" class="input" style="width: 90%; height: 100px;
                            overflow: auto;">
                            <asp:CheckBoxList ID="cblRegisteredCompany" runat="server" RepeatColumns="10" RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="navSelect">
                <eluc:TabStrip ID="MenuAddressFind" runat="server" OnTabStripCommand="AddressFind_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divgrid">
                <asp:GridView ID="gvAddressPurpose" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                    OnRowCommand="gvAddressPurpose_RowCommand" OnRowDataBound="gvAddressPurpose_RowDataBound"
                    OnRowCancelingEdit="gvAddressPurpose_RowCancelingEdit" OnRowEditing="gvAddressPurpose_RowEditing"
                    OnRowDeleting="gvAddressPurpose_RowDeleting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAddressContactIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCONTACTID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email 1">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Email 1
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAddressId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblEmail1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL1") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Title">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Title
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                Department
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone1">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Phone 1
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <HeaderStyle Wrap="false"></HeaderStyle>
                            <HeaderTemplate>
                                Action
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
