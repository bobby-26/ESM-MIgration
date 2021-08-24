<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDMRVesselTankConfiguration.aspx.cs" Inherits="RegistersDMRVesselTankConfiguration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Noon Report Range Config</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvlnk">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmNoonReportRangeConfig" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNoonReportConfig">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Vessel-Tank Configuration"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblSearch" width="100%">
                        <tr>
                            <%--<td>
                                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td>
                               <asp:TextBox ID="txtAgentName" CssClass="input" runat="server" />
                            </td>--%>
                            <td>
                                 <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" VesselsOnly="true" AppendDataBoundItems="true" /> 
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuDMRRangeConfig" runat="server" OnTabStripCommand="MenuDMRRangeConfig_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1" width="100%">
                    <asp:GridView ID="gvVesselTankConfig" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvVesselTankConfig_RowCommand" OnRowDataBound="gvVesselTankConfig_ItemDataBound"
                        OnRowCancelingEdit="gvVesselTankConfig_RowCancelingEdit" OnRowDeleting="gvVesselTankConfig_RowDeleting"
                        OnRowEditing="gvVesselTankConfig_RowEditing" OnRowUpdating="gvVesselTankConfig_RowUpdating" OnSorting="gvVesselTankConfig_Sorting" ShowFooter="true"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                                                       
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" ></ItemStyle>                               
                                <ItemTemplate>     
                                     <asp:Label ID="lblConfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTANKCONFIGID") %>'></asp:Label>                              
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:Label ID="lblConfigIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTANKCONFIGID") %>'></asp:Label>
                                    <eluc:Vessel ID="ucVesselEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Vessel ID="ucVesselAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                                </FooterTemplate>
                            </asp:TemplateField>     
                            
                            <asp:TemplateField HeaderText="Horizintal Value">
                                <ItemStyle Wrap="True" HorizontalAlign="Right" ></ItemStyle> 
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />                              
                                <ItemTemplate>                                   
                                    <asp:Label ID="lblRowValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHORIZONTALVALUE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:DropDownList runat="server" ID="ddlHorizontalValueEdit" CssClass="dropdown_mandatory" AppendDataBoundItems="true" >
                                           <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    
                                </EditItemTemplate>
                                <FooterTemplate>
                                     <asp:DropDownList runat="server" ID="ddlHorizontalValueAdd" CssClass="dropdown_mandatory" AppendDataBoundItems="true" >
                                           <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                   
                                </FooterTemplate>
                            </asp:TemplateField>       
                            
                            <asp:TemplateField HeaderText="Vertical Value">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblColumnValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERTICALVALUE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:DropDownList runat="server" ID="ddlVerticalValueEdit" CssClass="dropdown_mandatory" AppendDataBoundItems="true" >
                                           <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                   
                                </EditItemTemplate>
                                <FooterTemplate>
                                     <asp:DropDownList runat="server" ID="ddlVerticalValueAdd" CssClass="dropdown_mandatory" AppendDataBoundItems="true" >
                                           <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    
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
