<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferJobProgress.aspx.cs" Inherits="DataTransferJobProgress" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <title>Folders</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script> 
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlDataTransfer">
            <ContentTemplate>
             <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align:top; width:100%; position:absolute; z-index:+1">
               
                <div class="subHeader" style="position: relative">                    
                    <div id="divHeading">
                    <eluc:Title runat="server" ID="ucTitle" Text="Data Synchronizer - Scheduled Jobs" ShowMenu="false" />                                 
                    </div>
                </div> 
               
                <div id="divGrid" style="position: relative; z-index: 0;">
                    <asp:GridView ID="gvProgress" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        CellPadding="3" EnableViewState="false" 
                        OnRowCommand="gvProgress_RowCommand" 
                        Style="margin-bottom: 0px; margin-top: 34px;" Width="100%"
                        ShowFooter="false" ShowHeader="true">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                                
                        <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    Transfer Code
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblTransferCode" runat="server" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSFERCODE") %>' CommandArgument="<%# Container.DataItemIndex %>" CommandName="SHOWTHIS"></asp:LinkButton>
                                </ItemTemplate>                                
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Table Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Table Name
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTableName" runat="server" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDTABLENAME") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>                      
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    Record Count
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRecordCount" runat="server" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECORDCOUNT") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>                            
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    File Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFileName" runat="server" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>      
                        </Columns>
                    </asp:GridView>
                </div>
               </div> 
            </ContentTemplate>            
        </asp:UpdatePanel>  
    </form>
</body>
</html>
