<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockPurchase.aspx.cs"
    Inherits="DryDockPurchase" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.DryDock" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:radcodeblock id="rad1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
         <script type="text/javascript">
                    $(document).ready(function () {
                        var browserHeight = $telerik.$(window).height();
                        $("#gvPurchaseRequisition,gvformitem").height(browserHeight - 30);
                    });

        </script>

    </telerik:radcodeblock>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:radscriptmanager id="radscript1" runat="server"></telerik:radscriptmanager>
        <telerik:radskinmanager id="radskin1" runat="server"></telerik:radskinmanager>
      
            <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="MenuStockItem_TabStripCommand" TabStrip="true"></eluc:TabStrip>
       

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div id="search">
            <table>
                <tr>
                    <td>
                       <b> <telerik:radlabel id="lblStockType" runat="server" text="Stock Type"></telerik:radlabel> </b>
                    </td>
                    <td>
                        <telerik:raddropdownlist id="ddlStockType" runat="server" visible="true" autopostback="true" ontextchanged="ddlStockType_TextChanged">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="Dummy" />
                                <telerik:DropDownListItem Text="Store" Value="STORE" />
                                <telerik:DropDownListItem Text="Spare" Value="SPARE" />
                                <telerik:DropDownListItem Text="Service" Value="SERVICE" />
                            </Items>

                        </telerik:raddropdownlist>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divGrid" style="position: relative;">

            <telerik:radgrid rendermode="Lightweight" id="gvPurchaseRequisition" runat="server" allowcustompaging="true" allowsorting="true" allowpaging="true"
                cellspacing="0" gridlines="None"  
                onneeddatasource="gvPurchaseRequisition_NeedDataSource" 
                onitemdatabound="gvPurchaseRequisition_ItemDataBound1"
                onitemcommand="gvPurchaseRequisition_ItemCommand" enableheadercontextmenu="true" groupingenabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDFORMNO" TableLayout="Fixed" Height="10px">
                   
                       <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />

                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Form No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="VIEW" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Stock Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Actual Cost">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                          
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDACTUALTOTAL")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                           
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFORMSTATUS") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vendor">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cost Classification">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                            <ItemTemplate>
                                <telerik:RadDropDownList ID="ddlCostClassification" runat="server" AppendDataBoundItems="true">
                                </telerik:RadDropDownList>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:radgrid>
        </div>
        <br />
        <br />
        <b>Additional Purchase Items</b>
        <div id="divGrid1" style="position: relative;">
            <%--  <asp:GridView ID="gvformitem" runat="server" AutoGenerateColumns="False" CellPadding="3"
                Font-Size="11px" OnRowCommand="gvformitem_RowCommand" OnRowDataBound="gvformitem_ItemDataBound"
                OnRowEditing="gvformitem_RowEditing" ShowHeader="true" Width="100%" EnableViewState="false"
                OnRowCancelingEdit="gvformitem_RowCancelingEdit" OnRowDeleting="gvformitem_RowDeleting"
                ShowFooter="true" OnRowUpdating="gvformitem_RowUpdating">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>--%>
           
                 <telerik:RadGrid ID="gvformitem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                         Width="100%" CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                         CellSpacing="0" GridLines="None"  OnNeedDataSource="gvformitem_NeedDataSource"
                         GroupingEnabled="false" EnableHeaderContextMenu="true"                        
                         OnItemCommand= "gvformitem_ItemCommand"
                        OnItemDataBound="gvformitem_ItemDataBound1"
                     OnUpdateCommand="gvformitem_UpdateCommand">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                   DataKeyNames="FLDFORMID" TableLayout="Fixed" Height="10px">
                    
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />

                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Title">
                            <ItemStyle wrap="False" horizontalalign="Left"></ItemStyle>
                            
                            <ItemTemplate>
                            <telerik:RadLabel ID="lblFormid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFormTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTITLE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <telerik:RadLabel ID="lblFormid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtFormTitleEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTITLE") %>'
                                CssClass="gridinput_mandatory" Width="90%"></telerik:RadTextBox>
                        </EditItemTemplate>
                            <FooterTemplate>
                            <telerik:RadTextBox ID="txtFormTitleAdd" runat="server" CssClass="gridinput_mandatory" Width="90%"></telerik:RadTextBox>
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Date">
                            <ItemStyle wrap="False" horizontalalign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                            <telerik:RadLabel ID="lblOrderedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <eluc:Date ID="txtOrderedDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE") %>'
                                CssClass="gridinput_mandatory" />
                        </EditItemTemplate>
                            <FooterTemplate>
                            <eluc:Date ID="txtOrderedDateAdd" runat="server" CssClass="gridinput_mandatory" />
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <ItemStyle wrap="False" horizontalalign="Right"></ItemStyle>
                            <ItemTemplate>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>'></telerik:RadLabel>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <eluc:Currency ID="ucCurrencyEdit" runat="server" CssClass="gridinput_mandatory" Width="90%" AppendDataBoundItems="true" CurrencyList="<%# PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true)%>" />
                        </EditItemTemplate>
                            <FooterTemplate>
                            <eluc:Currency ID="ucCurrency" runat="server" CssClass="gridinput_mandatory" Width="90%" AppendDataBoundItems="true" CurrencyList="<%# PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true)%>" />
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Actual Cost">
                            <ItemStyle wrap="False" horizontalalign="Right"></ItemStyle>
                          
                            <ItemTemplate>
                            <telerik:RadLabel ID="lblActualTotal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTUALTOTAL")%>'></telerik:RadLabel>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <eluc:Number ID="txtActualCostEdit" runat="server" CssClass="gridinput_mandatory" Width="90%" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTUALTOTAL")%>' />
                        </EditItemTemplate>
                            <FooterTemplate>
                            <eluc:Number ID="txtActualCostAdd" runat="server" CssClass="gridinput_mandatory" Width="90%" />
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vendor">
                            <ItemStyle wrap="False" horizontalalign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                            <telerik:RadLabel ID="lblVendorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <span id="spnPickListMakerEdit">
                                <telerik:RadTextBox ID="txtVendorCodeEdit" runat="server" CssClass="input_mandatory"
                                    ReadOnly="false" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtVenderNameEdit" runat="server" CssClass="input_mandatory"
                                    ReadOnly="false" Width="180px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>'></telerik:RadTextBox>
                                <asp:LinkButton id="ImgSupplierPickList" runat="server"  style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" >
                                    <span class="icon"><i class="fas fa-tasks"></i></span>

                                </asp:LinkButton>
                                <telerik:RadTextBox ID="txtVendorIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORID") %>'></telerik:RadTextBox>
                            </span>
                        </EditItemTemplate>
                            <FooterTemplate>
                            <span id="spnPickListMaker">
                                <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input_mandatory"
                                    ReadOnly="false" Width="60px"></telerik:RadTextBox>
                                <br />
                                <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input_mandatory"
                                    ReadOnly="false" Width="100px"></telerik:RadTextBox> &nbsp
                                <asp:LinkButton id="ImgSupplierPickListadd" runat="server"
                                   style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" >
                                    <span class="icon"><i class="fas fa-tasks"></i></span>

                                </asp:LinkButton>
                                <telerik:RadTextBox ID="txtVendorId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            </span>
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cost Classification">
                            <ItemStyle wrap="False" horizontalalign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                            <telerik:RadLabel ID="lblCostClassification" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASSIFICATIONNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <telerik:RadDropDownList ID="ddlCostClassificationEdit" runat="server" AppendDataBoundItems="true" Width="90%">
                                
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                            <FooterTemplate>
                            <telerik:RadDropDownList ID="ddlCostClassification" runat="server" AppendDataBoundItems="true" Width="90%"
                                DataSource="<%# PhoenixDryDockMultiSpec.ListDryDockMultiSpec(10)%>" DataTextField="FLDNAME" DataValueField="FLDMULTISPECID">
                                
                            </telerik:RadDropDownList>
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle horizontalalign="Center" verticalalign="Middle" width="75px"></HeaderStyle>
                            
                            <ItemStyle wrap="False" horizontalalign="Center" width="100px"></ItemStyle>
                            <ItemTemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompEdit"
                                ToolTip="Edit">
                                 <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:LinkButton Width="20px" Height="20px"  runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCompDelete"
                                ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompSave"
                                ToolTip="Save">
                                <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" alt=""/>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel" 
                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompCancel"
                                ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                            <footerstyle horizontalalign="Center" />
                            <FooterTemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompAdd"
                                ToolTip="Add New">
                                <span class="icon"><i class ="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:radgrid>
        </div>

        <%-- <Triggers>
            <asp:PostBackTrigger ControlID="gvPurchaseRequisition" />
        </Triggers>--%>
    </form>
</body>
</html>
