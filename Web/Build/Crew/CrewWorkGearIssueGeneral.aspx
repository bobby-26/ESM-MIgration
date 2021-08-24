<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkGearIssueGeneral.aspx.cs"
    Inherits="CrewWorkGearIssueGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Personal General</title>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Working Gear Issuance/Request" ShowMenu="false" />
            </div>
        </div>
        <table cellpadding="1" cellspacing="1" width="60%">
            <tr>
                <td>
                    <asp:Literal runat="server" ID="lblInwhichwayworkinggearIssuetoSeafarers" runat="server" Text="In which way working gear issue to seafarer's"></asp:Literal>
                </td>
                <td>
                    <asp:RadioButtonList ID="rdoIssueType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdoIssueType_SelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Text="Request from Vendor" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Issue from Stock" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <b>Previous Requests</b>
        <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
            <asp:GridView ID="gvReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" 
                OnRowCommand="gvReq_RowCommand" OnRowDataBound="gvReq_RowDataBound"
                EnableViewState="false" AllowSorting="true"
                OnSorting="gvReq_Sorting">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblRefNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDREFERENCENO"
                                ForeColor="White">Request Number</asp:LinkButton>
                            <img id="FLDREFERENCENO" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDREFERENCENO")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vessel">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rank">                        
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblOrderDateHeader" runat="server">Order Date</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDORDERDATE")%>
                            <asp:Label ID="lblRequestId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORDERID")%>'></asp:Label>
                            <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORDERSTATUS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                  
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDORDERSTATUSNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblSupplierHeader" runat="server">Supplier</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDSUPPLIER")%>
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
                            <asp:ImageButton runat="server" AlternateText="Cancel Request" ImageUrl="<%$ PhoenixTheme:images/red-symbol.png %>"
                                CommandName="CANCELREQUEST" CommandArgument='<%# Container.DataItemIndex %>'
                                ID="cmdCancelRequest" ToolTip="Cancel Request"></asp:ImageButton>
                        </ItemTemplate>
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
    </div>
    </form>
</body>
</html>
