﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreCourseCostCBT.aspx.cs" Inherits="CrewOffshoreCourseCostCBT" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Course Cost</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div runat="server" id="dvLink">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCourseCost" runat="server">
    
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    
        <asp:UpdatePanel runat="server" ID="pnlCourseCostEntry">
        <ContentTemplate>        
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">                
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Course Cost" />
                    </div>
                </div>              
                  <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MnuCourseCost" runat="server" OnTabStripCommand="CourseCost_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                </div>                
                
                <div id="divFind" style="position: relative; z-index: 2; top: 0px; left: 0px; width: 100%;">
                    <table id="tblCourseCost" width="100%">
                              <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                            </td>
                            <td>
                              <asp:DropDownList ID="ddlCourse" AutoPostBack="true" runat="server" AppendDataBoundItems="true"  CssClass="input_mandatory" Width="300px">
                                    </asp:DropDownList>
                               <%--  <eluc:Address runat="server" ID="ucInstitution" CssClass="dropdown_mandatory"
                                        AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                        AppendDataBoundItems="true" AutoPostBack="true" Width="300px" />        --%>
                            </td>                            
                        </tr>
                    </table>
                </div>
                
                <br />
                <%--<b><asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal></b>--%>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCourseCost" runat="server" OnTabStripCommand="MenuCourseCost_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvCourseCost" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCourseCost_RowCommand" OnRowDataBound="gvCourseCost_RowDataBound"
                        OnRowCreated="gvCourseCost_RowCreated" OnRowCancelingEdit="gvCourseCost_RowCancelingEdit"
                        OnRowDeleting="gvCourseCost_RowDeleting" OnRowUpdating="gvCourseCost_RowUpdating" OnRowEditing="gvCourseCost_RowEditing"
                        ShowFooter="true" ShowHeader="true" OnSorting="gvCourseCost_Sorting" AllowSorting="true"
                        EnableViewState="false">                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />                        
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="500px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblHeader" runat="server" Text="Institute">
                                        
                                    </asp:Label>                                  
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseCostId" runat="server" Visible="false" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSECOSTID") %>'></asp:Label>                                
                                    <asp:LinkButton ID="lnkInstituteName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTENAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCourseCostIdEdit" runat="server" Visible="false" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSECOSTID") %>'></asp:Label>                                   
                                   <eluc:Address runat="server" ID="ucInstitutionEdit" CssClass="dropdown_mandatory"
                                        AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                        AppendDataBoundItems="true" Width="300px" /> 
                                </EditItemTemplate>
                                <FooterTemplate> 
                                 <eluc:Address runat="server" ID="ucInstitutionAdd" CssClass="dropdown_mandatory"
                                        AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                        AppendDataBoundItems="true"  Width="200px" />                                          
                                   <%-- <asp:DropDownList ID="ddlCourseAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="200px">
                                    </asp:DropDownList>--%>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="500px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblHeader" runat="server" Text="Currency">                                        
                                    </asp:Label>                                  
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrencyName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME")  %>'></asp:Label>                                  
                                </ItemTemplate>
                                <EditItemTemplate>                                
                                    <asp:Label ID="lblCurrencyIdEdit" runat="server" Visible="false" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></asp:Label>                                   
                                 <eluc:Currency runat="server" ID="ucCurrencyEdit" ActiveCurrency="true" AppendDataBoundItems="true"
                                     CssClass="dropdown_mandatory"  />
                                </EditItemTemplate>
                                <FooterTemplate>                                          
                                  <eluc:Currency runat="server" ID="ucCurrencyAdd" ActiveCurrency="true" AppendDataBoundItems="true"
                                     CssClass="dropdown_mandatory"  />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblCostHeader" runat="server">
                                        Cost
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOST") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number runat="server" ID="txtCost" CssClass="input_mandatory"
                                         Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOST") %>' Width="80px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number runat="server" ID="txtCostAdd" CssClass="input_mandatory"
                                         Width="80px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblDurationHeader" runat="server" Text="Duration (In Days)">                                        
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDuation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number runat="server" ID="txtDuration" CssClass="input" IsInteger="true"
                                         Text='<%# DataBinder.Eval(Container, "DataItem.FLDDURATION") %>' Width="80px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number runat="server" ID="txtDurationAdd" CssClass="input"
                                         Width="80px" IsInteger="true"/>
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
