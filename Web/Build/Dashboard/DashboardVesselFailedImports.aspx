<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVesselFailedImports.aspx.cs" Inherits="Dashboard_DashboardVesselFailedImports" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Failed Imports</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script> 

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlDataTransfer">
            <ContentTemplate>
             <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align:top; width:100%; position:absolute; z-index:+1">
                <div id="divGrid" style="position: relative; z-index: 0;">      
                    <asp:GridView GridLines="None" ID="gvFolderList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
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
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                    <asp:Label ID="lblScheduledJobID" runat="server" Visible="false"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>  
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSeqNo" runat="server" Text="Seq. No"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSeqNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENTITYSERIAL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                       
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblLastRunOk" runat="server" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTRUNOK") %>'></asp:LinkButton>
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
