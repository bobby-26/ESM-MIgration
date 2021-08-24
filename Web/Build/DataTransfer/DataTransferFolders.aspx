<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferFolders.aspx.cs" Inherits="DataTransferFolders" %>

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
                    <eluc:Title runat="server" ID="ucTitle" Text="Data Synchronizer - Import Folders" ShowMenu="false" />                                 
                    </div>
                </div> 
               
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuFolderList" runat="server" OnTabStripCommand="FolderList_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                </div>
                
                <div id="divGrid" style="position: relative; z-index: 0;">
                    <asp:GridView ID="gvFolderList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        CellPadding="3" EnableViewState="false" 
                        OnRowCommand="gvFolderList_RowCommand" 
                        OnRowDataBound="gvFolderList_ItemDataBound"                                                
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
                                    Serial Number
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFolderFullName" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFOLDERFULLNAME") %>'></asp:Label>
                                    <asp:Label ID="lblDataTransferDateTime" runat="server" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDENTITYSERIAL") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Folder Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFolderName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkFolderName" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                </ItemTemplate>                                
                            </asp:TemplateField>                      
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    Created Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDataTransferStartTime" runat="server" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>                            
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    Modified Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDataTransferEndTime" runat="server" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>           
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdArchive" runat="server" AlternateText="Archive" 
                                        CommandArgument="<%# Container.DataItemIndex %>" CommandName="ARCHIVE" 
                                        ImageUrl="<%$ PhoenixTheme:images/archive.png %>" ToolTip="Archive" />
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
