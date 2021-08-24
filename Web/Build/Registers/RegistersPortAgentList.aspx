<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersPortAgentList.aspx.cs"
    Inherits="RegistersPortAgentList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Port Agent</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersPortAgent" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPortAgentEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Address"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureCompany" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td>
                               <asp:TextBox ID="txtAgentName" CssClass="input" runat="server" />
                            </td>
                            <td>
                                <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal> 
                            </td>
                            <td>
                                <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" AutoPostBack="true" VesselsOnly="true" AppendDataBoundItems="true" /> 
                            </td>
                            <td>
                                <asp:Literal ID="lblCountry" runat="server" Text="Country"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Country runat="server" ID="ucCountry" CssClass="input" AutoPostBack="true" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersCompany" runat="server" OnTabStripCommand="RegistersCompany_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1" width="100%">
                    <asp:GridView ID="gvPortAgent" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvPortAgent_RowCommand" OnRowDataBound="gvPortAgent_ItemDataBound"
                        OnRowCancelingEdit="gvPortAgent_RowCancelingEdit" OnRowDeleting="gvPortAgent_RowDeleting"
                        OnRowEditing="gvPortAgent_RowEditing" OnSorting="gvPortAgent_Sorting" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Agent Name">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkAgentNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAddressCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkAddressCode" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                    <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLADDRESS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>    
                            
                            <asp:TemplateField HeaderText="Vessel Name">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" ></ItemStyle>                               
                                <ItemTemplate>                                   
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>     
                            
                            <asp:TemplateField HeaderText="Address Type">
                                <ItemStyle Wrap="True" HorizontalAlign="Left" ></ItemStyle>                               
                                <ItemTemplate>                                   
                                    <asp:Label ID="lblAddressType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            
                            <asp:TemplateField HeaderText=" Country ">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>        
                            
                            <asp:TemplateField HeaderText=" State ">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkStateHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDSTATE" ForeColor="White">State&nbsp;</asp:LinkButton>
                                    <img id="FLDSTATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                      
                              <asp:TemplateField HeaderText=" City ">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCityHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDCITY" ForeColor="White">City&nbsp;</asp:LinkButton>
                                    <img id="FLDCITY" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                              <asp:TemplateField HeaderText=" Email-1 ">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                 <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png%>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
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
