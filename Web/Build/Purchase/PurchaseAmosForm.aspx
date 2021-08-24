<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseAmosForm.aspx.cs" Inherits="PurchaseAmosForm" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOrderForm">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="frmTitle" Text="Amos Form Details"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <asp:TextBox ID="lblorderId" runat ="server" ></asp:TextBox>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuOrderFormMain" runat="server" 
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 300px; width: 100%">
                </iframe>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvFormDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvFormDetails_RowCommand" OnRowDataBound="gvFormDetails_ItemDataBound"
                        OnRowCancelingEdit="gvFormDetails_RowCancelingEdit" OnRowDeleting="gvFormDetails_RowDeleting"
                        AllowSorting="true" OnRowEditing="gvFormDetails_RowEditing" ShowHeader="true"
                        EnableViewState="false" OnSorting="gvFormDetails_Sorting" OnSelectedIndexChanging="gvFormDetails_SelectedIndexChanging"
                        DataKeyNames="FLDORDERID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="DoubleClick"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblAbbreviationHeader" runat="server" CommandName="Sort" CommandArgument="FLDFORMNO"
                                        ForeColor="White">Number</asp:LinkButton>
                                    <img id="FLDFORMNO" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>
                                      <asp:LinkButton ID="lnkFormNumberName" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StockItem Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblTitelHeader" runat="server" CommandName="Sort" CommandArgument="FLDTITLE"
                                        ForeColor="White">Form Title</asp:LinkButton>
                                    <img id="FLDTITLE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStockItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vendor">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                                                                    </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:Label ID="lblVendorName" runat ="server" Text ='<%# DataBinder.Eval(Container,"DataItem.XSUP") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.XSUB") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                     <asp:Label ID="lblReceivedDateHeader" runat="server" >Ordered Date</asp:Label> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReceivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.XDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.XDIV") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>   
                             <asp:TemplateField HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.XSHIP") %>'></asp:Label>                                  
                                </ItemTemplate>
                            </asp:TemplateField>  
                                                                
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                                <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
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
