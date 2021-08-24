<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationLineItem.aspx.cs" Inherits="PurchaseQuotationLineItem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucUnit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation Line Items</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseQuotationVendorItems" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="Title1" ShowMenu="false"></eluc:Title>
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuSaveDetails" runat="server" TabStrip="false" OnTabStripCommand="MenuSaveDetails_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <br clear="all" />
                    <div id="divField" style="position: relative; z-index: 2; margin-top: 0px;">
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td width="30%">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPartNumber" runat="server" BorderWidth="1px" CssClass="readonlytextbox"
                                                    Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPartName" runat="server" BorderWidth="1px" Width="210px" CssClass="readonlytextbox"
                                                    Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMakerRef" runat="server" Text="Maker Ref."></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMakerReference" runat="server" Width="210px" Enabled="false" CssClass="readonlytextbox"
                                                    ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblDrawingNo" runat="server" Text="Drawing No"></asp:Literal>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDrawingNo" Enabled="false" runat="server" Width="90px" CssClass="readonlytextbox"
                                                    ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblPosition" runat="server" Text="Position"></asp:Literal>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPosition" Enabled="false" runat="server" Width="90px" CssClass="readonlytextbox"
                                                    ReadOnly="true"></asp:TextBox>
                                                <asp:TextBox ID="txtPartId" runat="server" Width="60px" Visible="false" CssClass="input"
                                                    ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCurrency" CssClass="readonlytextbox" Enabled="false" runat="server"></asp:TextBox>
                                                <%--<eluc:UserControlCurrency ID="ucCurrency" CssClass="readonlytextbox" AppendDataBoundItems="true" Enabled="false" runat="server" />--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblContent" runat="server" Text="Content"></asp:Literal>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContent" runat="server" BorderWidth="1px" Width="90px" CssClass="readonlytextbox"
                                                    Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStatus" runat="server" BorderWidth="1px" Width="90px" CssClass="readonlytextbox"
                                                    Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                                    &nbsp; 
                               <eluc:Custom ID="txtRemarks" runat="server" Width="99%" Height="150px" PictureButton="false" textonly="true"
                                   DesgMode="true" HTMLMode="true" PrevMode="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuRegistersStockItem" runat="server" OnTabStripCommand="MenuRegistersStockItem_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 1; width: 100%">
                        <asp:GridView ID="gvVendorItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvVendorItem_RowCommand" OnRowDataBound="gvVendorItem_RowDataBound"
                            EnableViewState="False" OnRowEditing="gvVendorItem_RowEditing"
                            AllowSorting="true" OnSorting="gvVendorItem_Sorting" OnRowUpdating="gvVendorItem_RowUpdating"
                            OnRowCancelingEdit="gvVendorItem_RowCancelingEdit" ShowHeader="true"
                            OnSelectedIndexChanging="gvVendorItem_SelectedIndexChanging">

                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblImageHeader" runat="server"> </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" BackColor="Transparent" ForeColor="Transparent" />
                                         <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                        <asp:Label ID="lblIsNotes" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLGNOTES") %>'></asp:Label>
                                        <asp:ImageButton ID="imgContractFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                        <asp:Label ID="lblIsContract" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTEXISTS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblSNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDSERIALNO"
                                            ForeColor="White">S.No</asp:LinkButton>
                                        <img id="FLDSERIALNO" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuotationLineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONLINEID") %>'></asp:Label>
                                        <asp:Label ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></asp:Label>
                                        <asp:Label ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                        <asp:Label ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                                        <asp:Label ID="lblLineid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="StockItem Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkStockItemCode" runat="server" CommandName="SELECT"
                                            CommandArgument='<%# Container.DataItemIndex %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton><br />
                                        <asp:Label ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Maker Reference">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblMakerReferenceHeader" runat="server">Maker Ref. </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMakerReference" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="ROB Quantity">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblROBQuantityHeader" runat="server">ROB</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblROBQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBQUANTITY","{0:n0}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblOrderQty" runat="server" Text="Order Qty"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unit">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblUnit" runat="server" Text="Unit"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>

                                        <asp:Label ID="lblunit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                                        <asp:Label ID="lblunitid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblItemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTID") %>'></asp:Label>
                                        <eluc:ucUnit ID="ucUnit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quoted Qty">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblQuotedQty" runat="server" Text="Quoted Qty"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtQuantityEdit" runat="server" CssClass="gridinput_mandatory" DefaultZero="true" IsPositive="true"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' mask="99999" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Price">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblPriceHeader" runat="server" CommandName="Sort" CommandArgument="FLDQUOTEDPRICE"
                                            ForeColor="White">Price</asp:LinkButton>
                                        <img id="FLDQUOTEDPRICE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n3}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtQuotedPriceEdit" runat="server" Width="120px" CssClass="gridinput_mandatory" IsPositive="true"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n3}") %>' DecimalPlace="3" MaxLength="16" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Discount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDiscountHeader" runat="server">Disc.(%)</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="txtDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtDiscountEdit" runat="server" CssClass="gridinput" IsPositive="true"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>' mask="99.99" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblTotalPrice" runat="server" Text="Total Price"></asp:Literal>
                                        &nbsp;(<%= strTotalAmount %>)
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="txtTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOTALPRICE","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Del Time">
                                    <ItemStyle Wrap="true" HorizontalAlign="Right" Width="30px"></ItemStyle>
                                    <HeaderStyle Wrap="true" Width="30px" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblDelTimeDays" runat="server" Text="Del. Time (Days)"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliveryTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME","{0:n0}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtDeliveryTimeEdit" runat="server" Width="30px" CssClass="gridinput" DefaultZero="false" IsInteger="true"
                                            MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME","{0:n0}") %>' />
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
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                        <img id="Img9" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Audit Trail" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                            CommandName="AUDITTRAIL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAudit"
                                            ToolTip="Audit Trail"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdUpdate"
                                            ToolTip="Update"></asp:ImageButton>
                                        <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
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
                    <div id="divPage" style="position: relative; z-index: 0;">
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
                                <td width="20px">&nbsp;
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
