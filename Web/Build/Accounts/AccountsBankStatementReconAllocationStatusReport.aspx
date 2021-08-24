<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsBankStatementReconAllocationStatusReport.aspx.cs" Inherits="Accounts_AccountsBankStatementReconAllocationStatusReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bank Statement Upload</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmForwarderExcelUpload" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="Attachment" Text="Bank Statement Upload"></eluc:Title>
            <eluc:Status runat="server" ID="ucStatus" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenutravelInvoice" runat="server" >
            </eluc:TabStrip>
        </div>        
        <hr />
        <br />
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuAttachment" runat="server" >
            </eluc:TabStrip>
        </div>
        
        
        Tag ID	Currency	Bank Amount Net	Allocated in Leger	Remaining amount	Allocation Status	Action


        <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvAttachment_RowDataBound"
            OnRowCommand="gvAttachment_RowCommand" AllowSorting="true">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <RowStyle Height="10px" />
            <Columns>
                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                
                 <asp:TemplateField HeaderText="Tag Id">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblTagID" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDBANKTAGNO")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Currency">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
               
                
                 <asp:TemplateField HeaderText="Bank Amount Net">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDBANKSTATEMENTAMOUNT")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                
                 
                 <asp:TemplateField HeaderText="Allocated in Leger">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDALLOCATEDAMOUNT")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>          
                
                         
                 <asp:TemplateField HeaderText="Remaining amount">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDREMAININGAMOUNT")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                
                         
                 <asp:TemplateField HeaderText="Allocation Status">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDALLOCATIONSTATUS")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                 
 
                
               
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                    </HeaderTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                      
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                            CommandName="VIEW" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                            ToolTip="View"></asp:ImageButton>          
                            
                            
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
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
        </table>
    </div>
    </form>
</body>
</html>
