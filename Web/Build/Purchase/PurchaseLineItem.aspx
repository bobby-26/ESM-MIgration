<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseLineItem.aspx.cs" Inherits="PurchaseLineItem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="../UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Line Items</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
            <ContentTemplate>

                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Order Line Items" ShowMenu="false"></eluc:Title>
                        <%--<asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"></eluc:TabStrip>
                </div>

                <table cellpadding="1" cellspacing="1" width="96%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFormNumber" runat="server" Text="Form Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblFormName" runat="server" Text="Form Title"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormName" runat="server" Width="90px" CssClass="readonlytextbox" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStatus" runat="server" Width="90px" CssClass="readonlytextbox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblmakerRef" runat="server" Text="Maker Reference"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMakerReference" runat="server" Width="120px" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPartName" runat="server" Width="300px" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblReceiptstatus" runat="server" Text="Receipt status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:UserControlQuick ID="ucReciptstatus" AppendDataBoundItems="true" CssClass="input"
                                runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPrice" runat="server"> Price </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrice" runat="server" Width="90px" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRequestedQty" runat="server" Text="Requested Qty"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtRequestedQty" runat="server" Width="90px" CssClass="readonlytextbox" ReadOnly="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblOrderQty" runat="server" Text="Order Qty"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtOrderQty" runat="server" Width="90px" CssClass="input" IsPositive="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblUnit" runat="server" Text="Unit"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Unit ID="ucUnit" AppendDataBoundItems="true" CssClass="input_mandatory" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblReceivedQty" runat="server" Text="Received Qty"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRecivedQty" runat="server" MaxLength="22" Width="90px" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCancelledQty" runat="server" Text="Cancelled Qty"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtCanceledQty" runat="server" Width="90px" CssClass="readonlytextbox" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <font color="blue">
                            <asp:Literal ID="lblNoteOrderQtycannotbealteredaftertheapproval" runat="server" Text="Note: 'Order Qty' cannot be altered after the approval."></asp:Literal>
                                
                            </font>
                        </td>
                    </tr>
                </table>

                <div class="navSelect* " style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
                </div>

                <div id="divGrid" style="position: relative; z-index: 1; width: 96%;">
                    <asp:GridView ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvLineItem_RowCommand" OnRowDataBound="gvLineItem_RowDataBound"
                        OnRowCancelingEdit="gvLineItem_RowCancelingEdit" OnRowDeleting="gvLineItem_RowDeleting"
                        OnRowEditing="gvLineItem_RowEditing" OnRowUpdating="gvLineItem_RowUpdating"
                        AllowSorting="True" EnableViewState="false" OnSorting="gvLineItem_Sorting" DataKeyNames="FLDORDERLINEID">

                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />

                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSerialNoHeader" runat="server">S. No.</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></asp:Label>
                                    <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMakerReferenceHeader" runat="server">Maker Reference </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMakerReference" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREF") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblProductCodeHeader" runat="server">Drawing No</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAWINGNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStockItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                    <asp:Label ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></asp:Label>
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

                            <asp:TemplateField HeaderText="Requested Quantity">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQuantityHeader" runat="server">Requested Qty</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblrequestedQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n0}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PreferredVendor">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUnitHeader" runat="server">Unit</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPrefferedVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblorderQtyHeader" runat="server">Order Qty</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblReceiptStatusHeader" runat="server">Receipt Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReceiptStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRemarksHeader" runat="server">
                                    Received Remarks
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblChkRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHKREMARKS") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
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
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Details" ImageUrl="<%$ PhoenixTheme:images/text-detail.png %>"
                                        CommandName="DETAILS" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDetails"
                                        ToolTip="Details"></asp:ImageButton>
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENTS" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAttachment"
                                            ToolTip="Attachment"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdNoAttachment"
                                            ToolTip="No Attachment"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="div2" style="position: relative;">
                    <table width="100%" border="0" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/detail-flag.png %>" /><asp:Label
                                    ID="lblMessage" runat="server" ForeColor="Red"> Line item Details. </asp:Label>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgContractExists" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/contract-exist.png %>" /><asp:Label
                                    ID="Label1" runat="server" ForeColor="Red"> Rate Contract exists for the Item. </asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage" style="position: relative; z-index: 0">
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
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev  </asp:LinkButton>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
