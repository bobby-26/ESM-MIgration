<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountAirfareMarkupRegister.aspx.cs"
    Inherits="Accounts_AccountAirfareMarkupRegister" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AIRFARE REGISTER</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmAirfareRegister" runat="server" submitdisabledcontrols="true">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
     <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           
                           <eluc:TabStrip ID="MenuAirfareMain" runat="server" OnTabStripCommand="MenuAirfareMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
            
                           
                               
                                    <eluc:TabStrip ID="MenuAirfare" runat="server" OnTabStripCommand="MenuAirfare_TabStripCommand">
                                    </eluc:TabStrip>
                             
                       
                <br />
                <table style="width:25%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBilltoCompanySetting" runat="server" Text="Bill To Company Setting"></telerik:RadLabel>
                        </td>
                        <td>
                           
                            <telerik:RadComboBox RenderMode="Lightweight" id="ucBillToCompanySetting" AutoPostBack="true" EnableLoadOnDemand="true" runat="server" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" CssClass="input_mandatory" >
                            </telerik:RadComboBox>
                          
                        </td>
                    </tr>
                </table>
                <br />

           <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvSupplier" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSupplier" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvSupplier_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvCashOut_SelectedIndexChanging"
                    OnItemDataBound="gvSupplier_ItemDataBound" OnItemCommand="gvSupplier_ItemCommand"
                    ShowFooter="true" ShowHeader="true" >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Supplier Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAirfareSupplierId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARESUPPLIERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSupplierCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblAirfareSupplierIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARESUPPLIERID") %>'></telerik:RadLabel>
                                    <span id="spnPickListSupplierEdit">
                                        <telerik:RadTextBox ID="txtSupplierCodeEdit" runat="server" Width="60px" CssClass="input_mandatory" 
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtSupplierNameEdit" runat="server" BorderWidth="1px" Width="1"
                                            CssClass="readonlytextbox"></telerik:RadTextBox>
                                        <asp:ImageButton ID="btnPickSupplierEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                        <telerik:RadTextBox ID="txtSupplierIdEdit" runat="server" Width="1" CssClass="input" 
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnPickListSupplierAdd">
                                        <telerik:RadTextBox ID="txtSupplierCodeAdd" runat="server" Width="60px" CssClass="input_mandatory"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtSupplierNameAdd" runat="server" BorderWidth="1px" Width="1"
                                            CssClass="readonlytextbox"></telerik:RadTextBox>
                                        <asp:ImageButton ID="btnPickSupplierAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                        <telerik:RadTextBox ID="txtSupplierIdAdd" runat="server" Width="1" CssClass="input"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Bill To Company">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:UserControlCompany ID="ddlCompanyEdit" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                                </EditItemTemplate>
                                <FooterTemplate>                                    
                                    <eluc:UserControlCompany ID="ddlCompanyAdd" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    CssClass="input_mandatory" runat="server" AppendDataBoundItems="true"/>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="India Domestic Sector">
                                <HeaderStyle HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIndiaDomesticSector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINDIADOMESTICSECTOR","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <eluc:Number ID="ucIndiaDomesticSectorEdit" runat="server"  Width="120px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINDIADOMESTICSECTOR","{0:#0.00}") %>'></eluc:Number>
                                    <%--<asp:TextBox ID="ucIndiaDomesticSectorEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINDIADOMESTICSECTOR","{0:#0.00}") %>'
                                        CssClass="input_mandatory" MaxLength="26" Width="50px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskIndiaDomesticSectorEdit" runat="server" TargetControlID="ucIndiaDomesticSectorEdit"
                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <FooterTemplate>
                                   
                                    <eluc:Number ID="ucIndiaDomesticSectorAdd" runat="server"  Width="120px"></eluc:Number>
                                    <%--<asp:TextBox ID ="ucIndiaDomesticSectorAdd" runat="server" CssClass="input_mandatory" Width="50px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskIndiaDomesticSectorAdd" runat="server" TargetControlID="ucIndiaDomesticSectorAdd"
                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="International Sector">
                                <HeaderStyle HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInternationalSector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERNATIONALSECTOR","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <eluc:Number ID="ucInternationalSectorEdit" runat="server"  Width="120px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERNATIONALSECTOR","{0:#0.00}") %>'></eluc:Number>
                                   <%-- <asp:TextBox ID="ucInternationalSectorEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERNATIONALSECTOR","{0:#0.00}") %>'
                                        CssClass="input_mandatory" MaxLength="26" Width="50px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskInternationalSectorEdit" runat="server" TargetControlID="ucInternationalSectorEdit"
                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <FooterTemplate>
                                    <eluc:Number ID="ucInternationalSectorAdd" runat="server"  Width="120px"></eluc:Number>
                                   <%-- <asp:TextBox ID ="ucInternationalSectorAdd" runat="server" CssClass="input_mandatory" Width="50px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskInternationalSectorAdd" runat="server" TargetControlID="ucInternationalSectorAdd"
                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Cancellation Ticket">
                                <HeaderStyle HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCancellationTicket" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLATIONTICKET","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucCancellationTicketEdit" runat="server"  Width="120px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLATIONTICKET","{0:#0.00}") %>'></eluc:Number>
                                   <%-- <asp:TextBox ID="ucCancellationTicketEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLATIONTICKET","{0:#0.00}") %>'
                                        CssClass="input_mandatory" MaxLength="26" Width="50px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskCancellationTicketEdit" runat="server" TargetControlID="ucCancellationTicketEdit"
                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <FooterTemplate>
                                     <eluc:Number ID="ucCancellationTicketAdd" runat="server"  Width="120px"></eluc:Number>
                                 <%--   <asp:TextBox ID ="ucCancellationTicketAdd" runat="server" CssClass="input_mandatory" Width="50px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskCancellationTicketAdd" runat="server" TargetControlID="ucCancellationTicketAdd"
                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                          
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                              
                                <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="Edit" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Delete" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
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
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                <br />
                <br />
                <br />
                      <br />
           <eluc:TabStrip ID="MenuOrderFormMain" runat="server"  TabStrip="true"></eluc:TabStrip>
        
              
                    <br />
                    <table id="tblAirfare" width="50%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblInvoiceAmount" runat="server" Text="Standard Cancellation Invoice Amount"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                    CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    OnTextChangedEvent="Invoice_SetExchangeRate" />
                                 <eluc:Number ID="txtAmount" runat="server"  Width="120px"></eluc:Number>
                               <%-- <asp:TextBox runat="server" ID="txtAmount" CssClass="input_mandatory" Style="text-align: right;"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskAmount" runat="server" TargetControlID="txtAmount"
                                    Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                </ajaxToolkit:MaskedEditExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMaxPrice" runat="server" Text="Max Markup Price"></telerik:RadLabel>
                            </td>
                            <td>
                                 <eluc:Number ID="txtMaxPrice" runat="server"  Width="120px"></eluc:Number>
                              <%--  <asp:TextBox runat="server" ID="txtMaxPrice" CssClass="input_mandatory" Style="text-align: right;"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskMaxPrice" runat="server" TargetControlID="txtMaxPrice"
                                    Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                </ajaxToolkit:MaskedEditExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCompany" runat="server" Text="Bill to Company"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlCompany ID="ddlBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPayingCompany" runat="server" Text="Paying Company"></telerik:RadLabel>
                            </td> 
                            <td>
                                <eluc:UserControlCompany ID="ddlPayingCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
              <%--  </div>--%>
                <br />
                <br />
          <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvAirfare" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAirfare" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvAirfare_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvAirfare_SelectedIndexChanging"
                    OnItemDataBound="gvAirfare_ItemDataBound" OnItemCommand="gvAirfare_ItemCommand"
                    ShowFooter="true" ShowHeader="true" >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >
                       <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Price Range(USD)" Name="Price" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                       
                    </ColumnGroups>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="Price">
                                <HeaderStyle  HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMarkupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPRANGEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblFromAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMAMOUNT","{0:###,###,###,##0.00}") %>'
                                        DecimalPlace="2"></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblMarkupIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPRANGEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblFromAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                
                                <FooterTemplate >
                                    <telerik:RadLabel ID="lblFromAmountAdd" runat="server"></telerik:RadLabel>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="Price">
                                 <HeaderStyle  HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblToAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <eluc:Number ID="txtToAmountEdit" runat="server"  Width="120px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOAMOUNT","{0:###,###,###,##0.00}") %>'></eluc:Number>
                                  <%--  <asp:TextBox ID="txtToAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOAMOUNT","{0:###,###,###,##0.00}") %>'
                                        CssClass="gridinput_mandatory" MaxLength="26" Width="100px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskToAmountEdit" runat="server" TargetControlID="txtToAmountEdit"
                                        Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </EditItemTemplate>
                                 <FooterStyle HorizontalAlign="Right" />
                                <FooterTemplate>
                                     <eluc:Number ID="txtToAmountAdd" runat="server"  Width="120px"></eluc:Number>
                                  <%--  <asp:TextBox ID="txtToAmountAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="25"
                                        Width="100px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskToAmountAdd" runat="server" TargetControlID="txtToAmountAdd"
                                        Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Markup %" ColumnGroupName="Price">
                                 <HeaderStyle  HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMarkupAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPAMOUNT","{0:#0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <eluc:Number ID="txtMarkupAmountEdit" runat="server"  Width="120px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPAMOUNT","{0:#0.00}") %>'></eluc:Number>
                                   <%-- <asp:TextBox ID="txtMarkupAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUPAMOUNT","{0:#0.00}") %>'
                                        CssClass="gridinput_mandatory" MaxLength="26" Width="50px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskMarkupAmountEdit" runat="server" TargetControlID="txtMarkupAmountEdit"
                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </EditItemTemplate>
                                 <FooterStyle HorizontalAlign="Right" />
                               
                                <FooterTemplate>
                                     <eluc:Number ID="txtMarkupAmountAdd" runat="server"  Width="120px"></eluc:Number>
                                  <%--  <asp:TextBox ID="txtMarkupAmountAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="25" Width="50px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskMarkupAmountAdd" runat="server" TargetControlID="txtMarkupAmountAdd"
                                        Mask="99.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                              
                                <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="Edit" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/move_items.png %>"
                                        CommandName="Delete" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Group"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
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
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
          <eluc:Status runat="server" ID="ucStatus" />
      </telerik:RadAjaxPanel>
    </form>
</body>
</html>


           
