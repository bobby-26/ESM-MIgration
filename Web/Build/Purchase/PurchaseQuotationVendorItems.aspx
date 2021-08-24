<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationVendorItems.aspx.cs"
    Inherits="PurchaseQuotationVendorItems" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register Src="../UserControls/UserControlCurrency.ascx" TagName="UserControlCurrency" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucUnit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation Line Items</title>
    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />


        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/jquery-1.12.4.min.js"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("rgvLine");
            var contentPane = splitter.getPaneById("listPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 150) + "px";
            console.log(grid._gridDataDiv.style.height, contentPane._contentElement.offsetHeight);
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmPurchaseQuotationVendorItems" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvLine">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="edittbl" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="lnkStockItemCode">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="edittbl" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuQuotationLineItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuQuotationLineItem" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="edittbl" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="menuSaveDetails">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="menuSaveDetails" />
                        <telerik:AjaxUpdatedControl ControlID="edittbl" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>

        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
            
            <eluc:TabStrip ID="MenuQuotationLineItem" runat="server" OnTabStripCommand="MenuQuotationLineItem_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                <telerik:RadPane ID="editPane" runat="server" Scrolling="None">
                    <eluc:TabStrip ID="menuSaveDetails" runat="server" TabStrip="false" OnTabStripCommand="menuSaveDetails_TabStripCommand"></eluc:TabStrip>

                    <table cellpadding="1" cellspacing="1" width="100%" runat="server" id="edittbl">
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtPartNumber" runat="server" Width="120px"
                                    Enabled="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text="Name"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtPartName" runat="server" Width="240px" 
                                    Enabled="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblMakerRef" runat="server" Text="Maker Ref."></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtMakerReference" runat="server" Width="240px" 
                                    ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblDrawingNo" runat="server" Text="Drawing No"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDrawingNo" runat="server" Width="120px" 
                                    ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPosition" runat="server" Text="Position"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtPosition" runat="server" Width="240px" 
                                    ReadOnly="true">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtPartId" runat="server" Width="0px" Visible="false" 
                                    ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>

                            </td>
                            <td>
                                <eluc:UserControlCurrency ID="ucCurrency" AppendDataBoundItems="true" Enabled="false" runat="server" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblContent" runat="server" Text="Content"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtContent" runat="server" Width="120px" 
                                    Enabled="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtStatus" runat="server" Width="120px" 
                                    Enabled="false">
                                </telerik:RadTextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <%--<asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>--%>
                                <%--<eluc:Custom ID="txtRemarks" runat="server" Height="150px" PictureButton="false" TextOnly="true"
                                            DesgMode="true" HTMLMode="true" PrevMode="true" />--%>
                                <%--<telerik:RadTextBox RenderMode="Lightweight" ID="txtRemarks" runat="server" Width="100%" Rows="5" TextMode="MultiLine" EmptyMessage="Type Remarks here"></telerik:RadTextBox>--%>
                                <telerik:RadEditor ID="txtRemarks" runat="server" Width="100%" Height="400px" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                                    <ImageManager ViewPaths="~/Attachments/Purchase/Editor"
                                            UploadPaths="~/Attachments/Purchase/Editor"
                                            EnableAsyncUpload="true"></ImageManager>
                                </telerik:RadEditor>
                            </td>
                        </tr>
                    </table>
                    <%--</telerik:RadAjaxPanel>--%>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
                <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                    <eluc:TabStrip ID="MenuRegistersStockItem" runat="server" OnTabStripCommand="RegistersStockItem_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="rgvLine" DecoratedControls="All" EnableRoundedCorners="true" />
                    <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnDeleteCommand="rgvLine_DeleteCommand" OnSortCommand="rgvLine_SortCommand"
                        OnNeedDataSource="rgvLine_NeedDataSource" OnInsertCommand="rgvLine_InsertCommand" OnEditCommand="rgvLine_EditCommand"
                        OnItemDataBound="rgvLine_ItemDataBound" OnItemCommand="rgvLine_ItemCommand" OnPreRender="rgvLine_PreRender" OnUpdateCommand="rgvLine_UpdateCommand">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDVESSELID,FLDPARTID,FLDORDERLINEID,FLDQUOTATIONID,FLDQUOTATIONLINEID" TableLayout="Fixed">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="" UniqueName="CHECKBOX" AllowSorting="false">
                                    <ItemStyle Width="40px" />
                                    <HeaderStyle Width="40px" />
                                    <ItemTemplate>
                                        <telerik:RadCheckBox ID="chkSelect" Checked="false" runat="server"></telerik:RadCheckBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="" UniqueName="FLAG" AllowSorting="false">
                                    <ItemStyle Width="40px" />
                                    <HeaderStyle Width="40px" />
                                    <ItemTemplate>
                                        <asp:ImageButton RenderMode="Lightweight" ID="imgContractFlag" runat="server" Enabled="false" ImageUrl="<% $ PhoenixTheme:images/contract-exist.png %>" CommandName="CONTRACTEXISTS" />
                                        <asp:ImageButton RenderMode="Lightweight" ID="imgFlag" runat="server" Enabled="false" ImageUrl="<% $PhoenixTheme:images/detail-flag.png%>" />
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblIsNotes" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLGNOTES") %>'></telerik:RadLabel>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblIsContract" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTEXISTS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="S.No" UniqueName="SNO" AllowSorting="true" SortExpression="FLDROWNUMBER">
                                    <ItemStyle Width="40px" />
                                    <HeaderStyle Width="40px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER" AllowSorting="false">
                                    <ItemStyle Width="90px" />
                                    <HeaderStyle Width="90px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPartId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                                    <ItemStyle Width="240px" />
                                    <HeaderStyle Width="240px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkStockItemCode" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                            Text='<%# DataBinder.Eval(Container, "DataItem.FLDNAME")%>'> </asp:LinkButton><br />
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Maker Ref" UniqueName="MAKERREF" AllowSorting="false">
                                    <ItemStyle Width="120px" />
                                    <HeaderStyle Width="120px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblMakerReference" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="ROB" UniqueName="ROB" AllowSorting="false">
                                    <ItemStyle Width="60px" HorizontalAlign="Right"/>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblROBQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Order Qty" UniqueName="QTY" AllowSorting="false">
                                    <ItemStyle Width="60px" HorizontalAlign="Right"/>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UNIT" AllowSorting="false">
                                    <ItemStyle Width="90px" />
                                    <HeaderStyle Width="90px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblunitid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:ucUnit ID="ucUnit" runat="server" Width="100%" AppendDataBoundItems="true" SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Quoted Qty" UniqueName="QUOTEDQTY" AllowSorting="false">
                                    <ItemStyle Width="70px" HorizontalAlign="Right"/>
                                    <HeaderStyle Width="70px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Numeber ID="txtQuantityEdit" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' 
                                            DecimalDigits="2" InterceptArrowKeys="false" InterceptMouseWheel="false" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Price" UniqueName="PRICE" AllowSorting="true" SortExpression="FLDQUOTEDPRICE">
                                    <ItemStyle Width="70px" HorizontalAlign="Right"/>
                                    <HeaderStyle Width="70px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n3}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Numeber ID="txtQuotedPriceEdit" runat="server" Width="100%" InterceptArrowKeys="false" InterceptMouseWheel="false" DecimalDigits="3"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n3}") %>' MaxLength="16" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Price (USD)" UniqueName="PRICEUSD" AllowSorting="false">
                                    <ItemStyle Width="60px" HorizontalAlign="Right"/>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotedPriceUSD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICEUSD","{0:n3}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Discount (%)" UniqueName="Discount" AllowSorting="false">
                                    <ItemStyle Width="60px" HorizontalAlign="Right"/>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="txtDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Numeber ID="txtDiscountEdit" runat="server" Width="100%" DecimalDigits="0" InterceptArrowKeys="false" InterceptMouseWheel="false" MaxValue="100"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Total" UniqueName="TOTAL" AllowSorting="true" SortExpression="TOTALPRICE">
                                    <ItemStyle Width="60px" HorizontalAlign="Right"/>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="txtTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOTALPRICE","{0:n2}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Item Type" UniqueName="ITEMTYPE" AllowSorting="false">
                                    <ItemStyle Width="1000px" HorizontalAlign="Left"/>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMTYPENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Hard ID="ddlType" runat="server" AppendDataBoundItems="true" CssClass="input" HardTypeCode="244" Width="90px" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDITEMTYPE") %>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Del Time" UniqueName="DELTIME" AllowSorting="false">
                                    <ItemStyle Width="60px" HorizontalAlign="Right" />
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDeliveryTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Numeber ID="txtDeliveryTimeEdit" runat="server" Type="Number" Width="100%" InterceptArrowKeys="false" InterceptMouseWheel="false" DecimalDigits="0"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME","{0:n0}") %>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action">
                                    <ItemStyle Width="90px" />
                                    <HeaderStyle Width="90px" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItem%>' ID="cmdEdit"
                                            ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Vendor"
                                            CommandName="VENDOR" CommandArgument='<%# Container.DataItem%>' ID="cmdVendor"
                                            ToolTip="Vendor">
                                                <span class="icon"><i class="fas fa-user-friends"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItem%>' ID="cmdDelete"
                                            ToolTip="Delete">
                                                <span class="icon"><i class="fas fa-trash"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Audit Trail"
                                            CommandArgument='<%# Container.DataItem %>' ID="cmdAudit" CommandName="AUDITTRAIL"
                                            ToolTip="Audit Trail">
                                                <span class="icon"><i class="fas fa-audittrail"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdUpdate"
                                            ToolTip="Update">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataItem%>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times"></i></span>
                                        </asp:LinkButton>
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
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                                PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox"/>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                        </ClientSettings>
                    </telerik:RadGrid>
                    <div id="div2" style="position: relative;">
                        <table width="100%" border="0" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/detail-flag.png %>" /><asp:Label
                                        ID="lblMessage" runat="server" ForeColor="Red"> Vendor Comments </asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgContractExists" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/contract-exist.png %>" /><asp:Label
                                        ID="Label1" runat="server" ForeColor="Red"> Rate Contract exists for the Item. </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </div>
    </form>
</body>
</html>
