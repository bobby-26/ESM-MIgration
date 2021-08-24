<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListRAExtn.aspx.cs" Inherits="CommonPickListRAExtn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Hazards</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersPortAgent" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPortAgentEntry">
        <ContentTemplate>                
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Risk Assessment" ShowMenu="false" /><asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPortAgent" runat="server" OnTabStripCommand="MenuPortAgent_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigurePortAgent" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                            </td>
                            <td>
                               <asp:RadioButtonList ID="rblType" runat="server" CssClass="input" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                               <asp:ListItem Text="Generic" Value="2"></asp:ListItem>
                               <asp:ListItem Text="Machinery" Value="3"></asp:ListItem>
                               <asp:ListItem Text="Navigation" Value="4"></asp:ListItem>
                               <asp:ListItem Text="Cargo" Value="5"></asp:ListItem>
                               </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Literal ID="lblKeyword" runat="server" Text="Keyword"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtActivity" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvPortAgent" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvPortAgent_RowCommand" OnRowDataBound="gvPortAgent_RowDataBound"
                        OnRowEditing="gvPortAgent_RowEditing" ShowHeader="true" AllowSorting="true" OnSorting = "gvPortAgent_Sorting">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblActivity" runat="server" Text="Activity"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY").ToString().Length>60 ? DataBinder.Eval(Container, "DataItem.FLDACTIVITY").ToString().Substring(0, 60) + "..." : DataBinder.Eval(Container, "DataItem.FLDACTIVITY").ToString() %> '></asp:Label>
                                        <eluc:ToolTip ID="ucToolTipActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblProcessName" runat="server" Text="Process Name"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference No"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRAId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID")  %>'></asp:Label>
                                        <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></asp:Label>
                                        <asp:Label ID="lblRefNo" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO")  %>'></asp:Label>
                                        <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRisksAspects" runat="server" Text="Risks/Aspects"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENT")  %>'></asp:Label>
                                    </ItemTemplate>
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
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvPortAgent" />
        </Triggers>
    </asp:UpdatePanel>            
    </form>
</body>
</html>
