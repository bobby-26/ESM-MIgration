<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceDepositUtilizationView.aspx.cs" Inherits="Crew_CrewLicenceDepositUtilizationView" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Airlines</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDepositUtilized" autocomplete="off" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDepositUtilization">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus"/>
                    
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Deposit View" ShowMenu="false" />
                    </div>
                </div>
                <div id="divFind" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDepositCurrencyAmount" runat="server" Text="Deposit Currency/Amount"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDepositCurrency" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="txtDepositAmount" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBalance" runat="server" Text="Balance"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBalance" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>                
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuDepositUtilized" runat="server" OnTabStripCommand="MenuDepositUtilized_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvDepositUtilized" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="True" Style="margin-bottom: 0px" EnableViewState="false" 
                         OnRowCommand="gvDepositUtilized_OnRowCommand" OnRowDataBound="gvDepositUtilized_ItemDataBound" >
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAdvancePaymentNumber" runat="server" Text="Advance Payment Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvancePaymentNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYMENTNUMBER") %>'></asp:Label>
                                    <asp:Label ID="lblRequestBalance" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBALANCEREMAINING") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRequestNumber" runat="server" Text="Request Number"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDepositUtilizationid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDEPOSITUTILIZAIONID") %>'></asp:Label><asp:Label
                                        ID="lblRequestNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREFNUMBER") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRequestAmount" runat="server" Text="Request Amount"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRequestAmount" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRequestedDate" runat="server" Text="Requested Date"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRequestedDate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDEPOSITDATE") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Remove Allocation" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                        CommandName="REMOVE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdRemove"
                                        ToolTip="Remove Allocation"></asp:ImageButton>
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
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton></td><td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
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
