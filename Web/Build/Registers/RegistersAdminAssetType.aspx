<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAssetType.aspx.cs" Inherits="RegistersAdminAssetType"
MaintainScrollPositionOnPostback="true"%>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Asset Type</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
  
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersAssetType" autocomplete="off" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAdminAssetTypeEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Asset Category"  visible="false"/>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureAdminAssetType" width="100%">
                        <tr>
                            <td>
                                Category
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlCategory" runat="server" Width="180px" CssClass="input_mandatory"
                                     AllowCustomText="true" EmptyMessage="Type to Select" AutoPostBack="true"></telerik:RadComboBox>
                            </td>
                            <td>
                                Type
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlType" runat="server" Width="180px" AutoPostBack="true"
                                    AllowCustomText="true" EmptyMessage="Type to Select" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Name
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" Width="180px" ></telerik:RadTextBox>
                            </td>
                            <td>
                                Assembly
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlAssembly" runat="server" Width="180px" 
                                    AllowCustomText="true" EmptyMessage="Type to Select" AutoPostBack="true"></telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersAdminAssetType" runat="server" OnTabStripCommand="MenuRegistersAdminAssetType_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;z-index:0">
                    <asp:GridView ID="gvAssetType" runat="server" AutoGenerateColumns="False" Font-Size="11px" EnableViewState="false" 
                        Width="100%" CellPadding="3" OnRowCommand="gvAssetType_RowCommand" OnRowDataBound="gvAssetType_RowDataBound"
                        OnRowCancelingEdit="gvAssetType_RowCancelingEdit" OnRowDeleting="gvAssetType_RowDeleting"
                        OnRowEditing="gvAssetType_RowEditing" ShowFooter="true" ShowHeader="true"
                         OnSorting="gvAssetType_Sorting" AllowSorting="true" OnRowCreated="gvAssetType_RowCreated"
                         OnSelectedIndexChanging="gvAssetType_SelectedIndexChanging" OnRowUpdating="gvAssetType_RowUpdating" >
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField FooterText="New Team Member">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="BindData"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                            CommandArgument="1" />
                                    <asp:LinkButton ID="lblAssetTypeNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME" ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAssetCategoryEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                    <asp:Label ID="lblAssetTypeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSETTYPEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkAssetTypeName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblAssetTypeIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSETTYPEID") %>'></asp:Label>
                                    <telerik:RadTextBox ID="txtAssetTypeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" Width="98%"></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtAssetTypeNameAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ToolTip="Enter Name" Width="98%"></telerik:RadTextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Asset Type Description">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>                                    
                                    <asp:Label ID="lblAssetTypeDesHeader" runat="server" CommandName="Sort" CommandArgument="FLDDESCRIPTION" ForeColor="White">Description&nbsp;</asp:Label>
                                    <img id="Img3" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAssetTypeDes" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>                                   
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <telerik:RadTextBox ID="txtAssetTypeDesEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" Width="98%"></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtAssetTypeDesAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ToolTip="Enter Description" Width="98%"></telerik:RadTextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>  
                                 <asp:Label ID="lblAssetItemTypeHeader" runat="server" CommandName="Sort" CommandArgument="FLDITEMTYPE" ForeColor="White">Type&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                 <asp:Label ID="lblAssetItemType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                      
                                    <telerik:RadComboBox ID="ddlItemTypeEdit" runat="server" CssClass="input_mandatory" Width="100%" AutoPostBack="true" DataTextField="FLDNAME"
                                        AllowCustomText="true" EmptyMessage="Type to Select"  DataValueField="FLDADMITEMTYPEID">
                                    </telerik:RadComboBox>
                                    <asp:Label ID="lblItemTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMTYPE") %>' Visible="false"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="ddlItemTypeAdd" runat="server" CssClass="input_mandatory" Width="100%" AutoPostBack="true" DataTextField="FLDNAME" 
                                        AllowCustomText="true" EmptyMessage="Type to Select" DataValueField="FLDADMITEMTYPEID">
                                    </telerik:RadComboBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>  
                                    <asp:Label ID="lblAssetAssemblyHeader" runat="server" CommandName="Sort" CommandArgument="FLDPARENTID" ForeColor="White">Assembly&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAssetAssembly" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSEMBLYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                      
                                    <telerik:RadComboBox ID="ddlAssetAssemblyEdit" runat="server" Width="100%" AutoPostBack="true" DataTextField="FLDNAME"
                                        AllowCustomText="true" EmptyMessage="Type to Select" DataValueField="FLDPARENTID">
                                    </telerik:RadComboBox>
                                    <asp:Label ID="lblParentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTID") %>' Visible="false"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="ddlAssetAssemblyAdd" runat="server" Width="100%" AutoPostBack="true" DataTextField="FLDNAME"
                                        AllowCustomText="true" EmptyMessage="Type to Select" DataValueField="FLDPARENTID">
                                    </telerik:RadComboBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
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
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
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
                                <telerik:RadTextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </telerik:RadTextBox>
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
