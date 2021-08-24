<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDeliveryItems.aspx.cs" Inherits="PurchaseDeliveryItems" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Delivery Line Items</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseDeliveryItems" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Delivery Items"></eluc:Title>
                    </div>
                </div>
                
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuDeliveryLineItem" runat="server" OnTabStripCommand="MenuDeliveryLineItem_TabStripCommand"  TabStrip="true">
                    </eluc:TabStrip>
                </div>
                
                <br clear="all" />
                <div id="divField" style="position: relative; z-index: 2">
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblPartNumber" runat="server" Text="Part Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartNumber" runat="server" BorderWidth="1px" Width="240px" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox>
                            </td>   
                            <td>
                                <asp:Literal ID="lblPartName" runat="server" Text="Part Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartName" runat="server" BorderWidth="1px" Width="240px" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox><br /> 
                                <asp:Label ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") + ",  " + DataBinder.Eval(Container,"DataItem.FLDMAKERNAME")%>'></asp:Label>
                            </td>                        
                        </tr>
                         <tr>
                            <td>
                                <asp:Literal ID="lblMakerReference" runat="server" Text="Maker Reference"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMakerReference" runat="server" Width="240px" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox>
                            </td>   
                            <td>
                                <asp:Literal ID="lblUnit" runat="server" Text="Unit"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Unit runat="server" ID="ucUnit" CssClass="input" Enabled="false" />
                            </td>                         
                        </tr>                                             
                        <tr>
                            <td>
                                <asp:Literal ID="lblPurchased" runat="server" Text="Purchased"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchased" runat="server" Width="90px" style="text-align: right" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblQuantity" runat="server" Text="Quantity"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" runat="server" Width="90px" style="text-align: right" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox>                                
                            </td>
                        </tr>                                             
                        <tr>
                            <td>
                                <asp:Literal ID="lblLocation" runat="server" Text="Location"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLocation" runat="server" Width="240px" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox>
                            </td>
                        </tr>   
                    </table>
                </div>
                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuDeliveryItemList" runat="server" OnTabStripCommand="MenuDeliveryItemList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                 <div id="divGrid" style="position: relative; z-index: 1" width:100%;">
                    <asp:GridView ID="gvDeliveryItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvDeliveryItem_RowCommand" OnRowDataBound="gvDeliveryItem_RowDataBound"
                        EnableViewState="False" OnRowDeleting="gvDeliveryItem_RowDeleting" OnRowEditing="gvDeliveryItem_RowEditing"
                        AllowSorting="true" OnSorting="gvDeliveryItem_Sorting" OnRowCreated ="gvDeliveryItem_RowCreated"
                        OnRowCancelingEdit="gvDeliveryItem_RowCancelingEdit" ShowHeader="true"
                        onrowupdating="gvDeliveryItem_RowUpdating" OnSelectedIndexChanging="gvDeliveryItem_SelectedIndexChanging" >
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />  
                                   
                            <asp:TemplateField HeaderText="number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPartNumberHeader" runat="server" >Part Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDeliveryLineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYLINEID") %>'></asp:Label>
                                    <asp:Label ID="lblDeliveryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYID") %>'></asp:Label>
                                    <asp:Label ID="lblMakerRef" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREF") %>'></asp:Label>
                                    <asp:Label ID="lblOrderLineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkPartNumber" runat="server" OnCommand="onPurchaseDeliveryLine"
                                        CommandArgument='<%# Container.DataItemIndex %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></asp:LinkButton>                                        
                                    <%--<eluc:ToolTip ID="ucToolTipComponent" runat="server" Text='<%# "Component Name: " + DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") + "<br/>" + "Component Number: " + DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER")%>' />     --%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Part Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPartNameHeader" runat="server" >Part Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPartName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNAME") %>' Font-Bold="true"></asp:Label><br /> 
                                    <asp:Label ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") + ",  " + DataBinder.Eval(Container,"DataItem.FLDMAKERNAME")%>'></asp:Label> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Purchased">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPurchasedHeader" runat="server" >Purchased</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPurchased" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Quantity">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQuantityHeader" runat="server" >Quantity</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></asp:Label>
                                </ItemTemplate> 
                                <EditItemTemplate>
                                    <asp:Label ID="lblUnitIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'></asp:Label>
                                    <asp:Label ID="lblDeliveryLineIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYLINEID") %>'></asp:Label>
                                    <asp:Label ID="lblDeliveryIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYID") %>'></asp:Label>
                                    <asp:Label ID="lblOrderLineIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></asp:Label>
                                    <asp:Label ID="lblPurchasedEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></asp:Label>
                                    <eluc:Numeber ID="txtQuantityEdit" runat="server" Width="90px" CssClass="gridinput_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' Mask="99,999" />
                                </EditItemTemplate>                              
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Unit">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUnitNameHeader" runat="server" >Unit</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUnitId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'></asp:Label>
                                    <asp:Label ID="lblunit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Located">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLocatedHeader" runat="server" >Located </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLocated" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME","{0:n0}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtLocated" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></asp:TextBox>
                                </EditItemTemplate>
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
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdUpdate"
                                        ToolTip="Update"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative; z-index: 0;">
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
