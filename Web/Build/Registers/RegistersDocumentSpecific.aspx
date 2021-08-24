<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentSpecific.aspx.cs" Inherits="RegistersDocumentSpecific" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Specific Document</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersDocumentSpecific" runat="server" submitdisabledcontrols="true">

    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>    
    <asp:UpdatePanel runat="server" ID="pnlDocumentSpecificEntry">
        <ContentTemplate>                
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" id="ucTitle" Text="Specific Document"></eluc:Title>
                    </div>
                </div>
                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersDocumentSpecific" runat="server" OnTabStripCommand="RegistersDocumentSpecific_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;z-index:0">               
                    <asp:GridView ID="dgDocumentSpecific" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="dgDocumentSpecific_RowCommand" OnRowDataBound="dgDocumentSpecific_ItemDataBound"
                        OnRowCancelingEdit="dgDocumentSpecific_RowCancelingEdit" OnRowDeleting="dgDocumentSpecific_RowDeleting"
                        OnRowEditing="dgDocumentSpecific_RowEditing" ShowFooter="true" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        
                        <Columns>  
                                                  
                            <asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeHeader" runat="server">Type&nbsp;<asp:ImageButton
                                        runat="server" ID="cmdTypeDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                        CommandName="FLDTYPE" CommandArgument="1" AlternateText="Country name desc" />
                                        <asp:ImageButton runat="server" ID="cmdTypeAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDTYPE" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    <asp:Label ID="lblSpecificDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPECIFICDOCUMENTID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkType" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:LinkButton>                                    
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    <asp:Label ID="lblSpecificDocumentIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPECIFICDOCUMENTID") %>'></asp:Label>
                                    <asp:TextBox ID="txtTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTypeAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ToolTip="Enter Type"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>  
                                            
                            <asp:TemplateField HeaderText="Document">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDocumentsHeader" runat="server">Document&nbsp;<asp:ImageButton
                                        runat="server" ID="cmdDocumentsDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                        CommandName="FLDDOCUMENT" CommandArgument="1" AlternateText="Country name desc" />
                                        <asp:ImageButton runat="server" ID="cmdDocumentsAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDDOCUMENT" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDocuments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENT") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                    
                                    <asp:TextBox ID="txtDocumentsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENT") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtDocumentsAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ToolTip="Enter Name of Document"></asp:TextBox>
                                </FooterTemplate>
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
                                    <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
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

