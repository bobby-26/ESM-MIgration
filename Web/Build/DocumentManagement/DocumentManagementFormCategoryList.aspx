<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormCategoryList.aspx.cs" Inherits="DocumentManagementFormCategoryList" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form Field</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmField" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCountryEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Category" ShowMenu="true"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvFormCategory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvFormCategory_RowCommand" OnRowDataBound="gvFormCategory_ItemDataBound"
                        OnRowCancelingEdit="gvFormCategory_RowCancelingEdit" OnRowDeleting="gvFormCategory_RowDeleting"
                        OnRowUpdating="gvFormCategory_RowUpdating" OnRowEditing="gvFormCategory_RowEditing" ShowFooter="true"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSelectedIndexChanging="gvFormCategory_SelectedIndexChanging"
                        OnSorting="gvFormCategory_Sorting" DataKeyNames="FLDCATEGORYID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                             <asp:TemplateField HeaderText="Count">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    No.
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRowNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                 <EditItemTemplate>
                                     <eluc:MaskNumber ID="ucCategoryNumberEdit" runat="server" CssClass="gridinput_mandatory"
                                         Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNUMBER") %>' DecimalPlace="0"
                                         IsInteger="true" IsPositive="true" MaxLength="4" />
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                    <eluc:MaskNumber ID="ucCategoryNumberAdd" runat="server" CssClass="gridinput_mandatory"
                                         DecimalPlace="0" IsInteger="true" IsPositive="true" MaxLength="4" />
                                 </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Category Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCategoryName" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:LinkButton>
                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCategoryIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtCategoryNameEdit" runat="server" CssClass="gridinput_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtCategoryNameAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>      
                            <asp:TemplateField HeaderText="Directory">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Directory
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDirectoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDIRECTORYNAME") %>' Width="250px"></asp:Label>
                                    <asp:Label ID="lblDirectoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDIRECTORYID") %>' Width="250px" Visible="false"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlDirectoryEdit" runat="server" CssClass="gridinput_mandatory"
                                        Width="220px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblCategoryNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDIRECTORYNAME") %>'
                                        Width="250px" Visible="false"></asp:Label>
                                    <asp:Label ID="lblDirectoryIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDIRECTORYID") %>'
                                        Width="250px" Visible="false"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlDirectoryAdd" runat="server" CssClass="gridinput_mandatory"
                                        Width="220px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>                      
                            <asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Type
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></asp:Label>
                                    <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                    
                                <asp:Label ID="lblTypeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>' Visible="false"></asp:Label>
                                    <asp:RadioButtonList ID="rListEdit" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Office" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Shipboard" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:RadioButtonList ID="rListAdd" runat="server" RepeatDirection="Horizontal" ValidationGroup="type">
                                        <asp:ListItem Text="Office" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Shipboard" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </FooterTemplate>
                            </asp:TemplateField>                   
                            <asp:TemplateField HeaderText="Count">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Count
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:Label>
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
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                   <%-- <img id="Img5" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />--%>
                                    <asp:ImageButton runat="server" AlternateText="APPROVE" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                        CommandName="APPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdApprove"
                                        ToolTip="Approve & Publish" Visible="false"></asp:ImageButton>
                                    <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="View Document" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                        CommandName="VIEWDOCUMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmgViewContent"
                                        ToolTip="View Document" Visible="false"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
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
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
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
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
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
