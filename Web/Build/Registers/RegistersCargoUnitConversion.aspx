<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCargoUnitConversion.aspx.cs"
    Inherits="RegistersCargoUnitConversion" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Direction </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="VesselDirectionlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegisterVesselDirection" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCargoUnitConversion">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Cargo Unit Conversion"></eluc:Title>
                    </div>
                </div>
 
                 <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersVesselUnitConversion" runat="server" OnTabStripCommand="RegistersVesselUnitConversion_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvUnotConversion" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvUnotConversion_RowCommand" OnRowDataBound="gvUnotConversion_ItemDataBound"
                        OnRowCreated="gvUnotConversion_RowCreated" OnRowCancelingEdit="gvUnotConversion_RowCancelingEdit"
                        OnRowDeleting="gvUnotConversion_RowDeleting" AllowSorting="true" OnRowEditing="gvUnotConversion_RowEditing"
                        OnRowUpdating="gvUnotConversion_RowUpdating" ShowFooter="true"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />

                            <asp:TemplateField FooterText="New Cargo">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblFromUnitHeader" runat="server" ForeColor="White">From Unit &nbsp;</asp:Label>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                     <asp:Label ID="lblFromUnit" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMUNIT") %>'></asp:Label>
                                     <asp:Label ID="lblUnitId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOUNITID") %>'></asp:Label>
                                </ItemTemplate>   
                                 
                                  <EditItemTemplate>
                                   <asp:TextBox ID="txtFromUnitEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMUNIT") %>' runat="server" CssClass="gridinput_mandatory" MaxLength="200" />
                                    <asp:Label ID="lblUnitIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOUNITID") %>'></asp:Label>
                                </EditItemTemplate>

                                <FooterTemplate>
                                    <asp:TextBox ID="txtFromUnitAdd" runat="server"  CssClass="gridinput_mandatory" MaxLength="200"  ToolTip="Enter From Unit" />    
                                </FooterTemplate>
                                
                            </asp:TemplateField>
                            
                             <asp:TemplateField FooterText="New Short">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblToUnitHeader" runat="server" ForeColor="White">To Unit&nbsp;</asp:Label>
                                </HeaderTemplate>
                                                        
                                <ItemTemplate>                                   
                                    <asp:Label ID="lblToUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOUNIT") %>'></asp:Label>
                                </ItemTemplate>
                                
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtToUnitEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOUNIT") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                
                                <FooterTemplate>
                                    <asp:TextBox ID="txtToUnitAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter To Unit"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField FooterText="New Short">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblunitconversionHeader" runat="server" ForeColor="White">Unit Conversion&nbsp;</asp:Label>
                                </HeaderTemplate>
                                                        
                                <ItemTemplate>                                   
                                    <asp:Label ID="lblUnitConversion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSION") %>'></asp:Label>
                                </ItemTemplate>
                                
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUnitConversionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSION") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                
                                <FooterTemplate>
                                    <asp:TextBox ID="txtUnitConversionAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter Conversion Unit"></asp:TextBox>
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
