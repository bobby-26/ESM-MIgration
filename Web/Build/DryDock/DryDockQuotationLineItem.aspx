<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockQuotationLineItem.aspx.cs"
    Inherits="DryDockQuotationLineItem" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvQuotationLineItem.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDryDockQuotationLineItem" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadButton ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuQuotationLineItem" runat="server" OnTabStripCommand="QuotationLineItem_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
            </div>
           

                <telerik:RadGrid RenderMode="Lightweight" ID="gvQuotationLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    
                    OnNeedDataSource="gvQuotationLineItem_NeedDataSource"
                    OnItemCommand="gvQuotationLineItem_ItemCommand"
                    OnItemDataBound="gvQuotationLineItem_ItemDataBound1"
                    OnUpdateCommand="gvQuotationLineItem_UpdateCommand"   EnableHeaderContextMenu="true"    GroupingEnabled="false"  >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDJOBID" TableLayout="Fixed" Height="10px">
                       
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
                            <telerik:GridTemplateColumn HeaderText="Number">
                                <HeaderStyle Width="75px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Description">
                                  <HeaderStyle Width="175px" />
                                <itemstyle horizontalalign="Left"></itemstyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQuotationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuotationLineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONLINEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblOrderLineid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblOrderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblJobDetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblQuotationIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuotationLineIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONLINEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblOrderLineidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblOrderidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblJobDetailidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbldtkeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Component count">
                                  <HeaderStyle Width="60px" />
                                <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTCOUNT")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Order Quantity">
                                 <HeaderStyle Width="60px" />
                                <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOrderQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtOrderQuantityEdit" runat="server" CssClass="input txtNumber"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERQUANTITY") %>' Width="50px"
                                        MaxLength="6" ></eluc:Number>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Quoted Quantity">
                                 <HeaderStyle Width="60px" />
                                <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <eluc:Number ID="txtQQuantityEdit" runat="server" CssClass="input txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDQUANTITY") %>'
                                        Width="120px" MaxLength="6"></eluc:Number>
                                </EditItemTemplate>--%>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit">
                                 <HeaderStyle Width="60px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit Price">
                                <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                               <HeaderStyle Width="60px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblUnitPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Decimal ID="txtUnitPriceEdit" runat="server" Width="50px" CssClass="gridinput"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE","{0:n2}") %>' DecimalDigits="3" MinValue="0"
                                         />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Gross Price">
                                 <HeaderStyle Width="60px" />
                                <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                             
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblGrossPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROSSPRICE","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Decimal ID="txtGrossPriceEdit" runat="server" Width="50px" CssClass="gridinput"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROSSPRICE","{0:n4}") %>' 
                                         />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Disc.(%)">
                                 <HeaderStyle Width="60px" />
                                <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="txtDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Decimal ID="txtDiscountEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'
                                        MinValue="0" MaxValue="100" DecimalDigits="2" Width="50px" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Net Price">
                                 <HeaderStyle Width="60px" />
                                <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="txtNetPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNETPRICE","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Decimal ID="txtNetPriceEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNETPRICE","{0:n2}") %>'
                                         Width="50px" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                 <HeaderStyle Width="80px" />
                                <itemstyle horizontalalign="Left"></itemstyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtRemarks" runat="server"  Width="60px" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                             <%-- <telerik:GridTemplateColumn HeaderText="Percentage of Completion">
                                  <HeaderStyle Width="80px" />
                                   <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                                    <ItemTemplate>
                                    <telerik:RadLabel ID="radlblpercentagecompletion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDPERCENTAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:MaskNumber ID="radtbpercentagecompletion" runat="server" Width="55px" MaskText="###" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDPERCENTAGE") %>'
                            MaxLength="3"  />
                                </EditItemTemplate>
                                  </telerik:GridTemplateColumn>--%>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                 <HeaderStyle Width="100px" />
                                <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                
                                <itemstyle wrap="False" horizontalalign="Center" width="50px"></itemstyle>
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit">
                                        
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete" 
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete">
                                         <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save">
                                         <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel" 
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
      

            <%-- </div>--%>
     

    </form>
</body>
</html>
