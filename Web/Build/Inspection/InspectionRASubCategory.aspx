<%@ Page Language="C#" AutoEventWireup="True" CodeFile="InspectionRASubCategory.aspx.cs"
    Inherits="InspectionRASubCategory" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sub Category</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRASubCategory" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRASubCategoryEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Sub Category"></eluc:Title>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2;">
                    <table id="tblConfigure" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                            </td>
                            <td>
                                  <eluc:Category ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                            <td>
                                <asp:literal ID="lblSubCategory" runat="server" Text="Sub Category"></asp:literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" MaxLength="100" Width="360px" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRASubCategory" runat="server" OnTabStripCommand="RASubCategory_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvRASubCategory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvRASubCategory_RowCommand" OnRowDataBound="gvRASubCategory_ItemDataBound"
                        OnRowCreated="gvRASubCategory_RowCreated" OnRowCancelingEdit="gvRASubCategory_RowCancelingEdit"
                        OnRowDeleting="gvRASubCategory_RowDeleting" AllowSorting="true" OnRowEditing="gvRASubCategory_RowEditing"
                        OnRowUpdating="gvRASubCategory_RowUpdating" OnSorting="gvRASubCategory_Sorting" ShowFooter="true"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblCategoryHeader" runat="server" CommandName="Sort" CommandArgument="FLDCATEGORYNAME"
                                        ForeColor="White"> Category&nbsp;</asp:LinkButton>
                                    <img id="FLDCATEGORYNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Category ID="ucCategoryEdit" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        CategoryList='<%# PhoenixInspectionRiskAssessmentCategory.ListRiskAssessmentCategory() %>'
                                        SelectedCategory='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Category ID="ucCategoryAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        CategoryList='<%# PhoenixInspectionRiskAssessmentCategory.ListRiskAssessmentCategory() %>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Sub Category&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubCategoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYID") %>'></asp:Label>
                                    <asp:Label ID="lblCategoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></asp:Label>
                                      <asp:Label ID="lblAssessmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSESSMENTID") %>'></asp:Label>
                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSubCategoryIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYID") %>'></asp:Label>
                                    <asp:TextBox ID="txtNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                           
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Risk Analysis" ImageUrl="<%$ PhoenixTheme:images/process.png %>"
                                        CommandName="RISKANALYSIS" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdRiskAnalysis"
                                        ToolTip="Risk Analysis"></asp:ImageButton>
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
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
