<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaAddressInvoiceApprovalUserMap.aspx.cs"
    Inherits="PreSeaAddressInvoiceApprovalUserMap" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Designation" Src="~/UserControls/UserControlDesignation.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Supplier Invoice Approval </title>
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
    <form id="form1" runat="server">
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <div id="div1" style="vertical-align: top">
                <asp:Label runat="server" ID="Label1" Font-Bold="true" Text="Office Admin" Width="360px"></asp:Label>
            </div>
        </div>
        <div class="navSelectHeader" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuOfficeAdmin" runat="server" OnTabStripCommand="MenuOfficeAdmin_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" Visible="false" />
        <div id="divGrid" style="position: relative; z-index: 0">
            <asp:GridView ID="gvVesselAdminUser" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                CellPadding="3" EnableViewState="TRUE" Font-Size="11px" OnRowCancelingEdit="gvVesselAdminUser_RowCancelingEdit"
                OnRowCommand="gvVesselAdminUser_RowCommand" OnRowCreated="gvVesselAdminUser_RowCreated"
                OnRowDataBound="gvVesselAdminUser_ItemDataBound" OnRowDeleting="gvVesselAdminUser_RowDeleting"
                OnRowEditing="gvVesselAdminUser_RowEditing" OnRowUpdating="gvVesselAdminUser_RowUpdating"
                OnSorting="gvVesselAdminUser_Sorting" ShowFooter="true" ShowHeader="true" Width="100%">
                <FooterStyle CssClass="datagrid_footerstyle" />
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <Columns>
                    <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                    <asp:TemplateField HeaderText="VesselId">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderTemplate>
                            <asp:Label ID="lblVesselIdHeader" runat="server" Visible="true">
                            </asp:Label>
                            <asp:Label ID="lnkVesselIdHeader" runat="server" CommandArgument="FLDVESSELID" CommandName="Sort"
                                ForeColor="White">Designation Name&nbsp;</asp:Label>
                            <img id="VesselAbbreviation" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDesignationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Designation ID="ucDesignationEdit" runat="server" AppendDataBoundItems="true"
                                CssClass="gridinput_mandatory" DesignationList="<%#PhoenixRegistersDesignation.ListDesignation()%>" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Designation ID="ucDesignation" runat="server" AppendDataBoundItems="true" CssClass="gridinput_mandatory"
                                DesignationList="<%#PhoenixRegistersDesignation.ListDesignation()%>" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supplier Code">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderTemplate>
                            <asp:Label ID="lblSupplierCodeHeader" runat="server" CommandArgument="FLDSUPPLIERCODE"
                                CommandName="Sort" ForeColor="White">Person In Charge &nbsp;</asp:Label>
                            <img id="imgSupplierCode" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lnkSupplierId" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICUSERNAME") %>'></asp:Label>
                            <asp:Label Visible="false" ID="lblVesselAdminUserMapCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELADMINUSERMAPCODE") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Visible="false" ID="lblVesselAdminUserMapCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELADMINUSERMAPCODE") %>'></asp:Label>
                            <eluc:UserName ID="ucPICEdit" runat="server" AppendDataBoundItems="true" CssClass="gridinput_mandatory"
                                SelectedUser='<%# DataBinder.Eval(Container,"DataItem.FLDPICUSERID") %>' UserNameList="<%# PhoenixUser.UserList()%>" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:UserName ID="ucPIC" runat="server" AppendDataBoundItems="true" CssClass="gridinput_mandatory"
                                UserNameList="<%# PhoenixUser.UserList()%>" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <HeaderTemplate>
                            <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataItemIndex %>"
                                CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>" ToolTip="Edit" />
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                                CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                CommandName="Update" ImageUrl="<%$ PhoenixTheme:images/save.png %>" ToolTip="Save" />
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton ID="cmdCancel" runat="server" AlternateText="Cancel" CommandArgument="<%# Container.DataItemIndex %>"
                                CommandName="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Cancel" />
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:ImageButton ID="cmdAdd" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                CommandName="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>" ToolTip="Add New" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
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
    </div>
    </form>
</body>
</html>
