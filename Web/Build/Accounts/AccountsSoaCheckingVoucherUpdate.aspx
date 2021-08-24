<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaCheckingVoucherUpdate.aspx.cs" Inherits="Accounts_AccountsSoaCheckingVoucherUpdate" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unbilled Voucher Detail</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCountryEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                            <%--<eluc:Title runat="server" ID="ucTitle" ShowMenu="false" Text="Unbilled Voucher Detail"></eluc:Title>--%>
                            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <div id="divGrid" style="width: 100%;">
                        <asp:GridView ID="gvCountry" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowCreated="gvCountry_RowCreated" Width="100%" CellPadding="3" OnRowCommand="gvCountry_RowCommand"
                            OnRowDataBound="gvCountry_ItemDataBound" OnRowCancelingEdit="gvCountry_RowCancelingEdit"
                            OnRowDeleting="gvCountry_RowDeleting" OnRowUpdating="gvCountry_RowUpdating" OnRowEditing="gvCountry_RowEditing"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnSorting="gvCountry_Sorting">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblVoucherDate" runat="server" Text="VoucherDate"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblvoucherdetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDETAILID") %> '></asp:Label>
                                        <asp:Label ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblPhoenixVoucher" runat="server" Text="Phoenix Voucher"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPhoenixVoucher" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHOENIXVOUCHER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblStatementReference" runat="server" Text="Statement Reference"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatementReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlStatementReference" runat="server" CssClass="dropdown_mandatory"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblStatus" runat="server" Text="Voucher Status"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERSTATUS") %>'></asp:Label>
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
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
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
                                <td width="20px">&nbsp;
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
                            <eluc:Status runat="server" ID="ucStatus" />
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
