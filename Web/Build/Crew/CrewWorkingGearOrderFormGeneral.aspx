<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkingGearOrderFormGeneral.aspx.cs"
    Inherits="CrewWorkingGearOrderFormGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddr" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Working Gear Order Form</title>
       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Request" ShowMenu="<%# Title1.ShowMenu %>">
                    </eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuWorkGearGeneral" runat="server" OnTabStripCommand="MenuWorkGearGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGeneral" runat="server" style="position: relative; z-index: 2; width: 100%;">
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblOrderNo" runat="server" Text="Order No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblOrderDate" runat="server" Text="Order Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtOrderDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <asp:Literal ID="lblSupplierName" runat="server" Text="Supplier Name"></asp:Literal>
                            </td>
                            <td>
                                <eluc:MultiAddr ID="ucMultiAddr" AddressType="130,131,132" runat="server" CssClass="input_mandatory"
                                    Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Currency ID="ddlCurrency" AppendDataBoundItems="true" CssClass="input_mandatory" runat="server"
                                    ActiveCurrency="false" AutoPostBack="true" OnTextChangedEvent="CurrencySelection" />
                            </td>
                            <%--<td>
                                <asp:Literal ID="lblExchangeRate" runat="server" Text="Exchange Rate (1 INR = )"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtExchangeRate" runat="server" CssClass="input" DefaultZero="false"
                                    Width="90px" DecimalPlace="6" />
                            </td>--%>
                            <td>
                                <asp:Literal ID="lblRequisitionfortheZone" runat="server" Text="Requisition for the Zone"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Zone ID="ucZone" AppendDataBoundItems="true" AppendItemAllZone="true" CssClass="input_mandatory"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br style="clear: both" />
                <h4>
                    <asp:Literal ID="lblRequisitionItemDetails" runat="server" Text="Requisition Item Details"></asp:Literal></h4>
                <div id="divItems" runat="server">
                    <div class="navSelect" style="position: relative; z-index: 0; width: 15px">
                        <eluc:TabStrip ID="MenuWorkGearItem" runat="server" OnTabStripCommand="MenuWorkGearItem_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvWorkGearItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvWorkGearItem_RowDataBound" OnRowEditing="gvWorkGearItem_RowEditing"
                            OnRowCancelingEdit="gvWorkGearItem_RowCancelingEdit" OnRowUpdating="gvWorkGearItem_RowUpdating"
                            OnRowDeleting="gvWorkGearItem_RowDeleting" ShowHeader="true" ShowFooter="true"
                            EnableViewState="false" DataKeyNames="FLDORDERLINEID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDWORKINGGEARITEMNAME"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="Size">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDSIZENAME"]%>
                                    </ItemTemplate>
                                      <EditItemTemplate>
                                        <asp:Label ID="lblSizeid" runat="server"  Width="90px" Visible="false"
                                            Text='<%#((DataRowView)Container.DataItem)["FLDSIZEID"] %>' />
                                          <asp:Label ID="lblSizeName" runat="server"  Width="90px"
                                            Text='<%#((DataRowView)Container.DataItem)["FLDSIZENAME"] %>' />
                                    </EditItemTemplate>
                                    
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Unit">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDUNITNAME"] %>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDQUANTITY"] %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtQuantity" runat="server" CssClass="input_mandatory" Width="90px"
                                            Text='<%#((DataRowView)Container.DataItem)["FLDQUANTITY"] %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Price">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDUNITPRICE"] %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtUnitPrice" runat="server" CssClass="input_mandatory" Width="90px"
                                            Text='<%#((DataRowView)Container.DataItem)["FLDUNITPRICE"] %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        Total Amount as per Invoice (Invoice Currency)
                                        <br />
                                        Less: Discount (Invoice Currency)
                                        <br />
                                        Net Amount (Invoice Currency)<br />
                                        Net Amount (INR)
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Price">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDTOTALPRICE"] %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
                                </td>
                                <td width="20px">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br style="clear: both" />
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblDiscount" runat="server" Text="Discount %"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtDiscount" runat="server" CssClass="input" DefaultZero="false"
                                    Width="90px" />
                            </td>
                            <td>
                                <asp:Literal ID="lblReceivedDate" runat="server" Text="Received Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtReceivedDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Literal ID="lblTotalAmounttothePaid" runat="server" Text="Total Amount to be Paid"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtTotalAmount" runat="server" CssClass="readonlytextbox" DefaultZero="false"
                                    Width="90px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td colspan="7">
                                <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" CssClass="input"
                                    Width="370px">
                                </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
