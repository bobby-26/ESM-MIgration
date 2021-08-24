<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAnalyticChart.aspx.cs" Inherits="RegistersAnalyticChart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmCrewList" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCrewList">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Chart" />
                        </div>
                    </div>
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                            </td>
                            <td>
                                <span id="spnPickListAddress">
                                    <asp:TextBox ID="txtOwnerCode" runat="server" CssClass="input_mandatory"
                                        Width="80px"></asp:TextBox>
                                    <asp:TextBox ID="txtOwnerName" runat="server" CssClass="input_mandatory"
                                        Width="120px"></asp:TextBox>
                                    <img runat="server" id="imgShowMaker" style="cursor: pointer; vertical-align: top"
                                        src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListAddress', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=127,128', true); " />
                                    <asp:TextBox ID="txtOwnerId" runat="server" CssClass="hidden" Width="0px"></asp:TextBox>
                                </span>
                            </td>
                        </tr>
                    </table>
                   
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuList" runat="server" OnTabStripCommand="MenuList_TabStripCommand"></eluc:TabStrip>
                    </div>

                    <asp:GridView GridLines="None" ID="gvAnalyticList" runat="server" AutoGenerateColumns="False" Width="100%"
                        CellPadding="3" ShowFooter="true" OnRowDataBound="gvAnalyticList_RowDataBound" OnRowEditing="gvAnalyticList_RowEditing"
                        OnRowCommand="gvAnalyticList_RowCommand" OnRowCancelingEdit="gvAnalyticList_RowCancelingEdit" 
                        OnRowUpdating="gvAnalyticList_RowUpdating" OnRowDeleting="gvAnalyticList_RowDeleting"
                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDANALYTICID">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                         <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSrNo" runat="server">Sr.No</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOwnerName" runat="server" Text="Owner"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDOWNERNAME")%>
                                </ItemTemplate>                                
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblModuleName" runat="server" Text="Module"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDMODULENAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                      <asp:TextBox ID="txtModuleNameEdit" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="100" ToolTip="Enter Module" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMODULENAME")%>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtModuleNameAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="100" ToolTip="Enter Chart Name"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblChartName" runat="server" Text="Chart Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCHARTNAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                      <asp:TextBox ID="txtChartNameEdit" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="100" ToolTip="Enter Chart Name" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCHARTNAME")%>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtChartNameAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="100" ToolTip="Enter Chart Name"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblURLName" runat="server" Text="URL"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblURL" runat="server"></asp:Label>
                                    <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' />                                                                        
                                </ItemTemplate>
                                <EditItemTemplate>
                                      <asp:TextBox ID="txtURLEdit" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="400" ToolTip="Enter URL" Text='<%# DataBinder.Eval(Container, "DataItem.FLDURL")%>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtURLAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="400" ToolTip="Enter URL"></asp:TextBox>
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
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>                                    
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
