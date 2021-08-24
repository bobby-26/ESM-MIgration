<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVesselList.aspx.cs" Inherits="InspectionVesselList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersVesselList" runat="server" submitdisabledcontrols="true">

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>    
    <asp:UpdatePanel runat="server" ID="pnlVesselListEntry">
        <ContentTemplate>                
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblVesselList" runat="server" Text="Vessel List"></asp:Literal>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureVesselList" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblvesselName" runat="Server" Text="Vessel Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchVesselList" runat="server" MaxLength="100" CssClass="input" Width="360px" ></asp:TextBox>
                            </td>                            
                        </tr>
                    </table>
                </div>
                <div style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersVesselList" runat="server" OnTabStripCommand="RegistersVesselList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;z-index:+1000">               
                    <asp:GridView ID="gvVesselList" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnSorting="gvVesselList_Sorting"
                        Width="100%" CellPadding="3" OnRowCommand="gvVesselList_RowCommand" OnRowDataBound="gvVesselList_ItemDataBound"
                        OnRowCancelingEdit="gvVesselList_RowCancelingEdit" OnRowDeleting="gvVesselList_RowDeleting" AllowSorting="true"
                        OnRowEditing="gvVesselList_RowEditing" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        
                        <Columns>                              
                            <asp:TemplateField HeaderText="Vessel Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDVESSELNAME"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                    <asp:LinkButton ID="lblVesselNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME" ForeColor="White">Vessel Name&nbsp;</asp:LinkButton>
                                    <img id="FLDVESSELNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                </ItemTemplate>                                
                            </asp:TemplateField> 
                                                    
                            <asp:TemplateField HeaderText="Vessel Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblVesselNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNUMBER" ForeColor="White">Vessel Number&nbsp;</asp:LinkButton>
                                    <img id="FLDVESSELNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNUMBER") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>  
                             
                            <asp:TemplateField HeaderText="IMO Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblIMONumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDIMONUMBER" ForeColor="White">IMO Number&nbsp;</asp:LinkButton>
                                    <img id="FLDIMONUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIMONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMONUMBER") %>'></asp:Label>
                                </ItemTemplate>                          
                            </asp:TemplateField> 
                                                    
                            <asp:TemplateField HeaderText="Flag">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblFlagHeader" runat="server">Flag&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELFLAG") %>'></asp:Label>
                                </ItemTemplate>       
                                
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeHeader" runat="server">Type&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'></asp:Label>
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

