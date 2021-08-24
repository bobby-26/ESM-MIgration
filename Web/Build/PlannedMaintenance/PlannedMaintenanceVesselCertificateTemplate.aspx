<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselCertificateTemplate.aspx.cs" 
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselCertificateTemplate" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Certificate Template</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="Div1">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmVesselCertificateTemplate" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlVesselCertificateTemplate">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Certificate Template"></eluc:Title>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureSubDepartment" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblTemplatecode" runat="server" Text="Template Code"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTemplatecode" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblTemplatename" runat="server" Text="Template Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTemplatename" runat="server" CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                </div>
               <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCertificateTemplate" runat="server" OnTabStripCommand="MenuCertificateTemplate_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvCertificateTemplate" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvCertificateTemplate_RowDataBound" 
                        OnRowCancelingEdit="gvCertificateTemplate_RowCancelingEdit" OnSorting="gvCertificateTemplate_Sorting"
                        OnRowUpdating="gvCertificateTemplate_RowUpdating" OnRowEditing="gvCertificateTemplate_RowEditing"
                        OnRowDeleting="gvCertificateTemplate_RowDeleting" ShowFooter="true" ShowHeader="true" AllowSorting="true"
                        EnableViewState="false" OnRowCommand="gvCertificateTemplate_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblTemplateid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblTemplateidEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>' CssClass="input_mandatory"
                                     Width="98%"  MaxLength="10"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblTemplatecode" runat="server" CommandName="Sort" CommandArgument="FLDCODE"
                                        ForeColor="White">Template Code&nbsp;</asp:LinkButton>
                                    <img id="FLDCODE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTemplatecode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCODE") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTemplatecodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>' CssClass="input_mandatory"
                                     Width="98%"  MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                    <asp:TextBox ID="txtTemplatecodeAdd" runat="server" CssClass="input_mandatory" Width="98%" MaxLength="10"></asp:TextBox>
                                </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblTemplatename" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Template Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTemplatename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                               <%-- <asp:LinkButton ID="lblTemplatename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' 
                                    CommandName="SELECT"  CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTemplatenameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' CssClass="input_mandatory"
                                     Width="98%"  MaxLength="200"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                    <asp:TextBox ID="txtTemplatenameAdd" runat="server" CssClass="input_mandatory" Width="98%" MaxLength="200"></asp:TextBox>
                                </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"  Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="delete" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cetificates" ImageUrl="<%$ PhoenixTheme:images/showlist.png %>"
                                        CommandName="certificates" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCertificates"
                                        ToolTip="Certificates" />
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
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
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
