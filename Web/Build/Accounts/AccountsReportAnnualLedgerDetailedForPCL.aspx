<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportAnnualLedgerDetailedForPCL.aspx.cs" Inherits="Accounts_AccountsReportAnnualLedgerDetailedForPCL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AccountsReportAnnualLedgerAccount</title>
    <style type="text/css">
        .style1
        {
            width: 61px;
        }
    </style>
    <div id="from1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</head>
<body>
        <form id="AccReport" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
            width: 100%">
            <div class="subHeader">
                <div class="divFloatLeft">
                    <eluc:Title runat="server" ID="Title3" Text="Ledger Account Report Detailed" ShowMenu="true">
                    </eluc:Title>
                </div>
                <asp:Button runat="server" ID="cmdHiddenSubmit" />
                <div class="divFloat">
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MenuMainFilter" runat="server" OnTabStripCommand="MenuMainFilter_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
            </div>
            <div class="subHeader">
                <div class="divFloat" style="clear: right">
                    <eluc:TabStrip ID="AnnualReport" runat="server" OnTabStripCommand="AnnualReport_TabStripCommand">
                    </eluc:TabStrip>
                </div>
            </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        <div id="divFind" style="position: relative; z-index: 2">
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td class="style1">
                    <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" Width="80px" CssClass="input_mandatory" />
                </td>
                <td>
                    <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" Width="80px" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="input_mandatory">
                    <asp:ListItem Value="1">Base Currency</asp:ListItem>
                    <asp:ListItem Value="2">Reportting Currency</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <table style="float: left; width: 100%;">
                        <tr>
                            <td style="white-space:nowrap">
                                <asp:Literal ID="lblAccountCode" runat="server" Text="Account Code"></asp:Literal>
                                <asp:TextBox runat="server" ID="txtAccountSearch" CssClass="input"
                                    MaxLength="10" Width="150px"></asp:TextBox>&nbsp;
                                <asp:ImageButton runat="server"
                                    ImageUrl="<%$ PhoenixTheme:images/search.png %>" ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click"
                                    ToolTip="Search" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblAccount" runat="server" Text="Account"></asp:Literal>
                </td>
                <td>
                    <div runat="server" id="dvaccount" class="input" style="overflow: auto; width: 80%;
                        height: 220px">
                        <asp:CheckBoxList runat="server" ID="cblAccount" Height="100%" RepeatColumns="1"
                            AutoPostBack="True" OnSelectedIndexChanged="AccountSelection" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td>               
                    <asp:GridView ID="gvSelectedAccount" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvSelectedAccount_ItemDataBound" OnRowDeleting="gvSelectedAccount_RowDeleting"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        
                        <Columns>                              
                            <asp:TemplateField HeaderText="Account">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSelectedAccounts" runat="server" Text="Selected Account(s)"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></asp:Label>
                                    <asp:Label ID="lblAccountName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNAME") %>'></asp:Label>
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
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>                                
                            </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </div>
    </form>
</body>
</html>
