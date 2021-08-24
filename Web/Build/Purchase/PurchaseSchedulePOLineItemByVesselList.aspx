<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSchedulePOLineItemByVesselList.aspx.cs" Inherits="PurchaseSchedulePOLineItemByVesselList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerBudgetGroup" Src="~/UserControls/UserControlOwnerBudgetGroup.ascx" %>
<%@ Register src="../UserControls/UserControlMaskNumber.ascx" tagname="number" tagprefix="eluc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Line Item By Vessel</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="link" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOwnerBudgetCode" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOwnreBudgetcodeEntry">
        <ContentTemplate>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute; z-index: +1">                
                <eluc:Status runat="server" ID="ucStatus" />
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="ucTitle" Text="Vessels"></eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                 <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBulkPO" runat="server" OnTabStripCommand="MenuBulkPO_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divPOLineItem" style="position: relative; z-index: +1;">
                    <asp:GridView ID="gvSchedulePOLineItem" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" OnSorting="gvSchedulePOLineItem_Sorting" Width="100%" CellPadding="3"
                        OnRowCommand="gvSchedulePOLineItem_RowCommand" OnRowDataBound="gvSchedulePOLineItem_ItemDataBound"
                        AllowSorting="true" OnRowEditing="gvSchedulePOLineItem_RowEditing" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDLINEITEMID" OnSelectedIndexChanging="gvSchedulePOLineItem_SelectedIndexChanging">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Item Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblItemNameHeader" runat="server">Item Name&nbsp; </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkItemName" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMNAME") %>'></asp:LinkButton>                                                                                                                        
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item Number" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblItemNumberHeader" runat="server">Item Number&nbsp; </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMNUMBER") %>'></asp:Label>
                                    <asp:Label ID="lblLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMID") %>'
                                        Visible="false"></asp:Label>                                     
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBudgetCodeHeader" runat="server">Budget Code&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUnitHeader" runat="server">Unit&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit Price" >
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Literal ID="lblUnitPrice" runat="server" Text="Unit Price"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUnitPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE") %>'></asp:Label>                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requested Qty">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRequestedQtyHeader" runat="server">Total Order Quantity
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRequestedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALORDERQUANTITY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage1" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber1" runat="server"> </asp:Label>
                                <asp:Label ID="lblPages1" runat="server"> </asp:Label>
                                <asp:Label ID="lblRecords1" runat="server"> </asp:Label>&nbsp;&nbsp;
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
                                <asp:TextBox ID="txtnopage1" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                                <asp:Button ID="btnGo1" runat="server" Text="Go" OnClick="cmdGo_Click1" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <br />
                <br />
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuVessel" runat="server" OnTabStripCommand="MenuVessel_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1;">
                    <asp:GridView ID="gvSchedulePOLineItemByVessel" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" OnSorting="gvSchedulePOLineItemByVessel_Sorting" Width="100%"
                        CellPadding="3" OnRowCommand="gvSchedulePOLineItemByVessel_RowCommand" OnRowDataBound="gvSchedulePOLineItemByVessel_ItemDataBound"
                        OnRowCancelingEdit="gvSchedulePOLineItemByVessel_RowCancelingEdit" OnRowUpdating="gvSchedulePOLineItemByVessel_RowUpdating"
                        AllowSorting="true" OnRowEditing="gvSchedulePOLineItemByVessel_RowEditing" OnRowDeleting="gvSchedulePOLineItemByVessel_RowDeleting"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDLINEITEMBYVESSELID"
                        
                        OnSelectedIndexChanging="gvSchedulePOLineItemByVessel_SelectedIndexChanging" 
                        onrowcreated="gvSchedulePOLineItemByVessel_RowCreated">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Item Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lnkVesselNameHeader" runat="server">Vessel Name&nbsp; </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblLineItemByVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMBYVESSELID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBudgetCodeHeader" runat="server">Budget Code&nbsp; </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListMainBudgetEdit">
                                        <asp:TextBox ID="txtBudgetCodeEdit" runat="server" Width="60px" CssClass="input"
                                            Enabled="False" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:TextBox>
                                        <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." />
                                        <asp:TextBox ID="txtBudgetNameEdit" runat="server" CssClass="input" Enabled="False"
                                            Width="0px"></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requested Qty">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRequestedQtyHeader" runat="server">Order Quantity
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRequestedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:number ID="ucRequestedQtyEdit" runat="server" CssClass="input_mandatory" MaxLength="7" 
                                        IsInteger="true" IsPositive="true" DecimalPlace="0" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY") %>' />
                                </EditItemTemplate>                               
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit Price">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                  <asp:Literal ID="lblUnitPrice" runat="server" Text="Unit Price"></asp:Literal> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUnitPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE") %>'></asp:Label>
                                </ItemTemplate>                                                            
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Total">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                  <asp:Literal ID="lblTotal" runat="server" Text="Total"></asp:Literal> 
                                                                      
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></asp:Label>
                                </ItemTemplate>                                                            
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
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
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
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
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
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
