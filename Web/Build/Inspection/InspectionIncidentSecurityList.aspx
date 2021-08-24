﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentSecurityList.aspx.cs" Inherits="InspectionIncidentSecurityList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Incident Security List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div id="IncidentSearchlink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </div>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmIncidentSecuritySearch" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlIncidentSecuritySearchEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Consequence"></eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuIncidentGeneral" runat="server" TabStrip="true" OnTabStripCommand="IncidentGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuIncidentReportGeneral" TabStrip="true" runat="server" OnTabStripCommand="IncidentReportGeneral_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                
                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 300px; width: 100%">
                </iframe>
                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuIncidentSecuritySearch" runat="server" OnTabStripCommand="IncidentSecuritySearch_TabStripCommand">
                    </eluc:TabStrip>
                </div>                
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvIncidentSecurity" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvIncidentSecurity_RowCommand"
                        OnRowDataBound="gvIncidentSecurity_ItemDataBound" OnRowDeleting="gvIncidentSecurity_RowDeleting"
                        OnSorting="gvIncidentSecurity_Sorting" AllowSorting="true" OnRowEditing="gvIncidentSecurity_RowEditing"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDINSPECTIONINCIDENTSECURITYID">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                       
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblNatureOfIncidentHeader" runat="server">Type&nbsp; </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIncidentSecurityId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTSECURITYID") %>'></asp:Label>
                                    <asp:Label ID="lblIncidentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkTypeOfSecurityName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFSECURITYNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:label ID="lblEstimatedCosetHeader" runat="Server">Estimated Cost&nbsp;</asp:label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESTIMATEDCOST" ) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblConsequenceCategory" runat="server">Consequence <br />Category</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></asp:Label>                                    
                                </ItemTemplate>
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
                                        ToolTip="Edit Incident" Visible="false"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete Incident"></asp:ImageButton>
                                </ItemTemplate>
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
