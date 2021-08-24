<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionSupplierContacts.aspx.cs"
    Inherits="OptionSupplierContacts" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUserName" Src="../UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tax" Src="~/UserControls/UserControlTaxMaster.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmQuotationVendor" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="Title1" Text="Supplier Contacts" ShowMenu="false">
                </eluc:Title>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuVender" runat="server" OnTabStripCommand="MenuVender_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div>
            <br clear="all" />
            <asp:UpdatePanel runat="server" ID="pnlFormGeneral">
                <ContentTemplate>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="navigation" id="Div2" style="top: 30px; margin-left: 0px; vertical-align: top;
                        width: 100%">
                        <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
                        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                            border: none; width: 100%">
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                                    </td>
                                    <td colspan="4">
                                        <span id="spnPickListMaker">
                                            <asp:TextBox ID="txtVenderCode" runat="server" Width="60px" CssClass="input"></asp:TextBox>
                                            <asp:TextBox ID="txtVenderName" runat="server" BorderWidth="1px" Width="210px" CssClass="input"></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132', true);"
                                                Text=".." />
                                            <asp:TextBox ID="txtVenderID" runat="server" Width="1" CssClass="input"></asp:TextBox>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblStockType" runat="server" Text="Stock Type"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlStockType" CssClass="input_mandatory" AutoPostBack="true">
                                            <asp:ListItem Text="--Select--" Value="Dummy" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Spares" Value="SPARE"></asp:ListItem>
                                            <asp:ListItem Text="Stores" Value="STORE"></asp:ListItem>
                                            <asp:ListItem Text="Service" Value="SERVICE"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="navSelect" style="position: relative; width: 15px">
                                <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand">
                                </eluc:TabStrip>
                            </div>
                            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                                <asp:GridView ID="gvSupplierContact" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" OnRowCommand="gvSupplierContact_RowCommand" OnRowCancelingEdit="gvSupplierContact_RowCancelingEdit"
                                    OnRowEditing="gvSupplierContact_RowEditing" OnRowDeleting="gvSupplierContact_RowDeleting"
                                    OnRowDataBound="gvSupplierContact_ItemDataBound" ShowFooter="false" 
                                    ShowHeader="true" OnSorting="gvSupplierContact_Sorting" AllowSorting="true">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>
                                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                        <asp:TemplateField HeaderText="number">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lblDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDCREATEDDATE"
                                                    ForeColor="White">Date</asp:LinkButton>
                                                <img id="FLDCREATEDDATE" runat="server" visible="false" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="number">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                                    ForeColor="White">Supplier</asp:LinkButton>
                                                <img id="FLDNAME" runat="server" visible="false" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterText="New MaritalStatus">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblEmailHeader" runat="server">Email Address
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRelationShipId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSRELATIONSHIPID") %>'></asp:Label>
                                                <asp:Label ID="lblEmailAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILADDRESS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRelationHeader" runat="server">Purpose</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRelation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELATIONSHIP") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblValueHeader" runat="server">Email Option</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmailOption" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILOPTION") %>'></asp:Label>
                                                <asp:RadioButtonList ID="rblEmailOption" runat="server" Enabled="false" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="TO">To</asp:ListItem>
                                                    <asp:ListItem Value="CC">Cc</asp:ListItem>
                                                    <asp:ListItem Value="NA">None</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lbluserHeader" runat="server">Created By</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divPage" style="position: relative;">
                                <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                                            <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                                            </asp:TextBox>
                                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                                Width="40px"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
