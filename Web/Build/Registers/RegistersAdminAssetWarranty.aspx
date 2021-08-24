<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAssetWarranty.aspx.cs"
    Inherits="RegistersAdminAssetWarranty" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title>Asset Warranty</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="js" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAssetWarranty" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAssetWarranty">
        <ContentTemplate>            
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Warranty" />
                    </div>
                </div>
                <div id="divAssetWarranty" style="position: relative; z-index: 2">
                    <table id="tblAssetWarranty" width="100%">
                        <tr>
                            <td width= "5%">
                                <asp:literal ID="lblCategoryType" runat="server" Text="Category"></asp:literal>
                            </td>
                            <td width="16.6%">
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="input_mandatory" Width="150px" AutoPostBack="true">                          
                                </asp:DropDownList>                          
                            </td>
                            <td width="5%">
                            <asp:Literal ID="lblLocation" runat="server" Text="Zone"></asp:Literal> 
                        </td>
                        <td width="16.6%">
                            <eluc:Zone ID="ddlLocation" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="120px" AutoPostBack="true"/>
                        </td>
                            <td width= "5%">
                                <asp:literal ID="lblAssetName" runat="server" Text="Asset"></asp:literal>
                            </td>
                            <td width="16.6%">
                                <asp:TextBox ID="txtAssetName" runat="server" CssClass="input" Width="150px"></asp:TextBox>                              
                            </td>
                            <td width= "4%">
                                Active
                            </td>
                            <td width="16.6%">
                                <asp:DropDownList ID="ddlActive" runat="server" CssClass="input" AutoPostBack="true">
                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuAdminAssetWarranty" runat="server" OnTabStripCommand="MenuAdminAssetWarranty_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvAdminWarranty" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdminWarranty_RowCommand" OnRowDataBound="gvAdminWarranty_ItemDataBound"
                        OnRowCancelingEdit="gvAdminWarranty_RowCancelingEdit" OnRowDeleting="gvAdminWarranty_RowDeleting"
                        OnRowEditing="gvAdminWarranty_RowEditing" ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnRowUpdating="gvAdminWarranty_RowUpdating" OnSorting="gvAdminWarranty_Sorting" OnSelectedIndexChanging="gvAdminWarranty_SelectedIndexChanging">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetNameHeader" runat="server">
                                    Asset
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAssetWarrantyID" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWARRANTYID")) %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblAssetName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDASSETNAME")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <span id="spnAssetNameEdit">
                                     <asp:TextBox ID="txtAssetNameEdit" runat="server" CssClass="input_mandatory" 
                                       width="250px" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDASSETNAME")) %>'></asp:TextBox>
                                     <asp:ImageButton runat="server" ID="imgAssetNameEdit" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandArgument="<%# Container.DataItemIndex %>"/> 
                                     <asp:TextBox ID="txtAssetIdEdit" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDASSETID")) %>' CssClass="input_mandatory" Width="0px"></asp:TextBox> 
                                     <asp:Label ID="lblAssetWarrantyIDEdit" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWARRANTYID")) %>' Visible="false"></asp:Label>                                 
                                   </span>
                                </EditItemTemplate> 
                                <FooterTemplate>
                                    <span id="spnAssetNameAdd">
                                     <asp:TextBox ID="txtAssetNameAddSearch" runat="server" CssClass="input_mandatory" width="250px"></asp:TextBox>
                                     <asp:ImageButton runat="server" ID="imgAssetNameAdd" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandArgument="<%# Container.DataItemIndex %>"/> 
                                     <asp:TextBox ID="txtAssetIdAdd" runat="server" CssClass="input_mandatory" 
                                         Width="0px"></asp:TextBox>                                  
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="35%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAddressHeader" runat="server">
                                    Address 
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAddressName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDRESSNAME")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnAddressEdit">
                                     <asp:TextBox runat="server" ID="txtAddressSupCodeEdit" width="0px"  CssClass="input" MaxLength="20"></asp:TextBox>                                                                           
                                     <asp:TextBox ID="txtAddressNameEdit" runat="server" CssClass="input_mandatory" Enabled="false"
                                         Width="360px" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDRESSNAME")) %>'></asp:TextBox>
                                     <asp:ImageButton runat="server" ID="imgAddressEdit" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandArgument="<%# Container.DataItemIndex %>"/>                                   
                                     <asp:TextBox ID="txtAddressCodeEdit" runat="server" width="0px" CssClass="input" 
                                        Enabled="false" MaxLength="50" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDRESSID")) %>' ></asp:TextBox> 
                                   </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnAddressAdd">
                                     <asp:TextBox runat="server" ID="txtAddressSupCodeAdd" CssClass="input" Width="0px" MaxLength="20"></asp:TextBox>
                                     <asp:TextBox ID="txtAddressNameAdd" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="360px"></asp:TextBox> 
                                     <asp:ImageButton runat="server" ID="imgAddressAdd" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandArgument="<%# Container.DataItemIndex %>"/>                                   
                                     <asp:TextBox ID="txtAddressCodeAdd" runat="server" CssClass="input" Width="0px"
                                        Enabled="false" MaxLength="50"></asp:TextBox>     
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblValidUntilHeader" runat="server" CommandName="SORT" CommandArgument="FLDVALIDUNTIL"
                                        ForeColor="White">Valid Until</asp:LinkButton>
                                    <img id="FLDVALIDUNTIL" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblValidUntil" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDVALIDUNTIL")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:UserControlDate ID="ucValidUntilEdit" runat="server" DatePicker="true" CssClass="input_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDVALIDUNTIL")) %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:UserControlDate ID="ucValidUntilAdd" runat="server" DatePicker="true" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" width="15%"/>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActiveYNHeader" runat="server">Active</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActiveYNItem" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkActiveEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'/>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
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
                <div id="divPage" style="position: relative; z-index:0">
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
