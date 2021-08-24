<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDirectPOPISplitDetails.aspx.cs"
    Inherits="AccountsDirectPOPISplitDetails" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
  <%: Scripts.Render("~/bundles/js") %>
     <%: Styles.Render("~/bundles/css") %>
    <style type="text/css">
        .hidden
        {
            display: none;
        }
        .ChangeZIndex {
            z-index: 12101 !important;
        }
    </style>
     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
     <script type="text/javascript">
        $("[src*=collapsed]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "../css/Theme1/images/expanded.gif");
    });
        $("[src*=expanded]").live("click", function () {
            $(this).attr("src", "../css/Theme1/images/collapsed.gif");
        $(this).closest("tr").next().remove();
    });
</script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseSatging" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="92%">
            
              
               
                    <eluc:TabStrip ID="MenuDirectPO" runat="server" OnTabStripCommand="RegistersStockItem_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
               
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
             
                <%--    <eluc:TabStrip ID="MenuRegistersStockItem" runat="server" OnTabStripCommand="RegistersStockItem_TabStripCommand">
                    </eluc:TabStrip>--%>
             
                <table cellpadding="1" cellspacing="1" width="80%">
                    <tr>
                       
                        <td>
                            <telerik:RadLabel ID="lblMedical" runat="server" Text="Medical Ref No."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMedical" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="90px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtEmployee" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="180px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPono" runat="server" Text=" PO No."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPono" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="130px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="130px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>

              <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvOrderLine" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvOrderLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvOrderLine_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" 
                    OnItemDataBound="gvOrderLine_ItemDataBound" OnItemCommand="gvOrderLine_ItemCommand"
                    ShowFooter="true" ShowHeader="true"  Height="94%">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >

              
                        <Columns>
                              <telerik:GridTemplateColumn>
                              <ItemTemplate>
                                       <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/54.png %>"
                                        CommandName="INFO1" CommandArgument='<%# Container.DataSetIndex %>' ID="imgnotes"
                                        ></asp:ImageButton>
                                       
                                    <eluc:ToolTip ID="ucToolTipNW" runat="server" />
                                      
                                  
                                 
                                
                              <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDHARDNAME")%>'></telerik:RadLabel>

                              </ItemTemplate>
                          </telerik:GridTemplateColumn>
                         
                          <telerik:GridTemplateColumn HeaderText ="Sub Type">
                             
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblsigner" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSIGNER")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblhead" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSIGNERYN")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblsubtype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDID")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblupdatedtkay" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUPDATEDTKEY")%>'></telerik:RadLabel>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDNAME")%>
                                    
                                     
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>Total</b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right">
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblHeader" runat="server" Text=""></telerik:RadLabel>   
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                        Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'>
                                    </eluc:Number>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterTemplate>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Amount (USD)" ItemStyle-HorizontalAlign="Right">
                                <%--<HeaderTemplate>
                                    <asp:Label ID="lblHeader1" runat="server" Text="Amount (USD)"></asp:Label>   
                                </HeaderTemplate>--%>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTUSD","{0:n2}") %>
                                </ItemTemplate>
                                
                                <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterTemplate>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>
                              
                                <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                    
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                     <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>

                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdUpdate"
                                        ToolTip="Update"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                
                            </telerik:GridTemplateColumn>
                           
                        </Columns>
                     <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                      <%--  <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />--%>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
      </telerik:RadAjaxPanel>
                
     
    </form>
</body>
</html>
                    

           <%--     <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                    <asp:GridView ID="gvOrderLine" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        GridLines="None" Width="100%" CellPadding="3"  OnRowCommand="gvOrderLine_RowCommand"
                        OnRowUpdating="gvOrderLine_RowUpdating" OnRowDataBound="gvOrderLine_RowDataBound"
                        EnableViewState="False" OnRowDeleting="gvOrderLine_RowDeleting" OnRowEditing="gvOrderLine_RowEditing"
                        OnPreRender="gvOrderLine_PreRender" OnRowCancelingEdit="gvOrderLine_RowCancelingEdit"
                        ShowFooter="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                              <asp:TemplateField>
                              <ItemTemplate>
                                       <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/54.png %>"
                                        CommandName="INFO1" CommandArgument='<%# Container.DataItemIndex %>' ID="imgnotes"
                                        ></asp:ImageButton>
                                       
                                    <eluc:ToolTip ID="ucToolTipNW" runat="server" />
                                      
                                  
                                 
                                
                              <asp:Label ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDHARDNAME")%>'></asp:Label>

                              </ItemTemplate>
                          </asp:TemplateField>
                         
                          <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBudgetIdHeader" runat="server">Sub Type</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblsigner" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSIGNER")%>'></asp:Label>
                                    <asp:Label ID="lblhead" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSIGNERYN")%>'></asp:Label>
                                    <asp:Label ID="lblsubtype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDID")%>'></asp:Label>
                                    <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'></asp:Label>
                                    <asp:Label ID="lblupdatedtkay" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUPDATEDTKEY")%>'></asp:Label>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDNAME")%>
                                    
                                     
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>Total</b>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                <HeaderTemplate>
                                    <asp:Label ID="lblHeader" runat="server" Text=""></asp:Label>   
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                        Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'>
                                    </eluc:Number>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                <HeaderTemplate>
                                    <asp:Label ID="lblHeader1" runat="server" Text="Amount (USD)"></asp:Label>   
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTUSD","{0:n2}") %>
                                </ItemTemplate>
                                
                                <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>
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
                                     <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>--%>
