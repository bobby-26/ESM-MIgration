<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCommunicationType.aspx.cs"
    Inherits="RegistersCommunicationType" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Communication Type</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="oiltypelink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegisterCommunicationType" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCommunicationType">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Communication Type"></eluc:Title>
                    </div>
                </div>

                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersCommunicationType" runat="server" OnTabStripCommand="RegistersCommunicationType_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvCommunicationType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCommunicationType_RowCommand" OnRowDataBound="gvCommunicationType_ItemDataBound"
                        OnRowCreated="gvCommunicationType_RowCreated" OnRowCancelingEdit="gvCommunicationType_RowCancelingEdit"
                        OnRowDeleting="gvCommunicationType_RowDeleting" AllowSorting="true" OnRowEditing="gvCommunicationType_RowEditing"
                        OnRowUpdating="gvCommunicationType_RowUpdating" OnSorting="gvCommunicationType_Sorting" ShowFooter="true"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />

                             <asp:TemplateField FooterText="New Short">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCommunicationShortName" Text="Short Name" Visible="true" runat="server">
                                        <asp:ImageButton runat="server" ID="cmdAbbreviation" OnClick="cmdSearch_Click" CommandName="FLDSHOWYESNO"
                                            ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                    </asp:Label>
                                   
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Label ID="lblCommunicationShortName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></asp:Label>                                   
                                </ItemTemplate>   
                                 
                                  <EditItemTemplate>
                                   <asp:TextBox ID="txtCommunicationShortNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'  CssClass="gridinput_mandatory" />
                                </EditItemTemplate>

                                <FooterTemplate>
                                   <asp:TextBox ID="txtCommunicationShortNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter Communication Short Name"/>    
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField FooterText="New Communication">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblCommunicationNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDOILTYPENAME"
                                        ForeColor="White">Communication Type&nbsp;</asp:LinkButton>
                                <img id="FLDOILTYPENAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                
                                <ItemTemplate>                                   
                                    <asp:Label ID="lblCommunicationTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMUNICATIONTYPECODE") %>'></asp:Label>
                                    <asp:Label ID="lblCommunicationTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMUNICATIONTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                                
                                <EditItemTemplate>
                                    <asp:Label ID="lblCommunicationTypeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMUNICATIONTYPECODE") %>'></asp:Label>
                                    <asp:TextBox ID="txtCommunicationTypeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMUNICATIONTYPENAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                
                                <FooterTemplate>
                                    <asp:TextBox ID="txtCommunicationTypeNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter CommunicationType Name"></asp:TextBox>
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
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
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
