﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionInspectingInspector.aspx.cs" Inherits="InspectionInspectingInspector" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="InspectionCompany" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInpectingInspector" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectingInspector">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Inspecting Inspector"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuInspectionInspector" runat="server" OnTabStripCommand="InspectionInspector_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvInspectionInspector" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="true" OnRowCommand ="gvInspection_RowCommand" 
                        OnRowEditing ="gvInspection_RowEditing" OnRowCancelingEdit ="gvInspection_RowCancelingEdit" OnRowUpdating ="gvInspection_RowUpdating"
                        ShowHeader="true" EnableViewState="false" DataKeyNames ="FLDDTKEY" OnRowDataBound="gvInspection_RowDataBound"  OnRowDeleting ="gvInspection_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblNameHeader" runat="server">Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                <asp:Label ID="lblInspectorName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTORNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                  
                                    <asp:TextBox ID="txtInspectorNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTORNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtInspectorNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDesignationHeader" runat="server">Designation</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                <asp:Label ID="lblInspectorDesignation" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTORDESIGNATION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                  
                                    <asp:TextBox ID="txtInspectorDesignationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTORDESIGNATION") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtInspectorDesignationAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCompanyNameHeader" runat="server">Company Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                <asp:Label ID="lblCompanyName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></asp:Label>
                                <asp:Label ID="lblCompanyID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                  <asp:DropDownList ID="ddlCompanyNameEdit" runat="server" 
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" DataTextField="FLDCOMPANYNAME"
                                        DataValueField="FLDCOMPANYID">
                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                    </asp:DropDownList>
                                  <asp:Label ID="lblCompanyID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></asp:Label>  
                                </EditItemTemplate>
                                <FooterTemplate>
                                  <asp:DropDownList ID="ddlCompanyNameAdd" runat="server" 
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" DataTextField="FLDCOMPANYNAME"
                                        DataValueField="FLDCOMPANYID">
                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                  </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
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
               </table>
                </div>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
