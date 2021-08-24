<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersPMUDoctor.aspx.cs" Inherits="RegistersPMUDoctor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PMU Doctor</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div runat="server" id="dvLink">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersPMUDoctor" runat="server">
    
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlPMUDoctorEntry">
        <ContentTemplate>
        
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="PMU Doctor" />
                    </div>
                </div>

                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuMedical" runat="server" OnTabStripCommand="MenuMedical_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip><asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />
                </div>
                
                <div id="divFind" style="position: relative; z-index: 2; top: 0px; left: 0px; width: 100%;">
                    <table id="tblConfigurePMUDoctor" width="100%">
                        <tr>                            
                            <td>
                                <asp:Literal ID="lblPMUDoctor" runat="server" Text="Doctor Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblActiveYN" runat="server" Text="Activity Y/N" Visible="false"></asp:Literal>
                            </td>
                            <td> 
                                <asp:CheckBox ID="chkActiveYN" runat="server" AutoPostBack="true" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>                
                <br />                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersPMUDoctor" runat="server" OnTabStripCommand="RegistersPMUDoctor_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPMUDoctor" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvPMUDoctor_RowCommand" OnRowDataBound="gvPMUDoctor_ItemDataBound"
                        OnRowCreated="gvPMUDoctor_RowCreated" OnRowCancelingEdit="gvPMUDoctor_RowCancelingEdit"
                        OnRowDeleting="gvPMUDoctor_RowDeleting" OnRowUpdating="gvPMUDoctor_RowUpdating" OnRowEditing="gvPMUDoctor_RowEditing"
                        ShowFooter="true" ShowHeader="true" OnSorting="gvPMUDoctor_Sorting" AllowSorting="true"
                        EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPMUDoctorNameHeader" Visible="true" runat="server">
                                        <asp:ImageButton runat="server" ID="cmdDoctorName" OnClick="cmdSearch_Click" CommandName="FLDDOCTORNAME"
                                            ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                    </asp:Label>
                                    <asp:LinkButton ID="lnkDoctorNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDDOCTORNAME"
                                        ForeColor="White">Doctor Name</asp:LinkButton>
                                    <img id="FLDDOCTORNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDoctorID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCTORID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkDoctorName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCTORNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblDoctorIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCTORID") %>'></asp:Label>
                                    <asp:TextBox ID="txtDoctorNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCTORNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtDoctorNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter Doctor Name"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>                            
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <FooterStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActiveYNHeader" runat="server">
                                    Active Y/N
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>                                
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkActiveYNAdd" runat="server" Checked="true"></asp:CheckBox>
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
