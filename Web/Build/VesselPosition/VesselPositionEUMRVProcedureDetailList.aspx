<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVProcedureDetailList.aspx.cs" Inherits="VesselPositionEUMRVProcedureDetailList" %>

<%@ Import Namespace="SouthNests.Phoenix.VesselPosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmVPRSLocation" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"  runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlVPRSLocation">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Company Procedure"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" TabStrip="true" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"></eluc:TabStrip>
                    </div>
                </div> 
                <div>
                <table width="50%">
                    <tr>
                        <td><asp:Label ID="lblprocedurefilter" Text="Procedure Name" runat="server"></asp:Label></td>
                        <td><asp:TextBox ID="txtprocedurefilter" runat="server" CssClass="input" ></asp:TextBox></td>
                    </tr>
                </table>
                    
                </div>  
                 <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="Location_TabStripCommand">
                    </eluc:TabStrip>
                </div>      
                     
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvProcedure" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvProcedure_RowCommand" OnRowDataBound="gvProcedure_ItemDataBound"
                        OnRowCreated="gvProcedure_RowCreated" OnRowCancelingEdit="gvProcedure_RowCancelingEdit"
                        OnRowDeleting="gvProcedure_RowDeleting" AllowSorting="true" OnRowEditing="gvProcedure_RowEditing"
                        OnRowUpdating="gvProcedure_RowUpdating" OnSorting="gvProcedure_Sorting" ShowFooter="true"
                        ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblTableHeader" runat="server" CommandName="Sort" CommandArgument="FLDCODE"
                                        ForeColor="White">Table</asp:LinkButton>
                                    <img id="FLDCODE" runat="server" visible="false" />
                                </HeaderTemplate>                                
                                <ItemTemplate>
                                     <asp:Label ID="lblTable" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>
                                </ItemTemplate>                        
                            </asp:TemplateField> 

                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblProcedureHeader" runat="server" CommandName="Sort" CommandArgument="FLDPROCEDURE"
                                        ForeColor="White">Procedure Name</asp:LinkButton>
                                    <img id="FLDPROCEDURE" runat="server" visible="false" />
                                </HeaderTemplate>                                
                                <ItemTemplate>
                                     <asp:Label ID="lblProcedure" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURE") %>'></asp:Label>
                                     <asp:Label ID="lblProcedureIdadd" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDUREDETAILID") %>'></asp:Label>
                                      <asp:Label ID="lblProcedureID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVPROCEDUERID") %>'></asp:Label>
                                </ItemTemplate>                        
                            </asp:TemplateField> 
                                                       
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkVersionHeader" runat="server" CommandName="Sort" CommandArgument="FLDVERSION"
                                        ForeColor="White">Version</asp:LinkButton>
                                    <img id="FLDVERSION" runat="server" visible="false" />
                                </HeaderTemplate>                                                        
                                <ItemTemplate>                                   
                                    
                                    <asp:Label ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERSION") %>'></asp:Label>
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
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="Edit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                     <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="View" ImageUrl="<%$ PhoenixTheme:images/view-task.png %>"
                                        CommandName="WIEW" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdView"
                                        ToolTip="View" ></asp:ImageButton>
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
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
