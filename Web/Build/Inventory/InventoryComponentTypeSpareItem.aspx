<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentTypeSpareItem.aspx.cs"
    Inherits="InventoryComponentTypeSpareItem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPlannedMaintenanceComponentStockItem" runat="server"  autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlStockItemComponentType">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                 <eluc:Status runat="server" ID="ucStatus"/> 
                <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Parts" ShowMenu="false"></eluc:Title>
                    </div>
                    <div>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuGridComponentTypeStockItem" runat="server" OnTabStripCommand="MenuGridComponentTypeStockItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="dgComponentType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="dgComponentType_RowCommand" OnRowDataBound="dgComponentType_ItemDataBound"
                        OnRowCancelingEdit="dgComponentType_RowCancelingEdit" OnRowDeleting="dgComponentType_RowDeleting"
                        OnRowEditing="dgComponentType_RowEditing" OnRowCreated="dgComponentType_RowCreated" OnRowUpdating="dgComponentType_RowUpdating"
                        EnableViewState="false"  Style="margin-bottom: 0px"
                        AllowSorting="true" OnSorting="dgComponentType_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblStockItemNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDNUMBER"
                                        ForeColor="White">Number&nbsp;</asp:LinkButton>
                                    <img id="FLDNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStockItemComponentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMCOMPONENTTYPEID") %>'></asp:Label>
                                    <asp:Label ID="lblComponentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTTYPEID") %>'></asp:Label>
                                    <asp:Label ID="lblStockId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></asp:Label>
                                    <asp:Label ID="lblStockItemNumber" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblStockItemNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStockItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStockItemMakerNameHeader" runat="server">Makers Reference</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStockItemMakerName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblQuantityUseInComponentTypHeader" runat="server" >In Use</asp:Label> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantityUseInComponentTyp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITYINCOMPONENT","{0:n0}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtQuantityUseInComponentTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITYINCOMPONENT","{0:n0}") %>'
                                        CssClass="gridinput number" MaxLength="9"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskQuantityUseInComponent" runat="server" TargetControlID="txtQuantityUseInComponentTypeEdit"
                                        Mask="9999999" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </EditItemTemplate>
                            </asp:TemplateField>                        
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblDrawingNumberHeader" runat="server" >Drawing No.&nbsp;</asp:Label> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDrawingNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAWINGNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDrawingNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAWINGNUMBER") %>'
                                        CssClass="gridinput" MaxLength="6"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    
                                    
                                    <asp:Label ID="lblItemPositionHeader" runat="server" >Position&nbsp; </asp:Label> 
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblItemPosition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSITION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtItemPositionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSITION") %>'
                                        CssClass="gridinput" MaxLength="6"></asp:TextBox>
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
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" cellpadding="1" cellspacing="1" border="0" class="datagrid_pagestyle">
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
                                <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="dgComponentType" />
        </Triggers>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
