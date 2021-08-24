<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Help.aspx.cs" Inherits="Help" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supernumerary Sign-On/Off List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOtherCrew" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlHelp">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Help" ShowMenu="false"></eluc:Title>
                    </div>
                </div>
                <div class="navSelectHeader" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuHelpMode" runat="server" OnTabStripCommand="MenuHelpMode_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                </div>
                <div id="divView" runat="server">
                    <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <th>
                            <asp:literal ID="lblsummary" runat="server" Text="Summary"></asp:literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                           <div runat="server" ID="divSummary"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="lblFieldList" runat="server" Text="Field List"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvHelpView" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" Style="margin-bottom: 0px" EnableViewState="false"                               
                                DataKeyNames="FLDFIELDLISTID">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Field Name">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDFIELDNAME") %>
                                        </ItemTemplate>                                       
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDFIELDDESC") %>
                                        </ItemTemplate>                                       
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value Description">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDVALUEDESC")%>
                                        </ItemTemplate>                                        
                                    </asp:TemplateField>                                 
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="lblUsage" runat="server" Text="Usage"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <div runat="server" ID="divUsage"></div>                           
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="lblsubLinks" runat="server" Text="Sub Links"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvLink" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" Style="margin-bottom: 0px" EnableViewState="false" OnSelectedIndexChanging="gvLink_SelectedIndexChanging"                              
                                DataKeyNames="FLDPAGEID">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Other Links">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                                runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                            
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                </div>
                <div id="divEdit" runat="server" visible="false">
                 <div runat="server" id="divSubHeader" class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="" Width="360px"></asp:Label>
                    </div>
                </div>
                <div class="navSelect" style="top: 28px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuHelp" runat="server" OnTabStripCommand="MenuHelp_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <th>
                            <asp:Literal ID="lblMenuHelpSummary" runat="server" Text="Summary"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtSummary" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="50px" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                           <asp:Literal ID="lblHelpFieldList" runat="server" Text="Field List"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvHelp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" Style="margin-bottom: 0px" EnableViewState="false"
                                OnRowCancelingEdit="gvHelp_RowCancelingEdit" OnRowEditing="gvHelp_RowEditing" OnRowUpdating="gvHelp_RowUpdating"
                                ShowFooter="true" OnRowDeleting="gvHelp_RowDeleting" OnRowDataBound="gvHelp_RowDataBound"
                                OnRowCommand="gvHelp_RowCommand" DataKeyNames="FLDFIELDLISTID">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Field Name">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDFIELDNAME") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFiledNameEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="100" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDNAME") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFiledNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="100"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDFIELDDESC") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFieldDescEdit" runat="server" CssClass="gridinput" MaxLength="500" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDDESC") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFieldDescAdd" runat="server" CssClass="gridinput" MaxLength="500"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value Description">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDVALUEDESC")%>
                                        </ItemTemplate>
                                         <EditItemTemplate>
                                            <asp:TextBox ID="txtValueDescEdit" runat="server" CssClass="gridinput" MaxLength="500" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUEDESC") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtValueDescAdd" runat="server" CssClass="gridinput" MaxLength="500"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
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
                        </td>
                    </tr>
                    <tr>
                        <th>
                           <asp:Literal ID="lblLinkUsage" runat="server" Text="Usage"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <ajaxToolkit:Editor ID="txtUsage" runat="server" Width="100%" Height="90%" ActiveMode="Design" />                           
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="lblRowsubLinks" runat="server" Text="Sub Links"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvLinkEdit" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" Style="margin-bottom: 0px" EnableViewState="false" OnRowDataBound="gvLink_RowDataBound"
                                OnRowCancelingEdit="gvLink_RowCancelingEdit" OnRowEditing="gvLink_RowEditing" OnRowUpdating="gvLink_RowUpdating"
                                OnRowCommand="gvLink_RowCommand"                              
                                DataKeyNames="FLDLINKID" ShowFooter="true">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Other Links">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlPageEdit" CssClass="input_mandatory" runat="server" DataTextField="FLDDESCRIPTION"
                                                DataValueField="FLDPAGEID">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlPageAdd" CssClass="input_mandatory" runat="server" DataTextField="FLDDESCRIPTION"
                                                DataValueField="FLDPAGEID">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                         <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                         <EditItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="gridinput_mandatory" MaxLength="500" ToolTip="Description of the Link"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:TextBox>
                                         </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblOtherlinkActionHeader" runat="server">Action</asp:Label>
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
                        </td>
                    </tr>                  
                </table>
                </div>
                <div id="divDraft" runat="server" visible="false">
                 <div runat="server" id="div2" class="subHeader" style="position: relative">
                    <div id="div3" style="vertical-align: top">
                        <asp:Label runat="server" ID="Label1" Font-Bold="true" Text="" Width="360px"></asp:Label>
                    </div>
                </div>
                <div class="navSelect" style="top: 28px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuDraft" runat="server" OnTabStripCommand="MenuDraft_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <th>
                           <asp:Literal ID="lblMenuStrapSummary" runat="server" Text="Summary"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDraftSummary" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="50px" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                           <asp:Literal ID="lblHelpDraftFieldList" runat="server" Text="Field List"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvHelpDraft" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" Style="margin-bottom: 0px" EnableViewState="false"
                                OnRowCancelingEdit="gvHelp_RowCancelingEdit" OnRowEditing="gvHelp_RowEditing" OnRowUpdating="gvHelp_RowUpdating"
                                ShowFooter="true" OnRowDeleting="gvHelp_RowDeleting" OnRowDataBound="gvHelp_RowDataBound"
                                OnRowCommand="gvHelp_RowCommand" DataKeyNames="FLDFIELDLISTID">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Field Name">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDFIELDNAME") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFiledNameEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="100" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDNAME") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFiledNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="100"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDFIELDDESC") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFieldDescEdit" runat="server" CssClass="gridinput" MaxLength="500" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDDESC") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFieldDescAdd" runat="server" CssClass="gridinput" MaxLength="500"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value Description">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDVALUEDESC")%>
                                        </ItemTemplate>
                                         <EditItemTemplate>
                                            <asp:TextBox ID="txtValueDescEdit" runat="server" CssClass="gridinput" MaxLength="500" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUEDESC") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtValueDescAdd" runat="server" CssClass="gridinput" MaxLength="500"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblHelpDraftActionHeader" runat="server">Action</asp:Label>
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
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Literal ID="lblDraftUsage" runat="server" Text="Usage"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <ajaxToolkit:Editor ID="txtDraftUsage" runat="server" Width="100%" Height="90%" ActiveMode="Design" />                           
                        </td>
                    </tr>
                    <tr>
                        <th>
                           <asp:Literal ID="lblDraftSubLinks" runat="server" Text="Sub Links"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvLinkDraft" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" Style="margin-bottom: 0px" EnableViewState="false" OnRowDataBound="gvLink_RowDataBound"
                                OnRowCancelingEdit="gvLink_RowCancelingEdit" OnRowEditing="gvLink_RowEditing" OnRowUpdating="gvLink_RowUpdating"
                                OnRowCommand="gvLink_RowCommand"                              
                                DataKeyNames="FLDLINKID" ShowFooter="true">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Other Links">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlPageEdit" CssClass="input_mandatory" runat="server" DataTextField="FLDDESCRIPTION"
                                                DataValueField="FLDPAGEID">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlPageAdd" CssClass="input_mandatory" runat="server" DataTextField="FLDDESCRIPTION"
                                                DataValueField="FLDPAGEID">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                         <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                         <EditItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="gridinput_mandatory" MaxLength="500" ToolTip="Description of the Link"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:TextBox>
                                         </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDraftLinkActionHeader" runat="server">Action</asp:Label>
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
                        </td>
                    </tr>
                </table>
                </div>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
