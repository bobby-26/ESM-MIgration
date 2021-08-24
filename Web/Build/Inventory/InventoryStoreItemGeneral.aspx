<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreItemGeneral.aspx.cs" Inherits="InventoryStoreItemGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Item Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuStockItemGeneral" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <br clear="all" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%">
                        <eluc:MaskNumber runat="server" ID="txtNumber" CssClass="input_mandatory" MaxLength="20" MaskText="##.##.##" Enabled="true" Width="223px" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtName" CssClass="input_mandatory" MaxLength="200"
                            Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 10%"></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtPreferredVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtPreferredVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="200" Width="150px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="ImgShowMakerVendor" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListVendor', 'codehelp1', '', 'Common/CommonPickListAddress.aspx?addresstype=130,131,132&framename=ifMoreInfo', true);">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="cmdClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="input"></telerik:RadTextBox>
                        </span>&nbsp;
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblProductCode" runat="server" Text="Product Code"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorReference" runat="server" ReadOnly="false" CssClass="input"
                            MaxLength="200" Width="90px">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr valign="top" style="width: 15%">
                    <td>
                        <telerik:RadLabel ID="lblStockMaximum" runat="server" Text="Stock Maximum"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtStockMaximumQuantity" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="9" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReorderLevel" runat="server" Text="Reorder Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtReOrderLevel" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="2" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblStockMinimum" runat="server" Text="Stock Minimum"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtStockMinimumQuantity" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="9" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReorderQuantity" runat="server" Text="Reorder Quantity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtReOrderQuantity" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="9" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblWantedQuantity" runat="server" Text="Wanted Quantity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtWantedQuantity" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="9" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblUnit" runat="server" Text="Unit"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlUnit ID="ddlStockUnit" runat="server" CssClass="input_mandatory"
                            AppendDataBoundItems="true" Width="223px"/>
                        <asp:LinkButton runat="server" ID="imgUnitMap" ImageAlign="AbsMiddle" Text=".." ToolTip="Measurable Units">
                                <span class="icon"><i class="fas fa-balance-scale"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr>
                     <td>
                        <b><u>
                            <telerik:RadLabel ID="lblTotalStock" runat="server" Text="Total Stock"></telerik:RadLabel>
                        </u></b>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtTotalStock" CssClass="input txtNumber" Type="Number" Enabled="false" Width="223px"
                            MaxLength="9" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblStoreType" runat="server" Text="Store Type"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                        <eluc:UserControlHard ID="ddlStockClass" runat="server" Visible="true" OnTextChangedEvent="ddlStockClass_OnTextChangedEvent" CssClass="input_mandatory"
                            AppendDataBoundItems="true" AutoPostBack="true" Width="223px"/>
                    </td>
                    
                    </tr>
                <tr>
                   
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblStockAveragePrice" runat="server" Text="Stock Average Price"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlStockAveragePriceCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            runat="server" AppendDataBoundItems="true" Enabled="false" Width="80px"/>
                        <eluc:Decimal runat="server" ID="txtStockAveragePrice" CssClass="input txtNumber readonlytextbox" Type="Currency" Width="140px"
                            MaxLength="9" />
                    </td>
                     <td style="width: 15%">
                        <telerik:RadLabel ID="lblsubclass" runat="server" Text="Subclass Type"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlsubclasstype"  AutoPostBack="true" EnableLoadOnDemand="true" Width="223px"  DataTextField="FLDSUBCLASS" DataValueField="FLDSUBCLASSID">
                    </telerik:RadComboBox>

                      </td>
                   
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblLastPurchasedPrice" runat="server" Text="Last Purchased Price"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlLastPurchasedPriceCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            AppendDataBoundItems="true" runat="server" Width="80px"/>
                        <eluc:Decimal runat="server" ID="txtLastPurchasedPrice" CssClass="input txtNumber"
                            MaxLength="9" Width="140px" />
                    </td>
                     <td style="width: 15%">
                        <telerik:RadLabel ID="lblmaterialnumber" runat="server" Text="Material Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtmaterialnumber" runat="server"  CssClass="input"
                            Width="223px">
                        </telerik:RadTextBox>
                    </td>
                   
                   <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastPurchasedDate" runat="server" Text="Last Purchased Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtlastpurchaseDate" runat="server" CssClass="input" Width="90px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblItemisinMarket" runat="server" Text="Item is in Market"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMarket" runat="server" Checked="true" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTotalPrice" runat="server" Text="Stock Value"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <eluc:Number ID="txtTotalPrice" runat="server" CssClass="input" Width="90px" MaxLength="12" />
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <%-- Details--%>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDetail" runat="server" Visible="false" CssClass="input" MaxLength="200"
                            TextMode="MultiLine" Rows="3" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td></td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
