<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceProcessGeneral.aspx.cs" Inherits="CrewLicenceProcessGeneral" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/UserControlTabs.ascx" TagPrefix="eluc" TagName="TabStrip" %>
<%@ Register Src="~/UserControls/UserControlTitle.ascx" TagPrefix="eluc" TagName="Title" %>
<%@ Register Src="~/UserControls/UserControlErrorMessage.ascx" TagPrefix="eluc" TagName="Error" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="UserControlStatus" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Licence request List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>                
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>
    </telerik:RadCodeBlock>    
</head>
<body>
    <form id="frmLicReq" runat="server">
    
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="Title3" Text="" ShowMenu="false"></eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <div class="subHeader" style="position: relative;">
            <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                <eluc:TabStrip ID="CrewLicReq" runat="server" OnTabStripCommand="CrewLicReq_TabStripCommand">
                </eluc:TabStrip>
            </span>
        </div>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuLicenceList" runat="server" OnTabStripCommand="MenuLicenceList_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
            <asp:GridView ID="gvLicReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" OnRowDeleting="gvLicReq_RowDeleting" OnRowCancelingEdit="gvLicReq_RowCancelingEdit"
                OnRowDataBound="gvLicReq_RowDataBound" EnableViewState="false" OnRowEditing="gvLicReq_RowEditing"
                AllowSorting="true" OnSorting="gvLicReq_Sorting" OnRowUpdating="gvLicReq_RowUpdating">
                
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                ForeColor="White">Vessel</asp:LinkButton>
                            <img id="FLDVESSELNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblVesselId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                            <asp:Label ID="lblReqId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'></asp:Label>
                           <%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                        </ItemTemplate>
                    </asp:TemplateField> 
                      
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANKNAME"
                                ForeColor="White">Rank</asp:LinkButton>
                            <img id="FLDRANKNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField> 
                                  
                     <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="lblFirstHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                ForeColor="White">Name</asp:LinkButton>
                            <img id="FLDNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                     <asp:TemplateField>
                          <HeaderTemplate>
                            <asp:LinkButton ID="lblLicenceHeader" runat="server" CommandName="Sort" CommandArgument="FLDLICENCE"
                                ForeColor="White">Licence</asp:LinkButton>
                            <img id="FLDLICENCE" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDLICENCE")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                             <%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:Label ID="lblReqIdEdit" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUESTID")%>'></asp:Label>
                            <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input txtNumber"  Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'  MaxLength="14"  />
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                    
                    <%--<asp:TemplateField HeaderText="Budget Code">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span id="spnPickListTaxBudgetEdit">
                                <asp:TextBox ID="txtBudgetCodeEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                    MaxLength="20" CssClass="input" Width="80%"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="input" Enabled="False"></asp:TextBox>
                                <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                <asp:TextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                            </span>
                        </EditItemTemplate>
                    </asp:TemplateField>--%>
                    
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>                            
                            <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                             <%--<asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                            <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />--%>
                             <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
                        </ItemTemplate>
                         <%--<EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
        <eluc:UserControlStatus ID="ucStatus" runat="server" />
    </div>
    </form>
</body>
</html>
