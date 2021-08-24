<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendorQuoteCompare.aspx.cs" Inherits="PurchaseVendorQuoteCompare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

        <title>Purchase Quote Compare</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>  
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseQuoteCompare" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDashboard">
        <ContentTemplate>
               <div id="navigation" style="top: 0px; vertical-align: top; width: 100%; postion:absolute; margin-bottom:0px; bottom:0px;">
               <div class="subHeader">
                    <div class="divFloatLeft">
                        <eluc:Title runat="server" ID="ucTitle" Text="Quote Compare" />
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    </div>
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                            TabStrip="true" ></eluc:TabStrip>
                    </div>
                </div>
                <%--<div class="subHeader">
                    <div class="divFloatLeft" >
                        <eluc:Title runat="server" ID="Title1" Text="Users List" ShowMenu="false" />
                    </div>
                    <div class="navSelect" style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuPhoenixBroadcast" runat="server" OnTabStripCommand="PhoenixBroadcast_TabStripCommand" ></eluc:TabStrip>
                    </div>
                </div>--%>
                <table width="100%" style="height: 100%; margin-top: 0px;  vertical-align:top;">
                    <tr>
                        <td>
                           <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true" AssignedVessels="true"
                                CssClass="input" AutoPostBack="true" />
                        </td>
                        <td>
                           <asp:Literal ID="lblStockType" runat="server" Text="Stock Type"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlStockType" AutoPostBack="true" CssClass="input_mandatory">
                                <asp:ListItem Text="Spares" Value="SPARE"></asp:ListItem>
                                <asp:ListItem Text="Stores" Value="STORE"></asp:ListItem>
                                <asp:ListItem Text="Service" Value="SERVICE"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlVendor" AutoPostBack="true" CssClass="dropdown_mandatory" >
                            </asp:DropDownList>
                        </td>
                        <td>
                           <asp:Literal ID="lblMakerRef" runat="server" Text="Maker Ref."></asp:Literal>

                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMakerRef" CssClass="input" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblDateFrom" runat="server" Text="Date From"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateFrom" runat="server" Width="90px" CssClass="input" ></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ceDateFrom" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtDateFrom" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                           <asp:Literal ID="lblDateTo" runat="server" Text="Date To"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateTo" runat="server" Width="90px" CssClass="input" ></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ceDateTo" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtDateTo" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblPartNumber" runat="server" Text="Part Number"></asp:Literal>

                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPartNumber" CssClass="input" MaxLength="20"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblPartName" runat="server" Text="Part Name"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPartName" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                </table>      
                <table width="100%" style="height: 100%; margin-top: 0px;  vertical-align:top;">
                    <tr>
                        <td width="20%" valign="top">
                            <div class="navSelect" style="position: relative; width: 15px">
                                <eluc:TabStrip ID="MenuPOList" runat="server" OnTabStripCommand="MenuPOList_TabStripCommand">
                                </eluc:TabStrip>
                            </div>
                            <div style="border-width: 1px; border-style: groove; border-color: Navy; height: 100%; width: 100%; overflow-x: hidden; overflow-y: auto;">
                                <%--<div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                                    <eluc:TabStrip ID="MenuPOList" runat="server" OnTabStripCommand="MenuPOList_TabStripCommand">
                                    </eluc:TabStrip>
                                </div>    --%>        
                                
                                <asp:GridView ID="gvPOList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" OnRowCommand="gvPOList_RowCommand" OnRowDataBound="gvPOList_RowDataBound"
                                    OnRowEditing="gvPOList_RowEditing" ShowHeader="true" EnableViewState="false"
                                    BorderColor="Transparent" DataKeyNames="FLDORDERID">
                                    
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />
                                    
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <HeaderTemplate>
                           <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                                
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>
                                                <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>                                                
                                                <asp:Label ID="lblQuotationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>'></asp:Label>                                                
                                                <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <HeaderTemplate>
                           <asp:Literal ID="lblFormNo" runat="server" Text="Form No"></asp:Literal>
                                                
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'
                                                    CommandName="SELECTFORMNO" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <HeaderTemplate>
                           <asp:Literal ID="lblFormTitle" runat="server" Text="Form Title"></asp:Literal>
                                                
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblFormTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div id="div1" style="position: relative;">
                                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                                        <tr>
                                            <td nowrap align="center">
                                                <asp:Label ID="lblPagenumber1" runat="server">
                                                </asp:Label>
                                                <asp:Label ID="lblPages1" runat="server">
                                                </asp:Label>
                                                <asp:Label ID="lblRecords1" runat="server">
                                                </asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td nowrap align="left" width="50px">
                                                <asp:LinkButton ID="cmdPrevious1" runat="server" OnCommand="PagerButtonClick1" CommandName="prev">Prev << </asp:LinkButton>
                                            </td>
                                            <td width="20px">
                                                &nbsp;
                                            </td>
                                            <td nowrap align="right" width="50px">
                                                <asp:LinkButton ID="cmdNext1" OnCommand="PagerButtonClick1" runat="server" CommandName="next">Next >></asp:LinkButton>
                                            </td>
                                            <td nowrap align="center">
                                                <asp:TextBox ID="txtnopage1" MaxLength="5" Width="20px" runat="server" CssClass="input">
                                                </asp:TextBox>
                                                <asp:Button ID="btnGo1" runat="server" Text="Go" OnClick="cmdGo_Click1" CssClass="input"
                                                    Width="40px"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2" valign="top">
                            <br />
                            <br />
                            <div class="navSelect" style="position: relative; width: 15px">
                                <eluc:TabStrip ID="MenuLineItemList" runat="server" OnTabStripCommand="MenuLineItemList_TabStripCommand">
                                </eluc:TabStrip>
                            </div>
                            <div style="border-width: 1px; border-style: groove; border-color: Navy; height: 100%; width: 100%; overflow: hidden;">
                                <div>
                                    <asp:GridView ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px" 
                                        Width="100%" CellPadding="3" BorderColor="Transparent" OnRowDataBound="gvLineItem_ItemDataBound"  >
                                        
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <RowStyle Height="10px" />
                                        
                                        <Columns>
                                            <%--<asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    S. No.
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                           <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                                                    
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                           <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                                                    
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                           <asp:Literal ID="lblMakerRef" runat="server" Text="Maker Ref."></asp:Literal>
                                                    
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                           <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                                                    
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                           <asp:Literal ID="lblReceivedDate" runat="server" Text="Received Date"></asp:Literal>
                                                    
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                           <asp:Literal ID="lblQty" runat="server" Text="Qty"></asp:Literal>
                                                    
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                           <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                                                    
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                           <asp:Literal ID="lblPrice" runat="server" Text="Price"></asp:Literal>
                                                    
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                           <asp:Literal ID="lblDiscount" runat="server" Text="Discount (%)"></asp:Literal>
                                                    
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT") %>'></asp:Label>
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
                        </td>
                    </tr>
                </table>
                <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
