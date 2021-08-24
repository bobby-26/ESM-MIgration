<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestLineItem.aspx.cs" Inherits="CrewLicenceRequestLineItem" %>

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
<head id="Head1" runat="server">
    <title>Line Items</title>
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
            <eluc:Title runat="server" ID="Title" Text="Line Items" ShowMenu="false"></eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>  
        <div id="divFind" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Literal ID="lblLumpSumAmount" runat="server" Text="LumpSum Amount"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Number ID="ucNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                </tr>
            </table>
        </div>   
        <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
            <asp:GridView ID="gvLicReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" 
                OnRowDataBound="gvLicReq_RowDataBound" EnableViewState="false" 
                AllowSorting="true" OnSorting="gvLicReq_Sorting" >
                
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
                    </asp:TemplateField>                    
                    
                    <%--<asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>                            
                            <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>                            
                             <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
                        </ItemTemplate>                        
                    </asp:TemplateField>--%>
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
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </div>
    </form>
</body>
</html>
