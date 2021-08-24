<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalPurchaseQuotationVendorBulkSave.aspx.cs" Inherits="PortalPurchaseQuotationVendorBulkSave" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucUnit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation Line Bulk Save </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript" language="javascript">  
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;
        
        window.onload = function()
        {
            UpperBound = parseInt('<%= this.gvVendorItem.Rows.Count %>') - 1;
            LowerBound = 0;
            SelectedRowIndex = -1;        
        }

        function SelectRow(CurrentRow, RowIndex, CellIndex) {
            if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
                return;

            if (CellIndex == null) {
                SelectedRow = CurrentRow;
                SelectedRowIndex = RowIndex;
                return;
            }

            if (SelectedRow != null) {
                try { SelectedRow.cells[CellIndex].getElementsByTagName('INPUT')[0].focus(); } catch (e) { return true; }
            }

            if (CurrentRow != null) {
                try { CurrentRow.cells[CellIndex].getElementsByTagName('INPUT')[0].focus(); } catch (e) { return true; }
            }

            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
            //setTimeout("SelectedRow.focus();",0);

        }

        function SelectSibling(e) {
            var eleminscroll;
            e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;
            if (e.target) eleminscroll = e.target;
            if (e.srcElement) eleminscroll = e.srcElement;

            try {
                if (KeyCode == 40)
                    SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1, eleminscroll.parentNode.cellIndex);
                else if (KeyCode == 38)
                    SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1, eleminscroll.parentNode.cellIndex);
            }
            catch (ex) {
                return true;
            }
            return true;
        }            
            
        function bulkSaveOfQuotationLine()
        {
    	    var args = "function=BulkSaveVendorQuotationLine";
        	    
            var count = document.forms[0].length;
            var i = 0;
            var elemName;

            args += "|";
            args += "txtQuantityEdit=,";
            
            for (i = 0; i < count; i++)
            {
                var elm = document.forms[0].elements[i];
                var elmName = elm.id;
                if (elm.type == 'text') 
                {
                    if (elmName.indexOf('txtQuantityEdit') > 0) 
                    {
                        args += elm.value;
                        args += ",";
                    }
                }
            }

            //args = args.substring(0, args.length - 1);
            args += "|";
            args += "txtQuotedPriceEdit=,";

            for (i = 0; i < count; i++) 
            {
                var elm = document.forms[0].elements[i];
                var elmName = elm.id;
                if (elm.type == 'text') 
                {
                    if (elmName.indexOf('txtQuotedPriceEdit') > 0) 
                    {
                        args += elm.value;
                        args += ",";
                    }
                }
            }

            //args = args.substring(0, args.length - 1);
            args += "|";
            args += "txtDiscountEdit=,";

            for (i = 0; i < count; i++) 
            {
                var elm = document.forms[0].elements[i];
                var elmName = elm.id;
                if (elm.type == 'text') 
                {
                    if (elmName.indexOf('txtDiscountEdit') > 0) 
                    {
                        args += elm.value;
                        args += ",";
                    }
                }
            }
            
            //args = args.substring(0, args.length - 1);    
            args += "|";
            args += "ucUnit=,";

            for (i = 0; i < count; i++) 
            {
                var elm = document.forms[0].elements[i];
                var elmName = elm.id;
                if (elmName.indexOf('ddlUnit') > 0) 
                {
                    args += elm.options[elm.selectedIndex].value; 
                    args += ",";
                }
            }
            
             //args = args.substring(0, args.length - 1);    
            args += "|";
            args += "txtDeliveryEdit=,";

            for (i = 0; i < count; i++) 
            {
                var elm = document.forms[0].elements[i];
                var elmName = elm.id;
                if (elmName.indexOf('txtDeliveryEdit') > 0) 
                {
                    args += elm.value; 
                    args += ",";
                }
            }
            
            //args = args.substring(0, args.length - 1);
            args += "|";
            args += "hidQuotationLineId=,";

            count = document.getElementsByName("hidQuotationLineId").length;
            for (i = 0; i < count; i++) 
            {
                var elm = document.getElementsByName("hidQuotationLineId")[i];
                args += elm.value;
                args += ",";
            }

            //args = args.substring(0, args.length - 1);
            args += "|";
            args += "hidQuotationId=";

            var elm = document.getElementsByName("hidQuotationId")[0];
            args += elm.value;

            args = AjxPost(args, SitePath + "Options/OptionsAuthenticateUser.aspx", null, false);
          
            document.getElementById('<%= pnlErrorMessage.ClientID %>').style.display = 'block';
            
            if(navigator.appName == 'Microsoft Internet Explorer') 
            {
                document.getElementById('<%= spnErrorMessage.ClientID %>').innerText = args;
            }
            else
            {
                document.getElementById('<%= spnErrorMessage.ClientID %>').textContent = args;
            }
            var o;
            if (parent.document.getElementById('filterandsearch') != null)
                o = parent.document.getElementById('filterandsearch');
            if (parent.document.getElementById('fraPhoenixApplication') != null)
                o = parent.document.getElementById('fraPhoenixApplication').contentDocument.getElementById('filterandsearch');

//            var o;
//            
//            if (parent.document.getElementById('filterandsearch') != null)
//                o = parent.document.getElementById('filterandsearch');

            o.contentDocument.getElementById('cmdHiddenSubmit').click();
       }
    </script>
</head>
<body>
    <form id="frmQuotationLineBulkSave" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Quotation Line Item Details" ShowMenu="false"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="menuSaveDetails" runat="server" TabStrip="false" OnTabStripCommand="menuSaveDetails_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <asp:Panel runat="server" ID="pnlErrorMessage" HorizontalAlign="Justify" BorderColor="Black" BorderStyle="Double" Width="360px" >
                    <table width="100%">
                        <tr>
                            <td valign="top" colspan="2">
                                <font size="2"><b>
                                <asp:Literal ID="lblMessage" runat="server" Text="Message"></asp:Literal>
                                </b></font>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <span runat="server" id="spnHeaderMessage"></span><br />
                                <span runat="server" id="spnErrorMessage"></span><br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button runat="server" ID="cmdClose" Text="Close" OnClientClick="close()" Width="120px" CssClass="input"/>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <br clear="all" />
                <font color="blue"><b>
                <asp:Literal ID="lblNote" runat="server" Text="Note:"></asp:Literal></b> 
                <asp:Literal ID="lblFillupallthemandatoryfieldsmarkedin" runat="server" Text="Fill up all the mandatory fields marked in"></asp:Literal>
                 <font color="red"><asp:Literal ID="lblred" runat="server" Text="red"></asp:Literal>
                 </font><asp:Literal ID="lblcolourandthenclickSaveLineitemswhicharepartiallyfilledupwillnotbeupdated" runat="server" Text="colour and then click 'Save'. Line items which are partially filled up will not be updated."></asp:Literal>
                 </font>                
                <div id="divGrid" style="position: relative; z-index: 1" width:100%;">
                    <asp:GridView ID="gvVendorItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCreated="gvVendorItem_RowCreated" OnRowDataBound="gvVendorItem_RowDataBound"
                        EnableViewState="true" AllowSorting="true" ShowHeader="true" >
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSNo" runat="server" Text="S.No"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                             
                            <asp:TemplateField HeaderText="number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPartNumberHeader" runat="server">Part Number </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuotationLineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONLINEID") %>'></asp:Label>
                                     <asp:Label ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></asp:Label>
                                     <asp:Label ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                                     <asp:Label ID="lblLineid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></asp:Label>
                                     <input type="hidden" name="hidQuotationId" id="hidQuotationId" value='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>' />
                                     <input type="hidden" name="hidQuotationLineId" id="hidQuotationLineId" value='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONLINEID") %>' />                                     
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="StockItem Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPartNameHeader" runat="server">Part Name </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<a href="#"><%# DataBinder.Eval(Container,"DataItem.FLDNAME") %></a>--%>
                                     <asp:Label ID="lnkStockItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label><br /> 
                                    <asp:Label ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></asp:Label>                                                            
                                   </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Maker Reference">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMakerReferenceHeader" runat="server">Maker Reference </asp:Label>                                   
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMakerReference" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE")%>'></asp:Label>                                                            
                                   </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Quantity">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblOrdeQuantityHeader" runat="server">Quantity </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></asp:Label>
                                </ItemTemplate>                               
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Unit">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUnitNameHeader" runat="server">Unit </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                   
                                    <asp:Label ID="lblunit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                                    <asp:Label ID="lblunitid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'></asp:Label> 
                                    <asp:Label ID="lblItemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTID") %>'></asp:Label>                              
                                    <asp:DropDownList runat="server" ID="ddlUnit" CssClass="input_mandatory" DataTextField="FLDUNITNAME" DataValueField="FLDUNITID" ></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                                                       
                            <asp:TemplateField HeaderText="Quoted Qty">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQuantityHeader" runat="server">Quoted Qty </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></asp:Label>
                                    <eluc:Numeber ID="txtQuantityEdit" runat="server" Width="90px" CssClass="gridinput_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' Mask="99999" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Price">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPriceHeader" runat="server">Price </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuotedPrice" runat="server" Visible="false" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n3}") %>'></asp:Label>
                                    <eluc:Numeber ID="txtQuotedPriceEdit" runat="server" Width="90px" CssClass="gridinput_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n3}") %>' DecimalPlace="3" MaxLength="16"/>
                                         <%--<asp:TextBox ID="txtQuotedPriceEdit" runat="server" CssClass="gridinput_mandatory" Width="90px" style="text-align: right;"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n3}") %>'></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="txtNumberMask" runat="server" TargetControlID="txtQuotedPriceEdit" 
                                                        Mask="9999999.999" MaskType="Number" InputDirection="RightToLeft">
                                        </ajaxToolkit:MaskedEditExtender>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField HeaderText="Discount">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label  ID="lblDiscountHeader" runat="server">Discount(%)</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtDiscount" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></asp:Label>
                                    <eluc:Numeber ID="txtDiscountEdit" runat="server" Width="90px" CssClass="gridinput"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>' Mask="99.99" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" Width="50px" HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblDeliveryHeader" runat="server">Del.Time(Days)</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <eluc:Numeber ID="txtDeliveryEdit" runat="server" Width="50px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME","{0:n0}") %>' MaxLength="3"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <%--<asp:TemplateField HeaderText="Total">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label  ID="lblTotalHeader" runat="server">Total Price</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOTALPRICE","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvVendorItem" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
