<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationCompareBulkSave.aspx.cs" Inherits="PurchaseQuotationCompareBulkSave" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation Compare</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
    <script type="text/javascript" language="javascript">  
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;
        
        window.onload = function()
        {
            UpperBound = parseInt('<%= this.gvVender.Rows.Count %>') - 1;
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
        
        function bulkSaveOfOrderLine()
        {
    	    var args = "function=BulkSaveOrderLineCompareScreen";
        	    
            var count = document.forms[0].length;
            var i = 0;
            var elemName;

            
            //args = args.substring(0, args.length - 1);
            args += "|";
            args += "txtOrderedQuantity=,";

            for (i = 0; i < count; i++) 
            {
                var elm = document.forms[0].elements[i];
                var elmName = elm.id;
                if (elm.type == 'text') 
                {
                    if (elmName.indexOf('txtOrderQty') > 0)
                    {
                        args += elm.value;
                        args += ",";
                    }
                }
            }

            
            //args = args.substring(0, args.length - 1);
            args += "|";
            args += "hidOrderLineId=,";

            count = document.getElementsByName("hidOrderLineId").length;
            for (i = 0; i < count; i++) 
            {
                var elm = document.getElementsByName("hidOrderLineId")[i];
                args += elm.value;
                args += ",";
            }

            //args = args.substring(0, args.length - 1);
            args += "|";
            args += "hidOrderId=";

            var elm = document.getElementsByName("hidOrderId")[0];
            args += elm.value;

            args +="|";
            args += "hidVesselId=";

            var elm = document.getElementsByName("hidVesselId")[0];
            args += elm.value;

            args = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
            document.getElementById('<%= spnErrorMessage.ClientID %>').innerText = args;            
            //document.getElementById('<%= spnErrorMessage.ClientID %>').InnerHtml = "<font color='#ff0000'><b>* " + args + "</b></font><br/>";
            document.getElementById('<%= pnlErrorMessage.ClientID %>').style.display = 'block';
            //return;
            
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

            o.contentDocument.getElementById('cmdHiddenSubmit').click();
       }
    </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseQuotationCompare" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Order Line Item Details" ShowMenu="false">
                        </eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="menuSaveDetails" runat="server" TabStrip="false" OnTabStripCommand="menuSaveDetails_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:Panel runat="server" ID="pnlErrorMessage" HorizontalAlign="Justify" BorderColor="Black"
                    BorderStyle="Double" Width="360px">
                    <table width="100%">
                        <tr>
                            <td valign="top" colspan="2">
                                <font size="2"><b>
                                    <asp:Literal ID="lblMessage" runat="server" Text="Message"></asp:Literal></b></font>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <span runat="server" id="spnHeaderMessage"></span>
                                <br />
                                <span runat="server" id="spnErrorMessage"></span>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button runat="server" ID="cmdClose" Text="Close" OnClientClick="close()" Width="120px"
                                    CssClass="input" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvVender" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvVender_RowCommand" OnRowDataBound="gvVender_ItemDataBound"
                        OnRowEditing="gvVender_RowEditing" OnRowCancelingEdit="gvVender_RowCancelingEdit"
                        OnRowCreated="gvVender_RowCreated" ShowHeader="true" EnableViewState="false"
                        ShowFooter="true">
                        <FooterStyle CssClass="datagrid_footerstyle" />
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSNo" runat="server" Text="S.No"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVenderHeader" runat="server">Item
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Literal ID="lblTotal" runat="server" Text="Total"></asp:Literal>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StockItem Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSendDateHeader" runat="server">Name
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSendDateCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Maker">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRecivedDateHeader" runat="server">Unit
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRecivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                                    <input type="hidden" name="hidOrderLineId" id="hidOrderLineId" value='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>' />
                                    <input type="hidden" name="hidVesselId" id="hidVesselId" value='<%# Filter.CurrentPurchaseVesselSelection %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQuantityHeader" runat="server">Requested Qty
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n2}") %>'
                                        CssClass="txtNumber"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Wanted">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPriceHeader" runat="server">Approved Qty
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <eluc:Decimal ID="txtOrderQty" runat="server" Width="90px" Mask="99999" CssClass="gridinput_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
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
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdUpdate"
                                        ToolTip="Update"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <b>All Figures in USD.</b>
                </div>
            </div>
            <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvVender" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
